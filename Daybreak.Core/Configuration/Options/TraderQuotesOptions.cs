using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Trader Quotes")]
[OptionsSynchronizationIgnore]
[OptionsIgnore]
internal sealed class TraderQuotesOptions
{
    [OptionIgnore]
    public string HttpsUri { get; set; } = "https://kamadan.gwtoolbox.com/";
}
