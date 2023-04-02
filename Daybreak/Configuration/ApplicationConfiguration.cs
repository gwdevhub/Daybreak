using Daybreak.Configuration.FocusView;
using Daybreak.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration;

public sealed class ApplicationConfiguration
{
    [JsonProperty(nameof(SetGuildwarsWindowSizeOnLaunch))]
    public bool SetGuildwarsWindowSizeOnLaunch { get; set; }
    [JsonProperty(nameof(DesiredGuildwarsScreen))]
    public int DesiredGuildwarsScreen { get; set; }
    [JsonProperty(nameof(BrowsersEnabled))]
    public bool BrowsersEnabled { get; set; } = true;
    [JsonProperty(nameof(ToolboxPath))]
    public string? ToolboxPath { get; set; }
    [JsonProperty(nameof(TexmodPath))]
    public string? TexmodPath { get; set; }
    [JsonProperty(nameof(ToolboxAutoLaunch))]
    public bool ToolboxAutoLaunch { get; set; }
    [JsonProperty(nameof(LeftBrowserDefault))]
    public string? LeftBrowserDefault { get; set; } = "https://gwpvx.fandom.com/wiki/PvX_wiki";
    [JsonProperty(nameof(RightBrowserDefault))]
    public string? RightBrowserDefault { get; set; } = "https://wiki.guildwars.com/wiki/Quick_access_links";
    [JsonProperty(nameof(GuildwarsPaths))]
    public List<GuildwarsPath> GuildwarsPaths { get; set; } = new();
    [JsonProperty(nameof(ProtectedLoginCredentials))]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = new();
    [JsonProperty(nameof(AddressBarReadonly))]
    public bool AddressBarReadonly { get; set; } = true;
    [JsonProperty(nameof(ExperimentalFeatures))]
    public ExperimentalFeatures ExperimentalFeatures { get; set; } = new();
    [JsonProperty(nameof(ShortcutLocation))]
    public string? ShortcutLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    [JsonProperty(nameof(PlaceShortcut))]
    public bool PlaceShortcut { get; set; }
    [JsonProperty(nameof(AutoCheckUpdate))]
    public bool AutoCheckUpdate { get; set; } = true;
    [JsonProperty(nameof(ProtectedGraphAccessToken))]
    public string? ProtectedGraphAccessToken { get; set; }
    [JsonProperty(nameof(ProtectedGraphRefreshToken))]
    public string? ProtectedGraphRefreshToken { get; set; }
    [JsonProperty(nameof(FocusViewOptions))]
    public FocusViewOptions FocusViewOptions { get; set; } = new FocusViewOptions();
    [JsonProperty(nameof(ScreenManagerOptions))]
    public ScreenManagerOptions ScreenManagerOptions { get; set; } = new ScreenManagerOptions();
}
