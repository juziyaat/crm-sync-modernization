# Phase 1: Foundation - Complete Issue List

## Overview
**Duration**: Weeks 1-4
**Total Issues**: 25
**Estimated Effort**:
- AI: 75 hours
- Human Review: 12 hours
- Calendar Time: 4 weeks

---

## Week 1: Setup & Standards (Issues #1-5)

### Issue #1: Project Infrastructure Setup

**Title**: Set up GitHub repository infrastructure and project boards

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 2 hours (AI)

**Description**:
Initialize the GitHub repository with proper structure and project management tools for AI-human collaboration.

**Tasks**:

#### AI Tasks
- [ ] Create `.github/ISSUE_TEMPLATE/` directory structure
- [ ] Create issue templates (epic, user-story, technical-task, bug)
- [ ] Create PR template with AI collaboration sections
- [ ] Set up GitHub Actions for project automation
- [ ] Create `docs/` directory structure (architecture, project-plan, adrs, meeting-notes)
- [ ] Configure `.gitignore` for .NET projects
- [ ] Create repository README with project overview
- [ ] Document project board setup instructions

#### Human Tasks
- [ ] Create GitHub repository
- [ ] Enable GitHub Projects
- [ ] Set up branch protection rules (require PR reviews, CI passing)
- [ ] Add team members with appropriate permissions
- [ ] Review and approve repository structure

**Acceptance Criteria**:
- [ ] Repository accessible to team
- [ ] Branch protection enabled on main
- [ ] Issue and PR templates functional
- [ ] Automation workflows configured
- [ ] Documentation complete

---

### Issue #2: Solution Structure

**Title**: Create .NET 9 solution structure following Clean Architecture

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 1 hour (Human)

**Description**:
Set up the complete .NET 9 solution with proper Clean Architecture project structure and dependencies.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `CcaSyncModernization.sln`
- [ ] Create `src/CCA.Sync.Domain/CCA.Sync.Domain.csproj`
- [ ] Create `src/CCA.Sync.Application/CCA.Sync.Application.csproj`
- [ ] Create `src/CCA.Sync.Infrastructure/CCA.Sync.Infrastructure.csproj`
- [ ] Create `src/CCA.Sync.WebApi/CCA.Sync.WebApi.csproj`
- [ ] Create `src/CCA.Sync.Worker/CCA.Sync.Worker.csproj`
- [ ] Create `src/CCA.Sync.Shared/CCA.Sync.Shared.csproj`
- [ ] Create `tests/Domain.Tests/Domain.Tests.csproj`
- [ ] Create `tests/Application.Tests/Application.Tests.csproj`
- [ ] Create `tests/Infrastructure.Tests/Infrastructure.Tests.csproj`
- [ ] Create `tests/Integration.Tests/Integration.Tests.csproj`
- [ ] Create `Directory.Build.props` with common settings
- [ ] Create `Directory.Packages.props` for central package management
- [ ] Create `.editorconfig` with C# 13 conventions
- [ ] Set up project references (Domain → Application → Infrastructure)
- [ ] Add core NuGet packages
- [ ] Create README.md for each project
- [ ] Ensure solution builds successfully

#### Human Review Tasks
- [ ] Verify project structure matches architecture
- [ ] Check dependency flow (no circular references)
- [ ] Verify NuGet packages are appropriate
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Solution builds with `dotnet build`
- [ ] No circular dependencies
- [ ] Domain project has no external dependencies (except core libraries)
- [ ] Application depends only on Domain
- [ ] Infrastructure depends on Application and Domain
- [ ] Test projects properly reference tested projects
- [ ] .NET 9, C# 13, nullable enabled
- [ ] TreatWarningsAsErrors enabled

**Technical Notes**:
- Target .NET 9.0
- Enable nullable reference types globally
- Use C# 13 features
- ImplicitUsings enabled
- Central package management with Directory.Packages.props

---

### Issue #3: Architecture Documentation

**Title**: Create comprehensive architecture documentation and ADRs

**Labels**: `documentation`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 30 min (Human)

**Description**:
Document the system architecture using C4 diagrams and Architecture Decision Records.

**Tasks**:

#### AI Tasks
- [ ] Create `docs/architecture/README.md` with overview
- [ ] Create C4 Context diagram (system in environment)
- [ ] Create C4 Container diagram (high-level architecture)
- [ ] Create C4 Component diagrams (domain, application, infrastructure)
- [ ] Document folder structure and conventions
- [ ] Create `docs/adrs/ADR-001-Clean-Architecture.md`
- [ ] Create `docs/adrs/ADR-002-CQRS-Pattern.md`
- [ ] Create `docs/adrs/ADR-003-Multi-Tenant-Design.md`
- [ ] Create `docs/adrs/ADR-004-Database-Per-Tenant.md`
- [ ] Create `docs/adrs/ADR-005-Result-Pattern.md`
- [ ] Document domain model structure
- [ ] Create development setup guide

#### Human Review Tasks
- [ ] Review architecture diagrams for accuracy
- [ ] Validate ADRs reflect correct decisions
- [ ] Approve documentation

**Acceptance Criteria**:
- [ ] C4 diagrams clearly show system structure
- [ ] ADRs follow standard format (Context, Decision, Consequences)
- [ ] All major architectural decisions documented
- [ ] Documentation is clear and helpful for onboarding
- [ ] Diagrams use Mermaid or PlantUML for version control

**Deliverables**:
- Architecture overview
- C4 diagrams (Context, Container, Component)
- 5 Architecture Decision Records
- Development setup guide

---

### Issue #4: Shared Kernel - Common Domain Patterns

**Title**: Implement shared kernel with base domain classes

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 5 hours (AI), 1 hour (Human)

**Description**:
Create the foundational domain classes and patterns that will be used across all aggregates.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Common/Entity.cs` (base entity with Id)
- [ ] Create `Domain/Common/AggregateRoot.cs` (domain events support)
- [ ] Create `Domain/Common/ValueObject.cs` (value equality)
- [ ] Create `Domain/Common/DomainEvent.cs` (base domain event)
- [ ] Create `Domain/Common/IDomainEventDispatcher.cs`
- [ ] Create `Domain/Common/Result.cs` (Result<T> pattern)
- [ ] Create `Domain/Common/Error.cs` (error handling)
- [ ] Create `Domain/Common/IRepository.cs` (generic repository)
- [ ] Create `Domain/Common/IUnitOfWork.cs`
- [ ] Create `Domain/Specifications/ISpecification.cs`
- [ ] Create `Domain/Specifications/Specification.cs` (base implementation)
- [ ] Create `Domain/Specifications/AndSpecification.cs`
- [ ] Create `Domain/Specifications/OrSpecification.cs`
- [ ] Create `Domain/Specifications/NotSpecification.cs`
- [ ] Create comprehensive unit tests for all classes (30+ tests)

#### Human Review Tasks
- [ ] Review Result<T> pattern implementation
- [ ] Verify domain event mechanism
- [ ] Check specification pattern implementation
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All base classes are generic and reusable
- [ ] Entity equality based on Id
- [ ] ValueObject equality based on properties
- [ ] AggregateRoot can raise and clear domain events
- [ ] Result<T> supports success/failure states
- [ ] Specification pattern supports composition
- [ ] 90%+ code coverage
- [ ] No dependencies on other layers

**Technical Notes**:
- Use records for ValueObject where appropriate
- Result<T> should be immutable
- Domain events should be queued in AggregateRoot
- Specifications should support LINQ expressions

---

### Issue #5: Domain Value Objects

**Title**: Implement common domain value objects

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 45 min (Human)

**Description**:
Create reusable value objects that will be used across multiple aggregates.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/ValueObjects/EmailAddress.cs`
  - Email regex validation
  - Factory method with Result<T>
  - ImplicitOperator for string conversion
- [ ] Create `Domain/ValueObjects/PhoneNumber.cs`
  - US format validation
  - Formatting methods (display, E.164)
- [ ] Create `Domain/ValueObjects/Address.cs`
  - Street, City, State, ZipCode
  - US address validation
- [ ] Create `Domain/ValueObjects/CustomerName.cs`
  - FirstName, LastName, MiddleName
  - FullName computed property
- [ ] Create `Domain/ValueObjects/AccountNumber.cs`
  - Validation rules
  - Equality comparison
- [ ] Create `Domain/ValueObjects/MeterNumber.cs`
  - Format validation
- [ ] Create `Domain/ValueObjects/TenantId.cs`
  - Strong typing for tenant identifier
- [ ] Create comprehensive tests for each value object (40+ tests)

#### Human Review Tasks
- [ ] Verify validation rules are correct
- [ ] Check US address validation logic
- [ ] Test with real sample data
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All value objects are immutable
- [ ] Validation in factory methods
- [ ] Proper error messages
- [ ] Equality based on all properties
- [ ] 85%+ code coverage
- [ ] XML documentation on public methods

**Technical Notes**:
- Use C# records for immutability
- Factory methods should return Result<ValueObject>
- Consider implicit operators for common conversions
- Phone numbers should support multiple formats

---

## Week 2: Core Domain Aggregates (Issues #6-10)

### Issue #6: Customer Aggregate Root

**Title**: Implement Customer aggregate root following DDD patterns

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 6 hours (AI), 1.5 hours (Human)

**Description**:
Create the Customer aggregate root as the core domain entity.

**Business Context**:
A Customer represents a utility customer enrolled with a CCA. They can have multiple utility accounts across different providers (PG&E, SCE, SDG&E). The aggregate must enforce business rules and maintain consistency.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/Customer/Customer.cs`
  - Factory method `Create(...)` with validation
  - Method `AddUtilityAccount(...)`
  - Method `RemoveUtilityAccount(...)`
  - Method `UpdateContactInfo(...)`
  - Method `UpdateEmail(...)`
  - Method `SyncToDynamics365(...)`
  - Method `MarkSyncFailed(...)`
  - Private setters for encapsulation
  - Domain events for all state changes

- [ ] Create `Domain/Aggregates/Customer/UtilityAccount.cs`
  - Entity within Customer aggregate
  - Private constructor
  - Factory method with validation
  - AccountNumber (unique per provider)
  - MeterNumber
  - UtilityProvider enum
  - ServiceAddress

- [ ] Create `Domain/Enums/UtilityProvider.cs`
  - PGE, SCE, SDGE, Other

- [ ] Create `Domain/Enums/SyncStatus.cs`
  - Pending, InProgress, Synced, Failed, Retrying

- [ ] Create `Domain/Events/CustomerCreatedEvent.cs`
- [ ] Create `Domain/Events/UtilityAccountAddedEvent.cs`
- [ ] Create `Domain/Events/UtilityAccountRemovedEvent.cs`
- [ ] Create `Domain/Events/CustomerSyncedEvent.cs`
- [ ] Create `Domain/Events/CustomerSyncFailedEvent.cs`
- [ ] Create `Domain/Events/CustomerContactInfoUpdatedEvent.cs`

#### AI Testing Tasks
- [ ] Create `tests/Domain.Tests/Aggregates/CustomerTests.cs` (25+ tests)
  - Test: Create_ValidData_ReturnsSuccess
  - Test: Create_InvalidEmail_ReturnsFailure
  - Test: Create_EmptyName_ReturnsFailure
  - Test: AddUtilityAccount_ValidAccount_AddsSuccessfully
  - Test: AddUtilityAccount_DuplicateAccountNumber_Fails
  - Test: AddUtilityAccount_RaisesDomainEvent
  - Test: RemoveUtilityAccount_ValidAccount_RemovesSuccessfully
  - Test: RemoveUtilityAccount_NonExistent_ReturnsFailure
  - Test: SyncToDynamics365_ValidId_UpdatesStatus
  - Test: SyncToDynamics365_RaisesDomainEvent
  - Test: MarkSyncFailed_SetsStatusAndMessage
  - Test: UpdateEmail_ValidEmail_UpdatesSuccessfully
  - Test: UpdateEmail_InvalidEmail_Fails
  - Test: UtilityAccountCollection_IsReadOnly
  - Test: CustomerWithMultipleAccounts_MaintainsAllAccounts

#### Human Review Tasks
- [ ] Review Customer business logic
- [ ] Verify utility account rules
- [ ] Test with real CCA customer data
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All unit tests passing
- [ ] Code coverage ≥ 85%
- [ ] No public setters on aggregate
- [ ] All state changes raise domain events
- [ ] Factory methods enforce validation
- [ ] XML documentation complete
- [ ] No dependencies on other layers
- [ ] Follows SOLID principles

**Definition of Done**:
- [ ] All AI tasks completed
- [ ] PR created and linked
- [ ] All tests green
- [ ] Code coverage ≥ 85%
- [ ] Documentation complete
- [ ] Human review approved
- [ ] PR merged to main

---

### Issue #7: LdcAccount Aggregate Root

**Title**: Implement LdcAccount aggregate root for CCA account management

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 5 hours (AI), 1 hour (Human)

**Description**:
Create the LdcAccount aggregate representing a CCA's LDC (Local Distribution Company) account.

**Business Context**:
An LdcAccount represents the CCA's account with a utility provider (LDC). Each account has configuration, credentials, and sync settings.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/LdcAccount/LdcAccount.cs`
  - Factory method `Create(...)`
  - Method `UpdateCredentials(...)`
  - Method `UpdateConfiguration(...)`
  - Method `EnableSync()` / `DisableSync()`
  - Method `UpdateLastSyncTime(...)`
  - Provider information
  - API credentials (encrypted)
  - Sync configuration

- [ ] Create `Domain/Aggregates/LdcAccount/SyncConfiguration.cs`
  - Value object for sync settings
  - Schedule settings
  - Batch size
  - Retry policy

- [ ] Create `Domain/Enums/LdcProvider.cs`
  - PGE, SCE, SDGE

- [ ] Create `Domain/Events/LdcAccountCreatedEvent.cs`
- [ ] Create `Domain/Events/LdcAccountCredentialsUpdatedEvent.cs`
- [ ] Create `Domain/Events/LdcAccountSyncEnabledEvent.cs`
- [ ] Create `Domain/Events/LdcAccountSyncDisabledEvent.cs`

#### AI Testing Tasks
- [ ] Create `tests/Domain.Tests/Aggregates/LdcAccountTests.cs` (20+ tests)
  - Test: Create_ValidData_ReturnsSuccess
  - Test: Create_InvalidProvider_ReturnsFailure
  - Test: UpdateCredentials_ValidCredentials_UpdatesSuccessfully
  - Test: UpdateCredentials_RaisesDomainEvent
  - Test: EnableSync_WhenDisabled_EnablesSuccessfully
  - Test: EnableSync_WhenAlreadyEnabled_ReturnsFailure
  - Test: DisableSync_WhenEnabled_DisablesSuccessfully
  - Test: UpdateConfiguration_ValidConfig_UpdatesSuccessfully

#### Human Review Tasks
- [ ] Review credential handling
- [ ] Verify sync configuration logic
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All tests passing
- [ ] Credentials marked for encryption (note in docs)
- [ ] Code coverage ≥ 85%
- [ ] Domain events raised appropriately
- [ ] Factory pattern enforced

---

### Issue #8: SyncJob Aggregate Root

**Title**: Implement SyncJob aggregate for tracking sync operations

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 5 hours (AI), 1 hour (Human)

**Description**:
Create the SyncJob aggregate for tracking data synchronization jobs between systems.

**Business Context**:
SyncJob tracks the execution of sync operations, including status, progress, errors, and statistics.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/SyncJob/SyncJob.cs`
  - Factory method `Create(...)`
  - Method `Start()`
  - Method `RecordProgress(...)`
  - Method `RecordError(...)`
  - Method `Complete(...)`
  - Method `Fail(...)`
  - Job metadata
  - Statistics tracking

- [ ] Create `Domain/Aggregates/SyncJob/SyncJobStatistics.cs`
  - Value object for stats
  - Records processed, failed, skipped
  - Duration tracking

- [ ] Create `Domain/Aggregates/SyncJob/SyncJobError.cs`
  - Entity for error tracking
  - Error message, stack trace
  - Timestamp

- [ ] Create `Domain/Enums/SyncJobType.cs`
  - CustomerSync, UsageSync, InvoiceSync

- [ ] Create `Domain/Enums/SyncJobStatus.cs`
  - Pending, Running, Completed, Failed, Cancelled

- [ ] Create domain events for all state transitions

#### AI Testing Tasks
- [ ] Create `tests/Domain.Tests/Aggregates/SyncJobTests.cs` (20+ tests)
  - Test state transitions
  - Test statistics tracking
  - Test error recording
  - Test completion scenarios

#### Human Review Tasks
- [ ] Verify job tracking logic
- [ ] Review error handling
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All tests passing
- [ ] Code coverage ≥ 85%
- [ ] State machine properly enforced
- [ ] Statistics accurately tracked

---

### Issue #9: MonthlyUsage Aggregate

**Title**: Implement MonthlyUsage aggregate for usage data tracking

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 45 min (Human)

**Description**:
Create the MonthlyUsage aggregate for storing and managing utility usage data.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/MonthlyUsage/MonthlyUsage.cs`
  - Factory method
  - Usage amount (kWh)
  - Billing period
  - Read date
  - Usage type (actual vs estimated)

- [ ] Create `Domain/ValueObjects/BillingPeriod.cs`
  - Start date, end date
  - Validation

- [ ] Create `Domain/Enums/UsageType.cs`
  - Actual, Estimated, Adjusted

- [ ] Create domain events
- [ ] Create unit tests (15+ tests)

#### Human Review Tasks
- [ ] Verify usage calculation logic
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All tests passing
- [ ] Code coverage ≥ 80%
- [ ] Billing period validation correct

---

### Issue #10: InvoiceHistory Aggregate

**Title**: Implement InvoiceHistory aggregate for invoice tracking

**Labels**: `domain`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 45 min (Human)

**Description**:
Create the InvoiceHistory aggregate for invoice data from Dynamics 365.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/InvoiceHistory/InvoiceHistory.cs`
  - Factory method
  - Invoice number
  - Invoice date
  - Amount
  - Status
  - Line items

- [ ] Create `Domain/Aggregates/InvoiceHistory/InvoiceLineItem.cs`
  - Entity within aggregate
  - Description, amount, quantity

- [ ] Create `Domain/Enums/InvoiceStatus.cs`
  - Draft, Issued, Paid, Overdue, Cancelled

- [ ] Create domain events
- [ ] Create unit tests (15+ tests)

#### Human Review Tasks
- [ ] Verify invoice business rules
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All tests passing
- [ ] Code coverage ≥ 80%
- [ ] Invoice totals calculated correctly

---

## Week 3: Infrastructure Setup (Issues #11-16)

### Issue #11: Database Context Setup

**Title**: Implement EF Core DbContext with multi-tenant support

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 6 hours (AI), 1.5 hours (Human)

**Description**:
Create the Entity Framework Core database context with multi-tenancy interceptors and configurations.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Infrastructure/Persistence/TenantDbContext.cs`
  - DbSets for all aggregates
  - OnModelCreating configuration
  - Multi-tenant filtering

- [ ] Create `Infrastructure/Persistence/Configurations/CustomerConfiguration.cs`
  - Entity configuration
  - Property mappings
  - Indexes
  - Owned entities for value objects

- [ ] Create `Infrastructure/Persistence/Configurations/LdcAccountConfiguration.cs`
- [ ] Create `Infrastructure/Persistence/Configurations/SyncJobConfiguration.cs`
- [ ] Create `Infrastructure/Persistence/Configurations/MonthlyUsageConfiguration.cs`
- [ ] Create `Infrastructure/Persistence/Configurations/InvoiceHistoryConfiguration.cs`

- [ ] Create `Infrastructure/Persistence/Interceptors/TenantInterceptor.cs`
  - Auto-set TenantId on insert
  - Filter queries by TenantId

- [ ] Create `Infrastructure/Persistence/Interceptors/AuditInterceptor.cs`
  - Auto-populate CreatedAt, CreatedBy
  - Auto-populate UpdatedAt, UpdatedBy

- [ ] Create `Infrastructure/Persistence/Interceptors/DomainEventInterceptor.cs`
  - Dispatch domain events on SaveChanges

#### AI Testing Tasks
- [ ] Create `tests/Infrastructure.Tests/Persistence/TenantDbContextTests.cs`
  - Test: TenantId_AutoSet_OnInsert
  - Test: Queries_Filtered_ByTenantId
  - Test: AuditFields_AutoPopulated
  - Test: DomainEvents_Dispatched_OnSaveChanges

#### Human Review Tasks
- [ ] Review entity configurations
- [ ] Verify multi-tenant filtering
- [ ] Test with sample data
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] DbContext properly configured
- [ ] All entity configurations complete
- [ ] Interceptors working correctly
- [ ] Tests covering key scenarios
- [ ] No tenant data leakage

**Technical Notes**:
- Use value converters for value objects
- Configure owned entities for complex value objects
- Ensure all queries filtered by TenantId
- Use shadow properties for audit fields

---

### Issue #12: Repository Implementation

**Title**: Implement generic repository pattern with specifications

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 5 hours (AI), 1 hour (Human)

**Description**:
Implement the repository pattern with specification support for data access.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Infrastructure/Persistence/Repositories/Repository.cs`
  - Generic repository implementation
  - CRUD operations
  - Specification support

- [ ] Create `Infrastructure/Persistence/Repositories/ReadRepository.cs`
  - Read-only repository for queries
  - Optimized for read operations

- [ ] Create `Infrastructure/Persistence/SpecificationEvaluator.cs`
  - Convert specifications to LINQ
  - Support for Include, OrderBy, Paging

- [ ] Create `Infrastructure/Persistence/UnitOfWork.cs`
  - Transaction management
  - SaveChanges implementation
  - Domain event dispatching

#### AI Testing Tasks
- [ ] Create comprehensive repository tests (25+ tests)
  - Test: Add_ValidEntity_SavesSuccessfully
  - Test: Update_ExistingEntity_UpdatesSuccessfully
  - Test: Delete_ExistingEntity_DeletesSuccessfully
  - Test: GetById_ExistingEntity_ReturnsEntity
  - Test: GetBySpec_MatchingEntities_ReturnsFiltered
  - Test: UnitOfWork_SaveChanges_CommitsTransaction

#### Human Review Tasks
- [ ] Review repository implementation
- [ ] Verify specification evaluator
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Repository pattern fully implemented
- [ ] Specifications work correctly
- [ ] Unit of work manages transactions
- [ ] Tests cover all major scenarios
- [ ] Code coverage ≥ 85%

---

### Issue #13: Initial Database Migration

**Title**: Create initial EF Core migration for all aggregates

**Labels**: `infrastructure`, `database`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 3 hours (AI), 1 hour (Human)

**Description**:
Create and verify the initial database migration with all tables and relationships.

**Tasks**:

#### AI Tasks
- [ ] Create initial migration
- [ ] Review generated migration code
- [ ] Add data seeding for reference data
- [ ] Create migration documentation
- [ ] Test migration up and down

#### Human Review Tasks
- [ ] Review migration code
- [ ] Verify table structure
- [ ] Test against local database
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Migration creates all tables correctly
- [ ] Indexes properly configured
- [ ] Foreign keys established
- [ ] Migration reversible
- [ ] Seed data included

---

### Issue #14: Domain Event Dispatcher

**Title**: Implement domain event dispatcher and handlers infrastructure

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 1 hour (Human)

**Description**:
Create the infrastructure for dispatching and handling domain events.

**Tasks**:

#### AI Implementation Tasks
- [ ] Create `Infrastructure/DomainEvents/DomainEventDispatcher.cs`
- [ ] Create `Infrastructure/DomainEvents/IDomainEventHandler.cs`
- [ ] Integrate with dependency injection
- [ ] Create sample event handlers for testing
- [ ] Add logging support

#### AI Testing Tasks
- [ ] Create tests for event dispatching (10+ tests)

#### Human Review Tasks
- [ ] Review event flow
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Events dispatched correctly
- [ ] Handlers invoked asynchronously
- [ ] Error handling implemented
- [ ] Tests passing

---

### Issue #15: Logging and Telemetry Setup

**Title**: Configure Serilog and Application Insights telemetry

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 3 hours (AI), 30 min (Human)

**Description**:
Set up structured logging with Serilog and Application Insights integration.

**Tasks**:

#### AI Tasks
- [ ] Install Serilog packages
- [ ] Configure structured logging
- [ ] Add request logging
- [ ] Configure log sinks (Console, File, AppInsights)
- [ ] Create logging configuration
- [ ] Add correlation IDs
- [ ] Document logging standards

#### Human Review Tasks
- [ ] Review logging configuration
- [ ] Verify log output
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Structured logging working
- [ ] Logs include correlation IDs
- [ ] Application Insights configured
- [ ] Log levels configurable

---

### Issue #16: Configuration Management

**Title**: Implement configuration management with Azure Key Vault support

**Labels**: `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 3 hours (AI), 45 min (Human)

**Description**:
Set up configuration management with support for multiple environments and secrets.

**Tasks**:

#### AI Tasks
- [ ] Configure appsettings.json hierarchy
- [ ] Create appsettings.Development.json
- [ ] Create appsettings.Staging.json
- [ ] Create appsettings.Production.json
- [ ] Add Azure Key Vault configuration provider
- [ ] Create configuration classes (strongly-typed)
- [ ] Document configuration settings

#### Human Review Tasks
- [ ] Review configuration structure
- [ ] Verify Key Vault integration
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Configuration loads correctly
- [ ] Secrets from Key Vault
- [ ] Strongly-typed settings
- [ ] Environment-specific configs work

---

## Week 4: Testing & DevOps (Issues #17-25)

### Issue #17: Integration Test Infrastructure

**Title**: Set up integration testing with Testcontainers

**Labels**: `testing`, `infrastructure`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 1 hour (Human)

**Description**:
Create integration test infrastructure using Testcontainers for database isolation.

**Tasks**:

#### AI Tasks
- [ ] Install Testcontainers packages
- [ ] Create `IntegrationTestBase.cs`
- [ ] Configure PostgreSQL container
- [ ] Configure test database seeding
- [ ] Create helper methods for test data
- [ ] Create sample integration test
- [ ] Document integration test patterns

#### Human Review Tasks
- [ ] Review test infrastructure
- [ ] Run integration tests locally
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Testcontainers working
- [ ] Database spins up per test class
- [ ] Tests isolated
- [ ] Sample test passing

---

### Issue #18: End-to-End POC Test

**Title**: Create end-to-end proof-of-concept integration test

**Labels**: `testing`, `phase-1-foundation`, `ai-task`, `priority-critical`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 1.5 hours (Human)

**Description**:
Create comprehensive E2E test that validates entire stack from domain to database.

**Tasks**:

#### AI Tasks
- [ ] Create `CustomerIntegrationTests.cs`
  - Test: CreateCustomer → AddUtilityAccount → Save → Retrieve
  - Test: Multi-tenant data isolation
  - Test: Audit fields auto-populated
  - Test: Domain events dispatched
  - Test: Concurrent updates handling

- [ ] Create `SyncJobIntegrationTests.cs`
  - Test: Create job → Start → Record progress → Complete
  - Test: Error handling and recording

- [ ] Create performance test
  - Test: Batch insert 1000 customers
  - Test: Query performance with indexes

#### Human Tasks
- [ ] Run all integration tests
- [ ] Verify multi-tenancy works
- [ ] Check performance metrics
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All E2E tests passing
- [ ] Multi-tenancy verified
- [ ] Performance acceptable
- [ ] No data leakage between tenants

---

### Issue #19: Docker Compose Setup

**Title**: Create Docker Compose for local development environment

**Labels**: `infrastructure`, `devops`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 3 hours (AI), 1 hour (Human)

**Description**:
Set up Docker Compose with PostgreSQL, pgAdmin, and application services.

**Tasks**:

#### AI Tasks
- [ ] Create `docker-compose.yml`
  - PostgreSQL service
  - pgAdmin service
  - API service
  - Worker service
- [ ] Create Dockerfiles for each service
- [ ] Configure environment variables
- [ ] Create setup script
- [ ] Document Docker usage

#### Human Review Tasks
- [ ] Test Docker Compose locally
- [ ] Verify services connect
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] All services start correctly
- [ ] Database accessible from API
- [ ] pgAdmin works
- [ ] Environment variables configured

---

### Issue #20: CI Pipeline Setup

**Title**: Configure GitHub Actions CI pipeline

**Labels**: `devops`, `phase-1-foundation`, `ai-task`, `priority-high`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 4 hours (AI), 1 hour (Human)

**Description**:
Set up continuous integration pipeline with build, test, and code quality checks.

**Tasks**:

#### AI Tasks
- [ ] Create `.github/workflows/ci.yml`
- [ ] Configure build job
- [ ] Configure unit test job
- [ ] Configure integration test job
- [ ] Add code coverage reporting
- [ ] Add code quality checks (dotnet format)
- [ ] Configure test result publishing
- [ ] Add PR comments with coverage

#### Human Review Tasks
- [ ] Review workflow configuration
- [ ] Trigger test workflow
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] CI runs on every PR
- [ ] All tests execute
- [ ] Coverage reports generated
- [ ] Build failures block PR
- [ ] Code quality checks pass

---

### Issue #21: Code Coverage Configuration

**Title**: Set up code coverage with quality gates

**Labels**: `testing`, `devops`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 2 hours (AI), 30 min (Human)

**Description**:
Configure code coverage collection and enforcement with 80% threshold.

**Tasks**:

#### AI Tasks
- [ ] Configure Coverlet for coverage collection
- [ ] Set up ReportGenerator for reports
- [ ] Configure coverage thresholds (80% line coverage)
- [ ] Add coverage to CI pipeline
- [ ] Create coverage badge
- [ ] Document coverage standards

#### Human Review Tasks
- [ ] Review coverage reports
- [ ] Verify thresholds
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Coverage collected on all tests
- [ ] Reports generated
- [ ] 80% threshold enforced
- [ ] Coverage visible in PRs

---

### Issue #22: Security Scanning Setup

**Title**: Configure security scanning and dependency checks

**Labels**: `devops`, `security`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 2 hours (AI), 30 min (Human)

**Description**:
Set up automated security scanning for vulnerabilities and dependencies.

**Tasks**:

#### AI Tasks
- [ ] Configure Dependabot
- [ ] Add security scanning to CI
- [ ] Configure SAST tools
- [ ] Set up secret scanning
- [ ] Document security practices

#### Human Review Tasks
- [ ] Review security configuration
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Dependabot configured
- [ ] Security scans run on PRs
- [ ] Vulnerabilities reported
- [ ] Secrets detection active

---

### Issue #23: Development Documentation

**Title**: Create comprehensive development documentation

**Labels**: `documentation`, `phase-1-foundation`, `ai-task`, `priority-medium`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 3 hours (AI), 45 min (Human)

**Description**:
Create documentation for developers to get started and understand the system.

**Tasks**:

#### AI Tasks
- [ ] Create `docs/development/SETUP.md`
- [ ] Create `docs/development/CONTRIBUTING.md`
- [ ] Create `docs/development/CODING-STANDARDS.md`
- [ ] Create `docs/development/TESTING-GUIDE.md`
- [ ] Create `docs/development/DATABASE-GUIDE.md`
- [ ] Update root README.md with overview
- [ ] Create architecture diagrams
- [ ] Document common tasks

#### Human Review Tasks
- [ ] Review documentation accuracy
- [ ] Test setup instructions
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] New developer can follow setup
- [ ] Coding standards clear
- [ ] Testing guide comprehensive
- [ ] Documentation up to date

---

### Issue #24: API Documentation Setup

**Title**: Configure Swagger/OpenAPI documentation

**Labels**: `api`, `documentation`, `phase-1-foundation`, `ai-task`, `priority-low`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 2 hours (AI), 30 min (Human)

**Description**:
Set up Swagger for API documentation (even though no endpoints yet).

**Tasks**:

#### AI Tasks
- [ ] Install Swashbuckle packages
- [ ] Configure Swagger UI
- [ ] Add API versioning support
- [ ] Configure JWT authentication in Swagger
- [ ] Add XML documentation generation
- [ ] Create sample controller for testing

#### Human Review Tasks
- [ ] Review Swagger UI
- [ ] Approve PR

**Acceptance Criteria**:
- [ ] Swagger UI accessible
- [ ] API versioning configured
- [ ] Authentication configured
- [ ] Documentation generated from XML comments

---

### Issue #25: Phase 1 Completion Report

**Title**: Generate Phase 1 completion report and retrospective

**Labels**: `documentation`, `phase-1-foundation`, `ai-task`, `priority-low`

**Milestone**: Phase 1 - Foundation

**Assignee**: AI

**Estimated**: 2 hours (AI), 1 hour (Human)

**Description**:
Create completion report for Phase 1 with metrics, lessons learned, and Phase 2 readiness.

**Tasks**:

#### AI Tasks
- [ ] Generate metrics report
  - Issues completed
  - Code coverage achieved
  - Lines of code written
  - Test count
  - Time spent
- [ ] Document lessons learned
- [ ] Create Phase 2 readiness checklist
- [ ] Identify technical debt
- [ ] Create recommendations for Phase 2

#### Human Tasks
- [ ] Review completion report
- [ ] Add human perspective
- [ ] Plan Phase 2 kickoff
- [ ] Archive Phase 1 documentation

**Acceptance Criteria**:
- [ ] Comprehensive metrics collected
- [ ] Lessons learned documented
- [ ] Phase 2 ready to start
- [ ] All Phase 1 issues closed

**Deliverables**:
- Phase 1 completion report
- Metrics dashboard
- Lessons learned document
- Phase 2 readiness assessment

---

## Summary of Phase 1 Issues

### By Week
- **Week 1 (Issues #1-5)**: Setup & Standards - 19 AI hours
- **Week 2 (Issues #6-10)**: Core Domain - 24 AI hours
- **Week 3 (Issues #11-16)**: Infrastructure - 24 AI hours
- **Week 4 (Issues #17-25)**: Testing & DevOps - 27 AI hours

### By Type
- Domain: 7 issues
- Infrastructure: 9 issues
- Testing: 4 issues
- Documentation: 3 issues
- DevOps: 2 issues

### By Priority
- Critical: 7 issues
- High: 9 issues
- Medium: 7 issues
- Low: 2 issues

### Total Estimates
- **AI Hours**: 94 hours
- **Human Hours**: 17 hours
- **Total Issues**: 25
- **Calendar Time**: 4 weeks

---

## How to Use These Issues

### Creating Issues in GitHub

Use the GitHub CLI to create all issues at once:

```bash
# Install GitHub CLI if not already installed
# brew install gh  # macOS
# winget install GitHub.cli  # Windows

# Authenticate
gh auth login

# Create all issues (script provided separately)
./scripts/create-phase-1-issues.sh
```

### Or Create Manually

1. Go to GitHub repository
2. Click "Issues" → "New Issue"
3. Select template (epic or technical-task)
4. Copy content from this document
5. Add appropriate labels
6. Assign to milestone "Phase 1 - Foundation"
7. Assign to AI or Human as appropriate

### Project Board Setup

Add all issues to project board with columns:
- 📋 Backlog
- 🎯 Ready
- 🏗️ In Progress
- 👀 Review
- ✅ Done

### Labels to Create

Run this script to create all necessary labels:

```bash
gh label create "phase-1-foundation" --color "0E8A16"
gh label create "ai-task" --color "1D76DB"
gh label create "human-review" --color "FBCA04"
gh label create "domain" --color "D93F0B"
gh label create "infrastructure" --color "5319E7"
gh label create "testing" --color "0075CA"
gh label create "documentation" --color "C5DEF5"
gh label create "devops" --color "000000"
gh label create "priority-critical" --color "B60205"
gh label create "priority-high" --color "D93F0B"
gh label create "priority-medium" --color "FBCA04"
gh label create "priority-low" --color "0E8A16"
```

---

## Next Steps

After completing Phase 1:
1. Generate completion report (#25)
2. Review metrics and lessons learned
3. Create Phase 2 issues
4. Kickoff Phase 2: Application Layer

**Phase 2 Preview**: CQRS Application Layer with MediatR, Commands, Queries, Validation, and AutoMapper.
