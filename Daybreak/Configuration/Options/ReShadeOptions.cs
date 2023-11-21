using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "ReShade")]
internal sealed class ReShadeOptions
{
    [JsonProperty(nameof(Enabled))]
    [OptionName(Name = "Enabled", Description = "If true, Daybreak will attempt to inject ReShade into the starting Guild Wars executable")]
    public bool Enabled { get; set; } = false;

    [JsonProperty(nameof(AutoUpdate))]
    [OptionName(Name = "Auto-update", Description = "If true, Daybreak will periodically check ReShade for updates")]
    public bool AutoUpdate { get; set; } = true;

    [OptionIgnore]
    public string InstalledVersion { get; set; } = string.Empty;
}
