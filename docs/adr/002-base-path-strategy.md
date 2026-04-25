# ADR-002: Base Path Strategy — Two Publish Targets

**Date:** 2026-04-24
**Status:** Accepted (CI not yet implemented)

## Context
Blazor WASM bakes `<base href>` into `index.html` at publish time via `--pathbase`. Two hosting environments need different base paths:
- **GitHub Pages:** `https://jhr.github.io/Helldivers2Armorer/` → base path `/Helldivers2Armorer/`
- **Homelab Docker:** served from root → base path `/`

## Options Considered

| Option | Notes |
|---|---|
| Single build, base path `/` | Works for Docker; GitHub Pages sub-path routing breaks |
| Runtime `config.json` | Most flexible, but requires JS interop before Blazor boots — complex |
| Two separate `index.html` files maintained manually | Error-prone; easy to forget to update both |
| Two `dotnet publish` steps with `--pathbase` | Clean; same source, CI handles the difference; `index.html` generated correctly each time |

## Decision
**Two `dotnet publish` steps in CI**, one per environment, using `--pathbase`. Assets (hashed filenames) are identical between builds; only `index.html` differs.

## Current State
CI pipeline not yet set up. Local dev and initial deployment use `/` (default). See `docs/TODOS.md` for dynamic runtime base path as a future alternative.

## Consequences
- No manual `index.html` maintenance
- CI pipeline must run two publish jobs and route artifacts to the correct target
- If a third environment is added, it needs a third publish step
