using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "GWToolbox")]
public sealed class ToolboxOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "The path to the GWToolbox executable")]
    public string? Path { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch GWToolbox when launching GuildWars")]
    public bool Enabled { get; set; }
}
