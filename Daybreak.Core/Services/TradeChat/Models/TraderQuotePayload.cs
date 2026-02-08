using Daybreak.Shared.Converters;
using System.Text.Json.Serialization;

namespace Daybreak.Services.TradeChat.Models;
internal sealed class TraderQuotePayload
{
    [JsonPropertyName("p")]
    public int Price { get; set; }

    [JsonPropertyName("t")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime TimeStamp { get; set; }

    [JsonPropertyName("s")]
    public int Type { get; set; }
}
