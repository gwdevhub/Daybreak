using System.Text.Json.Serialization;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderMessageResponse
{
    [JsonPropertyName("m")]
    public string? Message { get; set; }

    [JsonPropertyName("s")]
    public string? Sender { get; set; }

    [JsonPropertyName("t")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? Timestamp { get; set; }

    // Replaces the message identified with this value
    [JsonPropertyName("r")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long? ReplaceTimestamp { get; set; }
}
