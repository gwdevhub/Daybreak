using Newtonsoft.Json;

namespace Daybreak.Models.Browser
{
    public sealed class OnContextMenuPayload
    {
        [JsonProperty("X")]
        public double X { get; set; }
        [JsonProperty("Y")]
        public double Y { get; set; }
        [JsonProperty("Selection")]
        public string? Selection { get; set; }
    }
}
