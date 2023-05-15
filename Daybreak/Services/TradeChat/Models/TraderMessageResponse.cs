using Newtonsoft.Json;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderMessageResponse
{
    [JsonProperty("m")]
    public string? Message { get; set; }

    [JsonProperty("s")]
    public string? Sender { get; set; }

    [JsonProperty("t")]
    public long? Timestamp { get; set; }

    // Replaces the message identified with this value
    [JsonProperty("r")]
    public long? ReplaceTimestamp { get; set; }
}
