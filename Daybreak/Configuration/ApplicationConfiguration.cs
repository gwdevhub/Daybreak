using Daybreak.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration;

public sealed class ApplicationConfiguration
{
    [JsonProperty("SetGuildwarsWindowSizeOnLaunch")]
    public bool SetGuildwarsWindowSizeOnLaunch { get; set; }
    [JsonProperty("DesiredGuildwarsScreen")]
    public int DesiredGuildwarsScreen { get; set; }
    [JsonProperty("BrowsersEnabled")]
    public bool BrowsersEnabled { get; set; } = true;
    [JsonProperty("ToolboxPath")]
    public string? ToolboxPath { get; set; }
    [JsonProperty("TexmodPath")]
    public string? TexmodPath { get; set; }
    [JsonProperty("ToolboxAutoLaunch")]
    public bool ToolboxAutoLaunch { get; set; }
    [JsonProperty("LeftBrowserDefault")]
    public string? LeftBrowserDefault { get; set; } = "https://gwpvx.fandom.com/wiki/PvX_wiki";
    [JsonProperty("RightBrowserDefault")]
    public string? RightBrowserDefault { get; set; } = "https://wiki.guildwars.com/wiki/Quick_access_links";
    [JsonProperty("GuildwarsPaths")]
    public List<GuildwarsPath> GuildwarsPaths { get; set; } = new();
    [JsonProperty("ProtectedLoginCredentials")]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = new();
    [JsonProperty("AddressBarReadonly")]
    public bool AddressBarReadonly { get; set; } = true;
    [JsonProperty("ExperimentalFeatures")]
    public ExperimentalFeatures ExperimentalFeatures { get; set; } = new();
    [JsonProperty("ShortcutLocation")]
    public string? ShortcutLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    [JsonProperty("PlaceShortcut")]
    public bool PlaceShortcut { get; set; }
    [JsonProperty("AutoCheckUpdate")]
    public bool AutoCheckUpdate { get; set; } = true;
    [JsonProperty("ProtectedGraphAccessToken")]
    public string? ProtectedGraphAccessToken { get; set; }
    [JsonProperty("ProtectedGraphRefreshToken")]
    public string? ProtectedGraphRefreshToken { get; set; }
}
