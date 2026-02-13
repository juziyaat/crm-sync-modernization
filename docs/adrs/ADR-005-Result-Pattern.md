# ADR-005: Result Pattern for Error Handling

**Date**: February 2026

**Status**: Accepted

**Context**: CCA CRM Sync Modernization Project

## Context

The application needs robust error handling that:
- Distinguishes between expected failures and exceptional conditions
- Provides detailed failure information without exceptions
- Enables clear error handling in the API layer
- Supports monadic error handling patterns
- Maintains clean code without try-catch blocks

### Error Handling Approaches

**Option 1: Exceptions for Everything**
```csharp
public async Task<Customer> CreateCustomerAsync(CreateCustomerCommand cmd)
{
    if (string.IsNullOrEmpty(cmd.Email))
        throw new ArgumentException("Email is required");

    var customer = new Customer(cmd.Email);
    await _repository.AddAsync(customer);
    return customer;
}

// Usage
try
{
    var customer = await CreateCustomerAsync(cmd);
}
catch (ArgumentException ex)
{
    // Handle validation error
}
catch (DuplicateException ex)
{
    // Handle duplicate customer
}
```

**Pros**: Simple, familiar
**Cons**: Exceptions for control flow, unclear contract, performance overhead

---

**Option 2: Nullable Returns**
```csharp
public async Task<Customer?> CreateCustomerAsync(CreateCustomerCommand cmd)
{
    if (string.IsNullOrEmpty(cmd.Email))
        return null;

    return new Customer(cmd.Email);
}

// Usage
var customer = await CreateCustomerAsync(cmd);
if (customer == null)
{
    // Handle failure - but why did it fail?
}
```

**Pros**: Simple, no exceptions
**Cons**: Loses error information, unclear why it failed

---

**Option 3: Result<T> Pattern**
```csharp
public Result<Customer> CreateCustomer(CreateCustomerCommand cmd)
{
    if (string.IsNullOrEmpty(cmd.Email))
        return Result<Customer>.Failure("Email is required");

    return Result<Customer>.Success(new Customer(cmd.Email));
}

// Usage
var result = CreateCustomer(cmd);
if (result.IsSuccess)
{
    var customer = result.Value;
}
else
{
    var error = result.Error;
}
```

**Pros**: Clear error information, no exceptions for control flow, composable
**Cons**: More code, requires Result type

## Decision

We will adopt the **Result<T> Pattern** for all domain and application layer operations.

### Architecture

```csharp
/// <summary>
/// Represents the result of an operation that can fail without throwing exceptions.
/// </summary>
public sealed record Result<T>(
    bool IsSuccess,
    T? Value,
    string Error
)
{
    /// <summary>
    /// Creates a successful result with a value.
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, string.Empty);

    /// <summary>
    /// Creates a failed result with an error message.
    /// </summary>
    public static Result<T> Failure(string error) => new(false, default, error);

    /// <summary>
    /// Maps the success value to a different type.
    /// </summary>
    public Result<TNext> Map<TNext>(Func<T, Result<TNext>> mapper)
    {
        return IsSuccess
            ? mapper(Value!)
            : Result<TNext>.Failure(Error);
    }

    /// <summary>
    /// Maps the success value to a different type synchronously.
    /// </summary>
    public Result<TNext> MapValue<TNext>(Func<T, TNext> mapper)
    {
        return IsSuccess
            ? Result<TNext>.Success(mapper(Value!))
            : Result<TNext>.Failure(Error);
    }

    /// <summary>
    /// Binds two operations together.
    /// </summary>
    public async Task<Result<TNext>> BindAsync<TNext>(Func<T, Task<Result<TNext>>> mapper)
    {
        return IsSuccess
            ? await mapper(Value!)
            : Result<TNext>.Failure(Error);
    }

    /// <summary>
    /// Executes a side effect on success.
    /// </summary>
    public Result<T> Tap(Action<T> action)
    {
        if (IsSuccess)
            action(Value!);
        return this;
    }

    /// <summary>
    /// Executes a side effect on failure.
    /// </summary>
    public Result<T> TapError(Action<string> action)
    {
        if (!IsSuccess)
            action(Error);
        return this;
    }
}

/// <summary>
/// Non-generic result for operations that don't return a value.
/// </summary>
public sealed record Result(
    bool IsSuccess,
    string Error
)
{
    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}
```

### Usage Patterns

**Basic Usage**

```csharp
public Result<Customer> CreateCustomer(CustomerName name, EmailAddress email)
{
    if (name == null)
        return Result<Customer>.Failure("Customer name is required");

    if (email == null)
        return Result<Customer>.Failure("Customer email is required");

    var customer = new Customer(name, email);
    return Result<Customer>.Success(customer);
}
```

**Chaining with Map**

```csharp
public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerCommand cmd)
{
    var nameResult = CustomerName.Create(cmd.FirstName, cmd.LastName);
    var emailResult = EmailAddress.Create(cmd.Email);

    var customerResult = await nameResult
        .BindAsync(name => emailResult
            .BindAsync(email => CreateCustomerInternalAsync(name, email))
        );

    return customerResult
        .MapValue(customer => _mapper.Map<CustomerDto>(customer));
}

private async Task<Result<Customer>> CreateCustomerInternalAsync(CustomerName name, EmailAddress email)
{
    var customer = new Customer(name, email);
    await _repository.AddAsync(customer);
    return Result<Customer>.Success(customer);
}
```

**In Command Handlers**

```csharp
public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly IRepository<Customer> _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken ct)
    {
        // Validate inputs
        var nameResult = CustomerName.Create(request.FirstName, request.LastName);
        if (nameResult.IsFailure)
            return Result<CustomerDto>.Failure(nameResult.Error);

        var emailResult = EmailAddress.Create(request.Email);
        if (emailResult.IsFailure)
            return Result<CustomerDto>.Failure(emailResult.Error);

        // Create domain entity
        var customer = new Customer(nameResult.Value, emailResult.Value);

        // Persist
        await _repository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync(ct);

        // Map to DTO
        var dto = _mapper.Map<CustomerDto>(customer);
        return Result<CustomerDto>.Success(dto);
    }
}
```

**In API Controllers**

```csharp
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer(
        [FromBody] CreateCustomerCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetCustomer), new { id = result.Value.Id }, result.Value)
            : BadRequest(new ErrorResponse { Error = result.Error });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken ct)
    {
        var query = new GetCustomerQuery { CustomerId = id };
        var customer = await _mediator.Send(query, ct);

        return customer != null
            ? Ok(customer)
            : NotFound();
    }
}

public class ErrorResponse
{
    public string Error { get; set; }
}
```

**Validation with Results**

```csharp
public static class EmailAddressValidator
{
    public static Result<EmailAddress> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<EmailAddress>.Failure("Email address is required");

        if (!email.Contains("@"))
            return Result<EmailAddress>.Failure("Email address format is invalid");

        if (email.Length > 254)
            return Result<EmailAddress>.Failure("Email address is too long");

        return Result<EmailAddress>.Success(new EmailAddress(email));
    }
}
```

**Error Mapping**

```csharp
public static class ResultExtensions
{
    /// <summary>
    /// Maps failure to an HTTP status code.
    /// </summary>
    public static int ToStatusCode<T>(this Result<T> result, Func<string, int> mapper)
    {
        return result.IsSuccess ? 200 : mapper(result.Error);
    }

    /// <summary>
    /// Maps failure error to a different error message.
    /// </summary>
    public static Result<T> MapError<T>(this Result<T> result, Func<string, string> mapper)
    {
        return result.IsSuccess
            ? result
            : Result<T>.Failure(mapper(result.Error));
    }
}
```

## Rationale

**Result<T> Pattern** was chosen because:

### ✅ Benefits

1. **Explicit Error Handling** - Error is part of the type contract
2. **No Exception Overhead** - Expected failures don't use exceptions
3. **Composable** - Results can be combined with Map/Bind
4. **Railway-Oriented** - Success and failure paths are clear
5. **Testable** - Easy to test both success and failure cases
6. **Clear Intent** - Type signature shows operation can fail
7. **Async-Friendly** - Works well with BindAsync
8. **DDD-Aligned** - Domain logic returns Results, not exceptions

### Use Cases

| Scenario | Pattern | Why |
|----------|---------|-----|
| Validation failure | Result<T> | Expected, should not throw |
| Business rule violation | Result<T> | Expected, business logic |
| Database constraint violation | Result<T> | Expected, data conflict |
| Unexpected database error | Exception | Exceptional, infrastructure failure |
| Null reference | Exception | Programming error, should crash |
| API connection timeout | Exception | Exceptional, infrastructure |

## Consequences

### Positive

✅ **Explicit error handling** - No hidden exceptions
✅ **Clean code** - No try-catch blocks for business logic
✅ **Composable** - Easy to chain operations
✅ **Testable** - Both paths easily tested
✅ **Performance** - No exception overhead for expected failures
✅ **Clear contracts** - Type signature shows failures possible

### Negative

⚠️ **More code** - Results require more plumbing
⚠️ **Team learning** - Need to understand Result pattern
⚠️ **Not idiomatic C#** - Exceptions are the C# way
⚠️ **Mapping boilerplate** - Result to DTO mapping needed
⚠️ **Null checking** - Still need nullability checks

## Implementation Rules

1. **Domain/Application**: Always use Result<T>
2. **Queries**: Can throw (data not found is exceptional)
3. **Commands**: Always use Result<T>
4. **Infrastructure**: Can throw
5. **Controllers**: Convert Result<T> to HTTP status codes
6. **Value Objects**: Always use Result<T>.Create() factory method

## Migration Path

For existing code:
1. New domain logic uses Result<T>
2. Gradually wrap existing methods
3. Convert layer boundaries to Result<T>
4. Eventually all business logic uses Result<T>

## Related ADRs

- [ADR-001: Clean Architecture](ADR-001-Clean-Architecture.md) - Domain layer uses Result pattern
- [ADR-002: CQRS Pattern](ADR-002-CQRS-Pattern.md) - Commands return Result<T>

## References

- Scott Wlaschin - "Railway-Oriented Programming"
- Vladimir Khorikov - "Domain-Driven Design in C#"
- Jimmy Bogard - "Functional C#"

## Sign-Off

- **Proposed By**: Claude (AI)
- **Status**: Accepted
- **Effective Date**: February 2026
