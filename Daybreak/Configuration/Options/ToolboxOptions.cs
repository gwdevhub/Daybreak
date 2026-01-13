using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "GWToolbox")]
[OptionsIgnore]
internal sealed class ToolboxOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will also launch GWToolboxdll when launching GuildWars")]
    public bool Enabled { get; set; }
}
