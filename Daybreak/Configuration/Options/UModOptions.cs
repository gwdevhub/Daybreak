﻿using Daybreak.Attributes;
using Daybreak.Models.UMod;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "uMod")]
public sealed class UModOptions
{
    [JsonProperty(nameof(DllPath))]
    [OptionName(Name = "Dll Path", Description = "The path to the uMod dll")]
    public string? DllPath { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch uMod when launching GuildWars")]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(AutoEnableMods))]
    [OptionName(Name = "Auto-Enable Mods", Description = "If true, mods downloaded through the launcher will be auto-placed in the managed mod list")]
    public bool AutoEnableMods { get; set; } = true;

    [JsonProperty(nameof(Mods))]
    [OptionIgnore]
    public List<UModEntry> Mods { get; set; } = new();
}
