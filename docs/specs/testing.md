# Spec: E2E Testing

## Test Project
`Helldivers2Armorer.E2E` — xUnit v3 + Playwright, self-contained linux-x64 publish for CI.

Follows the CamShare pattern at `c:/repos/CamShare/CamShare.E2E/`:
- `Fixtures/AppFixture.cs` — static file Kestrel host (see ADR-004)
- `Fixtures/BrowserFixture.cs` — shared Chromium instance (no fake media flags needed)
- `Fixtures/E2ECollection.cs` — xUnit collection sharing both fixtures
- `Tests/PageLoadTests.cs` — all tests live here (scope is small)

## Test Scenarios

All tests are pre-deploy (no `Category=Smoke` trait — no post-deploy smoke stage for this project).

### Page Load Tests (`PageLoadTests.cs`)
| Test | What it checks |
|---|---|
| `HomePage_Loads` | Navigate to `/`, HTTP 200, page has content (no blank WASM shell) |
| `AboutPage_Loads` | Navigate to `/about`, page renders, credits text present |
| `HomePage_HasWeightClassGroups` | "Light", "Medium", "Heavy" headings visible |
| `HomePage_HasFilterUI` | Passive filter dropdown and feature tag filter dropdown present |

## What Is Not Tested
- Filter interaction logic (selecting/removing chips, AND semantics) — deferred to TODOS
- Tile image loading — network-level, not meaningful in E2E
- Sort order correctness — verified manually against in-game order

## CI
Stage: `e2e` (runs after `build`, before `pages`/`docker`).
Image: `mcr.microsoft.com/playwright/dotnet:v1.58.0-noble`
Environment variable: none required (tests run against the local static host, not a deployed URL).
