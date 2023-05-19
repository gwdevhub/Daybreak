using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Kamadan Trade Chat")]
[OptionsIgnore]
public sealed class KamadanTradeChatOptions : ITradeChatOptions
{
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";

    public string WssUri { get; set; } = "wss://kamadan.gwtoolbox.com/";
}
