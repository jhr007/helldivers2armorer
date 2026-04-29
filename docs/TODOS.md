# Todos / Backlog

## In Progress
_nothing yet_

## Planned
- ~~Set up GitLab CI pipeline (build, mirror to GitHub, push to gh-pages)~~ — done
- Set up GitLab → GitHub repository mirroring

## Sort Order — Unowned Armors
Record in-game position for these when acquired and update `sortOrder` in `armorsets.json` (currently parked at 9000+).

**Light**
- AD-11 Livewire
- CE-67 Titan
- DS-10 Big Game Hunter
- GS-11 Democracy's Deputy
- O-3 Free Spirit *(new — Exo Experts warbond)*
- RS-100 Sanctioner

**Medium**
- AC-1 Dutiful
- AF-91 Field Chemist
- CE-81 Juggernaut
- DP-8 Mountain-Scaled
- FS-34 Exterminator
- O-44 Bonded Pilot *(new — Superstore)*
- RS-6 Fiend Destroyer
- SA-15 Drone Master
- TR-7 Ambassador of the Brand
- TR-9 Cavalier of Democracy
- UF-84 Doubt Killer

**Heavy**
- AF-52 Lockdown
- BP-77 Grand Juror
- CE-101 Guerilla Gorilla
- CPR-80 Bulwark
- FS-11 Executioner
- FS-61 Dreadnought
- O-2 Heavy Operator *(new — Exo Experts warbond)*
- RE-824 Bearer of the Standard
- SR-64 Cinderblock
- TR-62 Knight

## Someday / Nice to Have
- **Tile selection UX** — clicking a tile to select it sets user expectations that something will happen (e.g. compare two armors side-by-side, view a detail panel). Currently selection has no payoff. Either add a comparison feature or remove tile selection.
  - Idea: select 2 armors → show a comparison panel highlighting stat differences

- **Update armor images from wiki** — replace current images with higher-quality versions from [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2); images live in `Helldivers2Armorer/wwwroot/images/armor/`
- **Missing passive icons** — two passive icons are placeholders; the wiki icons have a "wiki-use only" restriction from their creator (User:Dogo314), so alternatives are needed (game file extraction or permission request):
  - `Acclimated` — currently using `inflammable.png`; proper icon: [`Acclimated_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Acclimated_Armor_Passive_Icon.svg)
  - `Oxygenator` — currently using `extrapadding.png`; proper icon: [`Oxygenator_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Oxygenator_Armor_Passive_Icon.svg)
- **Missing armor images** — three new Oxygenator armors need images in `wwwroot/images/armor/`: `o3freespirit.webp`, `o44bondedpilot.webp`, `o2heavyoperator.webp`


- **Switch data source to [helldivers-2/json](https://github.com/helldivers-2/json)** — community-maintained JSON API; evaluate schema compatibility before migrating
- **Dynamic base path at runtime** — serve a `config.json` from the container with the base path, read it before Blazor initializes. Currently hardcoded to `/`; two publish targets (`--pathbase`) handle GitHub Pages vs Docker. See ADR-002.
- ~~**Live text filter**~~ — done: searches armor name, passive, and feature tags; live on keystroke with X to clear
- **Wiki source link per armor** — show where to acquire each armor piece (warbond, premium, etc.). The [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2) is the reference source.
- **Armor acquisition source** — display warbond/source on each tile so users know if they own or can get the armor
- **Matching helmet/cape** — helmets and capes aren't grouped by armor set in the source data; would require a manual mapping or a richer data source

