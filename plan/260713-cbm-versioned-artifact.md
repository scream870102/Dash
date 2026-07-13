---
title: CBM Versioned Artifact
date: 2026-07-13
status: done
tags: [plan]
---

# Goal

Create a version-control-ready Codebase Memory graph artifact for this repository and check whether the installed CBM version has an update.

# Steps

- [x] Run a full repository index with persistence enabled.
- [x] Install the approved `zstandard` dependency.
- [x] Export `.codebase-memory/graph.db.zst` from the current SQLite index.
- [x] Verify Zstandard decompression, SQLite integrity, and Git visibility.
- [x] Check the installed CBM version against the available update without upgrading it.

# Affected Files

- `plan/260713-cbm-versioned-artifact.md`
- `.codebase-memory/graph.db.zst`

# Open Questions

None.
