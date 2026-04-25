using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace Helldivers2Armorer.E2E.Fixtures;

/// <summary>
/// Serves the published Blazor WASM static output via a minimal Kestrel static file host
/// so Playwright (out-of-process browser) can navigate to the app.
///
/// The published output is expected alongside the test binary — the CI build step
/// copies the publish output next to the E2E executable before running tests.
/// Locally, run `dotnet publish src/` first, then run the E2E tests.
/// </summary>
public sealed class AppFixture : IAsyncLifetime
{
    private WebApplication? _app;

    public string BaseUrl { get; private set; } = string.Empty;

    public async ValueTask InitializeAsync()
    {
        var publishPath = ResolvePublishPath();

        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        var app = builder.Build();

        var provider = new PhysicalFileProvider(publishPath);
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[".webp"] = "image/webp";

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = provider,
            ContentTypeProvider = contentTypeProvider,
        });

        // Blazor WASM SPA fallback — all non-file routes serve index.html
        app.MapFallback(async ctx =>
        {
            ctx.Response.ContentType = "text/html";
            await ctx.Response.SendFileAsync(Path.Combine(publishPath, "index.html"));
        });

        app.Urls.Add("http://127.0.0.1:0");
        await app.StartAsync();
        _app = app;

        var address = app.Urls.First().TrimEnd('/');
        BaseUrl = address;
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

    private static string ResolvePublishPath()
    {
        // Look for the wwwroot sibling to the test binary (CI sets this up).
        // Falls back to a local publish output for developer convenience.
        var baseDir = AppContext.BaseDirectory;
        var candidates = new[]
        {
            Path.Combine(baseDir, "wwwroot"),
            Path.GetFullPath(Path.Combine(baseDir, "../../../../src/bin/Debug/net10.0/wwwroot")),
            Path.GetFullPath(Path.Combine(baseDir, "../../../../src/bin/Release/net10.0/wwwroot")),
        };
        return candidates.FirstOrDefault(Directory.Exists)
            ?? throw new DirectoryNotFoundException(
                $"Could not find WASM publish output. Checked: {string.Join(", ", candidates)}");
    }
}
