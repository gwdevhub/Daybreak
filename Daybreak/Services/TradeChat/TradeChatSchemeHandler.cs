using Daybreak.Shared.Services.Themes;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Text;
using System.Text.RegularExpressions;
using static Daybreak.Shared.Models.Themes.Theme;

namespace Daybreak.Services.TradeChat;

public sealed partial class TradeChatSchemeHandler
{
    private const string KamadanOrigin = "https://kamadan.gwtoolbox.com";
    private const string AscalonOrigin = "https://ascalon.gwtoolbox.com";

    private readonly IHttpClient<TradeChatSchemeHandler> httpClient;
    private readonly IThemeManager themeManager;
    private readonly ILogger<TradeChatSchemeHandler> logger;

    public TradeChatSchemeHandler(
        IHttpClient<TradeChatSchemeHandler> httpClient,
        IThemeManager themeManager,
        ILogger<TradeChatSchemeHandler> logger)
    {
        this.httpClient = httpClient.ThrowIfNull();
        this.themeManager = themeManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<Stream> HandleRequestAsync(string url, CancellationToken cancellationToken)
    {
        try
        {
            var response = await this.httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogWarning("Failed to fetch {Url}: {StatusCode}", url, response.StatusCode);
                return Stream.Null;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;

            // Only inject script into HTML documents
            if (!contentType.Contains("text/html", StringComparison.OrdinalIgnoreCase))
            {
                return await response.Content.ReadAsStreamAsync(cancellationToken);
            }

            var html = await response.Content.ReadAsStringAsync(cancellationToken);
            var modifiedHtml = this.InjectThemeScript(html);

            return new MemoryStream(Encoding.UTF8.GetBytes(modifiedHtml));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error handling request for {Url}", url);
            return Stream.Null;
        }
    }

    private string InjectThemeScript(string html)
    {
        var themeMode = this.themeManager.CurrentTheme?.Mode is LightDarkMode.Light ? "light" : "dark";
        var script = $@"<script>localStorage.setItem('mode', '{themeMode}');</script>";

        // Inject script right after <head> tag
        var headPattern = HeadTagRegex();
        if (headPattern.IsMatch(html))
        {
            return headPattern.Replace(html, $"$0{script}", 1);
        }

        // Fallback: inject at the beginning of <body>
        var bodyPattern = BodyTagRegex();
        if (bodyPattern.IsMatch(html))
        {
            return bodyPattern.Replace(html, $"$0{script}", 1);
        }

        // Last resort: prepend to the document
        return script + html;
    }

    public static bool IsTradeChatUrl(string url)
    {
        return url.StartsWith(KamadanOrigin, StringComparison.OrdinalIgnoreCase) ||
               url.StartsWith(AscalonOrigin, StringComparison.OrdinalIgnoreCase);
    }

    public static string ConvertToProxyUrl(string originalUrl)
    {
        return originalUrl.Replace("https://", "tradechat://");
    }

    public static string ConvertFromProxyUrl(string proxyUrl)
    {
        return proxyUrl.Replace("tradechat://", "https://");
    }

    [GeneratedRegex(@"<head[^>]*>", RegexOptions.IgnoreCase)]
    private static partial Regex HeadTagRegex();

    [GeneratedRegex(@"<body[^>]*>", RegexOptions.IgnoreCase)]
    private static partial Regex BodyTagRegex();
}
