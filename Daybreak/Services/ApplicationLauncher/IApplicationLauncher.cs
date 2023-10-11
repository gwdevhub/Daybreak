﻿using Daybreak.Models.LaunchConfigurations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Services.ApplicationLauncher;

public interface IApplicationLauncher
{
    GuildWarsApplicationLaunchContext? GetGuildwarsProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);
    IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcesses(params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials);
    IEnumerable<Process> GetGuildwarsProcesses();
    Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(LaunchConfigurationWithCredentials launchConfigurationWithCredentials);
    void RestartDaybreak();
    void RestartDaybreakAsAdmin();
    void RestartDaybreakAsNormalUser();
}
