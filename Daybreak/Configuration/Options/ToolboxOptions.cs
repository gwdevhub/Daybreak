using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "GWToolbox")]
public sealed class ToolboxOptions
{
    [JsonProperty(nameof(DllPath))]
    [OptionName(Name = "DllPath", Description = "The path to GWToolboxdll.dll")]
    public string? DllPath { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch GWToolboxdll when launching GuildWars")]
    public bool Enabled { get; set; }
}
