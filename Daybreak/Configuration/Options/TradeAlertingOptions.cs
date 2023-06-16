using Daybreak.Attributes;
using Daybreak.Models.Trade;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
public sealed class TradeAlertingOptions
{
    public TimeSpan MaxLookbackPeriod { get; set; } = TimeSpan.FromDays(31);
    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;
    public List<TradeAlert> Alerts { get; set; } = new();
}
