using System.Core.Extensions;
using System.Extensions.Core;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Daybreak.Shared.Services.Wiki;
using Microsoft.Extensions.Logging;

namespace Daybreak.Services.Wiki;

public sealed partial class WikiPageLoader(
    IHttpClient<WikiPageLoader> httpClient,
    ILogger<WikiPageLoader> logger) : IWikiPageLoader
{
    public const string NavigateMessageType = "daybreak-frame-navigate";
    public const string HistoryMessageType = "daybreak-frame-history";

    private const string WikiHost = "wiki.guildwars.com";

    // Runs inside the loaded page. Navigating the frame itself would hit the wiki's
    // X-Frame-Options again, so link clicks and GET form submissions are handed to the
    // parent, which loads the target through this service. The mouse back/forward buttons
    // are handed over too, since pages are loaded without entries in the browser's history.
    private const string NavigationScript = """
        (() => {
            const pageUrl = document.baseURI.split('#')[0];
            const navigate = url => parent.postMessage({ type: '__TYPE__', url }, '*');
            const isHistoryButton = e => e.button === 3 || e.button === 4;
            const scrollToFragment = fragment => {
                if (fragment) {
                    document.getElementById(decodeURIComponent(fragment))?.scrollIntoView();
                }
            };

            document.addEventListener('click', e => {
                const anchor = e.target instanceof Element ? e.target.closest('a[href]') : null;
                if (!anchor || e.defaultPrevented || anchor.protocol === 'javascript:') {
                    return;
                }

                e.preventDefault();
                const [target, fragment] = anchor.href.split('#');
                if (target === pageUrl) {
                    scrollToFragment(fragment);
                    return;
                }

                navigate(anchor.href);
            }, true);

            document.addEventListener('submit', e => {
                const form = e.target;
                if (!(form instanceof HTMLFormElement) || form.method.toLowerCase() !== 'get') {
                    return;
                }

                e.preventDefault();
                const url = new URL(form.action || pageUrl);
                new FormData(form).forEach((value, key) => url.searchParams.set(key, value));
                navigate(url.href);
            }, true);

            document.addEventListener('mousedown', e => {
                if (isHistoryButton(e)) {
                    e.preventDefault();
                }
            }, true);

            document.addEventListener('mouseup', e => {
                if (isHistoryButton(e)) {
                    e.preventDefault();
                    parent.postMessage({ type: '__HISTORY_TYPE__', delta: e.button === 3 ? -1 : 1 }, '*');
                }
            }, true);

            document.addEventListener('DOMContentLoaded', () => scrollToFragment(__FRAGMENT__));
        })();
        """;

    private readonly IHttpClient<WikiPageLoader> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<WikiPageLoader> logger = logger.ThrowIfNull();

    public bool IsWikiUrl(Uri url) =>
        url.IsAbsoluteUri &&
        url.Scheme == Uri.UriSchemeHttps &&
        url.Host.Equals(WikiHost, StringComparison.OrdinalIgnoreCase);

    public async Task<string?> LoadPage(Uri url, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.IsWikiUrl(url))
        {
            scopedLogger.LogError("Refusing to load non-wiki url {url}", url);
            return default;
        }

        try
        {
            using var response = await this.httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to load {url}. Status code {statusCode}", url, response.StatusCode);
                return default;
            }

            var html = await response.Content.ReadAsStringAsync(cancellationToken);
            // Relative links must resolve against the page after redirects, not the requested url
            var pageUrl = response.RequestMessage?.RequestUri ?? url;
            return PrepareDocument(html, pageUrl, url.Fragment);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            scopedLogger.LogError(e, "Failed to load {url}", url);
            return default;
        }
    }

    /// <summary>
    /// Adds a base element, so links resolve and the wiki's styles, scripts and images keep loading from the wiki,
    /// and the navigation script, so clicks are routed back to Daybreak.
    /// </summary>
    public static string PrepareDocument(string html, Uri pageUrl, string? fragment = default)
    {
        var baseUrl = new UriBuilder(pageUrl) { Fragment = string.Empty }.Uri.AbsoluteUri;
        var script = NavigationScript
            .Replace("__TYPE__", NavigateMessageType)
            .Replace("__HISTORY_TYPE__", HistoryMessageType)
            .Replace("__FRAGMENT__", JsonSerializer.Serialize(fragment?.TrimStart('#') ?? string.Empty));
        var injected = $"<base href=\"{WebUtility.HtmlEncode(baseUrl)}\"><script>{script}</script>";

        var head = HeadRegex().Match(html);
        return head.Success
            ? html.Insert(head.Index + head.Length, injected)
            : injected + html;
    }

    [GeneratedRegex(@"<head(\s[^>]*)?>", RegexOptions.IgnoreCase)]
    private static partial Regex HeadRegex();
}
