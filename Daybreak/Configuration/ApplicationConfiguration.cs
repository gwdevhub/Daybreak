using Daybreak.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration
{
    public sealed class ApplicationConfiguration
    {
        [JsonProperty("ToolboxPath")]
        public string ToolboxPath { get; set; }
        [JsonProperty("LeftBrowserDefault")]
        public string LeftBrowserDefault { get; set; }
        [JsonProperty("RightBrowserDefault")]
        public string RightBrowserDefault { get; set; }
        [JsonProperty("GuildwarsPaths")]
        public List<GuildwarsPath> GuildwarsPaths { get; set; } = new();
        [JsonProperty("ProtectedLoginCredentials")]
        public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = new();
        [JsonProperty("AddressBarReadonly")]
        public bool AddressBarReadonly { get; set; } = true;
        [JsonProperty("ExperimentalFeatures")]
        public ExperimentalFeatures ExperimentalFeatures { get; set; } = new();
    }
}
