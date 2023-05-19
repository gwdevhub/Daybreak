using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Ascalon Trade Chat")]
[OptionsIgnore]
public sealed class AscalonTradeChatOptions : ITradeChatOptions
{
    public string HttpsUri { get; set; } = "https://ascalon.gwtoolbox.com/";

    public string WssUri { get; set; } = "wss://ascalon.gwtoolbox.com/";
}
