using System.Text.Encodings.Web;
using System.Text.Json;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// Resolves skill names to their canonical wiki image CDN URL via the
/// MediaWiki API (<c>prop=imageinfo</c>). Avoids the runtime
/// <c>Special:FilePath/...</c> redirect that has proven flaky.
///
/// Each skill carries one or more candidate base names (e.g. shouts ship with
/// both the unquoted and quoted form). For each base name we try the
/// high-resolution <c>"&lt;Base&gt; (large).jpg"</c> first, then plain
/// <c>"&lt;Base&gt;.jpg"</c>. Skills with no matching file get an empty URL,
/// matching the historical convention for PvP variants.
/// </summary>
internal sealed class IconResolver(WikiHttpClient client)
{
    private const string ApiBase = "https://wiki.guildwars.com/api.php";

    /// <summary>Maximum titles per <c>prop=imageinfo</c> query.</summary>
    /// <remarks>
    /// MediaWiki itself accepts 50, but the wiki's WAF/Cloudflare returns 403
    /// when the encoded URL grows past ~2 KB (which 50 skill titles routinely
    /// hit, especially with parentheses and apostrophes). 25 keeps every URL
    /// under the limit; <see cref="ResolveSliceAsync"/> additionally splits
    /// on the fly if a single batch still gets rejected.
    /// </remarks>
    private const int BatchSize = 25;

    public async Task<IReadOnlyDictionary<string, string>> ResolveAsync(
        IReadOnlyList<ParsedSkill> skills,
        CancellationToken cancellationToken)
    {
        // Track candidate (skillName, filename) pairs across passes; a skill
        // is removed from the work-set as soon as any candidate resolves.
        var resolved = new Dictionary<string, string>(StringComparer.Ordinal);
        var pending = skills
            .Where(s => !string.IsNullOrWhiteSpace(s.Name))
            .GroupBy(s => s.Name, StringComparer.Ordinal)
            .Select(g => g.First())
            .ToList();

        Console.WriteLine($"-> Resolving icon URLs for {pending.Count} skills…");

        // Build the (base name, suffix) sweep order. We try high-res first so
        // a skill that has both ends up with the larger asset.
        var maxCandidates = pending.Max(s => s.IconBaseNames.Count);
        if (maxCandidates == 0)
        {
            maxCandidates = 1;
        }

        for (var candidateIdx = 0; candidateIdx < maxCandidates; candidateIdx++)
        {
            foreach (var suffix in new[] { " (large)", string.Empty })
            {
                var work = pending
                    .Where(s => !resolved.ContainsKey(s.Name))
                    .Where(s => candidateIdx < s.IconBaseNames.Count
                        || (candidateIdx == 0 && s.IconBaseNames.Count == 0))
                    .Select(s => (
                        SkillName: s.Name,
                        BaseName: s.IconBaseNames.Count == 0 ? s.Name : s.IconBaseNames[candidateIdx]))
                    .ToList();

                if (work.Count == 0)
                {
                    continue;
                }

                var label = candidateIdx == 0
                    ? (suffix == " (large)" ? "(large)" : "fallback")
                    : (suffix == " (large)" ? $"alt#{candidateIdx} (large)" : $"alt#{candidateIdx} fallback");
                await ResolveBatchAsync(work, suffix, resolved, label, cancellationToken);
            }
        }

        var unresolved = pending.Count - resolved.Count;
        Console.WriteLine($"   icons resolved: {resolved.Count} hit, {unresolved} unresolved (empty url)");
        return resolved;
    }

    private async Task ResolveBatchAsync(
        IReadOnlyList<(string SkillName, string BaseName)> work,
        string suffix,
        Dictionary<string, string> resolved,
        string passLabel,
        CancellationToken cancellationToken)
    {
        var totalBatches = (work.Count + BatchSize - 1) / BatchSize;
        var batchIndex = 0;
        for (var offset = 0; offset < work.Count; offset += BatchSize)
        {
            batchIndex++;
            var slice = new List<(string SkillName, string BaseName)>();
            for (var i = offset; i < Math.Min(offset + BatchSize, work.Count); i++)
            {
                slice.Add(work[i]);
            }

            var found = await ResolveSliceAsync(slice, suffix, resolved, cancellationToken);
            Console.WriteLine($"   batch {batchIndex,2}/{totalBatches} ({passLabel}) … {found} resolved");
        }
    }

    /// <summary>
    /// Sends one <c>prop=imageinfo</c> query for the slice and falls back to
    /// halving + retry if the wiki's WAF rejects the URL with 403.
    /// </summary>
    private async Task<int> ResolveSliceAsync(
        IReadOnlyList<(string SkillName, string BaseName)> work,
        string suffix,
        Dictionary<string, string> resolved,
        CancellationToken cancellationToken)
    {
        if (work.Count == 0)
        {
            return 0;
        }

        var titleToSkill = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var titles = new List<string>(work.Count);
        foreach (var (skillName, baseName) in work)
        {
            var title = $"File:{baseName}{suffix}.jpg";
            titles.Add(title);
            titleToSkill[NormalizeWikiTitle(title)] = skillName;
        }

        var url = BuildImageInfoUrl(titles);
        string json;
        try
        {
            json = await client.GetStringAsync(url, cancellationToken);
        }
        catch (HttpRequestException ex) when (IsForbidden(ex) && work.Count > 1)
        {
            var mid = work.Count / 2;
            var left = work.Take(mid).ToList();
            var right = work.Skip(mid).ToList();
            var l = await ResolveSliceAsync(left, suffix, resolved, cancellationToken);
            var r = await ResolveSliceAsync(right, suffix, resolved, cancellationToken);
            return l + r;
        }

        return ExtractUrls(json, titleToSkill, resolved);
    }

    private static bool IsForbidden(HttpRequestException ex) =>
        ex.Message.Contains("403", StringComparison.Ordinal);

    private static int ExtractUrls(string json, IReadOnlyDictionary<string, string> titleToSkill, Dictionary<string, string> resolved)
    {
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        if (!root.TryGetProperty("query", out var query) ||
            !query.TryGetProperty("pages", out var pages))
        {
            return 0;
        }

        var hit = 0;
        // formatversion=2 normalises titles for us; map them back via the lookup table.
        var rewrites = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (query.TryGetProperty("normalized", out var normalized))
        {
            foreach (var entry in normalized.EnumerateArray())
            {
                if (entry.TryGetProperty("from", out var fromEl) &&
                    entry.TryGetProperty("to", out var toEl))
                {
                    rewrites[NormalizeWikiTitle(toEl.GetString() ?? "")] = NormalizeWikiTitle(fromEl.GetString() ?? "");
                }
            }
        }

        foreach (var page in pages.EnumerateArray())
        {
            if (!page.TryGetProperty("title", out var titleEl))
            {
                continue;
            }

            var canonical = NormalizeWikiTitle(titleEl.GetString() ?? "");
            var lookupKey = rewrites.GetValueOrDefault(canonical, canonical);
            if (!titleToSkill.TryGetValue(lookupKey, out var skillName))
            {
                continue;
            }

            if (!page.TryGetProperty("imageinfo", out var imageInfo) || imageInfo.GetArrayLength() == 0)
            {
                continue;
            }

            if (!imageInfo[0].TryGetProperty("url", out var urlEl))
            {
                continue;
            }

            var url = urlEl.GetString();
            if (!string.IsNullOrEmpty(url))
            {
                resolved[skillName] = url;
                hit++;
            }
        }

        return hit;
    }

    /// <summary>MediaWiki normalises whitespace and URL-encodes nothing in titles.</summary>
    private static string NormalizeWikiTitle(string title) => title.Replace('_', ' ').Trim();

    private static string BuildImageInfoUrl(IReadOnlyList<string> titles)
    {
        var encoder = UrlEncoder.Default;
        var encodedTitles = encoder.Encode(string.Join("|", titles));
        return $"{ApiBase}?action=query&format=json&formatversion=2&titles={encodedTitles}&prop=imageinfo&iiprop=url";
    }
}
