# CCA CRM Sync Modernization

> Modernizing the CCA CRM synchronization system using Clean Architecture, DDD, and .NET 9

[![CI](https://github.com/YOUR_USERNAME/crm-sync-modernization/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/crm-sync-modernization/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## 📋 Project Overview

This project modernizes the legacy CCA (Community Choice Aggregation) CRM synchronization system by implementing:

- **Clean Architecture** - Separation of concerns with clear dependencies
- **Domain-Driven Design (DDD)** - Rich domain models with business logic
- **CQRS Pattern** - Separate read and write models for scalability
- **Multi-Tenancy** - Database-per-tenant isolation
- **Event-Driven Architecture** - Domain events for decoupled communication
- **Modern .NET Stack** - .NET 9, C# 13, EF Core, MediatR

### Business Context

The system synchronizes customer, account, usage, and invoice data between:
- **LDC Systems** (PG&E, SCE, SDG&E) - Utility provider APIs
- **Dynamics 365** - CRM system
- **Internal Database** - Multi-tenant PostgreSQL

---

## 🏗️ Architecture

### Clean Architecture Layers

```
┌─────────────────────────────────────────┐
│          Presentation Layer             │
│    (WebAPI, Worker Services, CLI)       │
├─────────────────────────────────────────┤
│        Application Layer (CQRS)         │
│   (Commands, Queries, DTOs, MediatR)    │
├─────────────────────────────────────────┤
│           Domain Layer (DDD)            │
│  (Aggregates, Entities, Value Objects,  │
│   Domain Events, Business Rules)        │
├─────────────────────────────────────────┤
│        Infrastructure Layer             │
│  (EF Core, Repositories, External APIs, │
│   Persistence, Messaging, Logging)      │
└─────────────────────────────────────────┘
```

### Key Design Patterns

- **Repository Pattern** - Data access abstraction
- **Specification Pattern** - Reusable query logic
- **Result Pattern** - Explicit success/failure handling
- **Factory Pattern** - Controlled aggregate creation
- **Unit of Work** - Transaction management
- **Mediator Pattern** - Request/response with MediatR
- **Strategy Pattern** - Multi-provider sync strategies

---

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [PostgreSQL 16](https://www.postgresql.org/download/) (or use Docker)
- [GitHub CLI](https://cli.github.com/) (for project management)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

### 1. Clone Repository

```bash
git clone https://github.com/YOUR_USERNAME/crm-sync-modernization.git
cd crm-sync-modernization
```

### 2. Setup Project Management

Follow the [Quick Start Guide](docs/project-plan/Quick-Start-Guide.md) to set up GitHub issues, labels, and project boards.

**TL;DR**:
```bash
# Windows
.\scripts\setup-github-labels.ps1

# Linux/Mac
chmod +x scripts/setup-github-labels.sh
./scripts/setup-github-labels.sh
```

### 3. Build and Run (Coming in Phase 1)

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Start with Docker
docker-compose up
```

---

## 📁 Project Structure

```
crm-sync-modernization/
├── .github/
│   ├── ISSUE_TEMPLATE/          # Issue templates (epic, task, bug, user-story)
│   ├── workflows/               # GitHub Actions (CI, automation, reports)
│   └── pull_request_template.md # PR template
├── docs/
│   ├── architecture/            # Architecture diagrams and docs
│   ├── project-plan/            # Project planning and issues
│   │   ├── Phase-1-Issues.md   # All 25 Phase 1 issues detailed
│   │   ├── GitHub-Labels.md    # Label documentation
│   │   └── Quick-Start-Guide.md # Setup guide
│   ├── adrs/                    # Architecture Decision Records
│   └── meeting-notes/           # Meeting notes
├── src/
│   ├── CCA.Sync.Domain/         # Domain layer (aggregates, entities, value objects)
│   ├── CCA.Sync.Application/    # Application layer (CQRS, handlers, DTOs)
│   ├── CCA.Sync.Infrastructure/ # Infrastructure (EF Core, repositories, APIs)
│   ├── CCA.Sync.WebApi/         # REST API
│   ├── CCA.Sync.Worker/         # Background jobs
│   └── CCA.Sync.Shared/         # Shared kernel
├── tests/
│   ├── Domain.Tests/            # Domain unit tests
│   ├── Application.Tests/       # Application unit tests
│   ├── Infrastructure.Tests/    # Infrastructure unit tests
│   └── Integration.Tests/       # End-to-end integration tests
├── scripts/
│   ├── setup-github-labels.sh   # Label setup (Bash)
│   └── setup-github-labels.ps1  # Label setup (PowerShell)
└── README.md                    # This file
```

---

## 📊 Project Phases

### Phase 1: Foundation (Weeks 1-4) - 🏗️ In Progress

**Goal**: Establish domain layer, infrastructure, and testing foundation

- [x] Project setup and templates
- [ ] Domain model implementation
- [ ] Database infrastructure
- [ ] Repository pattern
- [ ] Integration tests
- [ ] CI/CD pipeline

**Issues**: 25 | **Est. Time**: AI 75h, Human 12h | **See**: [Phase-1-Issues.md](docs/project-plan/Phase-1-Issues.md)

### Phase 2: Application Layer (Weeks 5-12)

- CQRS with MediatR
- Commands and Queries
- Validation with FluentValidation
- AutoMapper profiles
- Application services

### Phase 3-8: Future Phases

See [Project Management Strategy](docs/Project_Management_Strategy.md) for complete roadmap.

---

## 🧪 Testing Strategy

### Test Pyramid

```
        /\
       /  \  E2E Tests (10%)
      /────\
     /      \  Integration Tests (20%)
    /────────\
   /          \  Unit Tests (70%)
  /────────────\
```

**Coverage Target**: ≥ 80% line coverage

### Test Types

- **Unit Tests**: Domain logic, business rules, value objects
- **Integration Tests**: Database, repositories, EF Core configurations
- **End-to-End Tests**: Full stack, API to database
- **Contract Tests**: External API integrations

### Running Tests

```bash
# All tests
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"

# Specific project
dotnet test tests/Domain.Tests

# Integration tests only
dotnet test --filter Category=Integration
```

---

## 🤖 AI-Human Collaboration

This project uses a unique **AI-Human pair programming** approach:

### Workflow

1. **Human**: Creates issue with requirements
2. **AI (Claude)**: Implements code and tests
3. **AI**: Creates PR with detailed description
4. **Human**: Reviews code and provides feedback
5. **AI**: Addresses feedback
6. **Human**: Approves and merges
7. **Automation**: Closes issue, updates board

### Labels

Issues are tagged with who does the work:
- `ai-task` - AI implements
- `human-review` - Human reviews
- `human-task` - Human implements
- `pair-programming` - Collaborative

### Benefits

- **Faster development** - AI generates boilerplate and tests
- **Better quality** - Human review ensures correctness
- **Knowledge transfer** - Both learn from each other
- **Comprehensive tests** - AI writes extensive test suites
- **Documentation** - AI documents as it codes

---

## 📖 Documentation

### Getting Started

- [Quick Start Guide](docs/project-plan/Quick-Start-Guide.md) - Setup in 10 minutes
- [Project Management Strategy](docs/Project_Management_Strategy.md) - Complete strategy
- [Phase 1 Issues](docs/project-plan/Phase-1-Issues.md) - Detailed issue breakdown

### Development

- [Contributing Guidelines](docs/development/CONTRIBUTING.md) _(Coming soon)_
- [Coding Standards](docs/development/CODING-STANDARDS.md) _(Coming soon)_
- [Testing Guide](docs/development/TESTING-GUIDE.md) _(Coming soon)_

### Architecture

- [Architecture Overview](docs/architecture/README.md) _(Coming in Phase 1)_
- [C4 Diagrams](docs/architecture/) _(Coming in Phase 1)_
- [ADRs](docs/adrs/) _(Coming in Phase 1)_

---

## 🛠️ Tech Stack

### Backend

- **.NET 9** - Latest .NET framework
- **C# 13** - Latest C# language features
- **EF Core 9** - ORM for database access
- **MediatR** - CQRS mediator pattern
- **FluentValidation** - Input validation
- **AutoMapper** - Object mapping
- **Serilog** - Structured logging

### Database

- **PostgreSQL 16** - Primary database
- **Entity Framework Core** - ORM
- **Npgsql** - PostgreSQL provider

### Testing

- **xUnit** - Test framework
- **FluentAssertions** - Assertion library
- **NSubstitute** - Mocking framework
- **Testcontainers** - Integration test containers
- **Coverlet** - Code coverage

### DevOps

- **Docker** - Containerization
- **GitHub Actions** - CI/CD
- **GitHub Projects** - Project management

---

## 📈 Project Metrics

### Current Status (Phase 1)

- **Issues Closed**: 0/25
- **PRs Merged**: 0
- **Code Coverage**: TBD
- **Lines of Code**: TBD
- **Tests Written**: TBD

_Metrics update automatically via GitHub Actions_

---

## 🤝 Contributing

We welcome contributions! Please see:

1. [Quick Start Guide](docs/project-plan/Quick-Start-Guide.md) - Get set up
2. [Phase 1 Issues](docs/project-plan/Phase-1-Issues.md) - Find tasks
3. [GitHub Labels](docs/project-plan/GitHub-Labels.md) - Understand labels

### Creating Issues

Always use issue templates:
- **Epic** - Large features or phases
- **User Story** - Business features
- **Technical Task** - Implementation work
- **Bug** - Defects

### Pull Requests

- Use PR template (auto-loaded)
- Link to issue: `Closes #123`
- Add tests (required)
- Ensure CI passes
- Request review from human

---

## 📅 Roadmap

### ✅ Completed

- [x] Project setup and infrastructure
- [x] Issue templates
- [x] GitHub Actions workflows
- [x] Project management system
- [x] Documentation structure

### 🏗️ In Progress (Phase 1: Foundation)

- [ ] Domain model implementation
- [ ] Database infrastructure
- [ ] Repository pattern
- [ ] Integration tests
- [ ] CI/CD pipeline

### 📋 Upcoming

- **Phase 2**: Application Layer (CQRS)
- **Phase 3**: Multi-Tenancy
- **Phase 4**: REST API
- **Phase 5**: Background Jobs
- **Phase 6**: Data Migration
- **Phase 7**: Production Deployment

See [Project Management Strategy](docs/Project_Management_Strategy.md) for full roadmap.

---

## 🐛 Bug Reports

Found a bug? Please create an issue using the **Bug Report** template.

Include:
- Steps to reproduce
- Expected behavior
- Actual behavior
- Environment details

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Team

- **Human Developer**: Project owner, code reviewer, domain expert
- **AI (Claude)**: Code generator, test writer, documentation

---

## 📞 Support

- **Issues**: Use GitHub Issues for bugs and features
- **Discussions**: Use GitHub Discussions for questions
- **Documentation**: Check [docs/](docs/) first

---

## 🎯 Goals

### Technical Goals

1. **Clean Architecture** - Maintainable, testable code
2. **80%+ Coverage** - Comprehensive test suite
3. **DDD Principles** - Rich domain model
4. **CQRS** - Scalable read/write separation
5. **Multi-Tenancy** - Secure data isolation

### Business Goals

1. **Reliable Sync** - 99.9% sync success rate
2. **Performance** - <5min sync cycles
3. **Scalability** - Support 100+ CCAs
4. **Maintainability** - Easy to modify and extend
5. **Observability** - Complete logging and monitoring

---

## 🌟 Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- Enterprise Patterns by Martin Fowler
- .NET Community for excellent tooling and libraries

---

**Built with ❤️ using .NET 9, Clean Architecture, and AI-Human Collaboration**

_Last Updated: 2026-02-12_
