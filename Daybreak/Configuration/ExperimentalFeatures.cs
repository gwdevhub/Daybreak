using Newtonsoft.Json;

namespace Daybreak.Configuration
{
    public sealed class ExperimentalFeatures
    {
        [JsonProperty("MultiLaunchSupport")]
        public bool MultiLaunchSupport { get; set; }
        [JsonProperty("ToolboxAutoLaunchDelay")]
        public int ToolboxAutoLaunchDelay { get; set; } = 5000;
    }
}
