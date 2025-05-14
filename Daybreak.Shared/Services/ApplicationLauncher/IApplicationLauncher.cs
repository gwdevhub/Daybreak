using Daybreak.Shared.Models.LaunchConfigurations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.ApplicationLauncher;

public interface IApplicationLauncher
{
    GuildWarsApplicationLaunchContext? GetGuildwarsProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);
    IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcesses(params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials);
    IEnumerable<Process> GetGuildwarsProcesses();
    void KillGuildWarsProcess(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext);
    Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);
    void RestartDaybreak();
    void RestartDaybreakAsAdmin();
    void RestartDaybreakAsNormalUser();
}
