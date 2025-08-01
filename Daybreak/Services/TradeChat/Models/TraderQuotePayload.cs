﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Daybreak.Services.TradeChat.Models;
internal sealed class TraderQuotePayload
{
    [JsonProperty("p")]
    public int Price { get; set; }

    [JsonProperty("t")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime TimeStamp { get; set; }

    [JsonProperty("s")]
    public int Type { get; set; }
}
