using Newtonsoft.Json;

namespace Daybreak.Configuration
{
    public sealed class ApplicationConfiguration
    {
        [JsonProperty("GamePath")]
        public string GamePath { get; set; }
        [JsonProperty("CharacterName")]
        public string CharacterName { get; set; }
        [JsonProperty("LeftBrowserDefault")]
        public string LeftBrowserDefault { get; set; }
        [JsonProperty("RightBrowserDefault")]
        public string RightBrowserDefault { get; set; }
    }
}
