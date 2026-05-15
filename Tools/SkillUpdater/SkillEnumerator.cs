using System.Text.Encodings.Web;
using System.Text.Json;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// Walks the five per-campaign "Skills" categories on the wiki using
/// <c>generator=categorymembers</c> joined with
/// <c>prop=revisions&amp;rvslots=main&amp;rvprop=content</c>. Pagination via
/// <c>gcmcontinue</c>. Pages without a <c>{{Skill infobox}}</c> block (category
/// indexes, "List of …" pages) are skipped. De-duplicates by page id.
/// </summary>
internal sealed class SkillEnumerator(WikiHttpClient client)
{
    private const string ApiBase = "https://wiki.guildwars.com/api.php";

    private static readonly string[] CampaignCategories =
    [
        "Core_skills",
        "Prophecies_skills",
        "Factions_skills",
        "Nightfall_skills",
        "Eye_of_the_North_skills",
    ];

    public async Task<IReadOnlyList<ParsedSkill>> EnumerateAsync(CancellationToken cancellationToken)
    {
        var seen = new Dictionary<int, ParsedSkill>();
        foreach (var category in CampaignCategories)
        {
            Console.WriteLine($"-> Category:{category}");
            var newCount = 0;
            var skipped = 0;
            await foreach (var (pageId, skill, isSkill) in this.EnumerateCategoryAsync(category, cancellationToken))
            {
                if (!isSkill)
                {
                    skipped++;
                    continue;
                }

                if (seen.TryAdd(pageId, skill!))
                {
                    newCount++;
                }
            }

            Console.WriteLine($"   category complete: +{newCount} new (skipped {skipped} non-skill pages, running total {seen.Count})");
        }

        return [.. seen.Values];
    }

    private async IAsyncEnumerable<(int PageId, ParsedSkill? Skill, bool IsSkill)> EnumerateCategoryAsync(
        string category,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string? gcmcontinue = null;
        var batchIndex = 0;
        do
        {
            batchIndex++;
            var url = BuildCategoryQueryUrl(category, gcmcontinue);
            Console.Write($"   batch {batchIndex,2} … ");
            var json = await client.GetStringAsync(url, cancellationToken);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            gcmcontinue = root.TryGetProperty("continue", out var cont) && cont.TryGetProperty("gcmcontinue", out var gcm)
                ? gcm.GetString()
                : null;

            var batchSkills = 0;
            var batchSkipped = 0;
            if (root.TryGetProperty("query", out var query) &&
                query.TryGetProperty("pages", out var pages))
            {
                foreach (var page in pages.EnumerateArray())
                {
                    if (TryProjectSkill(page, out var pageId, out var skill))
                    {
                        batchSkills++;
                        yield return (pageId, skill, true);
                    }
                    else
                    {
                        batchSkipped++;
                        yield return (0, null, false);
                    }
                }
            }

            Console.WriteLine($"{batchSkills} skills (+{batchSkipped} skipped){(gcmcontinue is null ? " [last]" : string.Empty)}");
        }
        while (gcmcontinue is not null);
    }

    private static bool TryProjectSkill(JsonElement page, out int pageId, out ParsedSkill skill)
    {
        pageId = 0;
        skill = null!;
        if (!page.TryGetProperty("title", out var titleEl) ||
            !page.TryGetProperty("pageid", out var pageIdEl))
        {
            return false;
        }

        var title = titleEl.GetString();
        if (string.IsNullOrEmpty(title))
        {
            return false;
        }

        if (!page.TryGetProperty("revisions", out var revisions) || revisions.GetArrayLength() == 0)
        {
            return false;
        }

        string? content = null;
        if (revisions[0].TryGetProperty("slots", out var slots) &&
            slots.TryGetProperty("main", out var main) &&
            main.TryGetProperty("content", out var contentEl))
        {
            content = contentEl.GetString();
        }

        if (string.IsNullOrWhiteSpace(content) ||
            !content.Contains("{{Skill infobox", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!WikiSkillParser.TryParse(content, out var parsed))
        {
            Console.Error.WriteLine($"     ! parse failed for '{title}' (pageid {pageIdEl.GetInt32()})");
            return false;
        }

        // The infobox `name` field can omit the `(PvP)` suffix even though the
        // page title carries it. Prefer the page title as the canonical name.
        if (!string.Equals(parsed.Name, title, StringComparison.Ordinal))
        {
            parsed = parsed with { Name = title };
        }

        pageId = pageIdEl.GetInt32();
        skill = parsed;
        return true;
    }

    private static string BuildCategoryQueryUrl(string category, string? gcmcontinue)
    {
        var encoder = UrlEncoder.Default;
        var continueParam = gcmcontinue is null ? string.Empty : $"&gcmcontinue={encoder.Encode(gcmcontinue)}";
        return $"{ApiBase}?action=query&format=json&formatversion=2"
            + $"&generator=categorymembers&gcmtitle=Category:{encoder.Encode(category)}"
            + "&gcmlimit=50&gcmtype=page&gcmnamespace=0"
            + "&prop=revisions&rvslots=main&rvprop=content"
            + continueParam;
    }
}
