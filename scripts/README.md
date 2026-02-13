# Setup Scripts for CCA CRM Sync Modernization

This directory contains automation scripts for setting up the project infrastructure.

## Available Scripts

### 1. Setup GitHub Labels

Creates all necessary GitHub labels for the project.

**Windows (PowerShell)**:
```powershell
.\scripts\setup-github-labels.ps1
```

**Linux/Mac (Bash)**:
```bash
chmod +x scripts/setup-github-labels.sh
./scripts/setup-github-labels.sh
```

**What it does**:
- Creates 44 labels across 6 categories
- Configures colors and descriptions
- Uses GitHub CLI (`gh`)

**Prerequisites**:
- GitHub CLI installed
- Authenticated with `gh auth login`
- Repository created on GitHub

---

## Prerequisites

### Install GitHub CLI

**Windows**:
```powershell
winget install GitHub.cli
```

**macOS**:
```bash
brew install gh
```

**Linux**:
```bash
# Debian/Ubuntu
sudo apt install gh

# Fedora
sudo dnf install gh

# Arch
sudo pacman -S github-cli
```

### Authenticate

```bash
gh auth login
```

Follow the prompts to authenticate with GitHub.

---

## Script Details

### setup-github-labels

**Labels Created**:

| Category | Count | Examples |
|----------|-------|----------|
| Type | 7 | epic, user-story, technical-task, bug |
| Phase | 8 | phase-1-foundation, phase-2-domain |
| Priority | 4 | priority-critical, priority-high |
| Assignee | 4 | ai-task, human-review |
| Technology | 6 | domain, application, api |
| Status | 5 | blocked, in-progress |
| Special | 10 | technical-debt, performance |

**Total**: 44 labels

**Runtime**: ~30 seconds

**Idempotent**: Safe to run multiple times (uses `--force` flag)

---

## Usage Examples

### Complete Project Setup

1. Create GitHub repository:
```bash
# Via GitHub CLI
gh repo create crm-sync-modernization --private --description "CCA CRM Sync Modernization"

# Or via GitHub web UI
```

2. Clone and setup:
```bash
git clone https://github.com/YOUR_USERNAME/crm-sync-modernization.git
cd crm-sync-modernization
```

3. Run label setup:
```bash
# Windows
.\scripts\setup-github-labels.ps1

# Linux/Mac
./scripts/setup-github-labels.sh
```

4. Push initial structure:
```bash
git add .
git commit -m "Initial project structure"
git push origin main
```

5. Configure branch protection (via GitHub UI):
   - Go to Settings → Branches
   - Add rule for `main`
   - Enable "Require pull request before merging"
   - Enable "Require status checks to pass before merging"

6. Create GitHub Projects board:
   - Go to Projects → New Project
   - Choose "Board" template
   - Name it "CCA Sync Modernization"
   - Add columns: Backlog, Ready, In Progress, Review, Done

---

## Troubleshooting

### "gh: command not found"

**Solution**: Install GitHub CLI (see Prerequisites above)

### "authentication required"

**Solution**: Run `gh auth login` and follow prompts

### "permission denied" (Linux/Mac)

**Solution**: Make script executable:
```bash
chmod +x scripts/setup-github-labels.sh
```

### Script fails with "repository not found"

**Solution**: Make sure you're in the repository directory and it exists on GitHub:
```bash
cd /path/to/crm-sync-modernization
gh repo view
```

---

## Next Steps

After running the setup scripts:

1. ✅ **Labels created** - All 44 labels ready
2. 📋 **Create issues** - Use Phase 1 issue templates
3. 🎯 **Setup project board** - Add issues to board
4. 🤖 **Configure automation** - GitHub Actions already configured
5. 🚀 **Start coding** - Begin with Issue #1

---

## Support

If you encounter issues:

1. Check script output for specific errors
2. Verify GitHub CLI is authenticated: `gh auth status`
3. Ensure you have repository permissions
4. Check [docs/project-plan/GitHub-Labels.md](../docs/project-plan/GitHub-Labels.md) for details

---

## Future Scripts

Planned automation scripts:

- [ ] `create-phase-1-issues.sh` - Create all 25 Phase 1 issues
- [ ] `setup-project-board.sh` - Configure GitHub Projects board
- [ ] `setup-branch-protection.sh` - Configure branch protection rules
- [ ] `setup-secrets.sh` - Configure GitHub Actions secrets
- [ ] `generate-progress-report.sh` - Generate manual progress report
