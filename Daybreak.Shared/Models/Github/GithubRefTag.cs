using Newtonsoft.Json;

namespace Daybreak.Shared.Models.Github;

public sealed class GithubRefTag
{
    [JsonProperty("ref")]
    public string? Ref;
    [JsonProperty("node_id")]
    public string? NodeId;
    [JsonProperty("url")]
    public string? Url;
}
