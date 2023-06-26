using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;
[OptionsName(Name = "DSOAL")]
public sealed class DSOALOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "The path to the DSOAL installation")]
    public string? Path { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch DSOAL when launching GuildWars")]
    public bool Enabled { get; set; }
}
