using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.UMod;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "uMod")]
[OptionsIgnore]
public sealed class UModOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch uMod when launching GuildWars")]
    public bool Enabled { get; set; }

    [JsonPropertyName(nameof(AutoEnableMods))]
    [OptionName(Name = "Auto-Enable Mods", Description = "If true, mods loaded into Daybreak will be enabled by default")]
    public bool AutoEnableMods { get; set; } = true;

    [JsonPropertyName(nameof(Mods))]
    [OptionIgnore]
    [OptionSynchronizationIgnore]
    public List<UModEntry> Mods { get; set; } = [];
}
