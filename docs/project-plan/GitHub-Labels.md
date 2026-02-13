# GitHub Labels for CCA CRM Sync Modernization

## Overview
This document defines all labels used in the project for consistent issue tracking and project management.

---

## Label Categories

### 🏷️ By Type

| Label | Color | Description | Usage |
|-------|-------|-------------|-------|
| `epic` | ![#0E8A16](https://via.placeholder.com/15/0E8A16/000000?text=+) `#0E8A16` | High-level feature or project phase | Use for milestone-level features |
| `user-story` | ![#1D76DB](https://via.placeholder.com/15/1D76DB/000000?text=+) `#1D76DB` | Business-facing feature | Use for user-centric features |
| `technical-task` | ![#5319E7](https://via.placeholder.com/15/5319E7/000000?text=+) `#5319E7` | Implementation work | Use for development tasks |
| `bug` | ![#D93F0B](https://via.placeholder.com/15/D93F0B/000000?text=+) `#D93F0B` | Something's broken | Use for defects |
| `documentation` | ![#C5DEF5](https://via.placeholder.com/15/C5DEF5/000000?text=+) `#C5DEF5` | Documentation only | Use for docs-only changes |
| `infrastructure` | ![#000000](https://via.placeholder.com/15/000000/000000?text=+) `#000000` | CI/CD, DevOps | Use for infrastructure work |
| `testing` | ![#0075CA](https://via.placeholder.com/15/0075CA/000000?text=+) `#0075CA` | Test-related work | Use for test improvements |

### 📅 By Phase

| Label | Color | Description |
|-------|-------|-------------|
| `phase-1-foundation` | ![#0E8A16](https://via.placeholder.com/15/0E8A16/000000?text=+) `#0E8A16` | Foundation & Domain Layer |
| `phase-2-domain` | ![#1D76DB](https://via.placeholder.com/15/1D76DB/000000?text=+) `#1D76DB` | Domain Model Completion |
| `phase-3-multi-tenancy` | ![#5319E7](https://via.placeholder.com/15/5319E7/000000?text=+) `#5319E7` | Multi-Tenancy Implementation |
| `phase-4-cqrs` | ![#D93F0B](https://via.placeholder.com/15/D93F0B/000000?text=+) `#D93F0B` | CQRS Application Layer |
| `phase-5-api` | ![#FBCA04](https://via.placeholder.com/15/FBCA04/000000?text=+) `#FBCA04` | REST API Development |
| `phase-6-jobs` | ![#FEF2C0](https://via.placeholder.com/15/FEF2C0/000000?text=+) `#FEF2C0` | Background Jobs & Sync |
| `phase-7-migration` | ![#BFD4F2](https://via.placeholder.com/15/BFD4F2/000000?text=+) `#BFD4F2` | Data Migration |
| `phase-8-polish` | ![#C2E0C6](https://via.placeholder.com/15/C2E0C6/000000?text=+) `#C2E0C6` | Polish & Deployment |

### 🎯 By Priority

| Label | Color | Description | SLA |
|-------|-------|-------------|-----|
| `priority-critical` | ![#B60205](https://via.placeholder.com/15/B60205/000000?text=+) `#B60205` | Blocking work, must be done ASAP | Start within 1 day |
| `priority-high` | ![#D93F0B](https://via.placeholder.com/15/D93F0B/000000?text=+) `#D93F0B` | Important, should be done soon | Start within 3 days |
| `priority-medium` | ![#FBCA04](https://via.placeholder.com/15/FBCA04/000000?text=+) `#FBCA04` | Normal priority | Start within 1 week |
| `priority-low` | ![#0E8A16](https://via.placeholder.com/15/0E8A16/000000?text=+) `#0E8A16` | Nice to have, when time permits | Start when convenient |

### 👥 By Assignee Type

| Label | Color | Description | Usage |
|-------|-------|-------------|-------|
| `ai-task` | ![#1D76DB](https://via.placeholder.com/15/1D76DB/000000?text=+) `#1D76DB` | Claude AI will implement | Auto-assigned to AI agent |
| `human-review` | ![#FBCA04](https://via.placeholder.com/15/FBCA04/000000?text=+) `#FBCA04` | Requires human review | Human must review |
| `human-task` | ![#D93F0B](https://via.placeholder.com/15/D93F0B/000000?text=+) `#D93F0B` | Human must do | Only human can complete |
| `pair-programming` | ![#5319E7](https://via.placeholder.com/15/5319E7/000000?text=+) `#5319E7` | AI + Human together | Collaborative work |

### 🔧 By Technology/Layer

| Label | Color | Description |
|-------|-------|-------------|
| `domain` | ![#D93F0B](https://via.placeholder.com/15/D93F0B/000000?text=+) `#D93F0B` | Domain layer work |
| `application` | ![#1D76DB](https://via.placeholder.com/15/1D76DB/000000?text=+) `#1D76DB` | Application layer work |
| `infrastructure` | ![#5319E7](https://via.placeholder.com/15/5319E7/000000?text=+) `#5319E7` | Infrastructure layer work |
| `api` | ![#0075CA](https://via.placeholder.com/15/0075CA/000000?text=+) `#0075CA` | API work |
| `database` | ![#000000](https://via.placeholder.com/15/000000/000000?text=+) `#000000` | Database work |
| `testing` | ![#0E8A16](https://via.placeholder.com/15/0E8A16/000000?text=+) `#0E8A16` | Testing work |
| `devops` | ![#C5DEF5](https://via.placeholder.com/15/C5DEF5/000000?text=+) `#C5DEF5` | DevOps/CI/CD work |
| `security` | ![#B60205](https://via.placeholder.com/15/B60205/000000?text=+) `#B60205` | Security-related work |

### 📊 By Status

| Label | Color | Description | Usage |
|-------|-------|-------------|-------|
| `blocked` | ![#B60205](https://via.placeholder.com/15/B60205/000000?text=+) `#B60205` | Cannot proceed | Add when blocked |
| `needs-discussion` | ![#FBCA04](https://via.placeholder.com/15/FBCA04/000000?text=+) `#FBCA04` | Decision needed | Needs team discussion |
| `ready-for-review` | ![#0E8A16](https://via.placeholder.com/15/0E8A16/000000?text=+) `#0E8A16` | AI done, needs human review | Auto-added by workflow |
| `in-progress` | ![#1D76DB](https://via.placeholder.com/15/1D76DB/000000?text=+) `#1D76DB` | Currently being worked | Add when starting work |
| `waiting-for-pr` | ![#FEF2C0](https://via.placeholder.com/15/FEF2C0/000000?text=+) `#FEF2C0` | PR not yet created | Waiting for implementation |

### 🏷️ Special Labels

| Label | Color | Description | Usage |
|-------|-------|-------------|-------|
| `good-first-issue` | ![#7057FF](https://via.placeholder.com/15/7057FF/000000?text=+) `#7057FF` | Good for newcomers | Easy onboarding tasks |
| `help-wanted` | ![#008672](https://via.placeholder.com/15/008672/000000?text=+) `#008672` | Extra attention needed | Community help wanted |
| `duplicate` | ![#CFD3D7](https://via.placeholder.com/15/CFD3D7/000000?text=+) `#CFD3D7` | Already exists | Duplicate issue |
| `wontfix` | ![#FFFFFF](https://via.placeholder.com/15/FFFFFF/000000?text=+) `#FFFFFF` | Not addressing | Won't be implemented |
| `question` | ![#D876E3](https://via.placeholder.com/15/D876E3/000000?text=+) `#D876E3` | Question or discussion | Needs clarification |
| `enhancement` | ![#A2EEEF](https://via.placeholder.com/15/A2EEEF/000000?text=+) `#A2EEEF` | New feature or improvement | Feature requests |
| `technical-debt` | ![#FEF2C0](https://via.placeholder.com/15/FEF2C0/000000?text=+) `#FEF2C0` | Code that needs refactoring | Technical debt items |
| `performance` | ![#FF6B6B](https://via.placeholder.com/15/FF6B6B/000000?text=+) `#FF6B6B` | Performance optimization | Performance work |
| `breaking-change` | ![#B60205](https://via.placeholder.com/15/B60205/000000?text=+) `#B60205` | Breaking API change | Breaking changes |
| `report` | ![#C5DEF5](https://via.placeholder.com/15/C5DEF5/000000?text=+) `#C5DEF5` | Progress/status reports | Auto-generated reports |

---

## Label Usage Guidelines

### When Creating an Issue

**Every issue should have AT LEAST:**
1. One **Type** label (epic, user-story, technical-task, bug, etc.)
2. One **Phase** label (phase-1-foundation, phase-2-domain, etc.)
3. One **Priority** label (priority-critical, priority-high, etc.)

**Additionally add:**
4. **Assignee Type** (ai-task, human-review, human-task, pair-programming)
5. **Technology/Layer** (domain, application, infrastructure, etc.)
6. **Status** labels as needed (blocked, needs-discussion, etc.)

### Example Label Combinations

#### Example 1: AI Domain Task
```
Labels: technical-task, phase-1-foundation, priority-high, ai-task, domain
```
**What it means**: This is a development task for Phase 1, high priority, AI will implement it, and it's domain layer work.

#### Example 2: Human Review Bug
```
Labels: bug, phase-3-multi-tenancy, priority-critical, human-task, infrastructure
```
**What it means**: Critical bug in Phase 3 affecting infrastructure, human must fix.

#### Example 3: Documentation Epic
```
Labels: epic, phase-1-foundation, priority-medium, ai-task, documentation
```
**What it means**: Documentation epic for Phase 1, medium priority, AI will create.

---

## Label Automation

### Auto-Applied Labels

GitHub Actions will automatically apply labels based on:

1. **Title Keywords**
   - Title contains "AI" → adds `ai-task`
   - Title contains "Foundation" → adds `phase-1-foundation`
   - Title contains "Domain" → adds `domain`
   - Title contains "Test" → adds `testing`

2. **Issue Body Content**
   - Body contains "AI Task" → adds `ai-task`
   - Body contains "Human Review" → adds `human-review`
   - Body contains specific phase keywords → adds phase label

3. **PR Creation**
   - PR created for issue → adds `ready-for-review` to issue
   - PR merged → removes all status labels

4. **Issue State Changes**
   - Issue assigned → adds `in-progress`
   - Issue closed → removes all status labels

---

## Creating Labels in GitHub

### Using GitHub CLI (Recommended)

Run the setup script:

```bash
./scripts/setup-github-labels.sh
```

Or create manually using `gh` CLI:

```bash
# Type labels
gh label create "epic" --color "0E8A16" --description "High-level feature or project phase"
gh label create "user-story" --color "1D76DB" --description "Business-facing feature"
gh label create "technical-task" --color "5319E7" --description "Implementation work"
gh label create "bug" --color "D93F0B" --description "Something is broken"
gh label create "documentation" --color "C5DEF5" --description "Documentation only"
gh label create "infrastructure" --color "000000" --description "CI/CD, DevOps"
gh label create "testing" --color "0075CA" --description "Test-related work"

# Phase labels
gh label create "phase-1-foundation" --color "0E8A16" --description "Foundation & Domain Layer"
gh label create "phase-2-domain" --color "1D76DB" --description "Domain Model Completion"
gh label create "phase-3-multi-tenancy" --color "5319E7" --description "Multi-Tenancy Implementation"
gh label create "phase-4-cqrs" --color "D93F0B" --description "CQRS Application Layer"
gh label create "phase-5-api" --color "FBCA04" --description "REST API Development"
gh label create "phase-6-jobs" --color "FEF2C0" --description "Background Jobs & Sync"
gh label create "phase-7-migration" --color "BFD4F2" --description "Data Migration"
gh label create "phase-8-polish" --color "C2E0C6" --description "Polish & Deployment"

# Priority labels
gh label create "priority-critical" --color "B60205" --description "Blocking work"
gh label create "priority-high" --color "D93F0B" --description "Important work"
gh label create "priority-medium" --color "FBCA04" --description "Normal priority"
gh label create "priority-low" --color "0E8A16" --description "Nice to have"

# Assignee type labels
gh label create "ai-task" --color "1D76DB" --description "Claude AI will implement"
gh label create "human-review" --color "FBCA04" --description "Requires human review"
gh label create "human-task" --color "D93F0B" --description "Human must do"
gh label create "pair-programming" --color "5319E7" --description "AI + Human together"

# Technology labels
gh label create "domain" --color "D93F0B" --description "Domain layer work"
gh label create "application" --color "1D76DB" --description "Application layer work"
gh label create "api" --color "0075CA" --description "API work"
gh label create "database" --color "000000" --description "Database work"
gh label create "devops" --color "C5DEF5" --description "DevOps/CI/CD work"
gh label create "security" --color "B60205" --description "Security-related work"

# Status labels
gh label create "blocked" --color "B60205" --description "Cannot proceed"
gh label create "needs-discussion" --color "FBCA04" --description "Decision needed"
gh label create "ready-for-review" --color "0E8A16" --description "AI done, needs review"
gh label create "in-progress" --color "1D76DB" --description "Currently being worked"
gh label create "waiting-for-pr" --color "FEF2C0" --description "PR not yet created"

# Special labels
gh label create "technical-debt" --color "FEF2C0" --description "Code needing refactoring"
gh label create "performance" --color "FF6B6B" --description "Performance optimization"
gh label create "breaking-change" --color "B60205" --description "Breaking API change"
gh label create "report" --color "C5DEF5" --description "Progress/status reports"
```

### Using GitHub Web UI

1. Go to repository → **Issues** → **Labels**
2. Click **New label**
3. Enter name, description, and color
4. Click **Create label**
5. Repeat for all labels above

---

## Label Metrics

Track label usage with these queries:

### Issues by Phase
```
is:issue label:phase-1-foundation
is:issue label:phase-2-domain
```

### Issues by Priority
```
is:issue label:priority-critical is:open
is:issue label:priority-high is:open
```

### AI vs Human Tasks
```
is:issue label:ai-task is:open
is:issue label:human-task is:open
```

### Blocked Issues
```
is:issue label:blocked is:open
```

### Ready for Review
```
is:issue label:ready-for-review is:open
```

---

## Best Practices

### DO ✅
- Always add at minimum: type, phase, priority labels
- Update status labels as work progresses
- Use `blocked` label when stuck
- Add `needs-discussion` when decision required
- Remove status labels when issue closed

### DON'T ❌
- Don't create custom labels without discussion
- Don't use too many labels (max 6-8 per issue)
- Don't leave issues without labels
- Don't forget to remove status labels
- Don't use color codes inconsistently

---

## Label Review Schedule

**Monthly**: Review label usage and effectiveness
**Quarterly**: Clean up unused labels
**Per Phase**: Update phase-specific labels

---

## Questions?

If you need a new label or have questions about labeling:
1. Create an issue with label `needs-discussion`
2. Tag the issue with `question`
3. Discuss with team
4. Update this document
