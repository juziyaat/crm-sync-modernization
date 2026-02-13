# ADR-003: Multi-Tenant Design

**Date**: February 2026

**Status**: Accepted

**Context**: CCA CRM Sync Modernization Project

## Context

The CCA CRM Sync system will serve multiple CCA organizations. Each organization needs:
- Isolated customer data
- Independent configuration
- Separate API credentials for third-party systems
- Audit trails per tenant

### Tenancy Models Considered

**Option 1: Single Database, Single Schema (Database-per-Instance)**
```
┌─────────────────────┐
│   PostgreSQL DB     │
├─────────────────────┤
│  TenantA (tables)   │
│  TenantB (tables)   │
│  TenantC (tables)   │
└─────────────────────┘
```
- ✅ Complete isolation per tenant
- ✅ Independent scaling
- ✅ Better for compliance/GDPR
- ❌ Operational complexity (N databases)
- ❌ Expensive maintenance

**Option 2: Single Database, Multi-Tenant (Row-Level Isolation)**
```
┌─────────────────────┐
│   PostgreSQL DB     │
├─────────────────────┤
│ Customers Table     │
│ ├─ TenantId=A,... │
│ ├─ TenantId=B,... │
│ └─ TenantId=C,... │
└─────────────────────┘
```
- ✅ Simple operations
- ✅ Lower cost
- ✅ Shared infrastructure
- ❌ Row-level security must be perfect
- ❌ Data leakage risk
- ❌ Compliance complexity

**Option 3: Hybrid (Database-per-Tenant, Logical Multi-DB)**
```
┌──────────────┬──────────────┬──────────────┐
│  TenantA DB  │  TenantB DB  │  TenantC DB  │
├──────────────┼──────────────┼──────────────┤
│   Schema A   │   Schema B   │   Schema C   │
└──────────────┴──────────────┴──────────────┘
```
- ✅ True isolation per tenant
- ✅ Simpler than separate instances
- ✅ Cost-effective
- ✅ Easier migrations per tenant
- ⚠️ Still need TenantId for security

## Decision

We will adopt **Hybrid Multi-Tenancy** with the following approach:

### Architecture

1. **Logical Database-per-Tenant** - Each tenant has independent schema/database
2. **Shared Connection Pool** - One physical connection pool for efficiency
3. **TenantId Context** - Every entity has TenantId for defense-in-depth
4. **Automatic Filtering** - Queries automatically filtered by TenantId

### Implementation Strategy

**Step 1: Tenant Context Resolution**

```csharp
// Extract tenant from HTTP context
public interface ITenantProvider
{
    Guid GetCurrentTenantId();
}

public class HttpContextTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Guid GetCurrentTenantId()
    {
        var tenantId = _httpContextAccessor.HttpContext?.Items["TenantId"];
        if (tenantId == null)
            throw new InvalidOperationException("TenantId not set in context");
        return (Guid)tenantId;
    }
}
```

**Step 2: Automatic TenantId Injection**

Via EF Core interceptor:
```csharp
public class TenantInterceptor : SaveChangesInterceptor
{
    private readonly ITenantProvider _tenantProvider;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct)
    {
        var tenantId = _tenantProvider.GetCurrentTenantId();
        var entities = eventData.Context?.ChangeTracker.Entries();

        foreach (var entity in entities ?? [])
        {
            if (entity.Entity is IMultiTenant multiTenant)
            {
                multiTenant.TenantId = tenantId;
            }
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}
```

**Step 3: Query Filtering**

All queries automatically filtered:
```csharp
public class TenantFilteringInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<IAsyncEnumerable<T>> EnumeratingAsync<T>(
        QueryExpressionEventData eventData,
        InterceptionResult<IAsyncEnumerable<T>> result)
    {
        // Add .Where(x => x.TenantId == currentTenantId) to all queries
        return result;
    }
}
```

Or in DbContext OnModelCreating:
```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    var tenantId = _tenantProvider.GetCurrentTenantId();

    foreach (var entity in builder.Model.GetEntityTypes())
    {
        if (typeof(IMultiTenant).IsAssignableFrom(entity.ClrType))
        {
            builder.Entity(entity.ClrType)
                .HasQueryFilter(e => EF.Property<Guid>(e, "TenantId") == tenantId);
        }
    }
}
```

**Step 4: Connection Management**

```csharp
// Razor-sharp security: Each connection string includes tenant info
public class TenantConnectionStringProvider : ITenantConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private readonly ITenantProvider _tenantProvider;

    public string GetConnectionString()
    {
        var tenantId = _tenantProvider.GetCurrentTenantId();
        var key = $"ConnectionStrings:Tenant_{tenantId}";

        return _configuration[key]
            ?? throw new InvalidOperationException($"No connection string for tenant {tenantId}");
    }
}

// In Startup:
services.AddDbContext<TenantDbContext>(
    (sp, options) =>
    {
        var connStringProvider = sp.GetRequiredService<ITenantConnectionStringProvider>();
        var connString = connStringProvider.GetConnectionString();
        options.UseNpgsql(connString);
        options.AddInterceptors(new TenantInterceptor(...));
    }
);
```

## Rationale

**Hybrid Multi-Tenancy** was chosen because:

### ✅ Benefits

1. **True Data Isolation** - Each tenant's data is logically separated
2. **Compliance** - Easier GDPR/data sovereignty compliance
3. **Security** - Defense-in-depth with both connection-level and row-level filtering
4. **Cost** - Much cheaper than full database-per-instance
5. **Operational** - Simpler than N separate databases
6. **Scaling** - Can scale tenants gradually (move to separate DB if needed)
7. **Per-Tenant Config** - Easy to configure per-tenant API credentials
8. **Audit Trail** - All data includes who accessed what

### 🔒 Security Measures

Multiple layers prevent data leakage:

| Layer | Protection | Example |
|-------|-----------|---------|
| **Connection** | Connection string per tenant | Each request gets tenant-specific DB |
| **Entity Filter** | Query filter on all entities | Queries include `.Where(x => x.TenantId == current)` |
| **Interceptor** | Auto-set TenantId on all entities | Save operations auto-inject TenantId |
| **Repository** | Repository respects TenantId | Never retrieves cross-tenant data |
| **Application** | CQRS commands validate tenant | Commands ensure user belongs to tenant |
| **API** | Authentication validates tenant | JWT token includes TenantId |

## Consequences

### Positive

✅ **Strong data isolation** - Row-level + connection-level security
✅ **Straightforward implementation** - No complex logic needed
✅ **Per-tenant customization** - Easy configuration per tenant
✅ **Scalable** - Can move tenants to separate DB later
✅ **GDPR-friendly** - Clear tenant boundaries
✅ **Audit trail** - All changes include TenantId
✅ **Testable** - Can test multi-tenant scenarios

### Negative

⚠️ **Connection string management** - Need to manage multiple connection strings
⚠️ **Migration complexity** - Migrations need to run per-tenant
⚠️ **Accidental leakage risk** - If TenantId filtering forgotten, data leaks
⚠️ **Testing complexity** - Tests need to consider tenancy

## Implementation Details

### Entity Design

All domain entities must inherit from IMultiTenant:

```csharp
public interface IMultiTenant
{
    Guid TenantId { get; set; }
}

public class Customer : AggregateRoot, IMultiTenant
{
    public Guid TenantId { get; set; }
    public Guid Id { get; set; }
    public CustomerName Name { get; set; }
    public EmailAddress Email { get; set; }
    // ...
}
```

### Database Schema

```sql
CREATE TABLE customers (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    created_by VARCHAR(255),
    updated_at TIMESTAMP,
    updated_by VARCHAR(255),
    FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

-- Index on TenantId for query performance
CREATE INDEX idx_customers_tenant_id ON customers(tenant_id);

-- Unique constraint per tenant (account number unique per tenant)
CREATE UNIQUE INDEX idx_customer_email_per_tenant
    ON customers(tenant_id, email);
```

### Configuration

**appsettings.json**
```json
{
  "ConnectionStrings": {
    "Tenant_00000000-0000-0000-0000-000000000001": "Server=localhost;Database=cca_tenant_1;...",
    "Tenant_00000000-0000-0000-0000-000000000002": "Server=localhost;Database=cca_tenant_2;...",
    "Default": "Server=localhost;Database=cca_system;..."
  }
}
```

### Tenant Service

```csharp
public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<Tenant> _tenantRepository;

    public async Task<Tenant> GetCurrentTenantAsync()
    {
        var tenantId = GetCurrentTenantId();
        var tenant = await _tenantRepository.GetByIdAsync(tenantId);

        if (tenant == null)
            throw new TenantNotFoundException(tenantId);

        return tenant;
    }

    public Guid GetCurrentTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext not available");

        var tenantClaim = httpContext.User.FindFirst("tenant_id");

        if (tenantClaim == null)
            throw new InvalidOperationException("TenantId claim not found");

        return Guid.Parse(tenantClaim.Value);
    }
}
```

## Related ADRs

- [ADR-001: Clean Architecture](ADR-001-Clean-Architecture.md) - Domain layer enforces tenancy
- [ADR-004: Database-Per-Tenant](ADR-004-Database-Per-Tenant.md) - Physical database strategy

## Tenant Lifecycle

```
1. Admin creates tenant in system
2. TenantId assigned
3. Connection string configured
4. Database migrations run for tenant
5. Tenant users added with TenantId in claims
6. Requests automatically filtered by TenantId
```

## Testing Multi-Tenancy

```csharp
[Fact]
public async Task GetCustomers_OnlyReturnsTenantCustomers()
{
    var tenantA = Guid.NewGuid();
    var tenantB = Guid.NewGuid();

    await _dbContext.Customers.AddRangeAsync(
        CreateCustomer(tenantA, "CustomerA1"),
        CreateCustomer(tenantA, "CustomerA2"),
        CreateCustomer(tenantB, "CustomerB1")
    );
    await _dbContext.SaveChangesAsync();

    // Set context to TenantA
    _tenantProvider.SetCurrentTenant(tenantA);

    var customers = await _repository.ListAsync();

    // Should only get TenantA customers
    Assert.Equal(2, customers.Count);
    Assert.All(customers, c => Assert.Equal(tenantA, c.TenantId));
}
```

## Migration Strategy

Each tenant gets its own migration:

```bash
# Add migration for new feature
dotnet ef migrations add AddCustomerPhoneNumber

# Apply to all tenants
for TENANT_ID in $TENANT_IDS; do
    dotnet ef database update \
        --connection "Server=localhost;Database=cca_$TENANT_ID;..."
done
```

## Monitoring & Auditing

All operations include TenantId:

```csharp
public class AuditInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.GetCurrentTenantId();
        var userId = _identityService.GetCurrentUserId();

        foreach (var entry in eventData.Context?.ChangeTracker.Entries() ?? [])
        {
            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedAt = DateTime.UtcNow;
                    auditable.CreatedBy = userId;
                    auditable.TenantId = tenantId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditable.UpdatedAt = DateTime.UtcNow;
                    auditable.UpdatedBy = userId;
                }
            }
        }

        return result;
    }
}
```

## Sign-Off

- **Proposed By**: Claude (AI)
- **Status**: Accepted
- **Effective Date**: February 2026
