using Daybreak.Attributes;
using Daybreak.Models.UMod;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "uMod")]
public sealed class UModOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch uMod when launching GuildWars")]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(AutoEnableMods))]
    [OptionName(Name = "Auto-Enable Mods", Description = "If true, mods loaded into Daybreak will be enabled by default")]
    public bool AutoEnableMods { get; set; } = true;

    [JsonProperty(nameof(Mods))]
    [OptionIgnore]
    [OptionSynchronizationIgnore]
    public List<UModEntry> Mods { get; set; } = [];
}
