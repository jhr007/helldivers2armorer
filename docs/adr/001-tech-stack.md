# ADR-001: Tech Stack — Blazor WASM + .NET 10

**Date:** 2026-04-24
**Status:** Accepted

## Context
Need a filterable, tiled armor browser. Requirements: static hosting (no server), image-heavy UI, owner is a .NET developer, two hosting targets (GitHub Pages, homelab Docker).

## Options Considered

| Option | Notes |
|---|---|
| Plain HTML/JS | Fast, zero build step, but state management and filter logic become messy quickly |
| React / Vue SPA | Great ecosystem, but introduces a JS toolchain the owner doesn't prefer |
| Blazor WASM (.NET 10) | C# throughout, static output, strong component model, familiar to owner |

## Decision
**Blazor WebAssembly targeting .NET 10.**

## Consequences
- Build produces static files deployable anywhere (GitHub Pages, Docker static serve)
- Initial load is heavier than a JS SPA (WASM runtime download), acceptable for this use case
- .NET 10 is current; revisit if LTS alignment becomes a concern
