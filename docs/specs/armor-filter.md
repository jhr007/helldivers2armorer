# Spec: Armor Filter Page

## Overview
Main page of the app. Displays all HD2 armor as image tiles, grouped by weight class, with chip-based filters for passives and feature tags.

## Layout

### Grouping
Armor tiles are grouped under weight class headings in order:
1. **Light**
2. **Medium**
3. **Heavy**

Each group heading is followed by a horizontal-wrapping tile grid. If a filter is active and no armor in a group matches, the group heading is hidden.

### Tile Design (HD2 aesthetic — dark, military)
Each tile displays:
- Armor image (copied locally from source repo)
- Armor name
- Weight class badge (Light / Medium / Heavy)
- Passive ability name + icon
- Feature tags as small chips

### Filter Bar
Located above the tile grid. Contains:

**Passive filter (dropdown)**
- Lists all passive abilities with icons
- Selecting a passive adds it as a removable pill/chip below the dropdown
- Multiple passives can be selected

**Feature tag filter (dropdown)**
- Lists all feature tags (fire-protection, electrical-protection, etc.)
- Selecting a tag adds it as a removable pill/chip
- Multiple tags can be selected

**Active filters display**
- All active filters (both passives and feature tags) shown as pills with an × to remove
- Removing a pill deactivates that filter immediately

### Filter Logic
- AND semantics: armor must satisfy ALL active filters to appear
- A passive filter matches if the armor's passive equals the selected passive
- A feature tag filter matches if the armor's passive maps to that tag (via `feature-tags.json`)
- Passives and feature tags from the same underlying passive are not double-counted (selecting "fire-protection" and "Inflammable" simultaneously shows only armor with Inflammable)

## Sort Order
Armor within each weight class is sorted by `sortOrder` field (ascending). Values start at 10, increment by 10, matching in-game ordering. Gaps allow manual insertion without renumbering.

## Data Source
- `Helldivers2Armorer/data/armorsets.json` — armor records
- `Helldivers2Armorer/data/feature-tags.json` — passive → feature tag mapping
- `Helldivers2Armorer/wwwroot/images/armor/` — armor images
- `Helldivers2Armorer/wwwroot/images/armorpassives/` — passive icons

## Out of Scope (see TODOS.md)
- Live text search across all fields
- Warbond/acquisition source display
