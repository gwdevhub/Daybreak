using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Github;

public sealed class GithubReleaseAsset
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("content_type")]
    public required string ContentType { get; init; }

    [JsonPropertyName("browser_download_url")]
    public required string BrowserDownloadUrl { get; init; }
}
