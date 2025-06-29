using Newtonsoft.Json;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderMessageQueryResponse
{
    [JsonProperty("query")]
    public string? Query { get; set; }

    [JsonProperty("num_results")]
    public int ResultCount { get; set; }

    [JsonProperty("messages")]
    public List<TraderMessageResponse>? Messages { get; set; }
}
