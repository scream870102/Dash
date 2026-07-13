# Suggested Commands

Unity project — build/run/test happen inside Unity Editor 2020.2.6f1, not CLI. No npm/dotnet CLI workflow.

- Open project: launch Unity Hub with Editor 2020.2.6f1, open `D:\Project\Dash`
- Batch compile check (only if user asks; slow): `& "<UnityEditorPath>\Unity.exe" -batchmode -quit -projectPath D:\Project\Dash -logFile -` (verify editor path first, don't guess)
- unityMCP server in `.mcp.json` (http://127.0.0.1:8080/mcp) only works while Unity Editor runs its MCP bridge; port closed = Editor not running

## Windows shell notes

- Primary shell PowerShell 7+; Git Bash available
- Use `Get-ChildItem`, `Select-String` equivalents handled by agent tools; forward slashes fine in most tools
- Git works as usual: `git status`, `git log --oneline`
