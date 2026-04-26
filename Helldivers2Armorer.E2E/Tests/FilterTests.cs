using Microsoft.Playwright;

namespace Helldivers2Armorer.E2E.Tests;

/// <summary>
/// Covers filtering bugs:
///   1. Text search ignored feature tags — searching "fire" returned only the one armor
///      whose display name contained "fire" (I-92 Fire Fighter), missing all other
///      Inflammable armors across Light/Medium/Heavy whose tag is "fire-protection".
///   2. Passive name search already worked cross-class; retained as regression guard.
/// </summary>
[Collection("E2E")]
public class FilterTests(AppFixture app, BrowserFixture browser)
{
    private async Task<IPage> LoadHomeAsync()
    {
        var ctx  = await browser.NewContextAsync();
        var page = await ctx.NewPageAsync();
        await page.GotoAsync($"{app.BaseUrl}/", new PageGotoOptions { Timeout = 30_000 });
        // .filter-bar only renders after data is loaded (it is inside the else branch)
        await page.WaitForSelectorAsync(".filter-bar", new() { Timeout = 30_000 });
        return page;
    }

    // ── Tag search ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Search_TagKeyword_ReturnsResultsAcrossAllWeightClasses()
    {
        // "fire" matches feature tag "fire-protection" on the Inflammable passive.
        // Inflammable exists on Light (PH-9 Predator), Medium (I-92 Fire Fighter,
        // I-102 Draconaught, PH-56 Jaguar), and Heavy (PH-202 Twigsnapper).
        // Bug: before fix only I-92 Fire Fighter appeared (name match only).
        var page = await LoadHomeAsync();

        await page.FillAsync(".search-input", "fire");
        await page.WaitForSelectorAsync(".weight-heading.light");

        var headings = await page.Locator(".weight-heading").AllTextContentsAsync();
        Assert.Contains(headings, h => h.Contains("Light",  StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Medium", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Heavy",  StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Search_TagKeyword_FindsArmorWhoseNameAndPassiveLackTheKeyword()
    {
        // I-09 Heatseeker: name = "I-09 Heatseeker", passive = "Inflammable" — neither
        // contains "fire". It should appear only because its tag is "fire-protection".
        var page = await LoadHomeAsync();

        await page.FillAsync(".search-input", "fire");
        await page.WaitForSelectorAsync(".weight-heading.light");

        var names = await page.Locator(".armor-name").AllTextContentsAsync();
        Assert.Contains(names, n => n.Contains("I-09 Heatseeker", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Search_TagKeyword_DoesNotShowArmorWithoutMatchingTag()
    {
        // "scout" matches the "reduced-detection" tag on Scout passive.
        // Armors whose name, passive, and tags have no connection to "scout"
        // should not appear. Verify by checking that Engineering Kit armors
        // (whose tags are "bonus-throwables" / "throwing-range") are absent.
        var page = await LoadHomeAsync();

        await page.FillAsync(".search-input", "scout");
        await page.WaitForSelectorAsync(".armor-tile");

        var passives = await page.Locator(".passive-name").AllTextContentsAsync();
        Assert.DoesNotContain(passives, p => p.Contains("Engineering Kit", StringComparison.OrdinalIgnoreCase));
    }

    // ── Passive-name search (regression — worked before, must stay working) ──

    [Fact]
    public async Task Search_PassiveName_ReturnsAcrossAllWeightClasses()
    {
        // "Fortified" exists on Light (FS-38 Eradicator), Medium (B-24 Enforcer),
        // and Heavy armors. Searching the passive name must show all three classes.
        var page = await LoadHomeAsync();

        await page.FillAsync(".search-input", "Fortified");
        await page.WaitForSelectorAsync(".weight-heading");

        var headings = await page.Locator(".weight-heading").AllTextContentsAsync();
        Assert.Contains(headings, h => h.Contains("Light",  StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Medium", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Heavy",  StringComparison.OrdinalIgnoreCase));
    }

    // ── Clear button ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Search_ClearButton_RestoresAllResults()
    {
        var page = await LoadHomeAsync();

        var totalBefore = await page.Locator(".armor-tile").CountAsync();

        await page.FillAsync(".search-input", "fire");
        await page.WaitForSelectorAsync(".weight-heading.light");
        var filteredCount = await page.Locator(".armor-tile").CountAsync();
        Assert.True(filteredCount < totalBefore, "Filtering should reduce tile count");

        await page.ClickAsync(".search-clear");
        await page.WaitForFunctionAsync($"document.querySelectorAll('.armor-tile').length === {totalBefore}");
        Assert.Equal(totalBefore, await page.Locator(".armor-tile").CountAsync());
    }
}
