using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat.Models;

internal sealed class TraderQuotesResponse
{
    [JsonProperty("buy")]
    public Dictionary<string, TraderQuotePayload> BuyQuotes { get; set; } = default!;

    [JsonProperty("sell")]
    public Dictionary<string, TraderQuotePayload> SellQuotes { get; set; } = default!;
}
