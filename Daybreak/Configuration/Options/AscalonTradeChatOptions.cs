using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Ascalon Trade Chat")]
public sealed class AscalonTradeChatOptions : ITradeChatOptions
{
    [OptionName(Name = "Https Uri", Description = "Base https uri of the trade chat")]
    public string HttpsUri { get; set; } = "https://ascalon.gwtoolbox.com/";

    [OptionName(Name = "Wss Uri", Description = "Base wss uri of the trade chat")]
    public string WssUri { get; set; } = "wss://ascalon.gwtoolbox.com/";

    [OptionName(Name = "Refresh Interval", Description = "Amount of seconds to wait in between polling requests")]
    [OptionRange<double>(MinValue = 1, MaxValue = 60)]
    public double RefreshInterval { get; set; } = 10;
}
