using Daybreak.Attributes;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Price History")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class PriceHistoryOptions
{
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromHours(1);

    public Dictionary<string, DateTime> PricingHistoryMetadata { get; set; } = [];
}
