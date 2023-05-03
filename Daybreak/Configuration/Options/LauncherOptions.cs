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
    public bool SetGuildwarsWindowSizeOnLaunch { get; set; }

    [JsonProperty(nameof(DesiredGuildwarsScreen))]
    public int DesiredGuildwarsScreen { get; set; }

    [JsonProperty(nameof(LaunchGuildwarsAsCurrentUser))]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;

    [JsonProperty(nameof(GuildwarsPaths))]
    public List<GuildwarsPath> GuildwarsPaths { get; set; } = new();

    [JsonProperty(nameof(ProtectedLoginCredentials))]
    public List<ProtectedLoginCredentials> ProtectedLoginCredentials { get; set; } = new();

    [JsonProperty(nameof(ShortcutLocation))]
    public string? ShortcutLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    [JsonProperty(nameof(PlaceShortcut))]
    public bool PlaceShortcut { get; set; }

    [JsonProperty(nameof(AutoCheckUpdate))]
    public bool AutoCheckUpdate { get; set; } = true;

    [JsonProperty(nameof(MultiLaunchSupport))]
    public bool MultiLaunchSupport { get; set; }

    [JsonProperty(nameof(DownloadIcons))]
    public bool DownloadIcons { get; set; }
}
