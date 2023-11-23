using Daybreak.Attributes;
using Daybreak.Views;
using Newtonsoft.Json;
using System;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Launcher")]
public sealed class LauncherOptions
{
    [JsonProperty(nameof(SetGuildwarsWindowSizeOnLaunch))]
    [OptionName(Name = "Set GuildWars Window Size On Launch", Description = "Sets the GuildWars window size and position on launch")]
    public bool SetGuildwarsWindowSizeOnLaunch { get; set; }

    [JsonProperty(nameof(DesiredGuildwarsScreen))]
    [OptionName(Name = "Desired GuildWars Screen", Description = "Sets the screen on which the GuildWars window will be placed")]
    [OptionSetterView<ScreenChoiceView>(Action = "Screen Selector")]
    public int DesiredGuildwarsScreen { get; set; }

    [JsonProperty(nameof(LaunchGuildwarsAsCurrentUser))]
    [OptionName(Name = "Launch GuildWars As Current User", Description = "If true, will attempt to launch GuildWars as the current user. Otherwise will attempt to launch as system user")]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;

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
    [OptionName(Name = "Download Icons", Description = "If true, the launcher will download icons that are not found in the local cache")]
    public bool DownloadIcons { get; set; } = true;

    [JsonProperty(nameof(ModStartupTimeout))]
    [OptionName(Name = "Mod Startup Timeout", Description = "Amount of seconds that Daybreak will wait for each mod to start-up before cancelling the tasks")]
    [OptionRange<double>(MinValue = 30, MaxValue = 300)]
    public double ModStartupTimeout { get; set; } = 30;

    [JsonProperty(nameof(PersistentLogging))]
    [OptionName(Name = "Persistent Logging", Description = "If true, the launcher will save logs in the local database. Otherwise, the launcher will only keep logs in a memory cache")]
    public bool PersistentLogging { get; set; } = false;

    [JsonProperty(nameof(BetaUpdate))]
    [OptionName(Name = "Beta Update", Description = "If true, the launcher will use the new update procedure")]
    public bool BetaUpdate { get; set; } = true;
}
