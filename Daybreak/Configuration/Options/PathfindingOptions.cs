using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Pathfinding")]
public sealed class PathfindingOptions
{
    [JsonProperty(nameof(EnablePathfinding))]
    [OptionName(Name = "Enable Pathfinding", Description = "If true, the pathfinder will attempt to produce paths from the player position to objectives")]
    public bool EnablePathfinding { get; set; } = true;

    [JsonProperty(nameof(ImprovedPathfinding))]
    [OptionName(Name = "Distance-Based Pathfinding", Description = "If true, the pathfinder will attempt use an improved algorithm to find paths. Slower but generally more precise")]
    public bool ImprovedPathfinding { get; set; } = false;

    [JsonProperty(nameof(OptimizePaths))]
    [OptionName(Name = "Path Optimization", Description = "If true, the pathfinder will attempt to optimize paths. Slower but generally more precise")]
    public bool OptimizePaths { get; set; } = true;

    [JsonProperty(nameof(UseCaching))]
    [OptionName(Name = "Use Caching", Description = "If true, the pathfinder will use more memory to cache parts of the pathfinding algorithm")]
    public bool UseCaching { get; set; } = true;
}
