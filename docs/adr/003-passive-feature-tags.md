# ADR-003: Passive Abilities → Feature Tag Mapping

**Date:** 2026-04-24
**Status:** Accepted (mapping is a living document — extend as needed)

## Context
Armor protection data in the source (`hd2-random-strat`) is not stored as explicit fields like `fireProtection: true`. Protection and utility come entirely from **passive abilities** (e.g., "Inflammable", "Engineering Kit"). Users want to filter by human-readable features like "fire protection" or "throwing range", not just passive names.

## Decision
Two separate filter dimensions:
1. **Passives filter** — filter directly by passive name, shown with passive icons
2. **Feature tags filter** — filter by human-readable tags derived from a static mapping

The mapping lives in `Helldivers2Armorer/data/feature-tags.json` and maps each passive to one or more feature tag strings. This file is the source of truth and should be updated when new passives are added or game descriptions are clarified.

## Initial Mapping (verify against game — may be incomplete)

| Passive | Feature Tags |
|---|---|
| Inflammable | `fire-protection` |
| Electrical Conduit | `electrical-protection` |
| Integrated Explosives | `explosive-protection`, `bonus-throwables` |
| Fortified | `explosive-protection`, `recoil-reduction` |
| Engineering Kit | `bonus-throwables`, `throwing-range` |
| Siege Ready | `throwing-range` |
| Servo-Assisted | `throwing-range`, `limb-protection` |
| Peak Physique | `throwing-strength`, `melee-bonus` |
| Advanced Filtration | `gas-protection` |
| Acclimated | `environmental-protection` |
| Extra Padding | `ballistic-protection` |
| Ballistic Padding | `ballistic-protection` |
| Concussive Padding | `concussive-protection` |
| Scout | `reduced-detection` |
| Reduced Signature | `reduced-detection` |
| Med-Kit | `medkit-bonus` |
| Democracy Protects | `random-survival` |
| Unflinching | `stagger-resistance` |
| Gunslinger | `sidearm-reload` |
| Feet First | `dive-bonus` |
| Adreno-Defibrillator | `revive-bonus` |
| Supplementary Adrenaline | `stamina-bonus` |
| Rock Solid | _unverified — update when confirmed_ |
| Desert Stormer | _unverified — update when confirmed_ |
| Reinforced Epaulettes | _unverified — update when confirmed_ |

## Consequences
- Feature tags can evolve independently of passive names
- Mapping must be manually maintained when the game adds or changes passives
- Unverified entries should be confirmed against the [Helldivers Wiki](https://helldivers.wiki.gg/wiki/Helldivers_2) before use
