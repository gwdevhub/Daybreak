using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Github;

public sealed class GithubRefTag
{
    [JsonPropertyName("ref")]
    public string? Ref { get; set; }
    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
