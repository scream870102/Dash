---
title: Repair Codex Windows Sandbox Helper
date: 2026-07-13
status: done
tags: [plan]
---

# Goal

Restore the missing Codex Windows sandbox helper so normal workspace commands and patches run without escalation.

# Steps

- [x] Identify the installed Codex distribution and expected helper location.
- [x] Repair the missing helper using the matching official installation mechanism.
- [x] Verify a non-escalated workspace command succeeds.
- [x] Verify `apply_patch` succeeds and reread the changed file.

# Affected Files

- Codex installation files outside the repository, as identified during diagnosis.
- `plan/260713-repair-codex-sandbox-helper.md`

# Open Questions

None.
