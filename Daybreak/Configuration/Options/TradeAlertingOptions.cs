using Daybreak.Attributes;
using Daybreak.Shared.Models.Trade;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Trade Alerts")]
internal sealed class TradeAlertingOptions
{
    [OptionIgnore]
    [OptionSynchronizationIgnore]
    public TimeSpan MaxLookbackPeriod { get; set; } = TimeSpan.FromDays(31);

    [OptionIgnore]
    [OptionSynchronizationIgnore]
    public DateTime LastCheckTime { get; set; } = DateTime.MinValue;

    [OptionIgnore]
    public List<ITradeAlert> Alerts { get; set; } = [];

    [OptionName(Name = "Quote Alerts Interval", Description = "The amount of seconds in between checking for alerts based on trade quotes")]
    [OptionRange<double>(MinValue = 30d, MaxValue = 1440d)]
    public double QuoteAlertsInterval { get; set; } = 30;
}
