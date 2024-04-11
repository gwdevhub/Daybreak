using Daybreak.Attributes;
using Daybreak.Services.PriceChecker.Models;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Price Checker")]
internal sealed class PriceCheckerOptions : ILiteCollectionOptions<PriceCheckDTO>
{
    [OptionName(Name = "Enabled", Description = "If true, the price checker will periodically check prices for items in inventory")]
    public bool Enabled { get; set; } = true;

    [OptionName(Name = "Check Interval", Description = "The interval in seconds between checking inventory and calculating prices")]
    [OptionRange<double>(MinValue = 1d, MaxValue = 30d)]
    public double CheckInterval { get; set; } = 5;

    [OptionName(Name = "Cache Duration", Description = "The amount of hours Daybreak will remember an item for a user. After this period expires, Daybreak will emit another notification for the same item if it detects it")]
    [OptionRange<double>(MinValue = 1d, MaxValue = 720d)]
    public double ItemCacheDuration { get; set; } = 24;

    [OptionName(Name = "Minimum Price", Description = "The lower threshold to trigger a notification")]
    [OptionRange<double>(MinValue = 200d, MaxValue = 10000d)]
    public double MinimumPrice { get; set; } = 300;

    [OptionIgnore]
    [OptionSynchronizationIgnore]
    public string CollectionName { get; } = "pricechecker_cache";
}
