using System.Globalization;
using System.Text.RegularExpressions;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// One skill page on the wiki: the display name Daybreak knows the skill by,
/// and the skill ids that name covers.
/// </summary>
/// <remarks>
/// The wiki is the roster only — it says which skills exist and what to call
/// them. Every emitted value comes from the GWToolbox API instead
/// (<see cref="SkillMapper"/>). The wiki is kept for the roster because the
/// client's own names are not unique: four different skills are called "Charm
/// Animal", where the wiki disambiguates them ("Charm Animal (White Mantle)"),
/// and those unique names are what the generated C# identifiers and the icon
/// lookup are keyed on.
/// </remarks>
public sealed record WikiSkillEntry(string Name, IReadOnlyList<int> Ids)
{
    /// <summary>
    /// Filenames (without the <c>File:</c> prefix) the icon resolver should
    /// try, in priority order. Shouts include the quoted form because their
    /// image files preserve the surrounding quotes
    /// (e.g. <c>"Save Yourselves!".jpg</c>).
    /// </summary>
    public IReadOnlyList<string> IconBaseNames { get; init; } = [];
}

/// <summary>
/// Pulls the roster out of a wiki page's <c>{{Skill infobox …}}</c>: the
/// <c>id</c> field, and nothing else.
/// </summary>
internal static partial class WikiSkillRosterParser
{
    /// <summary>
    /// Reads the skill ids out of a page's infobox. Some infoboxes carry
    /// several ids in the one field (e.g. <c>id = 1954, 2097</c> for the
    /// Luxon and Kurzick "Save Yourselves!"); each becomes its own emitted
    /// skill, with its own values from the API.
    /// </summary>
    public static bool TryParseIds(string? wikiText, out IReadOnlyList<int> ids)
    {
        ids = [];
        if (string.IsNullOrWhiteSpace(wikiText))
        {
            return false;
        }

        var body = ExtractInfoboxBody(wikiText);
        if (body is null)
        {
            return false;
        }

        var match = IdFieldRegex().Match(body);
        if (!match.Success)
        {
            return false;
        }

        var parsed = new List<int>();
        foreach (Match integer in IntegerRegex().Matches(match.Groups[1].Value))
        {
            if (int.TryParse(integer.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                parsed.Add(id);
            }
        }

        ids = parsed;
        return parsed.Count > 0;
    }

    /// <summary>
    /// Returns the infobox's body by matching braces from <c>{{Skill infobox</c>
    /// so that nested templates don't end it early.
    /// </summary>
    private static string? ExtractInfoboxBody(string wikiText)
    {
        const string startPattern = "{{Skill infobox";
        var startIndex = wikiText.IndexOf(startPattern, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1)
        {
            return null;
        }

        var pipeIndex = wikiText.IndexOf('|', startIndex);
        if (pipeIndex == -1)
        {
            return null;
        }

        var braceCount = 2;
        var index = startIndex + startPattern.Length;
        while (index < wikiText.Length && braceCount > 0)
        {
            if (wikiText[index] == '{' && index + 1 < wikiText.Length && wikiText[index + 1] == '{') { braceCount += 2; index += 2; }
            else if (wikiText[index] == '}' && index + 1 < wikiText.Length && wikiText[index + 1] == '}') { braceCount -= 2; index += 2; }
            else { index++; }
        }

        if (braceCount != 0)
        {
            return null;
        }

        return wikiText.Substring(pipeIndex + 1, index - pipeIndex - 3).Trim();
    }

    [GeneratedRegex(@"(?:^|\|)\s*id\s*=\s*([^|\r\n]*)", RegexOptions.IgnoreCase)] private static partial Regex IdFieldRegex();
    [GeneratedRegex(@"\d+")] private static partial Regex IntegerRegex();
}
