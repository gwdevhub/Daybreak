using Newtonsoft.Json;

namespace Daybreak.Models
{
    public sealed class GuildwarsPath
    {
        [JsonProperty("path")]
        public string? Path { get; set; }
        [JsonProperty("default")]
        public bool Default { get; set; }
    }
}
