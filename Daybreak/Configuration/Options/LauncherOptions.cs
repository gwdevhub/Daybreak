using Daybreak.Attributes;
using Daybreak.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Launcher")]
public sealed class LauncherOptions
{
    [JsonProperty(nameof(SetGuildwarsWindowSizeOnLaunch))]
    [OptionName(Name = "Set GuildWars Window Size On Launch", Description = "Sets the GuildWars window size and position on launch")]
    public bool SetGuildwarsWindowSizeOnLaunch { get; set; }

    [JsonProperty(nameof(DesiredGuildwarsScreen))]
    [OptionName(Name = "Desired GuildWars Screen", Description = "Sets the screen on which the GuildWars window will be placed")]
    public int DesiredGuildwarsScreen { get; set; }

    [JsonProperty(nameof(LaunchGuildwarsAsCurrentUser))]
    [OptionName(Name = "Launch GuildWars As Current User", Description = "If true, will attempt to launch GuildWars as the current user. Otherwise will attempt to launch as system user")]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;

    [JsonProperty(nameof(GuildwarsPaths))]
    [OptionIgnore]
    public List<GuildwarsPath> GuildwarsPaths { get; set; } = new();

    [JsonProperty(nameof(ProtectedLoginCredentials))]
    [OptionIgnore]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = new();

    [JsonProperty(nameof(ShortcutLocation))]
    [OptionName(Name = "Shortcut Location", Description = "Location where the shortcut will be placed")]
    public string? ShortcutLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    [JsonProperty(nameof(PlaceShortcut))]
    [OptionName(Name = "Shortcut Placed", Description = "If true, the shortcut will be placed. If false, the shortcut will be deleted")]
    public bool PlaceShortcut { get; set; }

    [JsonProperty(nameof(AutoCheckUpdate))]
    [OptionName(Name = "Auto-Check For Updates", Description = "Automatically check for updates")]
    public bool AutoCheckUpdate { get; set; } = true;

    [JsonProperty(nameof(MultiLaunchSupport))]
    [OptionName(Name = "Multi-Launch Support", Description = "If true, the launcher will support multiple executables being launched at the same time")]
    public bool MultiLaunchSupport { get; set; }

    [JsonProperty(nameof(DownloadIcons))]
    [OptionName(Name = "Download Icons", Description = "If true, the launcher will perform a check on the local icon cache and download any missing or corrupt icons")]
    public bool DownloadIcons { get; set; }
}
