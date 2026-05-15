using System.Globalization;
using System.Text.RegularExpressions;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// A skill in the exact shape the writer will emit it: every value is already
/// the literal C# token it should appear as in <c>Skill.g.cs</c>. This is
/// deliberately tool-specific — the runtime <c>WikiService</c> has its own
/// parser in <c>Daybreak.Shared</c> built around typed model objects.
/// </summary>
public sealed record ParsedSkill(
    int Id,
    string Name,
    string CampaignIdentifier,
    string ProfessionIdentifier,
    string AttributeIdentifier,
    bool PvEOnly,
    bool PvP,
    bool Elite,
    string TypeExpression,
    double? Energy,
    double? Activation,
    double? Recharge,
    double? Overcast,
    double? Adrenaline,
    double? Sacrifice,
    double? Upkeep,
    string Description,
    string ConciseDescription)
{
    /// <summary>
    /// Extra ids the wiki listed alongside <see cref="Id"/> in the same
    /// <c>id =</c> field (e.g. <c>id = 1954, 2097</c> for Luxon/Kurzick
    /// "Save Yourselves!"). Each one becomes its own emitted skill.
    /// </summary>
    public IReadOnlyList<int> AdditionalIds { get; init; } = [];

    /// <summary>
    /// Filenames (without the <c>File:</c> prefix) the icon resolver should
    /// try, in priority order. Defaults to just <see cref="Name"/>; shouts
    /// also include the quoted form because their image files preserve the
    /// surrounding quotes (e.g. <c>"Save Yourselves!".jpg</c>).
    /// </summary>
    public IReadOnlyList<string> IconBaseNames { get; init; } = [];
}

/// <summary>
/// Parses a wiki page's <c>{{Skill infobox …}}</c> into a <see cref="ParsedSkill"/>
/// directly addressed at the codegen output. Every helper here exists because
/// the codegen step needs it; nothing is exposed beyond <see cref="TryParse"/>.
/// </summary>
internal static partial class WikiSkillParser
{
    public static bool TryParse(string? wikiText, out ParsedSkill skill)
    {
        skill = null!;
        if (string.IsNullOrWhiteSpace(wikiText))
        {
            return false;
        }

        var cleaned = CleanWikiText(wikiText);
        var body = ExtractInfoboxBody(cleaned);
        if (body is null)
        {
            return false;
        }

        var fields = ExtractFields(body);

        string Get(string key) => fields.TryGetValue(key, out var v) ? v : string.Empty;
        bool YesFlag(string key) => Get(key) is var v && v.Length > 0 && v[0] is 'y' or 'Y';

        var allIds = ParseAllIds(Get("id"));

        skill = new ParsedSkill(
            Id: allIds.Count > 0 ? allIds[0] : 0,
            Name: NormalizeName(Get("name")),
            CampaignIdentifier: ResolveCampaign(Get("campaign")),
            ProfessionIdentifier: ResolveProfession(Get("profession")),
            AttributeIdentifier: ResolveAttribute(Get("attribute")),
            PvEOnly: YesFlag("pve-only"),
            PvP: YesFlag("is-pvp"),
            Elite: YesFlag("elite"),
            TypeExpression: ResolveSkillType(Get("type")),
            Energy: ParseNumber(Get("energy")),
            Activation: ParseNumber(Get("activation")),
            Recharge: ParseNumber(Get("recharge")),
            Overcast: ParseNumber(Get("overcast")),
            Adrenaline: ParseNumber(Get("adrenaline")),
            Sacrifice: ParseNumber(Get("sacrifice")),
            Upkeep: ParseNumber(Get("upkeep")),
            Description: Get("description"),
            ConciseDescription: Get("concise description"));
        if (allIds.Count > 1)
        {
            skill = skill with { AdditionalIds = [.. allIds.Skip(1)] };
        }

        return true;
    }

    /// <summary>
    /// Strips wiki markup the codegen output should not carry over (links,
    /// fraction templates, ranges, italics, HTML).
    /// </summary>
    private static string CleanWikiText(string wikiText)
    {
        var c = wikiText;
        c = HalfFractionRegex().Replace(c, "¹⁄₂");
        c = QuarterFractionRegex().Replace(c, "¹⁄₄");
        c = ThreeQuarterFractionRegex().Replace(c, "³⁄₄");
        c = ThreeOverTwoFractionRegex().Replace(c, "³⁄₂");
        c = DecimalRegex().Replace(c, "$1");
        c = GreenRangeRegex().Replace(c, "$1…$2");
        c = GreenRange2Regex().Replace(c, "$1…$2");
        c = GrayRangeRegex().Replace(c, "$1…$2");
        c = LinkWithDisplayTextRegex().Replace(c, "$2");
        c = SimpleLinkRegex().Replace(c, "$1");
        c = GrayTextRegex().Replace(c, "");
        c = GreyTextRegex().Replace(c, "");
        c = SicTextRegex().Replace(c, "");
        c = BoldTextRegex().Replace(c, "$1");
        c = ItalicTextRegex().Replace(c, "$1");
        c = HtmlTagRegex().Replace(c, "");
        return c;
    }

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

    /// <summary>
    /// Walks the body manually so that values containing pipes inside nested
    /// templates (<c>{{gr|5|11}}</c>) or links don't get truncated.
    /// </summary>
    private static Dictionary<string, string> ExtractFields(string body)
    {
        var fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var i = 0;
        while (i < body.Length)
        {
            while (i < body.Length && (body[i] == '|' || char.IsWhiteSpace(body[i])))
            {
                i++;
            }

            var nameStart = i;
            while (i < body.Length && body[i] != '=' && body[i] != '|')
            {
                i++;
            }

            if (i >= body.Length || body[i] != '=')
            {
                continue;
            }

            var name = body[nameStart..i].Trim();
            i++;

            var valueStart = i;
            int braceDepth = 0, bracketDepth = 0;
            while (i < body.Length)
            {
                var c = body[i];
                if (c == '{' && i + 1 < body.Length && body[i + 1] == '{') { braceDepth++; i += 2; continue; }
                if (c == '}' && i + 1 < body.Length && body[i + 1] == '}') { braceDepth--; i += 2; continue; }
                if (c == '[' && i + 1 < body.Length && body[i + 1] == '[') { bracketDepth++; i += 2; continue; }
                if (c == ']' && i + 1 < body.Length && body[i + 1] == ']') { bracketDepth--; i += 2; continue; }
                if (c == '|' && braceDepth == 0 && bracketDepth == 0)
                {
                    break;
                }

                i++;
            }

            var value = WhitespaceRegex().Replace(body[valueStart..i].Trim(), " ").Trim();
            fields[name] = value;
        }

        return fields;
    }

    /// <summary>
    /// Numeric infobox value → <c>double?</c>. Empty / non-numeric text
    /// (percentages, "morale boost", etc.) becomes <c>null</c>.
    /// </summary>
    private static double? ParseNumber(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var s = raw.Trim();

        // Plain mixed/unit fractions glyph-substituted by CleanWikiText.
        var fraction = s.Contains("¹⁄₂") ? 0.5
            : s.Contains("¹⁄₄") ? 0.25
            : s.Contains("³⁄₄") ? 0.75
            : s.Contains("³⁄₂") ? 1.5
            : (double?)null;

        if (fraction is double frac)
        {
            var fractionStart = s.IndexOfAny(['¹', '³']);
            if (fractionStart > 0 &&
                double.TryParse(s[..fractionStart].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var whole))
            {
                return whole + frac;
            }

            return frac;
        }

        if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
        {
            return d;
        }

        if (s.EndsWith('%') &&
            double.TryParse(s[..^1].TrimEnd('+'), NumberStyles.Float, CultureInfo.InvariantCulture, out var pct))
        {
            return pct / 100.0;
        }

        return null;
    }

    /// <summary>
    /// Some skill infoboxes carry multiple ids in one field
    /// (e.g. <c>id = 1954, 2097</c> for Luxon/Kurzick variants of "Save Yourselves!").
    /// We return every integer found in source order; the writer turns each
    /// one into its own emitted <c>Skill</c> entry.
    /// </summary>
    private static List<int> ParseAllIds(string raw)
    {
        var ids = new List<int>();
        if (string.IsNullOrWhiteSpace(raw))
        {
            return ids;
        }

        foreach (Match match in FirstIntegerRegex().Matches(raw))
        {
            if (int.TryParse(match.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                ids.Add(id);
            }
        }

        return ids;
    }

    /// <summary>
    /// Strips the surrounding double quotes some wiki page titles carry
    /// (shouts, e.g. <c>"Save Yourselves!"</c>) so the C# identifier and
    /// display name match the historical Daybreak convention.
    /// </summary>
    private static string NormalizeName(string raw)
    {
        var trimmed = raw.Trim();
        if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
        {
            return trimmed[1..^1];
        }

        return trimmed;
    }

    private static string ResolveSkillType(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return "SkillType.None";
        }

        var seen = new HashSet<string>(StringComparer.Ordinal);
        var parts = new List<string>();
        foreach (var token in TypeTokenSplitRegex().Split(raw))
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                continue;
            }

            if (TypeTokens.TryGetValue(token, out var canonical) && seen.Add(canonical))
            {
                parts.Add($"SkillType.{canonical}");
            }
        }

        return parts.Count == 0 ? "SkillType.None" : string.Join(" | ", parts);
    }

    private static string ResolveCampaign(string raw) =>
        Campaigns.GetValueOrDefault(raw.Trim(), "None");

    private static string ResolveProfession(string raw) =>
        Professions.GetValueOrDefault(raw.Trim(), "None");

    private static string ResolveAttribute(string raw) =>
        Attributes.GetValueOrDefault(raw.Trim(), "None");

    // -------------------------------------------------------------------------
    // The lookup tables intentionally live in this file, since they're a
    // codegen detail of the parser — the runtime types in Daybreak.Shared
    // already have their own canonical name lists.
    // -------------------------------------------------------------------------

    private static readonly Dictionary<string, string> Campaigns = new(StringComparer.OrdinalIgnoreCase)
    {
        [""] = "None",
        ["None"] = "None",
        ["Core"] = "Core",
        ["Prophecies"] = "Prophecies",
        ["Factions"] = "Factions",
        ["Nightfall"] = "Nightfall",
        ["Eye of the North"] = "EyeOfTheNorth",
        ["EotN"] = "EyeOfTheNorth",
        ["Bonus Mission Pack"] = "BonusMissionPack",
        ["BMP"] = "BonusMissionPack",
    };

    private static readonly Dictionary<string, string> Professions = new(StringComparer.OrdinalIgnoreCase)
    {
        [""] = "None",
        ["None"] = "None",
        ["Common"] = "None",
        ["Warrior"] = "Warrior",
        ["Ranger"] = "Ranger",
        ["Monk"] = "Monk",
        ["Necromancer"] = "Necromancer",
        ["Mesmer"] = "Mesmer",
        ["Elementalist"] = "Elementalist",
        ["Assassin"] = "Assassin",
        ["Ritualist"] = "Ritualist",
        ["Paragon"] = "Paragon",
        ["Dervish"] = "Dervish",
    };

    private static readonly Dictionary<string, string> Attributes = new(StringComparer.OrdinalIgnoreCase)
    {
        [""] = "None",
        ["None"] = "None",
        ["Fast Casting"] = "FastCasting",
        ["Illusion Magic"] = "IllusionMagic",
        ["Domination Magic"] = "DominationMagic",
        ["Inspiration Magic"] = "InspirationMagic",
        ["Blood Magic"] = "BloodMagic",
        ["Death Magic"] = "DeathMagic",
        ["Soul Reaping"] = "SoulReaping",
        ["Curses"] = "Curses",
        ["Air Magic"] = "AirMagic",
        ["Earth Magic"] = "EarthMagic",
        ["Fire Magic"] = "FireMagic",
        ["Water Magic"] = "WaterMagic",
        ["Energy Storage"] = "EnergyStorage",
        ["Healing Prayers"] = "HealingPrayers",
        ["Smiting Prayers"] = "SmitingPrayers",
        ["Protection Prayers"] = "ProtectionPrayers",
        ["Divine Favor"] = "DivineFavor",
        ["Strength"] = "Strength",
        ["Axe Mastery"] = "AxeMastery",
        ["Hammer Mastery"] = "HammerMastery",
        ["Swordsmanship"] = "Swordsmanship",
        ["Tactics"] = "Tactics",
        ["Beast Mastery"] = "BeastMastery",
        ["Expertise"] = "Expertise",
        ["Wilderness Survival"] = "WildernessSurvival",
        ["Marksmanship"] = "Marksmanship",
        ["Critical Strikes"] = "CriticalStrikes",
        ["Dagger Mastery"] = "DaggerMastery",
        ["Deadly Arts"] = "DeadlyArts",
        ["Shadow Arts"] = "ShadowArts",
        ["Spawning Power"] = "SpawningPower",
        ["Channeling Magic"] = "ChannelingMagic",
        ["Communing"] = "Communing",
        ["Restoration Magic"] = "RestorationMagic",
        ["Spear Mastery"] = "SpearMastery",
        ["Command"] = "Command",
        ["Motivation"] = "Motivation",
        ["Leadership"] = "Leadership",
        ["Scythe Mastery"] = "ScytheMastery",
        ["Wind Prayers"] = "WindPrayers",
        ["Earth Prayers"] = "EarthPrayers",
        ["Mysticism"] = "Mysticism",
    };

    private static readonly Dictionary<string, string> TypeTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        ["skill"] = "Skill",
        ["touch"] = "Touch",
        ["spell"] = "Spell",
        ["hex"] = "Hex",
        ["enchantment"] = "Enchantment",
        ["flash"] = "Flash",
        ["echo"] = "Echo",
        ["ward"] = "Ward",
        ["glyph"] = "Glyph",
        ["well"] = "Well",
        ["ritual"] = "Ritual",
        ["binding"] = "Binding",
        ["nature"] = "Nature",
        ["ebon"] = "EbonVanguard",
        ["vanguard"] = "EbonVanguard",
        ["item"] = "Item",
        ["weapon"] = "Weapon",
        ["attack"] = "Attack",
        ["axe"] = "Axe",
        ["bow"] = "Bow",
        ["melee"] = "Melee",
        ["hammer"] = "Hammer",
        ["sword"] = "Sword",
        ["spear"] = "Spear",
        ["pet"] = "Pet",
        ["lead"] = "Lead",
        ["off-hand"] = "OffHand",
        ["offhand"] = "OffHand",
        ["dual"] = "Dual",
        ["scythe"] = "Scythe",
        ["ranged"] = "Ranged",
        ["chant"] = "Chant",
        ["shout"] = "Shout",
        ["signet"] = "Signet",
        ["preparation"] = "Preparation",
        ["stance"] = "Stance",
        ["form"] = "Form",
        ["trap"] = "Trap",
    };

    [GeneratedRegex(@"\{\{1/2\}\}", RegexOptions.IgnoreCase)] private static partial Regex HalfFractionRegex();
    [GeneratedRegex(@"\{\{1/4\}\}", RegexOptions.IgnoreCase)] private static partial Regex QuarterFractionRegex();
    [GeneratedRegex(@"\{\{3/4\}\}", RegexOptions.IgnoreCase)] private static partial Regex ThreeQuarterFractionRegex();
    [GeneratedRegex(@"\{\{3/2\}\}", RegexOptions.IgnoreCase)] private static partial Regex ThreeOverTwoFractionRegex();
    [GeneratedRegex(@"\{\{(\d+(?:\.\d+)?)\}\}")] private static partial Regex DecimalRegex();
    [GeneratedRegex(@"\{\{gr\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)] private static partial Regex GreenRangeRegex();
    [GeneratedRegex(@"\{\{gr2\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)] private static partial Regex GreenRange2Regex();
    [GeneratedRegex(@"\{\{gray\|(\+?-?\d+)\|(\+?-?\d+)\|?-?\}\}", RegexOptions.IgnoreCase)] private static partial Regex GrayRangeRegex();
    [GeneratedRegex(@"\{\{sic(?:\|([^}]*))?\}\}", RegexOptions.IgnoreCase)] private static partial Regex SicTextRegex();
    [GeneratedRegex(@"\[\[([^\]|]+)\]\]")] private static partial Regex SimpleLinkRegex();
    [GeneratedRegex(@"\[\[([^|\]]+)\|([^\]]+)\]\]")] private static partial Regex LinkWithDisplayTextRegex();
    [GeneratedRegex(@"'''([^']+)'''")] private static partial Regex BoldTextRegex();
    [GeneratedRegex(@"''([^']+)''")] private static partial Regex ItalicTextRegex();
    [GeneratedRegex(@"<[^>]*>")] private static partial Regex HtmlTagRegex();
    [GeneratedRegex(@"\s+")] private static partial Regex WhitespaceRegex();
    [GeneratedRegex(@"\{\{gray\|([^}]+)\}\}", RegexOptions.IgnoreCase)] private static partial Regex GrayTextRegex();
    [GeneratedRegex(@"\{\{grey\|([^}]+)\}\}", RegexOptions.IgnoreCase)] private static partial Regex GreyTextRegex();
    [GeneratedRegex(@"[\s\-]+")] private static partial Regex TypeTokenSplitRegex();
    [GeneratedRegex(@"\d+")] private static partial Regex FirstIntegerRegex();
}
