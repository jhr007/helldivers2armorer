# Todos / Backlog

## In Progress
_nothing yet_

## Planned
- [x] Set up GitLab CI pipeline (build, mirror to GitHub, push to gh-pages)
- [x] Dynamic base path — served correctly for GitHub Pages (`/helldivers2armorer/`) and Docker (`/`)
- [x] Live text filter — searches armor name, passive, description, and feature tags; live on keystroke with X to clear
  - [ ] Also search `shortDescription` field
- [x] Set up GitLab → GitHub repository mirroring

## Someday / Nice to Have

- **Owned armor tracking** — let users mark which armors they own; persist in `localStorage`; surface in About that this is stored locally in the browser.
  - [ ] Checkbox on each tile for mass-editing; checkbox column toggled on/off via a filter bar control
  - [ ] `...` tile menu: toggle owned status; link to the armor's wiki page
  - [ ] Filter bar checkbox: "Hide unowned"

- **Armor info expander** — popup/panel triggered from the `...` tile menu showing additional armor detail (e.g. full passive description, acquisition source) without affecting tile layout or grid spacing. My just a mouseover icon

- **B-01 Tactical duplicate skins** — the B-01 Tactical armor has 3 variants with different skins but identical stats; all three need to be added as separate entries so they can be matched to the correct in-game sort position. Currently only one entry exists which makes sort-order placement ambiguous.

- **Collapsible weight sections** — allow Light / Medium / Heavy sections on the home page to be collapsed/expanded so users can focus on one weight class at a time.

- **Images and attribution**
  - [ ] Add image creator credits and license/usage details to the About page for all sourced assets
  - [ ] Update armor renders with higher-quality versions from [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2); images live in `Helldivers2Armorer/wwwroot/images/armor/`
  - [ ] Resolve placeholder passive icons — two icons carry a "wiki-use only" restriction from creator [User:Dogo314](https://helldivers.wiki.gg/wiki/User:Dogo314); need alternatives (game file extraction or permission request):
    - `Acclimated` — using `inflammable.png`; proper icon: [`Acclimated_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Acclimated_Armor_Passive_Icon.svg)
    - `Oxygenator` — using `extrapadding.png`; proper icon: [`Oxygenator_Armor_Passive_Icon.svg`](https://helldivers.wiki.gg/wiki/File:Oxygenator_Armor_Passive_Icon.svg)

- **Tile selection UX** — clicking a tile to select it sets user expectations that something will happen. Either add a comparison feature or remove tile selection.
  - Idea: select 2 armors → show a comparison panel highlighting stat differences

- **Wiki source link per armor** — show where to acquire each armor piece (warbond, premium, etc.). The [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2) is the reference source.

- **Armor acquisition source** — display warbond/source on each tile so users know if they own or can get the armor.

- **Matching helmet/cape** — helmets and capes aren't grouped by armor set in the source data; would require a manual mapping or a richer data source.
