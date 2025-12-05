using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "GWToolbox")]
[OptionsIgnore]
internal sealed class ToolboxOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch GWToolboxdll when launching GuildWars")]
    public bool Enabled { get; set; }
}
