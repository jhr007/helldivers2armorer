using Microsoft.Playwright;

namespace Helldivers2Armorer.E2E.Tests;

[Collection("E2E")]
public class FeatureTests(AppFixture app, BrowserFixture browser)
{
    private async Task<IPage> LoadHomeAsync()
    {
        var ctx  = await browser.NewContextAsync();
        var page = await ctx.NewPageAsync();
        await page.GotoAsync($"{app.BaseUrl}/", new PageGotoOptions { Timeout = 30_000 });
        await page.WaitForSelectorAsync(".filter-bar", new() { Timeout = 30_000 });
        return page;
    }

    // ── Search regression: "des" must not match via full passive description ──

    [Fact]
    public async Task Search_Des_DoesNotReturnDescriptionOnlyMatches()
    {
        // Before fix, "des" matched "Provides" in the full passive description, causing
        // FS-38 Eradicator (Fortified) and EX-00 Prototype X (Fortified) to appear.
        // Fix: search only shortDescription, displayName, passive name, and tags.
        var page = await LoadHomeAsync();

        await page.FillAsync(".search-input", "des");
        // .search-clear renders in the same Blazor pass as the filtered results,
        // so its presence proves the re-render is complete.
        await page.WaitForSelectorAsync(".search-clear", new() { Timeout = 5_000 });

        var names = await page.Locator(".armor-name").AllTextContentsAsync();
        Assert.DoesNotContain(names, n => n.Contains("FS-38 Eradicator",  StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(names, n => n.Contains("EX-00 Prototype X", StringComparison.OrdinalIgnoreCase));
    }

    // ── Collapsible weight sections ───────────────────────────────────────────

    [Fact]
    public async Task WeightSection_ClickHeading_CollapsesSection()
    {
        var page = await LoadHomeAsync();

        var lightTilesBefore = await page.Locator(".weight-section:has(.weight-heading.light) .armor-tile").CountAsync();
        Assert.True(lightTilesBefore > 0, "Expected at least one Light armor tile");

        await page.Locator(".weight-heading-row:has(.weight-heading.light)").ClickAsync();

        await page.WaitForFunctionAsync(
            "document.querySelectorAll('.weight-section:has(.weight-heading.light) .armor-tile').length === 0",
            null, new PageWaitForFunctionOptions { Timeout = 5_000 });

        Assert.Equal(0, await page.Locator(".weight-section:has(.weight-heading.light) .armor-tile").CountAsync());
    }

    [Fact]
    public async Task WeightSection_ClickHeadingTwice_ExpandsSection()
    {
        var page = await LoadHomeAsync();

        var lightTilesBefore = await page.Locator(".weight-section:has(.weight-heading.light) .armor-tile").CountAsync();
        var headingRow = page.Locator(".weight-heading-row:has(.weight-heading.light)");

        await headingRow.ClickAsync();
        await page.WaitForFunctionAsync(
            "document.querySelectorAll('.weight-section:has(.weight-heading.light) .armor-tile').length === 0",
            null, new PageWaitForFunctionOptions { Timeout = 5_000 });

        await headingRow.ClickAsync();
        await page.WaitForFunctionAsync(
            $"document.querySelectorAll('.weight-section:has(.weight-heading.light) .armor-tile').length === {lightTilesBefore}",
            null, new PageWaitForFunctionOptions { Timeout = 5_000 });

        Assert.Equal(lightTilesBefore, await page.Locator(".weight-section:has(.weight-heading.light) .armor-tile").CountAsync());
    }

    // ── Comparison panel ──────────────────────────────────────────────────────

    [Fact]
    public async Task Comparison_SelectTwoArmors_ShowsComparisonPanel()
    {
        var page = await LoadHomeAsync();

        var tiles = page.Locator(".armor-tile");
        await tiles.First.Locator(".armor-name").ClickAsync();
        await tiles.Nth(1).Locator(".armor-name").ClickAsync();

        await page.WaitForSelectorAsync(".comparison-panel", new() { Timeout = 5_000 });
        Assert.Equal(1, await page.Locator(".comparison-panel").CountAsync());
    }

    [Fact]
    public async Task Comparison_ClearButton_DismissesPanel()
    {
        var page = await LoadHomeAsync();

        var tiles = page.Locator(".armor-tile");
        await tiles.First.Locator(".armor-name").ClickAsync();
        await tiles.Nth(1).Locator(".armor-name").ClickAsync();
        await page.WaitForSelectorAsync(".comparison-panel");

        await page.Locator(".comparison-panel .clear-all").ClickAsync();
        await page.WaitForSelectorAsync(".comparison-panel", new() { State = WaitForSelectorState.Detached, Timeout = 5_000 });

        Assert.Equal(0, await page.Locator(".comparison-panel").CountAsync());
    }

    // ── Tile info modal ───────────────────────────────────────────────────────

    [Fact]
    public async Task Tile_InfoButton_OpensDetailModal()
    {
        var page = await LoadHomeAsync();

        await page.Locator(".tile-info-btn").First.ClickAsync();
        await page.WaitForSelectorAsync(".detail-modal", new() { Timeout = 5_000 });

        Assert.Equal(1, await page.Locator(".detail-modal").CountAsync());
    }

    [Fact]
    public async Task Tile_InfoModal_CloseButton_ClosesModal()
    {
        var page = await LoadHomeAsync();

        await page.Locator(".tile-info-btn").First.ClickAsync();
        await page.WaitForSelectorAsync(".detail-modal");

        await page.Locator(".detail-modal-close").ClickAsync();
        await page.WaitForSelectorAsync(".detail-modal", new() { State = WaitForSelectorState.Detached, Timeout = 5_000 });

        Assert.Equal(0, await page.Locator(".detail-modal").CountAsync());
    }

    // ── Owned badges ──────────────────────────────────────────────────────────

    [Fact]
    public async Task OwnedBadges_Toggle_HidesAndRestoresBadgesOnTiles()
    {
        // Badges are visible by default (showOwnedBadges defaults to true)
        var page = await LoadHomeAsync();

        await page.WaitForSelectorAsync(".owned-badge-btn", new() { Timeout = 5_000 });
        Assert.True(await page.Locator(".owned-badge-btn").CountAsync() > 0);

        await page.Locator(".filter-toggle", new() { HasText = "Hide owned icons" }).ClickAsync();
        await page.WaitForFunctionAsync(
            "document.querySelectorAll('.owned-badge-btn').length === 0",
            null, new PageWaitForFunctionOptions { Timeout = 5_000 });
        Assert.Equal(0, await page.Locator(".owned-badge-btn").CountAsync());

        await page.Locator(".filter-toggle", new() { HasText = "Show owned icons" }).ClickAsync();
        await page.WaitForSelectorAsync(".owned-badge-btn", new() { Timeout = 5_000 });
        Assert.True(await page.Locator(".owned-badge-btn").CountAsync() > 0);
    }

    [Fact]
    public async Task OwnedBadge_Click_TogglesOwnedState()
    {
        // Badges visible by default — no need to click "Show owned icons" first
        var page = await LoadHomeAsync();

        await page.WaitForSelectorAsync(".owned-badge-btn");

        var badge = page.Locator(".owned-badge-btn.is-unowned").First;
        await badge.ClickAsync();

        await page.WaitForSelectorAsync(".owned-badge-btn.is-owned", new() { Timeout = 5_000 });
        Assert.True(await page.Locator(".owned-badge-btn.is-owned").CountAsync() > 0);
    }
}
