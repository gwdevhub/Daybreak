using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Toolbox")]
public sealed class ToolboxOptions
{
    [JsonProperty(nameof(Path))]
    public string? Path { get; set; }

    [JsonProperty(nameof(Enabled))]
    public bool Enabled { get; set; }

    [JsonProperty(nameof(AutoLaunchDelay))]
    public int AutoLaunchDelay { get; set; } = 5000;
}
