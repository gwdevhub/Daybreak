using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "DXVK")]
[OptionsIgnore]
internal sealed class DXVKOptions
{
    [JsonPropertyName(nameof(Version))]
    [OptionSynchronizationIgnore]
    [OptionIgnore]
    public string? Version { get; set; }

    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch DXVK when launching GuildWars")]
    public bool Enabled { get; set; }
}
