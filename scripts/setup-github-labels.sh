#!/bin/bash
# Setup GitHub Labels for CCA CRM Sync Modernization
# Usage: ./scripts/setup-github-labels.sh

set -e

echo "🏷️  Setting up GitHub Labels for CCA CRM Sync Modernization"
echo ""

# Check if gh CLI is installed
if ! command -v gh &> /dev/null; then
    echo "❌ GitHub CLI (gh) is not installed."
    echo "📥 Please install it from: https://cli.github.com/"
    exit 1
fi

# Check if authenticated
if ! gh auth status &> /dev/null; then
    echo "🔐 Not authenticated with GitHub. Running gh auth login..."
    gh auth login
fi

echo "✅ GitHub CLI authenticated"
echo ""
echo "Creating labels..."
echo ""

# Type labels
echo "📦 Creating Type labels..."
gh label create "epic" --color "0E8A16" --description "High-level feature or project phase" --force || true
gh label create "user-story" --color "1D76DB" --description "Business-facing feature" --force || true
gh label create "technical-task" --color "5319E7" --description "Implementation work" --force || true
gh label create "bug" --color "D93F0B" --description "Something is broken" --force || true
gh label create "documentation" --color "C5DEF5" --description "Documentation only" --force || true
gh label create "infrastructure" --color "000000" --description "CI/CD, DevOps" --force || true
gh label create "testing" --color "0075CA" --description "Test-related work" --force || true

# Phase labels
echo "📅 Creating Phase labels..."
gh label create "phase-1-foundation" --color "0E8A16" --description "Foundation & Domain Layer" --force || true
gh label create "phase-2-domain" --color "1D76DB" --description "Domain Model Completion" --force || true
gh label create "phase-3-multi-tenancy" --color "5319E7" --description "Multi-Tenancy Implementation" --force || true
gh label create "phase-4-cqrs" --color "D93F0B" --description "CQRS Application Layer" --force || true
gh label create "phase-5-api" --color "FBCA04" --description "REST API Development" --force || true
gh label create "phase-6-jobs" --color "FEF2C0" --description "Background Jobs & Sync" --force || true
gh label create "phase-7-migration" --color "BFD4F2" --description "Data Migration" --force || true
gh label create "phase-8-polish" --color "C2E0C6" --description "Polish & Deployment" --force || true

# Priority labels
echo "🎯 Creating Priority labels..."
gh label create "priority-critical" --color "B60205" --description "Blocking work" --force || true
gh label create "priority-high" --color "D93F0B" --description "Important work" --force || true
gh label create "priority-medium" --color "FBCA04" --description "Normal priority" --force || true
gh label create "priority-low" --color "0E8A16" --description "Nice to have" --force || true

# Assignee type labels
echo "👥 Creating Assignee Type labels..."
gh label create "ai-task" --color "1D76DB" --description "Claude AI will implement" --force || true
gh label create "human-review" --color "FBCA04" --description "Requires human review" --force || true
gh label create "human-task" --color "D93F0B" --description "Human must do" --force || true
gh label create "pair-programming" --color "5319E7" --description "AI + Human together" --force || true

# Technology labels
echo "🔧 Creating Technology labels..."
gh label create "domain" --color "D93F0B" --description "Domain layer work" --force || true
gh label create "application" --color "1D76DB" --description "Application layer work" --force || true
gh label create "api" --color "0075CA" --description "API work" --force || true
gh label create "database" --color "000000" --description "Database work" --force || true
gh label create "devops" --color "C5DEF5" --description "DevOps/CI/CD work" --force || true
gh label create "security" --color "B60205" --description "Security-related work" --force || true

# Status labels
echo "📊 Creating Status labels..."
gh label create "blocked" --color "B60205" --description "Cannot proceed" --force || true
gh label create "needs-discussion" --color "FBCA04" --description "Decision needed" --force || true
gh label create "ready-for-review" --color "0E8A16" --description "AI done, needs review" --force || true
gh label create "in-progress" --color "1D76DB" --description "Currently being worked" --force || true
gh label create "waiting-for-pr" --color "FEF2C0" --description "PR not yet created" --force || true

# Special labels
echo "🌟 Creating Special labels..."
gh label create "good-first-issue" --color "7057FF" --description "Good for newcomers" --force || true
gh label create "help-wanted" --color "008672" --description "Extra attention needed" --force || true
gh label create "duplicate" --color "CFD3D7" --description "Already exists" --force || true
gh label create "wontfix" --color "FFFFFF" --description "Not addressing" --force || true
gh label create "question" --color "D876E3" --description "Question or discussion" --force || true
gh label create "enhancement" --color "A2EEEF" --description "New feature or improvement" --force || true
gh label create "technical-debt" --color "FEF2C0" --description "Code needing refactoring" --force || true
gh label create "performance" --color "FF6B6B" --description "Performance optimization" --force || true
gh label create "breaking-change" --color "B60205" --description "Breaking API change" --force || true
gh label create "report" --color "C5DEF5" --description "Progress/status reports" --force || true

echo ""
echo "✅ All labels created successfully!"
echo ""
echo "📋 Summary:"
echo "  - Type labels: 7"
echo "  - Phase labels: 8"
echo "  - Priority labels: 4"
echo "  - Assignee labels: 4"
echo "  - Technology labels: 6"
echo "  - Status labels: 5"
echo "  - Special labels: 10"
echo "  - Total: 44 labels"
echo ""
echo "🎉 GitHub labels setup complete!"
