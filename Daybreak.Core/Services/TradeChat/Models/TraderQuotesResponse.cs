using System.Text.Json.Serialization;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderQuotesResponse
{
    [JsonPropertyName("buy")]
    public Dictionary<string, TraderQuotePayload> BuyQuotes { get; set; } = default!;

    [JsonPropertyName("sell")]
    public Dictionary<string, TraderQuotePayload> SellQuotes { get; set; } = default!;
}
