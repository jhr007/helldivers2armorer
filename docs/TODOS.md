# Todos / Backlog

## In Progress
_nothing yet_

## Planned
- [x] Set up GitLab CI pipeline (build, mirror to GitHub, push to gh-pages)
- [x] Dynamic base path — served correctly for GitHub Pages (`/helldivers2armorer/`) and Docker (`/`)
- [x] Live text filter — searches armor name, passive, description, and feature tags; live on keystroke with X to clear
  - [x] Also search `shortDescription` field
- [x] Set up GitLab → GitHub repository mirroring

## Someday / Nice to Have

- **Owned armor tracking** — let users mark which armors they own; persist in `localStorage`; surface in About that this is stored locally in the browser.
  - [x] `...` tile menu: toggle owned status; link to the armor's wiki page
  - [x] Checkbox on each tile for mass-editing; checkbox column toggled on/off via a filter bar control ("Owned" button)
  - [x] Filter bar button: "Show owned" — filters grid to owned armors only (requires at least one owned armor to appear)

- **Armor info expander** — [x] basic implementation: `...` tile menu links to the armor's wiki page (full passive description, acquisition source visible there)

- **B-01 Tactical duplicate skins** — the B-01 Tactical armor has 3 variants with different skins but identical stats; all three need to be added as separate entries. BLOCKED: no image assets for the other two skins.

- **Collapsible weight sections** — [x] Light / Medium / Heavy sections are collapsible; collapse button also shows armor count.

- **Images and attribution**
  - [ ] Add image creator credits and license/usage details to the About page for all sourced assets
  - [ ] Update armor renders with higher-quality versions from [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2); images live in `Helldivers2Armorer/wwwroot/images/armor/`
  - [ ] Resolve placeholder passive icons — two icons carry a "wiki-use only" restriction from creator [User:Dogo314](https://helldivers.wiki.gg/wiki/User:Dogo314); need alternatives (game file extraction or permission request):
    - `Acclimated` — using `inflammable.png`; proper icon: [`Acclimated_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Acclimated_Armor_Passive_Icon.svg)
    - `Oxygenator` — using `extrapadding.png`; proper icon: [`Oxygenator_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Oxygenator_Armor_Passive_Icon.svg)

- **Tile selection UX** — [x] clicking 2 tiles shows a comparison panel highlighting stat differences; selecting a 3rd replaces the oldest.

- **Wiki source link per armor** — [x] each tile's `...` menu links to the armor's wiki page.

- **Armor acquisition source** — [x] `warbonds.json` created; `armorsets.json` updated to use `internalName` keys; warbond display name shown in the info modal detail panel.

- **Matching helmet/cape** — helmets and capes aren't grouped by armor set in the source data; would require a manual mapping or a richer data source.
