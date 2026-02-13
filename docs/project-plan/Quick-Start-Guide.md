# Quick Start Guide - CCA CRM Sync Modernization

## 🚀 Get Your Project Management Up and Running in 10 Minutes

This guide will help you go from zero to a fully configured project management system with GitHub.

---

## ✅ Checklist

Follow these steps in order:

- [ ] **Step 1**: Install Prerequisites (2 min)
- [ ] **Step 2**: Create GitHub Repository (1 min)
- [ ] **Step 3**: Setup Labels (1 min)
- [ ] **Step 4**: Create Project Board (2 min)
- [ ] **Step 5**: Configure Branch Protection (2 min)
- [ ] **Step 6**: Create First Issues (2 min)
- [ ] **Step 7**: Start Coding! (∞)

**Total Time**: ~10 minutes

---

## Step 1: Install Prerequisites (2 min)

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
# Ubuntu/Debian
sudo apt install gh

# Fedora
sudo dnf install gh
```

### Authenticate

```bash
gh auth login
```

Select:
- GitHub.com
- HTTPS
- Login with a web browser

✅ **Checkpoint**: Run `gh auth status` to verify

---

## Step 2: Create GitHub Repository (1 min)

### Option A: Via GitHub CLI (Recommended)

```bash
gh repo create crm-sync-modernization \
  --private \
  --description "CCA CRM Sync Modernization - Clean Architecture Implementation" \
  --clone
```

### Option B: Via GitHub Web UI

1. Go to https://github.com/new
2. Repository name: `crm-sync-modernization`
3. Description: "CCA CRM Sync Modernization - Clean Architecture Implementation"
4. Select: Private
5. **Don't** initialize with README (we already have one)
6. Click "Create repository"
7. Clone it:
```bash
git clone https://github.com/YOUR_USERNAME/crm-sync-modernization.git
cd crm-sync-modernization
```

### Push Initial Structure

```bash
# If you cloned an existing repo with files
git add .
git commit -m "Initial project structure with templates and documentation"
git push origin main
```

✅ **Checkpoint**: Visit your repo on GitHub and see the files

---

## Step 3: Setup Labels (1 min)

Run the label setup script:

**Windows (PowerShell)**:
```powershell
.\scripts\setup-github-labels.ps1
```

**Linux/Mac (Bash)**:
```bash
chmod +x scripts/setup-github-labels.sh
./scripts/setup-github-labels.sh
```

**What this does**:
- Creates 44 labels across 6 categories
- Type, Phase, Priority, Assignee, Technology, Status labels
- All with proper colors and descriptions

✅ **Checkpoint**: Go to your repo → Issues → Labels. You should see 44+ labels.

---

## Step 4: Create Project Board (2 min)

### Via GitHub Web UI

1. Go to your repository
2. Click **Projects** tab
3. Click **Add Project** → **New Project**
4. Choose **Board** template
5. Name it: `CCA Sync Modernization Roadmap`
6. Click **Create**

### Configure Board Columns

The board should have these columns (drag to reorder):

1. 📋 **Backlog** - Not yet ready to work on
2. 🎯 **Ready** - Ready to start
3. 🏗️ **In Progress** - Currently being worked
4. 👀 **Review** - Awaiting human review
5. ✅ **Done** - Completed

### Add a Second Board (Optional)

Create another board for current sprint:
- Name: `Current Sprint`
- Columns: Todo → AI Working → Human Review → Refinement → Completed

✅ **Checkpoint**: You should see your project board(s) in the Projects tab

---

## Step 5: Configure Branch Protection (2 min)

### Via GitHub Web UI

1. Go to repository **Settings**
2. Click **Branches** (left sidebar)
3. Click **Add branch protection rule**

**Configure the rule**:
- Branch name pattern: `main`
- ✅ Require a pull request before merging
  - ✅ Require approvals: 1
  - ✅ Dismiss stale pull request approvals when new commits are pushed
- ✅ Require status checks to pass before merging
  - Search and select: `build-and-test` (will appear after first CI run)
  - ✅ Require branches to be up to date before merging
- ✅ Require conversation resolution before merging
- ✅ Do not allow bypassing the above settings
- Click **Create**

✅ **Checkpoint**: Branch protection rule appears in the list

---

## Step 6: Create First Issues (2 min)

### Option A: Manually Create Issues

Use the comprehensive issue list in [docs/project-plan/Phase-1-Issues.md](Phase-1-Issues.md).

**To create an issue**:

1. Go to repository → **Issues** → **New Issue**
2. Choose template: **Technical Task**
3. Copy content from Phase-1-Issues.md (e.g., Issue #1)
4. Add labels:
   - `technical-task`
   - `phase-1-foundation`
   - `priority-critical`
   - `ai-task`
   - `infrastructure`
5. Set milestone: Create "Phase 1 - Foundation" milestone first
6. Click **Submit new issue**

**First 3 Critical Issues to Create**:

1. **Issue #1**: Project Infrastructure Setup
2. **Issue #2**: Solution Structure
3. **Issue #4**: Shared Kernel - Common Domain Patterns

### Option B: Create All 25 Phase 1 Issues (Advanced)

Coming soon: Automated script to create all Phase 1 issues.

For now, manually create the top 5 priority-critical issues to get started.

✅ **Checkpoint**: You should have 3-5 issues created and visible on your project board

---

## Step 7: Start Coding! (∞)

### Your Project is Now Ready! 🎉

You now have:
- ✅ Complete project structure
- ✅ All templates (issues, PRs)
- ✅ All 44 labels configured
- ✅ Project board(s) set up
- ✅ Branch protection enabled
- ✅ GitHub Actions workflows ready
- ✅ First issues created

### What's Next?

1. **Review the Architecture**
   - Read [docs/Project_Management_Strategy.md](Project_Management_Strategy.md)
   - Review [docs/project-plan/Phase-1-Issues.md](Phase-1-Issues.md)

2. **Start with Issue #1**
   - Assign to AI (Claude)
   - Move to "In Progress"
   - Let AI implement
   - Review the PR
   - Merge!

3. **Follow the Workflow**
   - AI creates code → PR created
   - Human reviews → Feedback
   - AI addresses feedback → Re-review
   - Human approves → Merge
   - Issue auto-closes → On to next!

---

## 🔄 Daily Workflow

### For Humans

**Morning**:
1. Check project board
2. Review PRs awaiting approval
3. Check "Ready for Review" column
4. Assign new issues to AI

**During Day**:
1. Review AI-generated PRs
2. Provide feedback on PRs
3. Approve and merge completed work
4. Update project board

**Evening**:
1. Check progress report (auto-generated at 5 PM)
2. Plan next day's priorities

### For AI (Claude)

**When assigned an issue**:
1. Read issue details carefully
2. Update issue to "In Progress"
3. Implement code
4. Write comprehensive tests
5. Create PR with detailed description
6. Move issue to "Ready for Review"
7. Wait for human feedback
8. Address feedback
9. Wait for approval

---

## 📊 Monitoring Progress

### Daily Progress Report

Automatically generated every day at 5 PM UTC (9 AM PST).

Check **Issues** tab for reports tagged with `report` label.

### Manual Progress Check

```bash
# See all open issues
gh issue list --state open

# See issues by phase
gh issue list --label "phase-1-foundation"

# See AI tasks
gh issue list --label "ai-task" --state open

# See blocked issues
gh issue list --label "blocked"
```

### Project Metrics

Go to **Insights** tab to see:
- Contributors
- Commit activity
- Code frequency
- Pulse (weekly summary)

---

## 🆘 Troubleshooting

### "Labels not created"

**Solution**:
```bash
# Run the script again (it's idempotent)
.\scripts\setup-github-labels.ps1
```

### "Can't create project board"

**Possible causes**:
- Need admin permissions on repo
- Organization restrictions

**Solution**: Use your personal account or ask org admin

### "Branch protection not working"

**Solution**:
- Make sure you created the rule for `main` branch exactly
- Check that you have admin permissions
- Wait for first CI run to complete

### "Issues not showing on project board"

**Solution**:
1. Go to project board
2. Click `+` to add items
3. Search for your issues
4. Add them manually to backlog column

Or use automation workflow (should auto-add after first push).

---

## 📚 Additional Resources

### Documentation

- [Project Management Strategy](Project_Management_Strategy.md) - Full strategy
- [Phase 1 Issues](Phase-1-Issues.md) - All 25 Phase 1 issues detailed
- [GitHub Labels](GitHub-Labels.md) - Complete label documentation
- [Scripts README](../../scripts/README.md) - Script usage guide

### GitHub Resources

- [GitHub CLI Manual](https://cli.github.com/manual/)
- [GitHub Projects Guide](https://docs.github.com/en/issues/planning-and-tracking-with-projects)
- [GitHub Actions](https://docs.github.com/en/actions)
- [Branch Protection Rules](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches)

---

## ✨ Pro Tips

1. **Use Milestones** - Group issues by phase for better tracking
2. **Use Labels Wisely** - Don't over-label (max 6-8 per issue)
3. **Link PRs to Issues** - Always use "Closes #123" in PR description
4. **Review Daily Reports** - Stay on top of progress
5. **Keep Issues Small** - Break large tasks into smaller issues
6. **Communicate in Issues** - Use comments for discussions
7. **Use Templates** - Always create issues/PRs from templates
8. **Trust the Process** - Follow the workflow consistently

---

## 🎯 Success Metrics

After Phase 1 (4 weeks), you should have:

- 25 issues closed
- 10-15 PRs merged
- 80%+ code coverage
- Complete domain layer
- Working database infrastructure
- CI/CD pipeline operational
- Integration tests passing
- 3,000+ lines of code
- ~100 hours AI time invested
- ~15 hours human review time

---

## Need Help?

1. Check documentation first
2. Search existing issues
3. Create new issue with `question` label
4. Tag with `needs-discussion` if decision required

---

## 🚀 Ready to Start?

Your project is fully set up! Time to start building.

**Recommended first action**:

```bash
# View Issue #1
gh issue view 1

# Assign to yourself (or AI if using automation)
gh issue edit 1 --add-assignee @me

# Move to In Progress on project board (via UI)

# Start implementing!
```

**Happy Coding!** 🎉
