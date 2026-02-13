# Setup GitHub Labels for CCA CRM Sync Modernization
# Usage: .\scripts\setup-github-labels.ps1

$ErrorActionPreference = "Stop"

Write-Host "Setting up GitHub Labels for CCA CRM Sync Modernization" -ForegroundColor Cyan
Write-Host ""

# Check if gh CLI is installed
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: GitHub CLI (gh) is not installed." -ForegroundColor Red
    Write-Host "Please install it from: https://cli.github.com/" -ForegroundColor Yellow
    Write-Host "   Or use: winget install GitHub.cli" -ForegroundColor Yellow
    exit 1
}

# Check if authenticated
try {
    gh auth status | Out-Null
    Write-Host "GitHub CLI authenticated" -ForegroundColor Green
} catch {
    Write-Host "Not authenticated with GitHub. Running gh auth login..." -ForegroundColor Yellow
    gh auth login
}

Write-Host ""
Write-Host "Creating labels..." -ForegroundColor Cyan
Write-Host ""

# Helper function to create label
function Create-Label {
    param (
        [string]$Name,
        [string]$Color,
        [string]$Description
    )

    try {
        gh label create $Name --color $Color --description $Description --force 2>&1 | Out-Null
        Write-Host "  + $Name" -ForegroundColor Green
    } catch {
        Write-Host "  ~ $Name (already exists or error)" -ForegroundColor Yellow
    }
}

# Type labels
Write-Host "Creating Type labels..." -ForegroundColor Cyan
Create-Label "epic" "0E8A16" "High-level feature or project phase"
Create-Label "user-story" "1D76DB" "Business-facing feature"
Create-Label "technical-task" "5319E7" "Implementation work"
Create-Label "bug" "D93F0B" "Something is broken"
Create-Label "documentation" "C5DEF5" "Documentation only"
Create-Label "infrastructure" "000000" "CI/CD, DevOps"
Create-Label "testing" "0075CA" "Test-related work"

# Phase labels
Write-Host ""
Write-Host "Creating Phase labels..." -ForegroundColor Cyan
Create-Label "phase-1-foundation" "0E8A16" "Foundation and Domain Layer"
Create-Label "phase-2-domain" "1D76DB" "Domain Model Completion"
Create-Label "phase-3-multi-tenancy" "5319E7" "Multi-Tenancy Implementation"
Create-Label "phase-4-cqrs" "D93F0B" "CQRS Application Layer"
Create-Label "phase-5-api" "FBCA04" "REST API Development"
Create-Label "phase-6-jobs" "FEF2C0" "Background Jobs and Sync"
Create-Label "phase-7-migration" "BFD4F2" "Data Migration"
Create-Label "phase-8-polish" "C2E0C6" "Polish and Deployment"

# Priority labels
Write-Host ""
Write-Host "Creating Priority labels..." -ForegroundColor Cyan
Create-Label "priority-critical" "B60205" "Blocking work"
Create-Label "priority-high" "D93F0B" "Important work"
Create-Label "priority-medium" "FBCA04" "Normal priority"
Create-Label "priority-low" "0E8A16" "Nice to have"

# Assignee type labels
Write-Host ""
Write-Host "Creating Assignee Type labels..." -ForegroundColor Cyan
Create-Label "ai-task" "1D76DB" "Claude AI will implement"
Create-Label "human-review" "FBCA04" "Requires human review"
Create-Label "human-task" "D93F0B" "Human must do"
Create-Label "pair-programming" "5319E7" "AI and Human together"

# Technology labels
Write-Host ""
Write-Host "Creating Technology labels..." -ForegroundColor Cyan
Create-Label "domain" "D93F0B" "Domain layer work"
Create-Label "application" "1D76DB" "Application layer work"
Create-Label "api" "0075CA" "API work"
Create-Label "database" "000000" "Database work"
Create-Label "devops" "C5DEF5" "DevOps/CI/CD work"
Create-Label "security" "B60205" "Security-related work"

# Status labels
Write-Host ""
Write-Host "Creating Status labels..." -ForegroundColor Cyan
Create-Label "blocked" "B60205" "Cannot proceed"
Create-Label "needs-discussion" "FBCA04" "Decision needed"
Create-Label "ready-for-review" "0E8A16" "AI done, needs review"
Create-Label "in-progress" "1D76DB" "Currently being worked"
Create-Label "waiting-for-pr" "FEF2C0" "PR not yet created"

# Special labels
Write-Host ""
Write-Host "Creating Special labels..." -ForegroundColor Cyan
Create-Label "good-first-issue" "7057FF" "Good for newcomers"
Create-Label "help-wanted" "008672" "Extra attention needed"
Create-Label "duplicate" "CFD3D7" "Already exists"
Create-Label "wontfix" "FFFFFF" "Not addressing"
Create-Label "question" "D876E3" "Question or discussion"
Create-Label "enhancement" "A2EEEF" "New feature or improvement"
Create-Label "technical-debt" "FEF2C0" "Code needing refactoring"
Create-Label "performance" "FF6B6B" "Performance optimization"
Create-Label "breaking-change" "B60205" "Breaking API change"
Create-Label "report" "C5DEF5" "Progress/status reports"

Write-Host ""
Write-Host "All labels created successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  - Type labels: 7"
Write-Host "  - Phase labels: 8"
Write-Host "  - Priority labels: 4"
Write-Host "  - Assignee labels: 4"
Write-Host "  - Technology labels: 6"
Write-Host "  - Status labels: 5"
Write-Host "  - Special labels: 10"
Write-Host "  - Total: 44 labels"
Write-Host ""
Write-Host "GitHub labels setup complete!" -ForegroundColor Green
