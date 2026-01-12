using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Github;

public sealed class GithubRelease
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("tag_name")]
    public required string TagName { get; init; }
    [JsonPropertyName("body")]
    public required string Body { get; init; }
    [JsonPropertyName("assets")]
    public required IReadOnlyList<GithubReleaseAsset> Assets { get; init; }
}
