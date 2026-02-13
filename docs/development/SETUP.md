# Development Setup Guide

Complete guide for setting up the CCA CRM Sync Modernization development environment.

## Prerequisites

### Required Software

- **Operating System**: Windows 10+, macOS 10.15+, or Linux (Ubuntu 20.04+)
- **.NET 9 SDK**: Download from [https://dotnet.microsoft.com/en-us/download/dotnet/9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
  - Verify installation: `dotnet --version` (should be 9.x)
- **Git**: [https://git-scm.com/downloads](https://git-scm.com/downloads)
  - Verify installation: `git --version`
- **IDE** (Choose one):
  - Visual Studio 2022 Community+ (recommended for Windows)
  - VS Code with C# Dev Kit
  - JetBrains Rider
- **PostgreSQL 15+**:
  - [https://www.postgresql.org/download/](https://www.postgresql.org/download/)
  - Or use Docker: `docker run -d -e POSTGRES_PASSWORD=password -p 5432:5432 postgres:15`
- **Docker** (Optional but recommended):
  - [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)

### Optional Tools

- **pgAdmin** - PostgreSQL management UI (or use psql CLI)
- **Redis** - For future caching features
- **Azure Storage Emulator** - For Azure integration testing
- **Postman** or **Insomnia** - For API testing

## Step-by-Step Setup

### 1. Clone the Repository

```bash
git clone https://github.com/juziyaat/crm-sync-modernization.git
cd crm-sync-modernization
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Database Setup

#### Option A: Using Docker (Recommended)

```bash
# Start PostgreSQL container
docker run -d \
  --name crm_postgres \
  -e POSTGRES_USER=admin \
  -e POSTGRES_PASSWORD=Admin123! \
  -e POSTGRES_INITDB_ARGS="-c max_connections=200" \
  -p 5432:5432 \
  -v postgres_data:/var/lib/postgresql/data \
  postgres:15

# Verify container is running
docker ps | grep crm_postgres
```

#### Option B: Local PostgreSQL Installation

```bash
# Windows (PowerShell)
"C:\Program Files\PostgreSQL\15\bin\psql.exe" -U postgres -c "CREATE USER admin WITH PASSWORD 'Admin123!' CREATEDB;"

# macOS/Linux
psql -U postgres -c "CREATE USER admin WITH PASSWORD 'Admin123!' CREATEDB;"
```

### 4. Configure Connection Strings

Create `appsettings.Development.json` in `src/CCA.Sync.WebApi/`:

```json
{
  "ConnectionStrings": {
    "System": "Server=localhost;Port=5432;Database=cca_system;User Id=admin;Password=Admin123!;Include Error Detail=true;",
    "Tenant_00000000-0000-0000-0000-000000000001": "Server=localhost;Port=5432;Database=cca_tenant_001;User Id=admin;Password=Admin123!;Include Error Detail=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": "Debug",
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/crm-sync-.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

**Note**: Add to `.gitignore` if it contains sensitive data:
```
**/appsettings.Development.json
**/appsettings.Production.json
```

### 5. Create Databases

```bash
# Connect to PostgreSQL
psql -h localhost -U admin -d postgres

# Create system database
CREATE DATABASE cca_system OWNER admin ENCODING 'UTF8';

# Create tenant database
CREATE DATABASE cca_tenant_001 OWNER admin ENCODING 'UTF8';

# Exit psql
\q
```

Or use Docker with psql:

```bash
docker exec -it crm_postgres psql -U admin -c "CREATE DATABASE cca_system OWNER admin ENCODING 'UTF8';"
docker exec -it crm_postgres psql -U admin -c "CREATE DATABASE cca_tenant_001 OWNER admin ENCODING 'UTF8';"
```

### 6. Apply Database Migrations

From the solution root:

```bash
# Apply migrations to system database
cd src/CCA.Sync.WebApi
dotnet ef database update -c SystemDbContext

# Apply migrations to tenant database
dotnet ef database update -c TenantDbContext

# Verify tables were created
# psql -h localhost -U admin -d cca_system -c "\dt"
```

### 7. Build the Solution

```bash
# From solution root
dotnet build --configuration Release

# Verify no build errors
# Output should show: "Build succeeded"
```

### 8. Run Unit Tests

```bash
# Run all tests
dotnet test --configuration Release

# Run specific test project
dotnet test tests/Domain.Tests --configuration Release

# Run with coverage
dotnet test /p:CollectCoverage=true
```

### 9. Start the Application

#### WebApi

```bash
cd src/CCA.Sync.WebApi
dotnet run

# Application should start at: https://localhost:5001
# Swagger documentation: https://localhost:5001/swagger
```

#### Background Worker

```bash
cd src/CCA.Sync.Worker
dotnet run
```

### 10. Verify Setup

- [ ] Solution builds without errors
- [ ] All tests pass
- [ ] WebApi starts successfully
- [ ] Can access Swagger at https://localhost:5001/swagger
- [ ] Database connections work
- [ ] Can see logs in `logs/` directory

## Using Docker Compose

For complete local environment setup with all services:

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

See [docker-compose.yml](../../docker-compose.yml) for configured services.

## IDE Setup

### Visual Studio 2022

1. Open `CcaSyncModernization.sln`
2. Set `CCA.Sync.WebApi` as startup project
3. Go to `Debug` → `Debug Properties`
4. Set `Launch` to `Project`
5. Build solution (Ctrl+Shift+B)
6. Press F5 to start debugging

### VS Code

1. Open folder in VS Code
2. Install recommended extensions (will be prompted)
3. Install NuGet packages: `dotnet restore`
4. Create `.vscode/launch.json`:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (WebApi)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/CCA.Sync.WebApi/bin/Debug/net9.0/CCA.Sync.WebApi.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/CCA.Sync.WebApi",
            "stopAtEntry": false,
            "serverReadyAction": {
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    ]
}
```

5. Press F5 to start debugging

### JetBrains Rider

1. Open `CcaSyncModernization.sln`
2. Right-click `CCA.Sync.WebApi` → `Set as Startup Project`
3. Edit Run Configuration if needed
4. Click Run or press Shift+F10

## Environment Variables

Create a `.env` file in the root (optional, for local overrides):

```bash
# Database
DB_HOST=localhost
DB_PORT=5432
DB_USER=admin
DB_PASSWORD=Admin123!
DB_SYSTEM=cca_system
DB_TENANT=cca_tenant_001

# Logging
LOG_LEVEL=Debug
LOG_PATH=logs

# Application
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:5001
```

## Common Development Tasks

### Running Unit Tests

```bash
# All tests
dotnet test

# Specific project
dotnet test tests/Domain.Tests

# Specific test class
dotnet test tests/Domain.Tests --filter "ClassName=CustomerTests"

# Watch mode (restart on file change)
dotnet test --watch
```

### Running Integration Tests

```bash
# Integration tests with Testcontainers
dotnet test tests/Integration.Tests --configuration Debug

# With Docker, ensures PostgreSQL container is available
docker-compose up -d postgres
```

### Database Migrations

```bash
# Create new migration
cd src/CCA.Sync.Infrastructure
dotnet ef migrations add AddNewFeature

# Apply pending migrations
dotnet ef database update

# Revert last migration
dotnet ef migrations remove

# List migrations
dotnet ef migrations list
```

### Code Analysis

```bash
# Run code analysis
dotnet build /p:EnforceCodeStyleInBuild=true

# Run StyleCop analyzer
dotnet build /p:AnalysisLevel=latest

# Run SonarAnalyzer
dotnet build /p:SonarAnalyzeEnabled=true
```

### Building for Release

```bash
# Build release configuration
dotnet build --configuration Release

# Run release tests
dotnet test --configuration Release

# Publish for deployment
dotnet publish -c Release -o ./publish
```

## Troubleshooting

### Issue: "ConnectionRefused - Could not connect to database"

**Solution**:
- Verify PostgreSQL is running: `docker ps` or check Services (Windows)
- Verify connection string in `appsettings.Development.json`
- Verify database exists: `psql -U admin -l`

### Issue: "Entity Framework Migrations Failed"

**Solution**:
```bash
# Ensure EF tools are installed
dotnet tool install --global dotnet-ef

# Try migration again
dotnet ef database update -v
```

### Issue: "Build Failed - Project reference has invalid path"

**Solution**:
```bash
# Clean and restore
dotnet clean
dotnet restore
```

### Issue: "Tests fail with 'docker is not running'"

**Solution**:
- Ensure Docker Desktop is running
- Or use local PostgreSQL instead of Testcontainers
- Set environment variable: `set TESTCONTAINERS_DOCKER_SOCKET_OVERRIDE=default` (Windows)

### Issue: "Port 5432 already in use"

**Solution**:
```bash
# Windows - Find process using port 5432
netstat -ano | findstr :5432

# Kill process (replace PID with actual process ID)
taskkill /PID <PID> /F

# macOS/Linux
lsof -i :5432
kill -9 <PID>
```

## Performance Tips

1. **Use Release Build**: `dotnet build -c Release`
2. **Enable Nullable Checks**: Enforced by `.editorconfig`
3. **Use Async/Await**: All I/O operations are async
4. **Profile with Profiler**: Use Rider's built-in profiler
5. **Check Query Performance**: Use Serilog logging to see SQL

## Next Steps

After setup is complete:

1. Read [Architecture Documentation](../architecture/README.md)
2. Review [Clean Architecture ADR](../adrs/ADR-001-Clean-Architecture.md)
3. Check [Coding Standards](CODING-STANDARDS.md)
4. Run example tests to understand patterns
5. Create a simple feature following the architecture

## Getting Help

- **Setup Issues**: Check [Troubleshooting](#troubleshooting) section
- **Architecture Questions**: Read [ADRs](../adrs/)
- **Code Examples**: See test projects for usage patterns
- **Microsoft Docs**: [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)

## References

- [.NET 9 Installation](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Getting Started](https://docs.docker.com/get-started/)
- [Visual Studio Code C# Dev Kit](https://code.visualstudio.com/docs/languages/csharp)
