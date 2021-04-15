using Daybreak.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Daybreak.Configuration
{
    public sealed class ApplicationConfiguration
    {
        [JsonProperty("ToolboxPath")]
        public string ToolboxPath { get; set; }
        [JsonProperty("ToolboxAutoLaunch")]
        public bool ToolboxAutoLaunch { get; set; }
        [JsonProperty("LeftBrowserDefault")]
        public string LeftBrowserDefault { get; set; } = "https://gwpvx.fandom.com/wiki/PvX_wiki";
        [JsonProperty("RightBrowserDefault")]
        public string RightBrowserDefault { get; set; } = "https://wiki.guildwars.com/wiki/Quick_access_links";
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
