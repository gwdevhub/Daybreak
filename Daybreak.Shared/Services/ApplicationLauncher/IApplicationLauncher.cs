using System.Diagnostics;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.ApplicationLauncher;

public interface IApplicationLauncher
{
    Task<bool> ReapplyMods(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext,
        IEnumerable<IModService> mods,
        CancellationToken cancellationToken
    );
    Task<IEnumerable<IModService>> CheckMods(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext,
        CancellationToken cancellationToken
    );
    IEnumerable<string> GetLoadedModules(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext
    );
    GuildWarsApplicationLaunchContext? GetGuildwarsProcess(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials
    );
    IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcesses(
        params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials
    );
    ReadOnlyMemory<Process> GetGuildwarsProcesses();
    void KillGuildWarsProcess(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext);
    Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials,
        CancellationToken cancellationToken
    );
    void RestartDaybreak();
    void RestartDaybreakAsAdmin();
    void RestartDaybreakAsNormalUser();
}
