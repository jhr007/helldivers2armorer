# Spec: About Page

## Overview
Describes the app's purpose, acknowledges data/asset sources, and links to community resources. Accessible via nav link.

## Content

### Purpose
A brief statement at the top explaining why the app exists — finding armor by protection type (fire, gas, acid, explosives) without clicking through everything in game.

### Credits
Subtitle: "Created by standing on the shoulders of giants, thank you:"

- **Data & Images:** [hd2-random-strat](https://github.com/Selenestica/hd2-random-strat) by Selenestica
- **Acclimated passive icon:** Hand-traced from Helldivers 2 game assets by [Dogo314](https://helldivers.wiki.gg/wiki/User:Dogo314) via the Helldivers Wiki. Used with attribution under Arrowhead Game Studios' EULA.

### Resources
- [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2) — authoritative reference for armor stats, passives, and acquisition

## Notes
- No longer contains an Unsorted Armor section — that has its own page at `/unsorted`
- Page heading is `h2` (not `h1`) to prevent Blazor's `FocusOnNavigate` from auto-focusing and outlining it on navigation
- No data injection needed; page is static markup only
