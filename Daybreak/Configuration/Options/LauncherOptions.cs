using Daybreak.Shared.Attributes;
using System.Text.Json.Serialization;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Launcher")]
public sealed class LauncherOptions
{
    [JsonPropertyName(nameof(LaunchGuildwarsAsCurrentUser))]
    [OptionName(Name = "Launch GuildWars As Current User", Description = "If true, will attempt to launch GuildWars as the current user. Otherwise will attempt to launch as system user")]
    public bool LaunchGuildwarsAsCurrentUser { get; set; } = true;

    [JsonPropertyName(nameof(ShortcutLocation))]
    [OptionName(Name = "Shortcut Location", Description = "Location where the shortcut will be placed")]
    [OptionSynchronizationIgnore]
    public string? ShortcutLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    [JsonPropertyName(nameof(PlaceShortcut))]
    [OptionName(Name = "Shortcut Placed", Description = "If true, the shortcut will be placed. If false, the shortcut will be deleted")]
    public bool PlaceShortcut { get; set; }

    [JsonPropertyName(nameof(AutoCheckUpdate))]
    [OptionName(Name = "Auto-Check For Updates", Description = "Automatically check for updates")]
    public bool AutoCheckUpdate { get; set; } = true;

    [JsonPropertyName(nameof(MultiLaunchSupport))]
    [OptionName(Name = "Multi-Launch Support", Description = "If true, the launcher will support multiple executables being launched at the same time")]
    public bool MultiLaunchSupport { get; set; }

    [JsonPropertyName(nameof(ModStartupTimeout))]
    [OptionName(Name = "Mod Startup Timeout", Description = "Amount of seconds that Daybreak will wait for each mod to start-up before cancelling the tasks")]
    [OptionRange<double>(MinValue = 30, MaxValue = 300)]
    public double ModStartupTimeout { get; set; } = 30;
}
