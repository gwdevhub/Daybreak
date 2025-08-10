using Daybreak.Shared.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;
[OptionsName(Name = "DXVK")]
internal sealed class DXVKOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "The path to the DXVK installation")]
    [OptionSynchronizationIgnore]
    public string? Path { get; set; }

    [JsonProperty(nameof(Version))]
    [OptionSynchronizationIgnore]
    [OptionIgnore]
    public string? Version { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch DXVK when launching GuildWars")]
    public bool Enabled { get; set; }
}
