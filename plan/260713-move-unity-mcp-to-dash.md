---
title: Move Unity MCP to Dash
date: 2026-07-13
status: done
tags: [plan]
---

# Goal

將 `unityMCP` 從 Codex global 設定移到 Dash 專案設定，使它只在 `D:\Project\Dash` 啟用。

# Steps

- [x] 確認 global 與 Dash 專案目前的 MCP 設定。
- [x] 從 global config 移除 `unityMCP` 區塊。
- [x] 將同一區塊加入 Dash `.codex/config.toml`。
- [x] 重讀兩份設定並依官方設定層級確認載入範圍。

# Affected files

- `C:\Users\screa\.codex\config.toml`
- `D:\Project\Dash\.codex\config.toml`
- `D:\Project\Dash\plan\260713-move-unity-mcp-to-dash.md`

# Open questions

無。
