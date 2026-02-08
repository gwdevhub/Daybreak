using System.Text.Json.Serialization;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderMessageQueryResponse
{
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    [JsonPropertyName("num_results")]
    public int ResultCount { get; set; }

    [JsonPropertyName("messages")]
    public List<TraderMessageResponse>? Messages { get; set; }
}
