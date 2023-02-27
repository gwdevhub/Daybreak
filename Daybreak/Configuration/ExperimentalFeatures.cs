using Daybreak.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration;

public sealed class ExperimentalFeatures
{
    [JsonProperty("MultiLaunchSupport")]
    public bool MultiLaunchSupport { get; set; }
    [JsonProperty("ToolboxAutoLaunchDelay")]
    public int ToolboxAutoLaunchDelay { get; set; } = 5000;
    [JsonProperty("DynamicBuildLoading")]
    public bool DynamicBuildLoading { get; set; } = true;
    [JsonProperty("LaunchGuildwarsAsCurrentUser")]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;
    [JsonProperty("CanInterceptKeys")]
    public bool CanInterceptKeys { get; set; }
    [JsonProperty("DownloadIcons")]
    public bool DownloadIcons { get; set; }
    [JsonProperty("FocusViewEnabled")]
    public bool FocusViewEnabled { get; set; }
    [JsonProperty("Macros")]
    public List<KeyMacro> Macros { get; set; } = new();
}
