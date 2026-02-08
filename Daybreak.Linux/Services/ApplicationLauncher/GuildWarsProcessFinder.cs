using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.ApplicationLauncher;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Extensions.Core;

namespace Daybreak.Linux.Services.ApplicationLauncher;

/// <summary>
/// Linux-specific Guild Wars process finder.
/// Scans /proc for Wine-hosted GW.exe processes in our Wine prefix,
/// then matches them to launch configurations by executable path
/// read from the process command line.
/// </summary>
public sealed class GuildWarsProcessFinder(
    IWinePrefixManager winePrefixManager,
    ILogger<GuildWarsProcessFinder> logger)
    : IGuildWarsProcessFinder
{
    private const string GuildWarsExeName = "Gw.exe";

    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly ILogger<GuildWarsProcessFinder> logger = logger;

    public IReadOnlyList<Process> GetGuildWarsProcesses()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var processes = new List<Process>();

        foreach (var (pid, _) in this.ScanForGuildWarsProcesses())
        {
            try
            {
                processes.Add(Process.GetProcessById(pid));
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Could not get process for PID {Pid}", pid);
            }
        }

        scopedLogger.LogDebug("Found {Count} Guild Wars process(es) under Wine", processes.Count);
        return processes;
    }

    public GuildWarsApplicationLaunchContext? FindProcess(LaunchConfigurationWithCredentials configuration)
    {
        return this.FindProcesses(configuration).FirstOrDefault();
    }

    public IEnumerable<GuildWarsApplicationLaunchContext?> FindProcesses(params LaunchConfigurationWithCredentials[] configurations)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        foreach (var (pid, executablePath) in this.ScanForGuildWarsProcesses())
        {
            // Match executable path from Wine cmdline to configuration
            var matchedConfig = configurations.FirstOrDefault(c =>
                ConfigurationMatchesPath(c, executablePath));

            if (matchedConfig is null)
            {
                continue;
            }

            Process process;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Could not get process for PID {Pid}", pid);
                continue;
            }

            yield return new GuildWarsApplicationLaunchContext
            {
                GuildWarsProcess = process,
                LaunchConfiguration = matchedConfig,
                ProcessId = (uint)pid
            };
        }
    }

    /// <summary>
    /// Scans /proc for processes running Gw.exe under our Wine prefix.
    /// Returns tuples of (Linux PID, executable path from cmdline).
    /// </summary>
    private IEnumerable<(int Pid, string ExecutablePath)> ScanForGuildWarsProcesses()
    {
        var prefixPath = this.winePrefixManager.GetWinePrefixPath();

        foreach (var procDir in Directory.EnumerateDirectories("/proc"))
        {
            var dirName = Path.GetFileName(procDir);
            if (!int.TryParse(dirName, out var pid))
            {
                continue;
            }

            string? executablePath = null;
            try
            {
                var cmdlinePath = Path.Combine(procDir, "cmdline");
                if (!File.Exists(cmdlinePath))
                {
                    continue;
                }

                var cmdline = File.ReadAllText(cmdlinePath);
                if (!cmdline.Contains(GuildWarsExeName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Verify this process belongs to our Wine prefix
                var environPath = Path.Combine(procDir, "environ");
                if (!File.Exists(environPath))
                {
                    continue;
                }

                var environ = File.ReadAllText(environPath);
                if (!environ.Contains(prefixPath, StringComparison.Ordinal))
                {
                    continue;
                }

                // Extract the executable path from cmdline
                // cmdline is null-separated; find the segment containing Gw.exe
                var segments = cmdline.Split('\0', StringSplitOptions.RemoveEmptyEntries);
                var exeSegment = segments.FirstOrDefault(s =>
                    s.EndsWith(GuildWarsExeName, StringComparison.OrdinalIgnoreCase));

                if (exeSegment is not null)
                {
                    // Convert Wine path (Z:\mnt\...\Gw.exe) to Linux path
                    executablePath = WinePathToLinuxPath(exeSegment);
                }
            }
            catch (UnauthorizedAccessException) { continue; }
            catch (IOException) { continue; }

            if (executablePath is not null)
            {
                yield return (pid, executablePath);
            }
        }
    }

    /// <summary>
    /// Converts a Wine Z: path back to a Linux path.
    /// "Z:\mnt\seagate\Games\Guild Wars\Gw.exe" -> "/mnt/seagate/Games/Guild Wars/Gw.exe"
    /// </summary>
    private static string WinePathToLinuxPath(string winePath)
    {
        // Strip Z: prefix and convert backslashes
        if (winePath.StartsWith("Z:", StringComparison.OrdinalIgnoreCase) ||
            winePath.StartsWith("z:", StringComparison.OrdinalIgnoreCase))
        {
            winePath = winePath[2..];
        }

        return winePath.Replace('\\', '/');
    }

    private static bool ConfigurationMatchesPath(
        LaunchConfigurationWithCredentials configuration,
        string executablePath)
    {
        if (configuration.ExecutablePath is null)
        {
            return false;
        }

        return string.Equals(
            Path.GetFullPath(configuration.ExecutablePath),
            Path.GetFullPath(executablePath),
            StringComparison.OrdinalIgnoreCase);
    }
}
