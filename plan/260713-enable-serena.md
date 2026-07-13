---
title: Enable Serena
date: 2026-07-13
status: done
tags: [plan]
---

# Goal

Enable Serena as a project-scoped MCP server for Claude Code and Codex without changing the existing Codebase Memory configuration.

# Steps

- [x] Run the bundled project initialization script with `-SkipCbm`.
- [x] Verify `.mcp.json` contains the Serena server.
- [x] Verify `.codex/config.toml` contains the Serena server.
- [x] Confirm existing MCP configuration remains present.

# Affected Files

- `.mcp.json`
- `.codex/config.toml`
- `plan/260713-enable-serena.md`

# Open Questions

None.
