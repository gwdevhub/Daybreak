﻿using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;
[OptionsName(Name = "DSOAL")]
internal sealed class DSOALOptions
{
    [JsonProperty(nameof(Path))]
    [OptionName(Name = "Path", Description = "The path to the DSOAL installation")]
    [OptionSynchronizationIgnore]
    public string? Path { get; set; }

    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, the launcher will also launch DSOAL when launching GuildWars")]
    public bool Enabled { get; set; }
}
