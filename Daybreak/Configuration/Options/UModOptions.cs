using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "uMod")]
public sealed class UModOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "The path to the uMod executable")]
    public string? Path { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch uMod when launching GuildWars")]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(AutoEnableMods))]
    [OptionName(Name = "Auto-Enable Mods", Description = "If true, mods downloaded through the launcher will be auto-placed in the managed mod list")]
    public bool AutoEnableMods { get; set; }
}
