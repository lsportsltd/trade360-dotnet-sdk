# Memory Bank

This directory contains the **Memory Bank** for the Trade360 .NET SDK project - a structured knowledge base that helps AI assistants and team members maintain context about the project.

## What is Memory Bank?

Memory Bank is a system for preserving project knowledge across sessions and team members. It solves common problems like:

1. **AI Context Loss**: Model loses context between sessions
2. **Knowledge Transfer**: New team members need comprehensive onboarding
3. **Forgotten Solutions**: Previous designs and decisions get lost
4. **Session Continuity**: Work continues seamlessly across interruptions

## File Structure

### Core Files (Always Present)

| File | Purpose | Update Frequency |
|------|---------|------------------|
| **projectbrief.md** | Project goals, objectives, success metrics | Rarely (only when goals change) |
| **productContext.md** | Why project exists, problems solved, user experience | Quarterly |
| **systemPatterns.md** | Architecture, design patterns, technical decisions | When architecture changes |
| **techContext.md** | Technology stack, setup, constraints, dependencies | When tech stack changes |
| **activeContext.md** | Current work focus, recent changes, next steps | **Daily/Weekly** |
| **progress.md** | What works, what's left, known issues, metrics | **Daily/Weekly** |

### Update Frequency Guide

**📝 Update Frequently** (activeContext.md, progress.md):
- After completing major features
- When starting new work
- After resolving issues
- Weekly status updates

**🔄 Update Occasionally** (systemPatterns.md, techContext.md):
- When adding new dependencies
- When making architectural changes
- When adopting new patterns
- During major refactoring

**📌 Update Rarely** (projectbrief.md, productContext.md):
- When project scope changes
- When goals are redefined
- During strategic pivots

## How to Use Memory Bank

### For AI Assistants (Cursor, GitHub Copilot, etc.)

These files are automatically read by AI assistants to understand the project context. The AI will:

1. Read relevant files based on the task
2. Understand project structure and patterns
3. Make decisions consistent with existing architecture
4. Suggest solutions aligned with project goals

### For Team Members

#### New Team Member Onboarding
Read files in this order:
1. **projectbrief.md** - Understand the "why"
2. **productContext.md** - Learn what problems we solve
3. **techContext.md** - Set up your development environment
4. **systemPatterns.md** - Understand how we build
5. **progress.md** - See what's done and what's next
6. **activeContext.md** - Know what's happening now

#### Daily Workflow
1. Check **activeContext.md** for current focus
2. Update **progress.md** when completing tasks
3. Update **activeContext.md** when switching focus areas

#### Before Major Changes
1. Review **systemPatterns.md** for architectural guidance
2. Check **techContext.md** for technical constraints
3. Update both after implementing changes

## Using with Cursor IDE

### Initial Setup

1. **Add Memory Bank Rules** to your project:
   - Open Cursor Settings
   - Go to Rules → Add Project Rule
   - Add rule from: https://gist.githubusercontent.com/ipenywis/1bdb541c3a612dbac4a14e1e3f4341ab/raw/cursor-memory-bank-rules.md

2. **Initialize Memory Bank**:
   ```
   @.memory-bank create memory bank structure
   ```

3. **Update Memory Bank** (after making changes):
   ```
   @.memory-bank update memory bank with recent changes
   ```

### Common Commands

- `@.memory-bank what's the current focus?` - Check activeContext.md
- `@.memory-bank what's left to build?` - Check progress.md
- `@.memory-bank explain the architecture` - Check systemPatterns.md
- `@.memory-bank how do I set up the project?` - Check techContext.md

## Best Practices

### ✅ DO
- Keep files up to date with reality
- Be specific and concrete
- Include code examples when relevant
- Document decisions and their rationale
- Update after significant changes

### ❌ DON'T
- Let files become stale or outdated
- Write vague or generic content
- Duplicate information across files
- Include sensitive credentials or keys
- Make files too long (aim for scannable)

## File Size Guidelines

| File | Target Length | Max Length |
|------|---------------|------------|
| projectbrief.md | 1-2 pages | 3 pages |
| productContext.md | 2-3 pages | 5 pages |
| systemPatterns.md | 3-5 pages | 8 pages |
| techContext.md | 2-4 pages | 6 pages |
| activeContext.md | 1-2 pages | 3 pages |
| progress.md | 2-3 pages | 5 pages |

## Maintenance Schedule

### Daily
- [ ] Update activeContext.md if focus changed
- [ ] Update progress.md when completing tasks

### Weekly
- [ ] Review and update activeContext.md
- [ ] Update progress.md with completed work
- [ ] Check all files for accuracy

### Monthly
- [ ] Full review of all files
- [ ] Update metrics and KPIs
- [ ] Archive old active context items
- [ ] Update progress milestones

### Quarterly
- [ ] Comprehensive review
- [ ] Update productContext.md if needed
- [ ] Refresh techContext.md
- [ ] Review and update projectbrief.md

## Integration with Other Tools

### Git Hooks
Consider adding a pre-commit hook to remind about Memory Bank updates:

```bash
#!/bin/bash
# .git/hooks/pre-commit
echo "📝 Remember to update Memory Bank files if needed:"
echo "   - activeContext.md (current work)"
echo "   - progress.md (completed tasks)"
```

### CI/CD
- Validate Memory Bank files are up to date
- Check for broken links
- Ensure proper formatting

### Documentation
- Link from main README.md
- Reference in contributor guidelines
- Include in onboarding checklist

## Support & Questions

If you have questions about Memory Bank:
- See the original guideline: https://lsports-data.atlassian.net/wiki/spaces/LSPORTS/pages/1730281482/Memory+Bank
- Check the template: https://gist.github.com/ipenywis/1bdb541c3a612dbac4a14e1e3f4341ab
- Ask the team in your project's communication channel

---

**Last Updated**: November 3, 2025  
**Maintained By**: Trade360 SDK Team

