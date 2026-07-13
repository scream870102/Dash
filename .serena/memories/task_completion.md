# Task Completion Checklist

No CLI linter/formatter/test runner configured. When a coding task is done:

1. Code must compile in Unity Editor 2020.2.6f1 — if Editor open with unityMCP bridge, use it to check console errors; else ask user to confirm compile, or (only if asked) batchmode compile per `mem:suggested_commands`
2. New/deleted/moved asset files: verify `.meta` consistency (see `mem:conventions`)
3. Touched `.inputactions`-generated code? Regenerate in Editor, never hand-edit
4. No commit/push unless user asks (global rule)
5. Plan file in `plan/` (if one was created): set frontmatter `status: done` (per `AGENTS.md`)
