# Spec: Armor Filter Page

## Overview
Main page (`/`) of the app. Displays all HD2 armor as image tiles, grouped by weight class, with filters for passives and feature tags, and a live text search.

## Layout

### Grouping
Armor tiles are grouped under weight class headings in order:
1. **Light**
2. **Medium**
3. **Heavy**

Each group heading is followed by a horizontal-wrapping tile grid. Groups with no matching armor are hidden entirely.

### Tile Design (`ArmorTile` component)
Each tile displays (left to right / top to bottom):
- **Armor image** — `object-fit: cover` fills the fixed-width image column
- **Armor name**
- **Passive ability** — icon + name; clicking the passive name adds it as an active filter
- **Stats** — AR / SPD / STA, color-coded high/low relative to weight-class defaults
- **Feature tags** — small clickable chips; clicking a tag adds it as an active filter
- **Passive short description** — condensed description of the passive's effect
- **Unsorted banner** (conditional) — shown on tiles with `SortOrder >= 9000`; yellow-tinted background/border; links to `/unsorted`

Tiles are clickable for selection (visual highlight only; no current action on selection).

### Filter Bar
Sticky below the header (`position: sticky; top: 52px`). Contains:

**Search box**
- Live text filter on keystroke; × button to clear
- Matches: armor display name, passive name, passive description, feature tag key and formatted label

**Passive filter (custom dropdown)**
- Lists all passives with icons, alphabetically
- Selecting a passive adds it as a removable pill below the controls
- Already-active passives are hidden from the list

**Feature tag filter (native `<select>`)**
- Lists all feature tags, formatted (e.g. `fire-protection` → `Fire Protection`)
- Selecting a tag adds it as a removable pill
- Already-active tags are hidden from the list

**Active filters row**
- Passive pills show the passive icon + name with an × to remove
- Feature tag pills show the formatted tag name with an × to remove
- "Clear all" button removes all active filters and clears the search

**Selection bar** (conditional)
- Shown when one or more tiles are selected; displays count and a "Clear selections" button

### Filter Logic
- **AND semantics:** armor must satisfy ALL active filters to appear
- Passive filter: exact match on armor's `passive` field
- Feature tag filter: match via `PassiveInfo.AbilityTags` (resolved at load time — no runtime dictionary lookup)
- Text search: OR across display name, passive name, passive description, and all ability tags (both raw key and formatted label)

## Sort Order
Within each weight class, tiles are ordered by `sortOrder` (ascending). Values start at 10, increment by 10 to match in-game ordering. Gaps allow insertion without renumbering. Armor with `sortOrder >= 9000` is considered unsorted and also surfaces on the `/unsorted` page.

## Data
All data is loaded once by `ArmorDataService` and cached for the session:
- `wwwroot/data/armorsets.json` — armor records
- `wwwroot/data/armorpassives.json` — passive definitions including `abilityTags` (replaces the former `feature-tags.json`)
- `wwwroot/images/armor/` — armor renders
- `wwwroot/images/armorpassives/` — passive icons

After loading, each `ArmorSet` has its `PassiveInfo` (`ArmorPassive?`) populated via a LINQ `Select` with `with { }` — no per-render dictionary lookups needed.

## Unsorted Page (`/unsorted`)
A companion page showing only armors with `SortOrder >= 9000`, grouped by weight class and ordered alphabetically. Includes a hint explaining how to report sort positions (note the two armors before and two after in-game, then open an issue).

## Out of Scope (see TODOS.md)
- Warbond/acquisition source display
- Collapsible weight class sections
- Owned armor tracking / hide-unowned filter
