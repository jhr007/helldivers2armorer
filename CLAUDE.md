# Helldivers 2 Armorer — Claude Context

## Project Summary
Static Blazor WASM app for filtering and browsing Helldivers 2 armor by passive abilities and feature tags (fire protection, electrical, explosive, throwing range, etc.). Armor is grouped by weight class (Light / Medium / Heavy) and displayed as image tiles.

## Tech Stack
- **Framework:** Blazor WebAssembly, .NET 10
- **Hosting:** GitHub Pages (primary) + homelab Docker (secondary)
- **CI/CD:** Local GitLab instance (`ssh://git@gitlab.lan:7122/jhr/helldivers2armorer.git`) — builds and mirrors to GitHub, pushes artifacts to `gh-pages` branch
- **GitHub Pages URL:** `https://jhr.github.io/Helldivers2Armorer/`

## Data
- Armor data and images copied locally from [hd2-random-strat](https://github.com/Selenestica/hd2-random-strat) by Selenestica
- Source of truth is local JSON files in `Helldivers2Armorer/data/`
- Images live in `Helldivers2Armorer/wwwroot/images/`
- Armor has a `sortOrder` field (starts at 10, increments by 10) to match in-game ordering — edit manually to reorder

## Key Docs
- `docs/adr/` — Architecture Decision Records (why decisions were made)
- `docs/specs/` — Feature specifications (what the system does)
- `docs/TODOS.md` — Deferred/someday backlog

## Testing
- E2E only: `Helldivers2Armorer.E2E/` — xUnit v3 + Playwright
- AppFixture serves published static WASM output via a minimal Kestrel static file host (see ADR-004)
- Reference pattern: `c:/repos/CamShare/CamShare.E2E/` — same fixture structure, same CI image
- Scope: page load / no-404 checks only. No smoke stage in CI.

### Run tests after making changes
After any non-trivial change, and before commit/push, publish the app and run the E2E suite:
```
dotnet publish Helldivers2Armorer/Helldivers2Armorer.csproj --configuration Release --output publish/
dotnet test Helldivers2Armorer.E2E/Helldivers2Armorer.E2E.csproj --configuration Release
```
All 4 tests must pass before considering the work done.

## Memory (Memorizer MCP)
This project has a dedicated workspace and project in the Memorizer MCP server. Use it to store and retrieve context across sessions — it persists independently of file-based memory.

**Workspace:** `Helldivers2Armorer` (ID: `5ed1398c-a77e-4ab1-a1c4-ce55437b95fe`)
**Project:** `Initial Build` (ID: `0f98bdfb-b0db-4cec-92fe-cb4bd2f83c23`)
**Web UI:** `http://memorizer.lan:5000`

### When to search
Before starting any non-trivial task, search for relevant context:
```
mcp__memorizer__search_memories(query: "...", projectId: "0f98bdfb-b0db-4cec-92fe-cb4bd2f83c23")
```

### When to store
After any significant decision, mapping, or discovery that isn't obvious from the code:
```
mcp__memorizer__store(type: "reference", source: "LLM", projectId: "0f98bdfb-b0db-4cec-92fe-cb4bd2f83c23", title: "...", text: "...", tags: [...])
```

### Useful tags
`architecture`, `reference`, `data`, `feature-tags`, `ci-cd`, `how-to`, `decision`

## Tooling
- **dotnet local tools manifest:** `dotnet-tools.json` at repo root (not `.config/`) — contains GitVersion and Slopwatch
- **Static assets (fonts):** managed by libman via `Helldivers2Armorer/libman.json` + `Microsoft.Web.LibraryManager.Build` NuGet package; restores automatically on `dotnet build` — no extra CI step needed. Fonts land in `Helldivers2Armorer/wwwroot/fonts/`.

## Conventions
- Sort order: numeric, starts at 10, increments by 10 — leave gaps for easy insertion
- All filter logic uses AND semantics (armor must match ALL selected filters)
- Passive → feature tag mapping is defined in `Helldivers2Armorer/data/feature-tags.json`
- Do not add error handling for impossible states; trust Blazor's component model
- No comments unless the WHY is non-obvious
