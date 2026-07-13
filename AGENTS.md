# Agent Guide (core)

Common rules for all agents (Claude Code / Codex / Antigravity); source of truth for this project.
Agent-specific rules: Claude Code also reads `agents/claude.md`, Codex reads `agents/codex.md`, Antigravity reads `agents/antigravity.md`. The specific file wins on conflict.

## Plan export

After a plan is approved and before implementation starts (or whenever the user asks to export), save it to `plan/<YYMMDD>-<kebab-title>.md` (YYMMDD = today's date; kebab-title = short kebab-case summary of the plan).

- Skip for small tasks: ≤2 files changed and no irreversible action (delete, external send, commit/push, config change).
- Format: Obsidian Markdown (use the `obsidian-markdown` skill if available). Frontmatter: `title`, `date`, `status` (`planned`/`in-progress`/`done`), `tags: [plan]`. Body: goal, steps, affected files, open questions.
- When execution finishes, set `status` to `done`.

## Token discipline

- Replies minimal: drop pleasantries, filler, and repetition; never cut technical substance.
- Code: simplest working solution — first question whether the requirement needs to exist (YAGNI); stdlib and platform-native before dependencies and custom code.
- Write normally (no compression) for: security warnings, irreversible-action confirmations, multi-step ordering, code / commits / PRs.

## Obsidian vault

Conversation logs and note organizing go to a separate Obsidian vault, not this working directory.
Vault path: **(not set)** — if unset or uncertain, ask the user first; never guess or write into the working directory. Once given, write the path back into this field and don't ask again.

<!-- codebase-memory-mcp:start -->
# Codebase Knowledge Graph (codebase-memory-mcp)

This project uses codebase-memory-mcp to maintain a knowledge graph of the codebase.
ALWAYS prefer MCP graph tools over grep/glob/file-search for code discovery.

## Priority Order

1. `search_graph` — find functions, classes, routes, variables by pattern
2. `trace_path` — trace who calls a function or what it calls
3. `get_code_snippet` — read specific function/class source code
4. `query_graph` — run Cypher queries for complex patterns
5. `get_architecture` — high-level project summary

## When to fall back to grep/glob

- Searching for string literals, error messages, config values
- Searching non-code files (Dockerfiles, shell scripts, configs)
- When MCP tools return insufficient results

## Examples

- Find a handler: `search_graph(name_pattern=".*OrderHandler.*")`
- Who calls it: `trace_path(function_name="OrderHandler", direction="inbound")`
- Read source: `get_code_snippet(qualified_name="pkg/orders.OrderHandler")`
<!-- codebase-memory-mcp:end -->
