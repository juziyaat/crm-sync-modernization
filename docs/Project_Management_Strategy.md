# Project Management Strategy for CCA CRM Sync Modernization

## Executive Summary

**YES - We absolutely should create a comprehensive project plan before starting.**

Without proper project tracking, even with AI assistance, we risk:
- ❌ Losing track of what's been done
- ❌ Missing dependencies between tasks
- ❌ Unable to measure progress
- ❌ Difficulty onboarding new team members
- ❌ No audit trail for decisions

**Recommended Approach:** GitHub Projects + Issues (Free, integrated, perfect for our needs)

---

## Why GitHub Projects is Perfect for This Project

### ✅ Advantages Over JIRA

| Feature | GitHub Projects | JIRA | Winner |
|---------|----------------|------|---------|
| **Cost** | Free for unlimited users | Free tier limited (10 users) | GitHub |
| **Integration** | Native with code/PRs | Requires setup | GitHub |
| **AI-Friendly** | I can create/update issues via API | Complex API | GitHub |
| **Learning Curve** | Minimal | Steep | GitHub |
| **Version Control** | Same platform as code | Separate tool | GitHub |
| **Markdown Support** | Excellent | Limited | GitHub |
| **Automation** | GitHub Actions | Complex workflows | GitHub |
| **Simplicity** | Clean, focused | Feature overload | GitHub |

### 🎯 Why It's Ideal for AI-Human Collaboration

1. **I can directly interact with it**
   - Create issues via bash/API
   - Update status programmatically
   - Link commits to issues automatically
   - Comment on progress in real-time

2. **Everything in one place**
   - Code + Issues + Discussions + Docs
   - No context switching
   - Single source of truth

3. **Perfect granularity**
   - Epic → Issue → Task List → PR
   - Easy to track AI-generated vs human work

4. **Built-in code review**
   - Issues → Branch → PR → Review → Merge
   - Automated checks

---

## Proposed Project Structure

### Repository Setup

```
cca-crm-sync-modernization/
├── .github/
│   ├── ISSUE_TEMPLATE/
│   │   ├── epic.md
│   │   ├── user-story.md
│   │   ├── technical-task.md
│   │   └── bug.md
│   ├── workflows/
│   │   ├── ci.yml
│   │   ├── cd.yml
│   │   └── project-automation.yml
│   └── pull_request_template.md
├── docs/
│   ├── architecture/
│   ├── project-plan/
│   ├── adrs/              (Architecture Decision Records)
│   └── meeting-notes/
├── src/                   (Code goes here)
└── README.md
```

---

## Project Board Structure

### Board 1: **Modernization Roadmap** (High-Level)

**Columns:**
```
📋 Backlog → 🎯 Ready → 🏗️ In Progress → 👀 Review → ✅ Done
```

**Swimlanes/Labels:**
- `phase-1-foundation`
- `phase-2-domain`
- `phase-3-multi-tenancy`
- `phase-4-cqrs`
- `phase-5-api`
- `phase-6-jobs`
- `phase-7-migration`
- `phase-8-polish`

### Board 2: **Current Sprint** (Tactical)

**Columns:**
```
📝 Todo → 🤖 AI Working → 👤 Human Review → 🔄 Refinement → ✅ Completed
```

**This shows WHO is working on what:**
- AI (me) generates code
- Human reviews
- Iterate if needed
- Done when merged

---

## Hierarchical Task Breakdown

### Level 1: **Epics** (GitHub Milestones)

Example:
```
Milestone: Phase 1 - Foundation (Week 1-4)
Due Date: March 13, 2026
Issues: 25
Progress: 0/25
```

### Level 2: **User Stories / Features** (GitHub Issues)

Example:
```
Issue #1: Implement Customer Aggregate Root
Labels: phase-1-foundation, domain, ai-task
Milestone: Phase 1 - Foundation
Assigned: @claude-ai

Description:
As a developer, I need a rich Customer aggregate root 
so that we can enforce business rules at the domain level.

Acceptance Criteria:
- [ ] Customer class with private setters
- [ ] Create factory method with validation
- [ ] AddUtilityAccount method
- [ ] SyncToDynamics365 method
- [ ] Domain events for all state changes
- [ ] 80%+ test coverage

Technical Notes:
- Follow DDD patterns from analysis doc
- Use Result<T> for operation results
- Ensure immutability where appropriate

Estimated Effort: 4 hours
```

### Level 3: **Technical Tasks** (Issue Task Lists)

Inside Issue #1:
```markdown
## Implementation Tasks

### AI Tasks (Claude)
- [ ] Create `Domain/Aggregates/Customer/Customer.cs`
- [ ] Create `Domain/Aggregates/Customer/UtilityAccount.cs`
- [ ] Create `Domain/ValueObjects/CustomerName.cs`
- [ ] Create `Domain/ValueObjects/EmailAddress.cs`
- [ ] Create `Domain/Events/CustomerCreatedEvent.cs`
- [ ] Create `Domain/Events/UtilityAccountAddedEvent.cs`
- [ ] Generate comprehensive unit tests

### Human Review Tasks
- [ ] Review domain model for business accuracy
- [ ] Validate business rules
- [ ] Test with sample data
- [ ] Approve PR

### Definition of Done
- [ ] All unit tests passing
- [ ] Code coverage ≥ 80%
- [ ] Documentation comments complete
- [ ] PR approved by human reviewer
- [ ] Merged to main branch
```

---

## Complete Task Breakdown for Phase 1

Let me show you what the full breakdown would look like:

### **Milestone: Phase 1 - Foundation (Weeks 1-4)**

#### **Week 1: Setup & Standards**

**Epic Issue #1: Project Infrastructure Setup**
```markdown
## Tasks
- [ ] Create GitHub repository
- [ ] Set up branch protection rules
- [ ] Configure GitHub Projects boards
- [ ] Create issue templates
- [ ] Set up CI/CD pipeline (GitHub Actions)
- [ ] Create Docker Compose for local dev
- [ ] Write contributing guidelines
- [ ] Set up pre-commit hooks

Assigned: Human (30 min) + AI (2 hours)
```

**Epic Issue #2: Solution Structure**
```markdown
## Tasks
- [ ] Create solution file
- [ ] Create project structure (Domain, Application, Infrastructure, etc.)
- [ ] Set up Directory.Build.props
- [ ] Configure .editorconfig
- [ ] Add NuGet package references
- [ ] Create README for each project
- [ ] Set up test projects

Assigned: AI (3 hours)
```

**Epic Issue #3: Architecture Documentation**
```markdown
## Tasks
- [ ] Create C4 context diagram
- [ ] Create C4 container diagram
- [ ] Create C4 component diagrams
- [ ] Document folder structure
- [ ] Write ADR-001: Why Clean Architecture
- [ ] Write ADR-002: Why CQRS
- [ ] Write ADR-003: Why Multi-Tenant Design

Assigned: AI (4 hours)
```

#### **Week 2-3: Core Domain Models**

**Epic Issue #4: Customer Aggregate**
```markdown
## AI Tasks
- [ ] Domain/Aggregates/Customer/Customer.cs
- [ ] Domain/Aggregates/Customer/UtilityAccount.cs
- [ ] Domain/ValueObjects/CustomerName.cs
- [ ] Domain/ValueObjects/EmailAddress.cs
- [ ] Domain/ValueObjects/Address.cs
- [ ] Domain/Events/CustomerCreatedEvent.cs
- [ ] Domain/Events/UtilityAccountAddedEvent.cs
- [ ] Domain/Events/CustomerSyncedEvent.cs
- [ ] tests/Domain.Tests/CustomerTests.cs (20+ tests)
- [ ] tests/Domain.Tests/UtilityAccountTests.cs (15+ tests)

## Human Tasks
- [ ] Review domain model
- [ ] Validate business rules
- [ ] Approve PR

Estimated: AI 6h, Human 1h
```

**Epic Issue #5: LdcAccount Aggregate**
```markdown
[Similar structure to Issue #4]
```

**Epic Issue #6: SyncJob Aggregate**
```markdown
[Similar structure to Issue #4]
```

**Epic Issue #7: Shared Kernel**
```markdown
## Tasks
- [ ] Domain/Common/AggregateRoot.cs
- [ ] Domain/Common/Entity.cs
- [ ] Domain/Common/ValueObject.cs
- [ ] Domain/Common/DomainEvent.cs
- [ ] Domain/Common/Result.cs
- [ ] Domain/Common/IRepository.cs
- [ ] Domain/Common/IUnitOfWork.cs
- [ ] Domain/Specifications/Specification.cs
- [ ] tests for all above

Assigned: AI (4 hours)
```

#### **Week 4: Infrastructure Setup**

**Epic Issue #8: Database Context**
```markdown
## Tasks
- [ ] Infrastructure/Persistence/TenantDbContext.cs
- [ ] Infrastructure/Persistence/Configurations/CustomerConfiguration.cs
- [ ] Infrastructure/Persistence/Configurations/LdcAccountConfiguration.cs
- [ ] Infrastructure/Persistence/Configurations/SyncJobConfiguration.cs
- [ ] Infrastructure/Persistence/Migrations/InitialCreate.cs
- [ ] Infrastructure/Persistence/Interceptors/AuditInterceptor.cs
- [ ] Infrastructure/Persistence/Interceptors/TenantInterceptor.cs
- [ ] tests/Infrastructure.Tests/TenantDbContextTests.cs

Assigned: AI (6 hours)
```

**Epic Issue #9: Repository Implementation**
```markdown
## Tasks
- [ ] Infrastructure/Persistence/Repositories/Repository.cs
- [ ] Infrastructure/Persistence/Repositories/ReadRepository.cs
- [ ] Infrastructure/Persistence/UnitOfWork.cs
- [ ] Infrastructure/Persistence/Specifications/SpecificationEvaluator.cs
- [ ] tests for all repositories

Assigned: AI (4 hours)
```

**Epic Issue #10: POC Integration Test**
```markdown
## Tasks
- [ ] Create integration test with Testcontainers
- [ ] Test: Create customer → Add utility account → Save → Retrieve
- [ ] Test: Multi-tenant data isolation
- [ ] Test: Audit fields auto-populated
- [ ] Performance test: 1000 records batch insert

Assigned: AI (3h) + Human (1h)
```

---

## Issue Labeling System

### By Type
- `epic` - High-level feature or phase
- `user-story` - Business-facing feature
- `technical-task` - Implementation work
- `bug` - Something's broken
- `documentation` - Docs only
- `infrastructure` - CI/CD, DevOps
- `testing` - Test-related work

### By Phase
- `phase-1-foundation`
- `phase-2-domain`
- `phase-3-multi-tenancy`
- `phase-4-cqrs`
- `phase-5-api`
- `phase-6-jobs`
- `phase-7-migration`
- `phase-8-polish`

### By Priority
- `priority-critical` - Blocking work
- `priority-high` - Important
- `priority-medium` - Normal
- `priority-low` - Nice to have

### By Who Does It
- `ai-task` - I (Claude) will do this
- `human-review` - Human must review
- `human-task` - Human must do
- `pair-programming` - We work together

### By Technology
- `domain` - Domain layer
- `application` - Application layer
- `infrastructure` - Infrastructure layer
- `api` - API work
- `database` - Database work
- `testing` - Test work

### By Status
- `blocked` - Can't proceed
- `needs-discussion` - Decision needed
- `ready-for-review` - AI done, human review needed
- `in-progress` - Currently being worked

---

## Workflow: How We'll Use This System

### Daily Workflow

#### Morning (Start of Work Session)

**Human:**
```bash
# Check the Current Sprint board
# Pick highest priority ready task
# Or review AI-completed work
```

**Me (Claude):**
```bash
# Check for assigned issues
# Read task details
# Generate code
# Create PR
# Move issue to "Human Review" column
# Comment on issue with progress
```

#### During Work

**For Each Task:**

1. **AI (Me) Creates Implementation PR**
   ```
   PR #23: Implement Customer Aggregate Root
   
   Closes #1
   
   Changes:
   - Added Customer.cs with factory method
   - Added UtilityAccount.cs entity
   - Added CustomerName value object
   - Added 25 unit tests
   - Code coverage: 87%
   
   @human-reviewer Please review:
   - Business rule validation in AddUtilityAccount
   - Domain event raising logic
   - Test coverage adequacy
   ```

2. **Human Reviews PR**
   ```
   Review Comments:
   - ✅ Code structure looks good
   - ⚠️ AddUtilityAccount should validate meter number format
   - ⚠️ Need test for duplicate account number across providers
   - ✅ Domain events correctly implemented
   
   Changes Requested
   ```

3. **AI (Me) Addresses Feedback**
   ```
   Commit: Add meter number validation and test
   
   - Added regex validation for meter number
   - Added test: AddUtilityAccount_DuplicateAcrossProviders_ShouldSucceed
   - Updated documentation
   
   @human-reviewer Ready for re-review
   ```

4. **Human Approves & Merges**
   ```
   ✅ LGTM - Merging
   
   [Issue #1 automatically closed]
   [Project board automatically updated to Done]
   ```

---

## GitHub Actions Automation

We'll automate project management with workflows:

### `.github/workflows/project-automation.yml`

```yaml
name: Project Automation

on:
  issues:
    types: [opened, closed, reopened]
  pull_request:
    types: [opened, closed, review_requested]

jobs:
  auto-assign:
    runs-on: ubuntu-latest
    steps:
      - name: Auto-assign to project
        uses: actions/add-to-project@v0.4.0
        with:
          project-url: https://github.com/users/YOUR_ORG/projects/1
          github-token: ${{ secrets.GITHUB_TOKEN }}
      
      - name: Label AI tasks
        if: contains(github.event.issue.body, 'AI Task')
        uses: actions/github-script@v6
        with:
          script: |
            github.rest.issues.addLabels({
              issue_number: context.issue.number,
              owner: context.repo.owner,
              repo: context.repo.repo,
              labels: ['ai-task']
            })
      
      - name: Move to In Progress when PR created
        if: github.event_name == 'pull_request' && github.event.action == 'opened'
        uses: actions/github-script@v6
        with:
          script: |
            // Move linked issue to "In Progress" column
            // Add implementation details
      
      - name: Move to Review when PR ready
        if: github.event_name == 'pull_request' && github.event.action == 'review_requested'
        uses: actions/github-script@v6
        with:
          script: |
            // Move to "Human Review" column
            // Notify reviewer
      
      - name: Close issue when PR merged
        if: github.event_name == 'pull_request' && github.event.action == 'closed' && github.event.pull_request.merged == true
        uses: actions/github-script@v6
        with:
          script: |
            // Close linked issues
            // Update project board
            // Generate completion metrics
```

### `.github/workflows/progress-report.yml`

```yaml
name: Daily Progress Report

on:
  schedule:
    - cron: '0 17 * * *'  # 5 PM daily

jobs:
  generate-report:
    runs-on: ubuntu-latest
    steps:
      - name: Generate Progress Report
        uses: actions/github-script@v6
        with:
          script: |
            const { data: issues } = await github.rest.issues.listForRepo({
              owner: context.repo.owner,
              repo: context.repo.repo,
              state: 'all',
              since: new Date(Date.now() - 24*60*60*1000).toISOString()
            });
            
            const completed = issues.filter(i => i.state === 'closed');
            const created = issues.filter(i => i.created_at > new Date(Date.now() - 24*60*60*1000));
            
            const report = `
            # Daily Progress Report - ${new Date().toDateString()}
            
            ## Summary
            - Issues Created: ${created.length}
            - Issues Completed: ${completed.length}
            - Issues In Progress: ${issues.filter(i => i.state === 'open').length}
            
            ## Completed Today
            ${completed.map(i => `- #${i.number}: ${i.title}`).join('\n')}
            
            ## Created Today
            ${created.map(i => `- #${i.number}: ${i.title}`).join('\n')}
            `;
            
            // Post to GitHub Discussions or send email
```

---

## How I Will Interact With Issues

### Creating Issues Programmatically

When you ask me to start work, I can:

```bash
# Create an issue via GitHub CLI
gh issue create \
  --title "Implement Customer Aggregate Root" \
  --body "$(cat issue-template.md)" \
  --label "ai-task,phase-1-foundation,domain" \
  --milestone "Phase 1" \
  --project "Modernization Roadmap"

# Or via API
curl -X POST https://api.github.com/repos/YOUR_ORG/cca-sync/issues \
  -H "Authorization: token $GITHUB_TOKEN" \
  -d '{
    "title": "Implement Customer Aggregate",
    "body": "Full description...",
    "labels": ["ai-task", "domain"],
    "milestone": 1
  }'
```

### Updating Progress

As I work:

```bash
# Comment on issue
gh issue comment 123 --body "✅ Created Customer.cs with 5 methods and 20 tests"

# Add to task list
gh issue edit 123 --body "$(sed 's/- \[ \] Create Customer.cs/- [x] Create Customer.cs/' issue.md)"

# Create PR linked to issue
gh pr create \
  --title "Implement Customer Aggregate Root" \
  --body "Closes #123" \
  --label "ai-implementation"
```

---

## Master Project Plan: Complete Task List

### Phase 1: Foundation (Weeks 1-4)

```markdown
Milestone: Phase 1 - Foundation
Due: March 13, 2026

Issues:
1. ✅ Project Infrastructure Setup [2h AI]
2. ✅ Solution Structure [3h AI]
3. ✅ Architecture Documentation [4h AI]
4. ⏳ Customer Aggregate [6h AI, 1h Human]
5. ⏳ LdcAccount Aggregate [6h AI, 1h Human]
6. ⏳ SyncJob Aggregate [4h AI, 1h Human]
7. ⏳ MonthlyUsage Aggregate [4h AI, 1h Human]
8. ⏳ InvoiceHistory Aggregate [4h AI, 1h Human]
9. ⏳ Shared Kernel [4h AI]
10. ⏳ Database Context [6h AI, 1h Human]
11. ⏳ Repository Implementation [4h AI]
12. ⏳ Integration Tests [3h AI, 1h Human]
13. ⏳ Docker Compose Setup [2h AI]
14. ⏳ CI/CD Pipeline [3h AI, 1h Human]
15. ⏳ Phase 1 Documentation [2h AI]

Total Estimated Effort: 
- AI: 57 hours
- Human: 8 hours
- Calendar Time: 4 weeks (with iterations)
```

### Phase 2: Application Layer (Weeks 5-12)

```markdown
Milestone: Phase 2 - CQRS Application Layer
Due: May 8, 2026

Issues:
16. ⏳ MediatR Setup & Pipelines [4h AI]
17. ⏳ Customer Commands [8h AI, 2h Human]
18. ⏳ Customer Queries [6h AI, 1h Human]
19. ⏳ LdcAccount Commands [8h AI, 2h Human]
20. ⏳ LdcAccount Queries [6h AI, 1h Human]
21. ⏳ SyncJob Commands [6h AI, 1h Human]
22. ⏳ SyncJob Queries [4h AI, 1h Human]
23. ⏳ MonthlyUsage Commands [6h AI, 1h Human]
24. ⏳ MonthlyUsage Queries [4h AI, 1h Human]
25. ⏳ InvoiceHistory Commands [6h AI, 1h Human]
26. ⏳ InvoiceHistory Queries [4h AI, 1h Human]
27. ⏳ FluentValidation Setup [4h AI]
28. ⏳ AutoMapper Profiles [6h AI, 1h Human]
29. ⏳ Application DTOs [8h AI]
30. ⏳ Validation Behavior [3h AI]
31. ⏳ Logging Behavior [3h AI]
32. ⏳ Transaction Behavior [4h AI]
33. ⏳ Exception Handling [4h AI]
34. ⏳ Application Tests [20h AI, 4h Human]
35. ⏳ Phase 2 Documentation [3h AI]

Total Estimated Effort:
- AI: 107 hours
- Human: 16 hours
- Calendar Time: 8 weeks
```

I can continue with all 8 phases if helpful, but you get the idea.

---

## Complete Epic/Issue Breakdown - First 2 Weeks

Let me show you the FIRST TWO WEEKS in complete detail:

### Week 1, Day 1-2: Setup

#### Issue #1: Project Infrastructure
```markdown
Title: Set up GitHub repository and project boards
Labels: infrastructure, phase-1-foundation, ai-task
Milestone: Phase 1 - Foundation
Assignee: @claude-ai
Estimated: 2 hours

## Description
Initialize the GitHub repository with proper structure and project management tools.

## Tasks
- [ ] Create GitHub repository
- [ ] Set up branch protection (require PR reviews, CI passing)
- [ ] Create GitHub Projects board: "Modernization Roadmap"
- [ ] Create GitHub Projects board: "Current Sprint"
- [ ] Configure project board columns
- [ ] Add issue templates (epic, user-story, technical-task, bug)
- [ ] Add PR template
- [ ] Configure GitHub Actions secrets
- [ ] Set up webhooks for automation

## Acceptance Criteria
- [ ] Repository accessible to team
- [ ] Branch protection enabled on main
- [ ] Both project boards created and configured
- [ ] Templates ready for use
- [ ] Documentation in README

## Human Tasks
- [ ] Review and approve repository settings
- [ ] Add team members with appropriate permissions
```

#### Issue #2: Solution Structure
```markdown
Title: Create .NET solution structure following Clean Architecture
Labels: infrastructure, phase-1-foundation, ai-task
Milestone: Phase 1 - Foundation
Assignee: @claude-ai
Estimated: 3 hours

## Description
Set up the complete .NET 9 solution with proper project structure.

## Tasks

### AI Tasks (Claude)
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
- [ ] Add `Directory.Build.props` with common settings
- [ ] Add `Directory.Packages.props` for central package management
- [ ] Add `.editorconfig` with C# conventions
- [ ] Add `.gitignore` for .NET
- [ ] Add `README.md` for each project
- [ ] Set up project references (Domain → Application → Infrastructure)
- [ ] Add NuGet packages to each project

### Human Review Tasks
- [ ] Verify project structure matches architecture
- [ ] Check that dependencies flow correctly
- [ ] Approve PR

## Deliverables
- Complete solution structure
- All projects building successfully
- README documentation
- Proper layering enforced

## Acceptance Criteria
- [ ] Solution builds with `dotnet build`
- [ ] No circular dependencies
- [ ] Domain project has no external dependencies
- [ ] Application depends only on Domain
- [ ] Infrastructure depends on Application and Domain
- [ ] Test projects can reference what they test

## Technical Notes
- Target .NET 9.0
- Enable nullable reference types
- Use C# 13 features
- ImplicitUsings enabled
- TreatWarningsAsErrors true
```

### Week 1, Day 3-5: Domain Models

#### Issue #4: Customer Aggregate Root
```markdown
Title: Implement Customer aggregate root following DDD patterns
Labels: domain, phase-1-foundation, ai-task, priority-high
Milestone: Phase 1 - Foundation
Assignee: @claude-ai
Estimated: 6 hours (AI), 1 hour (Human Review)

## Description
Create the Customer aggregate root as the core domain entity, following Domain-Driven Design principles.

## Business Context
A Customer represents a utility customer enrolled with a CCA. They can have multiple utility accounts across different providers (PG&E, SCE, SDG&E). The aggregate must enforce:
- Customer can only be created with valid name and email
- Utility accounts cannot have duplicate account numbers
- Sync status must be tracked
- All state changes must raise domain events

## Tasks

### AI Implementation Tasks
- [ ] Create `Domain/Aggregates/Customer/Customer.cs`
  - Private setters for encapsulation
  - Factory method `Create(...)` with validation
  - Method `AddUtilityAccount(...)`
  - Method `UpdateEmail(...)`
  - Method `SyncToDynamics365(...)`
  - Method `MarkSyncFailed(...)`
  - Domain events for all state changes
  
- [ ] Create `Domain/Aggregates/Customer/UtilityAccount.cs`
  - Entity with value equality by AccountNumber
  - Private constructor
  - Factory method with validation
  - Support for PG&E, SCE, SDG&E providers
  
- [ ] Create `Domain/ValueObjects/CustomerName.cs`
  - Immutable record
  - FirstName, LastName, MiddleName
  - FullName computed property
  - Validation in factory method
  
- [ ] Create `Domain/ValueObjects/EmailAddress.cs`
  - Immutable record
  - Regex validation
  - Factory method with Result<T>
  
- [ ] Create `Domain/ValueObjects/Address.cs`
  - Street, City, State, ZipCode
  - Validation for US addresses
  
- [ ] Create `Domain/ValueObjects/PhoneNumber.cs`
  - US phone number validation
  - Formatting
  
- [ ] Create `Domain/Enums/UtilityProvider.cs`
  - PGE, SCE, SDGE
  
- [ ] Create `Domain/Enums/SyncStatus.cs`
  - Pending, InProgress, Synced, Failed
  
- [ ] Create `Domain/Events/CustomerCreatedEvent.cs`
- [ ] Create `Domain/Events/UtilityAccountAddedEvent.cs`
- [ ] Create `Domain/Events/CustomerSyncedEvent.cs`
- [ ] Create `Domain/Events/CustomerSyncFailedEvent.cs`

### AI Testing Tasks
- [ ] Create `tests/Domain.Tests/Aggregates/CustomerTests.cs`
  - Test: Create_ValidData_ReturnsSuccess
  - Test: Create_InvalidEmail_ReturnsFailure
  - Test: Create_EmptyName_ReturnsFailure
  - Test: AddUtilityAccount_ValidAccount_AddsSuccessfully
  - Test: AddUtilityAccount_DuplicateAccountNumber_Fails
  - Test: AddUtilityAccount_NullAccountNumber_Fails
  - Test: AddUtilityAccount_RaisesDomainEvent
  - Test: SyncToDynamics365_ValidId_UpdatesStatus
  - Test: SyncToDynamics365_WhenAlreadySynced_Fails
  - Test: SyncToDynamics365_RaisesDomainEvent
  - Test: MarkSyncFailed_SetsStatusToFailed
  - Test: UpdateEmail_ValidEmail_UpdatesSuccessfully
  - Test: UpdateEmail_InvalidEmail_Fails
  - Test: UtilityAccountCollection_IsReadOnly
  - Test: CustomerWithMultipleAccounts_MaintainsAllAccounts

- [ ] Create `tests/Domain.Tests/ValueObjects/EmailAddressTests.cs`
  - Test: Create_ValidEmail_ReturnsSuccess
  - Test: Create_InvalidFormat_ReturnsFailure
  - Test: Create_EmptyEmail_ReturnsFailure
  - Test: Equality_SameEmail_AreEqual
  
- [ ] Create `tests/Domain.Tests/ValueObjects/CustomerNameTests.cs`
  - Test: FullName_CombinesCorrectly
  - Test: Equality_SameValues_AreEqual

### Human Review Tasks
- [ ] Review Customer business logic
  - Does AddUtilityAccount enforce correct rules?
  - Are there missing business validations?
  - Do events capture all state changes?
- [ ] Review test coverage
  - Are edge cases covered?
  - Are error messages clear?
- [ ] Test with sample data
  - Create customer with real CCA data
  - Try invalid inputs
- [ ] Approve PR

## Acceptance Criteria
- [ ] All unit tests passing
- [ ] Code coverage ≥ 85%
- [ ] No public setters on aggregate
- [ ] All state changes raise domain events
- [ ] Factory methods enforce validation
- [ ] XML documentation on all public methods
- [ ] No dependencies on other layers
- [ ] Follows SOLID principles
- [ ] Human reviewer approves

## Technical Notes
- Use `Result<T>` pattern for operations that can fail
- Domain events should be in `Domain/Events/` folder
- Follow naming convention: `{Entity}{Action}Event`
- Value objects should be immutable records
- Use `private set` for all properties that change
- No EF attributes in domain layer
- No JSON attributes in domain layer

## Definition of Done
- [ ] All AI tasks completed
- [ ] PR created and linked to this issue
- [ ] All tests green
- [ ] Code coverage report generated
- [ ] Human review completed
- [ ] Feedback addressed
- [ ] PR merged to main
- [ ] Issue automatically closed
- [ ] Project board updated to "Done"

## Related Issues
- Blocks: #5 (LdcAccount Aggregate)
- Blocks: #10 (Database Context)
- Related: #3 (Architecture Documentation)
```

---

## Metrics & Reporting

### Automated Reports

#### Daily Progress Report
```markdown
# Daily Progress - February 13, 2026

## Velocity
- Issues Completed: 3
- Issues Created: 5
- Story Points Burned: 21
- AI Hours: 12
- Human Hours: 2

## Status by Phase
- Phase 1: 15/25 (60%)
- Phase 2: 0/20 (0%)

## Completed Today
- ✅ #1: Project Infrastructure Setup
- ✅ #2: Solution Structure
- ✅ #3: Architecture Documentation

## In Progress
- 🏗️ #4: Customer Aggregate (AI Working)
- 🏗️ #5: LdcAccount Aggregate (Ready)

## Blocked
- None

## Needs Review
- 👀 #4: Waiting for human review

## Risks
- None identified
```

#### Weekly Summary
```markdown
# Weekly Summary - Week 1

## Highlights
- ✅ Complete project setup done
- ✅ Solution structure in place
- ✅ First aggregate (Customer) implemented
- 📈 Code coverage: 87%

## Metrics
- Issues Closed: 10
- PRs Merged: 10
- Tests Added: 156
- Lines of Code: 2,847
- AI Hours: 42
- Human Hours: 8

## Progress
- Phase 1: 40% complete (10/25 issues)
- On track for Week 4 milestone

## Next Week
- Complete remaining domain aggregates
- Set up database infrastructure
- Begin integration tests

## Risks & Issues
- None blocking
```

### Burndown Chart

GitHub Projects will automatically show:
- Issues remaining by milestone
- Velocity trend
- Projected completion date

---

## Setup Script

I can create this for you right now:

```bash
#!/bin/bash
# setup-project-management.sh

echo "🚀 Setting up CCA CRM Sync Modernization Project Management"

# 1. Create repository structure
echo "📁 Creating repository structure..."
mkdir -p .github/{ISSUE_TEMPLATE,workflows}
mkdir -p docs/{architecture,project-plan,adrs,meeting-notes}
mkdir -p src tests

# 2. Create issue templates
echo "📝 Creating issue templates..."

cat > .github/ISSUE_TEMPLATE/epic.md <<'EOF'
---
name: Epic
about: High-level feature or project phase
title: '[EPIC] '
labels: epic
assignees: ''
---

## Epic Description
Brief description of this epic and its business value.

## Goals
- Goal 1
- Goal 2

## User Stories
List of user stories that comprise this epic:
- #issue-number: Story title

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2

## Technical Notes
Any technical considerations.

## Estimated Effort
- AI Hours: XX
- Human Hours: XX
- Calendar Time: XX weeks
EOF

cat > .github/ISSUE_TEMPLATE/technical-task.md <<'EOF'
---
name: Technical Task
about: Implementation task
title: ''
labels: technical-task
assignees: ''
---

## Description
What needs to be done.

## Tasks

### AI Tasks
- [ ] Task 1
- [ ] Task 2

### Human Review Tasks
- [ ] Review item 1
- [ ] Review item 2

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2

## Definition of Done
- [ ] All tests passing
- [ ] Code coverage ≥ 80%
- [ ] Documentation complete
- [ ] PR approved and merged

## Technical Notes
Implementation details.

## Estimated Effort
AI: Xh, Human: Xh
EOF

# 3. Create PR template
cat > .github/pull_request_template.md <<'EOF'
## Description
Closes #issue-number

Brief description of changes.

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Changes Made
- Change 1
- Change 2

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] All tests passing locally
- [ ] Code coverage maintained/improved

## Review Checklist
- [ ] Code follows project conventions
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No new warnings

## Human Reviewer: Please Check
- [ ] Business logic is correct
- [ ] Edge cases handled
- [ ] Error messages are clear
- [ ] Tests are comprehensive

## Screenshots (if applicable)
EOF

# 4. Create GitHub Actions workflow
cat > .github/workflows/project-automation.yml <<'EOF'
name: Project Automation

on:
  issues:
    types: [opened, labeled, closed]
  pull_request:
    types: [opened, ready_for_review, closed]

jobs:
  manage-project:
    runs-on: ubuntu-latest
    steps:
      - name: Add to project
        uses: actions/add-to-project@v0.4.0
        with:
          project-url: ${{ vars.PROJECT_URL }}
          github-token: ${{ secrets.GITHUB_TOKEN }}
      
      - name: Label AI tasks
        if: contains(github.event.issue.title, 'AI') || contains(github.event.issue.labels.*.name, 'ai-task')
        uses: actions/github-script@v6
        with:
          script: |
            await github.rest.issues.addLabels({
              issue_number: context.issue.number,
              owner: context.repo.owner,
              repo: context.repo.repo,
              labels: ['ai-task']
            });
EOF

echo "✅ Project management setup complete!"
echo ""
echo "Next steps:"
echo "1. Create GitHub repository"
echo "2. Push these files"
echo "3. Create GitHub Projects boards"
echo "4. Start creating issues"
```

---

## My Recommendation

**Start with GitHub Projects - Here's why:**

1. **Free & Unlimited** - No cost, no restrictions
2. **Integrated** - Same platform as code
3. **AI-Friendly** - I can create/update issues programmatically
4. **Simple** - Learn in 30 minutes vs. days for JIRA
5. **Flexible** - Start simple, add complexity as needed
6. **Professional** - Used by Microsoft, Google, etc.

**JIRA is overkill** for a 1-2 person team + AI agents.

---

## Next Steps

### Option 1: I Set Everything Up (Recommended)
**Time: 2 hours**

Give me:
- GitHub repository name
- Your GitHub username

I will:
1. Create complete issue templates
2. Generate all Phase 1 issues (25 issues)
3. Set up project boards
4. Create automation workflows
5. Generate first week's detailed tasks
6. Give you a working system

### Option 2: We Do It Together
**Time: 4 hours**

We pair program:
- You create the repo
- I guide you through setup
- We create first 5 issues together
- You understand the system deeply

### Option 3: You Do It With My Templates
**Time: 6 hours**

I give you:
- All templates
- Setup script
- Documentation

You:
- Run the setup
- Create issues manually
- Learn GitHub Projects

---

## The Bottom Line

**YES - We absolutely need project management before starting.**

**Recommended: GitHub Projects**
- Free, integrated, AI-friendly
- I can interact with it programmatically
- Perfect for AI-human collaboration

**I can set it up in 2 hours** with:
- Complete issue templates
- 25+ Phase 1 issues ready to go
- Project boards configured
- Automation in place

**Then we start coding with:**
- Clear task breakdown
- Progress tracking
- Accountability
- No confusion about what's next

**Want me to create it now?** Just give me:
1. Repository name
2. Your GitHub username
3. Organization (if any)

I'll generate everything and you'll have a production-ready project management system.
