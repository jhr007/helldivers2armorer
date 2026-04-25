# Todos / Backlog

## In Progress
_nothing yet_

## Planned
- Set up GitLab CI pipeline (build, mirror to GitHub, push to gh-pages)
- Set up GitLab → GitHub repository mirroring

## Someday / Nice to Have
- **Dynamic base path at runtime** — serve a `config.json` from the container with the base path, read it before Blazor initializes. Currently hardcoded to `/`; two publish targets (`--pathbase`) handle GitHub Pages vs Docker. See ADR-002.
- **Live text filter** — a single text input that filters across all active filter dimensions as you type
- **Wiki source link per armor** — show where to acquire each armor piece (warbond, premium, etc.). The [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2) is the reference source.
- **Armor acquisition source** — display warbond/source on each tile so users know if they own or can get the armor
