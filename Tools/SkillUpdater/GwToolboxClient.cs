using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// Downloads the GWToolbox API's skill catalogue — the single source of truth
/// for every skill *value* the generator emits. One request; no throttling
/// needed, unlike <see cref="WikiHttpClient"/>.
/// </summary>
internal sealed class GwToolboxClient(string userAgent) : IDisposable
{
    private const string SkillsUrl = "https://api.gwtoolbox.com/v1/en/skills.json";

    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient httpClient = CreateHttpClient(userAgent);

    private static HttpClient CreateHttpClient(string userAgent)
    {
        var client = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };
        client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    /// <summary>Fetches the catalogue indexed by skill id.</summary>
    public async Task<GwToolboxCatalog> FetchSkillsAsync(CancellationToken cancellationToken)
    {
        await using var stream = await this.httpClient.GetStreamAsync(SkillsUrl, cancellationToken);
        var skills = await JsonSerializer.DeserializeAsync<List<GwToolboxSkill>>(stream, SerializerOptions, cancellationToken)
            ?? throw new InvalidOperationException($"GET {SkillsUrl} returned no skills.");

        return new GwToolboxCatalog(skills);
    }

    public void Dispose() => this.httpClient.Dispose();
}

/// <summary>
/// The API's skills indexed for lookup by id, and — for the wiki pages that
/// omit the id — by name.
/// </summary>
internal sealed partial class GwToolboxCatalog
{    private readonly Dictionary<int, GwToolboxSkill> byId;
    private readonly Dictionary<string, List<GwToolboxSkill>> byName;

    public GwToolboxCatalog(IReadOnlyList<GwToolboxSkill> skills)
    {
        this.byId = new Dictionary<int, GwToolboxSkill>(skills.Count);
        this.byName = new Dictionary<string, List<GwToolboxSkill>>(StringComparer.Ordinal);
        foreach (var skill in skills)
        {
            this.byId[skill.Id] = skill;

            var name = NormalizeName(skill.Name);
            if (name.Length == 0 || name == "(none)")
            {
                continue;
            }

            if (!this.byName.TryGetValue(name, out var bucket))
            {
                this.byName[name] = bucket = [];
            }

            bucket.Add(skill);
        }
    }

    public int Count => this.byId.Count;

    public bool TryGetById(int id, out GwToolboxSkill skill) => this.byId.TryGetValue(id, out skill!);

    /// <summary>
    /// Resolves a skill by name, but only when the name identifies exactly one
    /// — the client reuses names freely, so anything else is ambiguous.
    /// </summary>
    public bool TryGetUniqueByName(string name, out GwToolboxSkill skill)
    {
        skill = null!;
        if (!this.byName.TryGetValue(NormalizeName(name), out var bucket) || bucket.Count != 1)
        {
            return false;
        }

        skill = bucket[0];
        return true;
    }

    /// <summary>
    /// Matches the wiki's convention: collapsed whitespace (some API names
    /// carry a double space before a "(PvP)" suffix) and no surrounding quotes
    /// (which the API keeps on shouts and the wiki page titles do not).
    /// </summary>
    private static string NormalizeName(string? name)
    {
        var trimmed = WhitespaceRegex().Replace(name ?? string.Empty, " ").Trim();
        return trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"'
            ? trimmed[1..^1]
            : trimmed;
    }

    [GeneratedRegex(@"\s+")] private static partial Regex WhitespaceRegex();
}
