using Daybreak.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration;

public sealed class ExperimentalFeatures
{
    [JsonProperty(nameof(MultiLaunchSupport))]
    public bool MultiLaunchSupport { get; set; }
    [JsonProperty(nameof(ToolboxAutoLaunchDelay))]
    public int ToolboxAutoLaunchDelay { get; set; } = 5000;
    [JsonProperty(nameof(DynamicBuildLoading))]
    public bool DynamicBuildLoading { get; set; } = true;
    [JsonProperty(nameof(EnablePathfinding))]
    public bool EnablePathfinding { get; set; } = true;
    [JsonProperty(nameof(LaunchGuildwarsAsCurrentUser))]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;
    [JsonProperty(nameof(CanInterceptKeys))]
    public bool CanInterceptKeys { get; set; }
    [JsonProperty(nameof(DownloadIcons))]
    public bool DownloadIcons { get; set; }
    [JsonProperty(nameof(FocusViewEnabled))]
    public bool FocusViewEnabled { get; set; }
    [JsonProperty(nameof(MemoryReaderFrequency))]
    public double MemoryReaderFrequency { get; set; } = 16;
    [JsonProperty(nameof(Macros))]
    public List<KeyMacro> Macros { get; set; } = new();
}
