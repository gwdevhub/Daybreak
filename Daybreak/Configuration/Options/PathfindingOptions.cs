using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Pathfinding")]
internal sealed class PathfindingOptions
{
    [JsonProperty(nameof(EnablePathfinding))]
    [OptionName(Name = "Enable Pathfinding", Description = "If true, the pathfinder will attempt to produce paths from the player position to objectives")]
    public bool EnablePathfinding { get; set; } = true;

    [JsonProperty(nameof(HighSensitivity))]
    [OptionName(Name = "High Sensitivity", Description = "If true, the pathfinder generate much more accurate paths. This will greatly increase memory usage and map load times")]
    public bool HighSensitivity { get; set; } = false;
}
