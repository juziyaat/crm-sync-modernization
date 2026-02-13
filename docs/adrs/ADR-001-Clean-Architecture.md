# ADR-001: Clean Architecture

**Date**: February 2026

**Status**: Accepted

**Context**: CCA CRM Sync Modernization Project

## Context

The CCA CRM Sync Modernization project requires a scalable, maintainable architecture that can:

1. Handle complex business logic for customer and utility data synchronization
2. Support multiple data sources (Dynamics 365, utility provider APIs)
3. Enable independent testing of business logic
4. Facilitate team onboarding and knowledge transfer
5. Support future expansion to new integrations

We evaluated several architectural approaches:

### Option 1: Layered Architecture (Traditional 3-Tier)
- **Pros**: Simple, familiar, easy to understand
- **Cons**: Can lead to monolithic code, business logic spreads across layers, tight coupling to infrastructure

### Option 2: Clean Architecture (Onion/Hexagonal)
- **Pros**: Business logic isolated at center, dependencies point inward, highly testable, flexible
- **Cons**: More initial complexity, requires more structure

### Option 3: CQRS+Event Sourcing
- **Pros**: Perfect audit trail, eventual consistency support
- **Cons**: Adds complexity, overkill for current requirements, learning curve steep

## Decision

We will adopt **Clean Architecture** with the following structure:

### Layering Strategy

**Core (Center - No External Dependencies)**
```
Domain Layer
├── Entities
├── Aggregates
├── Value Objects
├── Domain Events
└── Specifications
```

**Middle Layer (Depends only on Domain)**
```
Application Layer
├── Commands
├── Queries
├── Handlers (MediatR)
├── DTOs
└── Mapping Profiles
```

**Outer Layer (Can depend on Domain + Application)**
```
Infrastructure Layer
├── Entity Framework Core
├── Repositories
├── External API Clients
├── Event Dispatcher
└── Logging/Telemetry
```

**Presentation Layer (Can depend on all)**
```
Web API / Background Worker
├── Controllers
├── Dependency Injection
├── Middleware
└── Configuration
```

### Key Principles

1. **Dependency Rule**: Dependencies always point inward. Domain layer has zero external dependencies.
2. **Separation of Concerns**: Each layer has a single responsibility
3. **Testability**: Business logic in Domain can be tested without infrastructure
4. **Flexibility**: Implementations can change without affecting business logic

## Rationale

Clean Architecture was chosen because:

1. **Business Logic Protection** - Core domain logic isolated from framework changes
2. **Testability** - Unit tests don't require databases, APIs, or frameworks
3. **Maintainability** - Clear boundaries make code easier to understand and modify
4. **Scalability** - Easy to add new features, data sources, or integrations
5. **Team Effectiveness** - New developers quickly understand code organization
6. **Framework Agnostic** - Can swap EF Core for Dapper, MediatR for simple handlers, etc.

### Example: Testability Benefit

Domain logic tests don't require:
```csharp
[Fact]
public void Customer_CreateWithValidData_Succeeds()
{
    // No database, API client, or dependency injection needed
    var result = Customer.Create(
        name: new CustomerName("John", "Doe"),
        email: EmailAddress.Create("john@example.com").Value
    );

    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
}
```

## Consequences

### Positive

✅ **Business logic is protected** from framework decisions
✅ **Highly testable** - domain logic has no external dependencies
✅ **Clear separation of concerns** - each layer has one responsibility
✅ **Flexible** - can swap implementations without affecting domain
✅ **Self-documenting** - project structure clearly shows architecture
✅ **Scales well** - easy to add new features and aggregates

### Negative

⚠️ **More projects/files** - requires more navigation
⚠️ **Initial learning curve** - team must understand layering rules
⚠️ **DTOs required** - Application layer needs data transfer objects
⚠️ **Mapping boilerplate** - AutoMapper configs for each aggregate
⚠️ **More upfront design** - requires thinking about boundaries early

## Implementation

### Solution Structure

```
src/
├── CCA.Sync.Domain/           # Domain Layer - Business Logic
│   ├── Aggregates/            # Root entities with business rules
│   ├── ValueObjects/          # Immutable business values
│   ├── Enums/                 # Domain enumerations
│   ├── Events/                # Domain events
│   └── Common/                # Base classes (Entity, AggregateRoot, etc.)
│
├── CCA.Sync.Application/      # Application Layer - Use Cases
│   ├── Commands/              # State-changing operations
│   ├── Queries/               # Query operations
│   ├── Handlers/              # Command/Query handlers (MediatR)
│   ├── DTOs/                  # Data transfer objects
│   ├── Validators/            # Command/Query validators
│   ├── MappingProfiles/       # AutoMapper profiles
│   └── Behaviors/             # Pipeline behaviors (validation, logging)
│
├── CCA.Sync.Infrastructure/   # Infrastructure Layer - Implementation Details
│   ├── Persistence/           # Database configurations and repositories
│   ├── Integrations/          # External API clients
│   └── DomainEvents/          # Event dispatcher implementations
│
└── CCA.Sync.WebApi/           # Presentation Layer - HTTP Interface
    ├── Controllers/           # REST endpoints
    └── Middleware/            # Cross-cutting middleware
```

### Project Dependencies

```
WebApi         → depends on → Application, Infrastructure
Worker         → depends on → Application, Infrastructure
Application    → depends on → Domain
Infrastructure → depends on → Application, Domain
Domain         → depends on → nothing (except BCL)
```

### No Circular Dependencies

This structure is enforced via:
- Visual Studio project references (one-way)
- Sonarqube analysis in CI/CD
- Code reviews

## Related ADRs

- [ADR-002: CQRS Pattern](ADR-002-CQRS-Pattern.md) - Complements Clean Architecture
- [ADR-003: Multi-Tenant Design](ADR-003-Multi-Tenant-Design.md) - Implemented within Domain layer

## References

- Robert C. Martin - "Clean Architecture: A Craftsman's Guide to Software Structure and Design"
- Jason Taylor - "Clean Architecture with .NET Core" (YouTube series)
- Microsoft .NET Architecture Guide: https://docs.microsoft.com/en-us/dotnet/architecture/

## Questions & Decisions

**Q: Why not use Mediator at the domain layer?**
A: Keeps domain pure and testable without dependency injection.

**Q: How do we handle cross-cutting concerns?**
A: Via MediatR behaviors and infrastructure interceptors, keeping domain clean.

**Q: How do we avoid anemic domain models?**
A: Business logic lives in domain entities, not in services or handlers.

## Sign-Off

- **Proposed By**: Claude (AI)
- **Status**: Accepted
- **Effective Date**: February 2026
