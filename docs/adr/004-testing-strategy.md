# ADR-004: Testing Strategy — Playwright E2E Only

**Date:** 2026-04-24
**Status:** Accepted

## Context
Need automated tests that verify the app works like a user would use it. The app is static Blazor WASM — no server-side logic, no SignalR, no WebRTC. Test scope is minimal: confirm pages load and key UI elements render (no 404s, no blank pages).

## Options Considered

| Option | Notes |
|---|---|
| bUnit (component tests) | Fast, no browser — but tests Razor components in isolation, not the full rendered page |
| Playwright only | Real browser, tests what a user sees — right for a UI-heavy filter app |
| Both | Comprehensive, but bUnit adds a second test project and framework for limited benefit here |

## Decision
**Playwright E2E only.** No bUnit. Scope is smoke-level: pages load, weight class groups render, filter UI is present.

## Pattern: Static File Host AppFixture
CamShare (`c:/repos/CamShare`) uses an `AppFixture` that boots a real Kestrel app. This project is static WASM — the equivalent is a minimal static file host:

```csharp
// AppFixture boots a lightweight Kestrel host serving published WASM output
var builder = WebApplication.CreateBuilder();
var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(publishPath)
});
app.MapFallbackToFile("index.html", new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(publishPath)
});
app.Urls.Add("http://127.0.0.1:0");
await app.StartAsync();
```

The publish output path is resolved relative to the test binary (same pattern CamShare uses for the `.staticwebassets.endpoints.json` copy step).

## CI Integration
- E2E runs in the `e2e` stage, pre-deploy, using `mcr.microsoft.com/playwright/dotnet:v1.58.0-noble`
- Self-contained linux-x64 publish of the E2E project (no dotnet install needed in CI)
- No post-deploy smoke stage — the e2e stage is sufficient for a static site
- See CamShare `.gitlab-ci.yml` as the reference template

## Consequences
- No bUnit dependency
- AppFixture is simpler than CamShare's — no service registration, no hubs
- Tests run against the real built output, so they catch asset/routing misconfigs too
- Adding interaction tests later (filter behavior) follows the same fixture pattern
