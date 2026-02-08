using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Services.Wiki;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using Attribute = Daybreak.Shared.Models.Guildwars.Attribute;

namespace Daybreak.Services.Wiki;
public sealed partial class WikiService(
    IHttpClient<WikiService> httpClient,
    ILogger<WikiService> logger)
    : IWikiService
{
    private const string WikiQueryTitlePlaceholder = "[TITLE]";
    private const string WikiQueryUrl = $"https://wiki.guildwars.com/api.php?action=query&format=json&titles={WikiQueryTitlePlaceholder}&prop=revisions&rvprop=content&rvslots=main";

    private static readonly ConcurrentDictionary<int, SkillDescription> DescriptionCache = [];

    private readonly IHttpClient<WikiService> httpClient = httpClient;
    private readonly ILogger<WikiService> logger = logger;

    public async Task<SkillDescription?> GetSkillDescription(Skill skill, CancellationToken cancellationToken)
    {
        if (DescriptionCache.TryGetValue(skill.Id, out var cachedDescription))
        {
            return cachedDescription;
        }

        var wikiText = await this.GetWikiText(skill.Name, cancellationToken);
        if (wikiText is null)
        {
            return default;
        }

        var description = ParseWikiText(wikiText);
        if (description is null)
        {
            return default;
        }

        DescriptionCache[skill.Id] = description;
        return description;
    }

    private async ValueTask<string?> GetWikiText(string name, CancellationToken cancellationToken)
    {
        while (true)
        {
            var urlEncodedName = UrlEncoder.Default.Encode(name);
            var wikiUrl = WikiQueryUrl.Replace(WikiQueryTitlePlaceholder, urlEncodedName);
            var response = await this.httpClient.GetAsync(wikiUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var wikiText = ExtractWikiTextFromJson(content);
            if (wikiText is null)
            {
                return default;
            }

            var redirectTarget = ExtractRedirectTarget(wikiText);
            if (redirectTarget is null)
            {
                return wikiText;
            }

            name = redirectTarget;
        }
    }

    private static string? ExtractRedirectTarget(string wikiText)
    {
        if (string.IsNullOrWhiteSpace(wikiText))
        {
            return null;
        }

        var match = RedirectRegex().Match(wikiText.Trim());
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string? ExtractWikiTextFromJson(string jsonContent)
    {
        try
        {
            using var json = JsonDocument.Parse(jsonContent);
            var root = json.RootElement;

            if (!root.TryGetProperty("query", out var query) ||
                !query.TryGetProperty("pages", out var pages))
            {
                return null;
            }

            // Get the first property (page) from pages object
            using var pagesEnumerator = pages.EnumerateObject();
            if (!pagesEnumerator.MoveNext())
            {
                return null;
            }

            var page = pagesEnumerator.Current.Value;

            if (!page.TryGetProperty("revisions", out var revisions) ||
                revisions.GetArrayLength() == 0)
            {
                return null;
            }

            var firstRevision = revisions[0];
            if (firstRevision.TryGetProperty("slots", out var slots) &&
                slots.TryGetProperty("main", out var main) &&
                main.TryGetProperty("*", out var content))
            {
                return content.GetString();
            }

            return null;
        }
        catch (Exception)
        {
            return default;
        }
    }

    private static SkillDescription? ParseWikiText(string wikiText)
    {
        var infoboxContent = ExtractSkillInfobox(CleanWikiText(wikiText));
        if (infoboxContent is not null)
        {
            return new SkillDescription(
                Id: ExtractField(infoboxContent, "id") is string idStr ? int.TryParse(idStr, out var id) ? id : -1 : -1,
                Campaign: ExtractField(infoboxContent, "campaign") is string campaignStr ? Campaign.TryParse(campaignStr, out var campaign) ? campaign : Campaign.None : Campaign.None,
                Name: ExtractField(infoboxContent, "name") ?? string.Empty,
                Profession: ExtractField(infoboxContent, "profession") is string professionStr ? Profession.TryParse(professionStr, out var profession) ? profession : Profession.None : Profession.None,
                Attribute: ExtractField(infoboxContent, "attribute") is string attrStr ? Attribute.TryParse(attrStr, out var attribute) ? attribute : Attribute.None :  Attribute.None,
                PveOnly: ExtractField(infoboxContent, "pve-only") is string pveOnlyStr && pveOnlyStr.StartsWith('y'),   // Some fields contain yes, others y
                Pvp: ExtractField(infoboxContent, "is-pvp") is string pvpStr && pvpStr.StartsWith('y'),
                Elite: ExtractField(infoboxContent, "elite") is string eliteStr && eliteStr.StartsWith('y'),            // Some fields contain yes, others y
                Type: ExtractField(infoboxContent, "type") ?? string.Empty,
                Energy: ExtractField(infoboxContent, "energy") ?? string.Empty,
                Activation: ExtractField(infoboxContent, "activation") ?? string.Empty,
                Recharge: ExtractField(infoboxContent, "recharge") ?? string.Empty,
                Overcast: ExtractField(infoboxContent, "overcast") ?? string.Empty,
                Adrenaline: ExtractField(infoboxContent, "adrenaline") ?? string.Empty,
                Sacrifice: ExtractField(infoboxContent, "sacrifice") ?? string.Empty,
                Upkeep: ExtractField(infoboxContent, "upkeep") ?? string.Empty,
                Description: ExtractField(infoboxContent, "description") ?? string.Empty,
                ConciseDescription: ExtractField(infoboxContent, "concise description") ?? string.Empty);
        }

        return default;
    }

    private static string CleanWikiText(string wikiText)
    {
        if (string.IsNullOrEmpty(wikiText))
        {
            return wikiText;
        }

        var cleaned = wikiText;

        // Handle common fraction templates
        cleaned = HalfFractionRegex().Replace(cleaned, "¹⁄₂");
        cleaned = QuarterFractionRegex().Replace(cleaned, "¹⁄₄");
        cleaned = ThreeQuarterFractionRegex().Replace(cleaned, "³⁄₄");
        cleaned = ThreeOverTwoFractionRegex().Replace(cleaned, "³⁄₂");

        // Handle decimal number templates
        cleaned = DecimalRegex().Replace(cleaned, "$1");

        // Handle {{gr|min|max}} templates (green range values)
        cleaned = GreenRangeRegex().Replace(cleaned, "$1…$2");
        cleaned = GreenRange2Regex().Replace(cleaned, "$1…$2");
        cleaned = GrayRangeRegex().Replace(cleaned, "$1…$2");

        // Handle simple links [[Link Text]]
        cleaned = SimpleLinkRegex().Replace(cleaned, "$1");

        // Handle {{gray|text}} templates
        cleaned = GrayTextRegex().Replace(cleaned, "");
        cleaned = GreyTextRegex().Replace(cleaned, "");

        // Handle {{sic|text}} templates (remove the sic notation but keep the content)
        cleaned = SicTextRegex().Replace(cleaned, "");

        // Handle links with display text [[Link|Display Text]]
        cleaned = LinkWithDisplayTextRegex().Replace(cleaned, "$2");

        // Handle skill type formatting
        cleaned = SkillTypeLinkRegex().Replace(cleaned, "$1");

        // Remove any remaining wiki markup
        cleaned = BoldTextRegex().Replace(cleaned, "$1"); // Bold
        cleaned = ItalicTextRegex().Replace(cleaned, "$1"); // Italic
        cleaned = HtmlTagRegex().Replace(cleaned, ""); // HTML tags

        // Clean up extra whitespace
        cleaned = WhitespaceRegex().Replace(cleaned, " ").Trim();

        return cleaned;
    }

    private static string? ExtractSkillInfobox(string wikiText)
    {
        var startPattern = "{{Skill infobox";
        var startIndex = wikiText.IndexOf(startPattern, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1)
            return null;

        // Find the pipe after "{{Skill infobox"
        var pipeIndex = wikiText.IndexOf('|', startIndex);
        if (pipeIndex == -1)
            return null;

        // Count braces to find the matching closing }}
        var braceCount = 2; // Start with 2 for the opening {{
        var index = startIndex + startPattern.Length;

        while (index < wikiText.Length && braceCount > 0)
        {
            if (wikiText[index] == '{' && index + 1 < wikiText.Length && wikiText[index + 1] == '{')
            {
                braceCount += 2;
                index += 2;
            }
            else if (wikiText[index] == '}' && index + 1 < wikiText.Length && wikiText[index + 1] == '}')
            {
                braceCount -= 2;
                index += 2;
            }
            else
            {
                index++;
            }
        }

        if (braceCount == 0)
        {
            // Extract content between the pipe and the closing }}
            return wikiText.Substring(pipeIndex + 1, index - pipeIndex - 3).Trim();
        }

        return null;
    }

    private static string? ExtractField(string infoboxContent, string fieldName)
    {
        var pattern = $@"(?:^|\|\s*){Regex.Escape(fieldName)}\s*=\s*(.*?)(?=\s*\||$)";
        var match = Regex.Match(infoboxContent, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    [GeneratedRegex(@"^#REDIRECT\s*\[\[([^\]]+)\]\]", RegexOptions.IgnoreCase)]
    private static partial Regex RedirectRegex();
    [GeneratedRegex(@"\{\{1/2\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex HalfFractionRegex();
    [GeneratedRegex(@"\{\{1/4\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex QuarterFractionRegex();
    [GeneratedRegex(@"\{\{3/4\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex ThreeQuarterFractionRegex();
    [GeneratedRegex(@"\{\{3/2\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex ThreeOverTwoFractionRegex();
    [GeneratedRegex(@"\{\{(\d+(?:\.\d+)?)\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex DecimalRegex();
    [GeneratedRegex(@"\{\{gr\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex GreenRangeRegex();
    [GeneratedRegex(@"\{\{gr2\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex GreenRange2Regex();
    [GeneratedRegex(@"\{\{gray\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex GrayRangeRegex();
    [GeneratedRegex(@"\{\{sic(?:\|([^}]*))?\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex SicTextRegex();
    [GeneratedRegex(@"\[\[([^\]|]+)\]\]", RegexOptions.IgnoreCase)]
    private static partial Regex SimpleLinkRegex();
    [GeneratedRegex(@"\[\[([^|]+)\|([^\]]+)\]\]", RegexOptions.IgnoreCase)]
    private static partial Regex LinkWithDisplayTextRegex();
    [GeneratedRegex(@"\[\[([^|]+)\]\]", RegexOptions.IgnoreCase)]
    private static partial Regex SkillTypeLinkRegex();
    [GeneratedRegex(@"'''([^']+)'''", RegexOptions.IgnoreCase)]
    private static partial Regex BoldTextRegex();
    [GeneratedRegex(@"''([^']+)''", RegexOptions.IgnoreCase)]
    private static partial Regex ItalicTextRegex();
    [GeneratedRegex(@"<[^>]*>", RegexOptions.IgnoreCase)]
    private static partial Regex HtmlTagRegex();
    [GeneratedRegex(@"\s+", RegexOptions.IgnoreCase)]
    private static partial Regex WhitespaceRegex();
    [GeneratedRegex(@"\{\{gray\|([^}]+)\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex GrayTextRegex();
    [GeneratedRegex(@"\{\{grey\|([^}]+)\}\}", RegexOptions.IgnoreCase)]
    private static partial Regex GreyTextRegex();
}
