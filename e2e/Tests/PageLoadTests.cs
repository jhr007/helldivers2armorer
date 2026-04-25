using Microsoft.Playwright;

namespace Helldivers2Armorer.E2E.Tests;

[Collection("E2E")]
public class PageLoadTests(AppFixture app, BrowserFixture browser)
{
    private async Task<IPage> GoAsync(string path)
    {
        var ctx = await browser.NewContextAsync();
        var page = await ctx.NewPageAsync();
        await page.GotoAsync($"{app.BaseUrl}{path}", new PageGotoOptions { Timeout = 20_000 });
        return page;
    }

    [Fact]
    public async Task HomePage_Loads_WithoutErrors()
    {
        var page = await GoAsync("/");
        // Wait for Blazor WASM to boot and render content
        await page.WaitForSelectorAsync(".filter-bar, .loading-state", new() { Timeout = 20_000 });
        // No 404 / error page
        Assert.DoesNotContain("404", await page.TitleAsync());
    }

    [Fact]
    public async Task HomePage_ShowsWeightClassHeadings()
    {
        var page = await GoAsync("/");
        await page.WaitForSelectorAsync(".weight-heading", new() { Timeout = 20_000 });
        var headings = await page.Locator(".weight-heading").AllTextContentsAsync();
        Assert.Contains(headings, h => h.Contains("Light", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Medium", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Heavy", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task HomePage_ShowsFilterDropdowns()
    {
        var page = await GoAsync("/");
        await page.WaitForSelectorAsync(".filter-select", new() { Timeout = 20_000 });
        var selects = await page.Locator(".filter-select").CountAsync();
        Assert.Equal(2, selects);
    }

    [Fact]
    public async Task AboutPage_Loads_WithCredits()
    {
        var page = await GoAsync("/about");
        await page.WaitForSelectorAsync(".about-page", new() { Timeout = 20_000 });
        var body = await page.ContentAsync();
        Assert.Contains("Selenestica", body);
        Assert.Contains("helldivers.wiki.gg", body);
    }
}
