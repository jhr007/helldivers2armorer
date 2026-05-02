# Spec: E2E Testing

## Test Project
`Helldivers2Armorer.E2E` — xUnit v3 + Playwright, self-contained linux-x64 publish for CI.

Follows the CamShare pattern at `c:/repos/CamShare/CamShare.E2E/`:
- `Fixtures/AppFixture.cs` — static file Kestrel host (see ADR-004)
- `Fixtures/BrowserFixture.cs` — shared Chromium instance
- `Fixtures/E2ECollection.cs` — xUnit collection sharing both fixtures
- `Tests/PageLoadTests.cs` — page load and structural checks
- `Tests/FilterTests.cs` — search/filter behaviour regression tests

## Test Scenarios

All tests are pre-deploy (no `Category=Smoke` trait — no post-deploy smoke stage).

### Page Load Tests (`PageLoadTests.cs`)
| Test | What it checks |
|---|---|
| `HomePage_Loads_WithoutErrors` | Navigate to `/`, wait for `.filter-bar` or `.loading-state`, assert zero browser console errors |
| `HomePage_ShowsWeightClassHeadings` | Light, Medium, Heavy `.weight-heading` elements all present after load |
| `HomePage_ShowsFilterDropdowns` | One `.filter-select` and one `.passive-dropdown` present |
| `AboutPage_Loads_WithCredits` | Navigate to `/about`, `.about-page` renders, "Selenestica" and "helldivers.wiki.gg" in page content |

### Filter Tests (`FilterTests.cs`)
Regression suite covering bugs found during development:

| Test | What it checks |
|---|---|
| `Search_TagKeyword_ReturnsResultsAcrossAllWeightClasses` | Searching "fire" shows Light + Medium + Heavy groups (tag `fire-protection` matches across classes) |
| `Search_TagKeyword_FindsArmorWhoseNameAndPassiveLackTheKeyword` | I-09 Heatseeker (name/passive don't contain "fire") appears via `fire-protection` tag |
| `Search_TagKeyword_DoesNotShowArmorWithoutMatchingTag` | Searching "scout" excludes Engineering Kit armors (no matching tag) |
| `Search_PassiveName_ReturnsAcrossAllWeightClasses` | Searching "Fortified" shows all three weight classes (passive name match) |
| `Search_ClearButton_RestoresAllResults` | After filtering, clicking × restores the full tile count |

## What Is Not Tested
- Filter chip interaction (selecting/removing passive and tag pills) — deferred
- Tile image loading — network-level, not meaningful in E2E
- Sort order correctness — verified manually against in-game order
- `/unsorted` page content

## CI
Stage: `e2e` (runs after `build`, before `pages`/`docker`/`tag-release`).
Image: `mcr.microsoft.com/playwright/dotnet:v1.59.0-noble`
No environment variables required (tests run against the local static Kestrel host).
