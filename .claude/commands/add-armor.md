# /add-armor

Fetch a new armor set or passive from the Helldivers Wiki and add it to the local data files.

## Usage

```
/add-armor <armor display name>
/add-armor <passive name>
```

**Examples:**
- `/add-armor "O-3 Free Spirit"`
- `/add-armor "Oxygenator"`

---

## What to do

The user has provided a name: **$ARGUMENTS**

### Step 1 — Determine what's being added

Check whether `$ARGUMENTS` is an armor set or a passive:
- If it matches an armor name pattern (e.g. "XX-## Name") it's an armor set
- If it's a passive name (e.g. "Oxygenator", "Scout") it's a passive
- It could be both — a passive might bring in the armors that use it

### Step 2 — Fetch from wiki

Fetch `https://helldivers.wiki.gg/wiki/<Name_with_underscores>` (replace spaces with underscores).

For an **armor set**, extract:
- displayName (exact, e.g. "O-3 Free Spirit")
- weight class (Light / Medium / Heavy)
- passive name (exact)
- armorRating, speed, stamina stats
- imageURL — fetch `https://helldivers.wiki.gg/wiki/File:<DisplayName_underscored>_Armor_Render.png` to get the real image URL, then download it to `Helldivers2Armorer/wwwroot/images/armor/<internalname>.png`

For a **passive**, extract:
- displayName (exact)
- full description text (preserve newlines between separate effects)
- short description (abbreviated, one line per effect)
- image icon — check `https://helldivers.wiki.gg/wiki/File:<PassiveName_underscored>_Armor_Passive_Icon.svg`, note usage restriction if present
- derive abilityTags from the description (reuse existing tags where possible, create new ones if needed)

### Step 3 — Assign internalName and sortOrder

- **internalName**: lowercase, no spaces or punctuation, e.g. "O-3 Free Spirit" → `o3freespirit`
- **sortOrder**: `9900` (or the next available multiple of 10 above 9900 if others exist at 9900). This marks it as unsorted — in-game position unknown.
- **index**: next integer after the current highest index in the relevant JSON file

### Step 4 — Check if passive exists

Read `Helldivers2Armorer/wwwroot/data/armorpassives.json`. If the passive for this armor doesn't exist yet, add it first (Step 3 for passives), then add the armor.

### Step 5 — Update JSON files

**For a new armor** — append to `Helldivers2Armorer/wwwroot/data/armorsets.json`:
```json
{
  "displayName": "...",
  "type": "Equipment",
  "category": "armor",
  "tags": [ "Light|Medium|Heavy" ],
  "armorRating": 0,
  "speed": 0,
  "stamina": 0,
  "passive": "...",
  "warbondCode": "unknown",
  "internalName": "...",
  "imageURL": "...",
  "tier": "b",
  "index": 0,
  "sortOrder": 9900
}
```

**For a new passive** — append to `Helldivers2Armorer/wwwroot/data/armorpassives.json`:
```json
{
  "displayName": "...",
  "type": "Equipment",
  "category": "armor",
  "tags": [ "ArmorPassive" ],
  "warbondCode": "unknown",
  "internalName": "...",
  "imageURL": "...",
  "tier": "b",
  "index": 0,
  "description": "...",
  "shortDescription": "...",
  "abilityTags": []
}
```

### Step 6 — Verify and test

Run publish + E2E tests:
```
dotnet publish Helldivers2Armorer/Helldivers2Armorer.csproj --configuration Release --output publish/
dotnet test Helldivers2Armorer.E2E/Helldivers2Armorer.E2E.csproj --configuration Release
```

All 9 tests must pass before reporting done.

### Step 7 — Update TODOS.md

Add the new armor to the "Sort Order — Unowned Armors" section under its weight class, noting its source (warbond name / Superstore / etc.).

### Step 8 — Commit

Stage and commit everything (JSON, images, TODOS) with a message like:
```
data: add <Name> (<weight class>/<passive>)
```
