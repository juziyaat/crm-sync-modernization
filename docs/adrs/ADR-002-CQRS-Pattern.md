# ADR-002: CQRS Pattern (Partial)

**Date**: February 2026

**Status**: Accepted

**Context**: CCA CRM Sync Modernization Project

## Context

The application needs to handle both data modification operations (sync jobs, customer updates) and data retrieval operations (reporting, dashboards) with different optimization strategies.

### Problem Statement

Traditional Create-Read-Update-Delete (CRUD) operations model reads and writes identically:

```csharp
// Traditional approach
public class CustomerController
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == dto.Id);
        customer.Update(dto);
        await _context.SaveChangesAsync();
        return Ok(customer);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        // Uses same entities as writes, loads unnecessary data
        var customer = await _context.Customers
            .Include(c => c.UtilityAccounts)
            .FirstOrDefaultAsync(c => c.Id == id);
        return Ok(customer);
    }
}
```

This approach has limitations:
- Write and read models are tightly coupled
- Queries load unnecessary data
- Scaling reads separately from writes is difficult
- Business logic mixes with query optimization

## Decision

We will adopt **CQRS (Command Query Responsibility Segregation)** with the following approach:

### CQRS Strategy

**Commands** (State-Changing Operations)
```csharp
// Commands modify state
public class CreateCustomerCommand : IRequest<Result<CustomerDto>>
{
    public CustomerName Name { get; init; }
    public EmailAddress Email { get; init; }
}

// Handlers implement business logic
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        var customer = Customer.Create(request.Name, request.Email);
        if (customer.IsFailure) return customer;

        await _repository.AddAsync(customer.Value);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer.Value));
    }
}
```

**Queries** (Data Retrieval Operations)
```csharp
// Queries retrieve data without side effects
public class GetCustomerQuery : IRequest<CustomerDetailsDto>
{
    public Guid CustomerId { get; init; }
}

// Handlers can use optimized read models
public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDetailsDto>
{
    public async Task<CustomerDetailsDto> Handle(GetCustomerQuery request, CancellationToken ct)
    {
        // Can use optimized queries, read-only views, or even a separate read database
        return await _readRepository.GetCustomerDetailsAsync(request.CustomerId);
    }
}
```

### Implementation Details

**Command vs. Query Behavior**

| Aspect | Command | Query |
|--------|---------|-------|
| **Purpose** | Modify state | Retrieve data |
| **Return Type** | `Result<T>` (can fail) | Data object (throws if not found) |
| **Side Effects** | Expected (save to DB) | None |
| **Caching** | Not cached | Can be cached |
| **Transaction** | Required | Optional |
| **Validation** | Strict business rules | Validation not needed |
| **Error Handling** | Result pattern | Exception if not found |

### MediatR Pipeline

Commands go through validation and transaction behaviors:

```
Command
  ↓
Validation Behavior      (FluentValidation)
  ↓
Logging Behavior         (Serilog)
  ↓
Transaction Behavior     (Unit of Work)
  ↓
Command Handler          (Business Logic)
  ↓
Event Dispatcher         (Domain Events)
  ↓
Result<T>
```

Queries go through logging but skip validation:

```
Query
  ↓
Logging Behavior         (Serilog)
  ↓
Query Handler            (Optimized read logic)
  ↓
Data Object
```

## Rationale

CQRS (Partial) was chosen because:

### ✅ Benefits

1. **Separation of Concerns** - Command logic separate from query logic
2. **Optimization** - Queries can be optimized independently (no N+1 problems)
3. **Scalability** - Read and write models can scale separately (future: CQRS with separate read database)
4. **Testability** - Commands can be tested independently of queries
5. **Performance** - Queries use projections, not full entity graphs
6. **Clear Intent** - API clearly shows which operations modify state
7. **Event Sourcing Ready** - Foundation for event sourcing if needed later

### 🚀 Future Extensibility

If needed in Phase 2+, we can:
- Separate read database (PostgreSQL replica)
- Implement read caches (Redis)
- Use event sourcing for complete audit trail
- Independent deployment of read/write services

## Consequences

### Positive

✅ **Clear API contracts** - Commands and Queries are explicit
✅ **Easy to scale queries** - Can optimize without affecting commands
✅ **Natural fit for DDD** - Commands align with domain events
✅ **Better testability** - Test command handlers and query handlers independently
✅ **Audit-friendly** - Commands are natural audit points
✅ **Cache-friendly** - Queries can be easily cached

### Negative

⚠️ **More types** - Separate types for each command and query
⚠️ **MediatR dependency** - Requires understanding MediatR pipeline
⚠️ **DTOs required** - Queries return DTOs, need mapping
⚠️ **Not traditional CRUD** - Team must adopt different mental model
⚠️ **Boilerplate** - More files and handlers

## Implementation Example

### Traditional CRUD (What We're Moving Away From)

```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly DbContext _context;

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        // Mixed responsibilities: API, business logic, data access
        var customer = new Customer { Name = dto.Name, Email = dto.Email };
        _context.Add(customer);
        await _context.SaveChangesAsync();
        return Ok(customer);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customer = await _context.Customers
            .Include(c => c.UtilityAccounts)  // Load unnecessary data
            .FirstOrDefaultAsync(c => c.Id == id);
        return Ok(customer);
    }
}
```

### CQRS Approach (What We're Adopting)

```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customer = await _mediator.Send(new GetCustomerQuery { CustomerId = id });
        return customer != null ? Ok(customer) : NotFound();
    }
}

// Command Handler - Business Logic
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly IRepository<Customer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        // Business logic with validation
        var customerResult = Customer.Create(command.Name, command.Email);
        if (customerResult.IsFailure) return customerResult;

        var customer = customerResult.Value;
        await _repository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
    }
}

// Query Handler - Optimized Read
public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDetailsDto>
{
    private readonly IReadRepository<Customer> _readRepository;

    public async Task<CustomerDetailsDto> Handle(GetCustomerQuery query, CancellationToken ct)
    {
        // Optimized query - only load needed fields
        return await _readRepository.GetCustomerDetailsAsync(query.CustomerId);
    }
}
```

## Project Structure

```
src/CCA.Sync.Application/
├── Commands/
│   ├── Customers/
│   │   ├── CreateCustomerCommand.cs
│   │   ├── CreateCustomerHandler.cs
│   │   └── CreateCustomerValidator.cs
│   └── SyncJobs/
│       ├── CreateSyncJobCommand.cs
│       └── CreateSyncJobHandler.cs
│
├── Queries/
│   ├── Customers/
│   │   ├── GetCustomerQuery.cs
│   │   ├── GetCustomerQueryHandler.cs
│   │   └── GetCustomersQuery.cs
│   └── SyncJobs/
│       ├── GetSyncJobQuery.cs
│       └── GetSyncJobQueryHandler.cs
│
├── Behaviors/
│   ├── ValidationBehavior.cs
│   ├── LoggingBehavior.cs
│   └── TransactionBehavior.cs
│
└── DTOs/
    ├── CustomerDto.cs
    ├── CustomerDetailsDto.cs
    └── SyncJobDto.cs
```

## Related ADRs

- [ADR-001: Clean Architecture](ADR-001-Clean-Architecture.md) - Provides the foundation for CQRS
- [ADR-003: Multi-Tenant Design](ADR-003-Multi-Tenant-Design.md) - CQRS supports multi-tenancy

## References

- Bertrand Meyer - "Command Query Separation Principle"
- Martin Fowler - CQRS: https://martinfowler.com/bliki/CQRS.html
- Greg Young - CQRS documents
- MediatR GitHub: https://github.com/jbogard/MediatR

## Future Enhancements

**Phase 2**: If query performance becomes an issue, we can:
- Introduce read-only projections
- Separate read database with eventual consistency
- Implement query-side caching

**Phase 3**: Event sourcing could replace relational writes

## Sign-Off

- **Proposed By**: Claude (AI)
- **Status**: Accepted
- **Effective Date**: February 2026
