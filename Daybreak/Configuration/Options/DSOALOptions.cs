using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "DSOAL")]
[OptionsIgnore]
internal sealed class DSOALOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch DSOAL when launching GuildWars")]
    public bool Enabled { get; set; }
}
