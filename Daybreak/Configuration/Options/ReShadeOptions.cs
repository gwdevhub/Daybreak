using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "ReShade")]
[OptionsIgnore]
internal sealed class ReShadeOptions
{
    [JsonPropertyName(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will attempt to inject ReShade into the starting Guild Wars executable")]
    public bool Enabled { get; set; } = false;

    [JsonPropertyName(nameof(AutoUpdate))]
    [OptionName(Name = "Auto-update", Description = "If true, Daybreak will periodically check ReShade for updates")]
    public bool AutoUpdate { get; set; } = true;
}
