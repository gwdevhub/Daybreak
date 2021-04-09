using Newtonsoft.Json;

namespace Daybreak.Configuration
{
    public sealed class ApplicationConfiguration
    {
        [JsonProperty("GamePath")]
        public string GamePath { get; set; }
        [JsonProperty("ToolboxPath")]
        public string ToolboxPath { get; set; }
        [JsonProperty("CharacterName")]
        public string CharacterName { get; set; }
        [JsonProperty("LeftBrowserDefault")]
        public string LeftBrowserDefault { get; set; }
        [JsonProperty("RightBrowserDefault")]
        public string RightBrowserDefault { get; set; }
        [JsonProperty("ProtectedUsername")]
        public string ProtectedUsername { get; set; }
        [JsonProperty("ProtectedPassword")]
        public string ProtectedPassword { get; set; }
        [JsonProperty("AddressBarReadonly")]
        public bool AddressBarReadonly { get; set; } = true;
    }
}
