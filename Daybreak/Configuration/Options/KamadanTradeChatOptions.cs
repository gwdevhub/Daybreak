using Daybreak.Attributes;
using Daybreak.Shared.Configuration.Options;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Kamadan Trade Chat")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
public sealed class KamadanTradeChatOptions : ITradeChatOptions
{
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    public string WssUri { get; set; } = "wss://kamadan.gwtoolbox.com/";
}
