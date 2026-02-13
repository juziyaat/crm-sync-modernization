# ADR-004: Database-Per-Tenant Strategy

**Date**: February 2026

**Status**: Accepted

**Context**: CCA CRM Sync Modernization Project

## Context

The CCA CRM Sync system serves multiple tenant organizations, each requiring:
- Complete data isolation
- Independent database maintenance
- Tenant-specific backup and recovery
- Flexible scaling per tenant
- Compliance with data residency requirements

### Physical Database Deployment Options

**Option 1: Single Database (Multi-Tenant Row-Level Isolation)**
- ✅ Simple deployment
- ❌ Risk of cross-tenant data leakage
- ❌ Difficult GDPR compliance
- ❌ Cannot isolate performance issues

**Option 2: Database-Per-Tenant (Full Physical Separation)**
- ✅ True data isolation at database level
- ✅ GDPR-compliant
- ✅ Independent backups per tenant
- ✅ Tenant-specific performance tuning
- ⚠️ Operational complexity

**Option 3: Connection String Per Tenant (Hybrid)**
- ✅ Logical separation with shared infrastructure
- ✅ Cost-effective
- ✅ Easy tenant onboarding
- ✅ Flexible scaling

## Decision

We will adopt **Connection String Per Tenant** strategy with **Logical Database-Per-Tenant** approach:

### Architecture Overview

```
Single PostgreSQL Instance
├── cca_system (System metadata database)
├── cca_tenant_001 (Tenant A data)
├── cca_tenant_002 (Tenant B data)
├── cca_tenant_003 (Tenant C data)
└── ... (N tenant databases)
```

### Implementation Strategy

**Step 1: Connection String Management**

Store tenant-specific connection strings:

```json
{
  "ConnectionStrings": {
    "System": "Server=postgres.example.com;Database=cca_system;Port=5432;...",
    "Tenant_00000000-0000-0000-0000-000000000001": "Server=postgres.example.com;Database=cca_tenant_001;Port=5432;...",
    "Tenant_00000000-0000-0000-0000-000000000002": "Server=postgres.example.com;Database=cca_tenant_002;Port=5432;...",
    "Tenant_00000000-0000-0000-0000-000000000003": "Server=postgres.example.com;Database=cca_tenant_003;Port=5432;..."
  }
}
```

**Step 2: Tenant Connection Provider**

```csharp
public interface ITenantConnectionStringProvider
{
    string GetConnectionString(Guid tenantId);
    string GetSystemConnectionString();
}

public class TenantConnectionStringProvider : ITenantConnectionStringProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TenantConnectionStringProvider> _logger;

    public TenantConnectionStringProvider(IConfiguration configuration, ILogger<TenantConnectionStringProvider> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GetConnectionString(Guid tenantId)
    {
        var key = $"ConnectionStrings:Tenant_{tenantId}";
        var connectionString = _configuration[key];

        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError("No connection string found for tenant {TenantId}", tenantId);
            throw new InvalidOperationException($"No connection string configured for tenant {tenantId}");
        }

        return connectionString;
    }

    public string GetSystemConnectionString()
    {
        return _configuration["ConnectionStrings:System"]
            ?? throw new InvalidOperationException("System connection string not configured");
    }
}
```

**Step 3: DbContext Configuration**

```csharp
public static class ServiceRegistration
{
    public static IServiceCollection AddTenantDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ITenantConnectionStringProvider>(
            sp => new TenantConnectionStringProvider(configuration, sp.GetRequiredService<ILogger<TenantConnectionStringProvider>>())
        );

        services.AddScoped<TenantDbContext>(sp =>
        {
            var tenantProvider = sp.GetRequiredService<ITenantProvider>();
            var connStringProvider = sp.GetRequiredService<ITenantConnectionStringProvider>();
            var tenantId = tenantProvider.GetCurrentTenantId();
            var connectionString = connStringProvider.GetConnectionString(tenantId);

            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            optionsBuilder.AddInterceptors(
                new TenantInterceptor(tenantProvider),
                new AuditInterceptor(tenantProvider)
            );

            return new TenantDbContext(optionsBuilder.Options);
        });

        return services;
    }

    public static IServiceCollection AddSystemDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISystemDbContext>(sp =>
        {
            var connStringProvider = sp.GetRequiredService<ITenantConnectionStringProvider>();
            var connectionString = connStringProvider.GetSystemConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<SystemDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new SystemDbContext(optionsBuilder.Options);
        });

        return services;
    }
}
```

**Step 4: Multiple DbContext Pattern**

```csharp
// Tenant-specific DbContext
public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<LdcAccount> LdcAccounts { get; set; }
    public DbSet<SyncJob> SyncJobs { get; set; }
    public DbSet<MonthlyUsage> MonthlyUsage { get; set; }
    public DbSet<InvoiceHistory> InvoiceHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);
    }
}

// System DbContext (for tenant metadata)
public interface ISystemDbContext
{
    DbSet<Tenant> Tenants { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public class SystemDbContext : DbContext, ISystemDbContext
{
    public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(255);
            entity.Property(t => t.IsActive).HasDefaultValue(true);
        });
    }
}

// Domain model for tenant metadata
public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DatabaseName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**Step 5: Database Migration Strategy**

```csharp
// Run migrations for system database
public static async Task MigrateSystemDatabaseAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    await context.Database.MigrateAsync();
}

// Run migrations for all tenant databases
public static async Task MigrateAllTenantDatabasesAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var systemContext = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    var tenants = await systemContext.Tenants.Where(t => t.IsActive).ToListAsync();

    foreach (var tenant in tenants)
    {
        await MigrateTenantDatabaseAsync(services, tenant.Id);
    }
}

// Run migration for specific tenant
public static async Task MigrateTenantDatabaseAsync(IServiceProvider services, Guid tenantId)
{
    using var scope = services.CreateScope();
    var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();

    // Temporarily set tenant context
    tenantProvider.SetCurrentTenant(tenantId);

    var context = scope.ServiceProvider.GetRequiredService<TenantDbContext>();
    await context.Database.MigrateAsync();

    Console.WriteLine($"✓ Migrated database for tenant {tenantId}");
}
```

## Rationale

**Connection String Per Tenant** was chosen because:

### ✅ Benefits

1. **True Data Isolation** - Each tenant has its own database
2. **GDPR Compliance** - Easy to identify and isolate tenant data
3. **Independent Backup/Recovery** - Tenant-specific backups
4. **Performance Isolation** - One tenant's load doesn't affect others
5. **Compliance Requirements** - Data residency easier to manage
6. **Operational Flexibility** - Can scale tenants independently
7. **Cost-Effective** - Shared infrastructure, not N separate databases
8. **Easy Onboarding** - New tenant = new database + connection string

### 🎯 Migration Strategy

```
Phase 1: Setup Infrastructure
├── Create system database (cca_system)
├── Create first tenant database (cca_tenant_001)
└── Deploy migration tooling

Phase 2: Add New Tenants
├── Create new database
├── Add connection string to configuration
├── Run migrations
└── Initialize tenant metadata

Phase 3: Operations
├── Regular backups per database
├── Health checks per database
├── Performance monitoring per tenant
└── Easy scaling per tenant
```

## Consequences

### Positive

✅ **Strong isolation** - Physical database-level isolation
✅ **Compliance-friendly** - GDPR, data residency requirements
✅ **Independent operations** - Backup, restore, upgrade per tenant
✅ **Performance** - No contention between tenants
✅ **Auditing** - All operations naturally scoped to tenant
✅ **Disaster recovery** - Granular RTO/RPO per tenant

### Negative

⚠️ **Connection string management** - Need configuration per tenant
⚠️ **Migration complexity** - Migrations run per-tenant
⚠️ **Monitoring complexity** - Monitor N databases instead of 1
⚠️ **Backup overhead** - More backup jobs (but smaller per backup)
⚠️ **Infrastructure cost** - More database instances (if using separate servers)

## Implementation Checklist

- [ ] Create system database
- [ ] Implement `ITenantConnectionStringProvider`
- [ ] Create `SystemDbContext` for tenant metadata
- [ ] Create `TenantDbContext` for tenant data
- [ ] Implement migration strategy
- [ ] Create tenant onboarding workflow
- [ ] Implement health checks per database
- [ ] Create backup automation
- [ ] Document tenant operations

## Scaling Path

**Phase 1 (Current)**:
- Single PostgreSQL instance
- Multiple databases (one per tenant)
- Shared infrastructure

**Phase 2 (If needed)**:
- Tenant-specific PostgreSQL instances
- Better isolation for large tenants
- Independent scaling

**Phase 3 (If needed)**:
- Multi-region deployment
- Data residency compliance
- Disaster recovery

## Related ADRs

- [ADR-003: Multi-Tenant Design](ADR-003-Multi-Tenant-Design.md) - Logical tenant separation
- [ADR-001: Clean Architecture](ADR-001-Clean-Architecture.md) - Infrastructure layer handles DB connections

## Sign-Off

- **Proposed By**: Claude (AI)
- **Status**: Accepted
- **Effective Date**: February 2026
