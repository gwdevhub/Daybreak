using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Kamadan Trade Chat")]
public sealed class KamadanTradeChatOptions : ITradeChatOptions
{
    [OptionName(Name = "Https Uri", Description = "Base https uri of the trade chat")]
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    [OptionName(Name = "Wss Uri", Description = "Base wss uri of the trade chat")]
    public string WssUri { get; set; } = "wss://kamadan.gwtoolbox.com/";

    [OptionName(Name = "Refresh Interval", Description = "Amount of seconds to wait in between polling requests")]
    [OptionRange<double>(MinValue = 1, MaxValue = 60)]
    public double RefreshInterval { get; set; } = 10;
}
