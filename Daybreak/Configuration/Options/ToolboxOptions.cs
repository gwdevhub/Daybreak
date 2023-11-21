using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "GWToolbox")]
internal sealed class ToolboxOptions
{
    [JsonProperty(nameof(DllPath))]
    [OptionName(Name = "DllPath", Description = "The path to GWToolboxdll.dll")]
    public string? DllPath { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch GWToolboxdll when launching GuildWars")]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(StartupDelay))]
    [OptionName(Name = "Startup Delay", Description = "Amount of seconds that Daybreak will wait for GWToolbox to start before continuing with the other mods")]
    [OptionRange<double>(MinValue = 1, MaxValue = 10)]
    public double StartupDelay { get; set; } = 1;
}
