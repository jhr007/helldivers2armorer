using Microsoft.Playwright;

namespace Helldivers2Armorer.E2E.Tests;

[Collection("E2E")]
public class PageLoadTests(AppFixture app, BrowserFixture browser)
{
    private async Task<IPage> GoAsync(string path)
    {
        var ctx = await browser.NewContextAsync();
        var page = await ctx.NewPageAsync();
        _consoleMessages.Clear();
        page.Console += (_, e) => _consoleMessages.Add($"[{e.Type}] {e.Text}");
        page.PageError += (_, e) => _consoleMessages.Add($"[page-error] {e}");
        await page.GotoAsync($"{app.BaseUrl}{path}", new PageGotoOptions { Timeout = 30_000 });
        return page;
    }

    private async Task WaitForBlazor(IPage page, string selector, int timeoutMs = 30_000)
    {
        try
        {
            await page.WaitForSelectorAsync(selector, new() { Timeout = timeoutMs });
        }
        catch (TimeoutException)
        {
            var html = await page.ContentAsync();
            var messages = string.Join("\n  ", _consoleMessages);
            throw new TimeoutException(
                $"Timed out waiting for '{selector}' after {timeoutMs}ms.\n" +
                $"BaseUrl: {app.BaseUrl}\n" +
                $"Console ({_consoleMessages.Count}):\n  {messages}\n" +
                $"Page HTML (first 2000 chars):\n{html[..Math.Min(2000, html.Length)]}");
        }
    }

    private readonly System.Collections.Concurrent.ConcurrentBag<string> _consoleMessages = [];

    [Fact]
    public async Task HomePage_Loads_WithoutErrors()
    {
        var page = await GoAsync("/");
        await WaitForBlazor(page, ".filter-bar, .loading-state");
        var errors = _consoleMessages.Where(m => m.StartsWith("[error]") || m.StartsWith("[page-error]")).ToList();
        Assert.True(errors.Count == 0, $"Browser errors:\n{string.Join("\n", errors)}");
        Assert.DoesNotContain("404", await page.TitleAsync());
    }

    [Fact]
    public async Task HomePage_ShowsWeightClassHeadings()
    {
        var page = await GoAsync("/");
        await WaitForBlazor(page, ".weight-heading");
        var headings = await page.Locator(".weight-heading").AllTextContentsAsync();
        Assert.Contains(headings, h => h.Contains("Light", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Medium", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(headings, h => h.Contains("Heavy", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task HomePage_ShowsFilterDropdowns()
    {
        var page = await GoAsync("/");
        await WaitForBlazor(page, ".filter-select");
        var selects = await page.Locator(".filter-select").CountAsync();
        Assert.Equal(2, selects);
    }

    [Fact]
    public async Task AboutPage_Loads_WithCredits()
    {
        var page = await GoAsync("/about");
        await WaitForBlazor(page, ".about-page");
        var body = await page.ContentAsync();
        Assert.Contains("Selenestica", body);
        Assert.Contains("helldivers.wiki.gg", body);
    }
}
