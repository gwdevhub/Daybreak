using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsSynchronizationIgnore]
[OptionsIgnore]
internal sealed class GuildWarsScreenPlacerOptions
{
    [JsonProperty(nameof(Enabled))]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(DesiredScreen))]
    public int DesiredScreen { get; set; }
}
