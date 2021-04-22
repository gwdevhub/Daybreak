using Newtonsoft.Json;

namespace Daybreak.Configuration
{
    public sealed class ExperimentalFeatures
    {
        [JsonProperty("MultiLaunchSupport")]
        public bool MultiLaunchSupport { get; set; }
        [JsonProperty("ToolboxAutoLaunchDelay")]
        public int ToolboxAutoLaunchDelay { get; set; } = 5000;
        [JsonProperty("DynamicBuildLoading")]
        public bool DynamicBuildLoading { get; set; } = true;
    }
}
