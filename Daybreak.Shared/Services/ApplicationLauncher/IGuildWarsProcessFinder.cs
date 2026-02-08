using System.Collections.Immutable;
using System.Diagnostics;
using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Services.ApplicationLauncher;

/// <summary>
/// Platform-specific service for finding running Guild Wars processes.
/// On Windows, enumerates processes by name using standard .NET APIs.
/// On Linux, scans for Wine-hosted GW processes via /proc and PID mapping.
/// </summary>
public interface IGuildWarsProcessFinder
{
    /// <summary>
    /// Returns all running Guild Wars processes.
    /// </summary>
    Memory<Process> GetGuildWarsProcesses();

    /// <summary>
    /// Finds a running Guild Wars process that matches the given launch configuration.
    /// Returns null if no matching process is found.
    /// </summary>
    GuildWarsApplicationLaunchContext? FindProcess(
        LaunchConfigurationWithCredentials configuration
    );

    /// <summary>
    /// Finds all running Guild Wars processes that match any of the given launch configurations.
    /// </summary>
    IEnumerable<GuildWarsApplicationLaunchContext?> FindProcesses(
        params LaunchConfigurationWithCredentials[] configurations
    );
}
