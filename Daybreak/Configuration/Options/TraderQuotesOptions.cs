using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Trader Quotes")]
[OptionsSynchronizationIgnore]
internal sealed class TraderQuotesOptions
{
    [OptionIgnore]
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    [OptionIgnore]
    public DateTime LastCheckTime { get; set;} = DateTime.MinValue;

    [OptionName(Name = "Cache Update Interval", Description = "The amount of minutes in between cache updates")]
    [OptionRange<double>(MinValue = 30d, MaxValue = 1440d)]
    public double CacheUpdateInterval { get; set; } = 60;
}
