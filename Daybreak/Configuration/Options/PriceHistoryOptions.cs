using Daybreak.Attributes;
using Daybreak.Services.TradeChat.Models;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Price History")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class PriceHistoryOptions : ILiteCollectionOptions<TraderQuoteDTO>
{
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    public string CollectionName => "price_history";

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromHours(1);

    public Dictionary<string, DateTime> ItemHistoryMetadata { get; set; } = [];
}
