using System.Diagnostics;
using Daybreak.Shared.Services.ApplicationLauncher;

namespace Daybreak.Linux.Services.ApplicationLauncher;

/// <summary>
/// Linux-specific Steam service.
/// Detects the running Steam client primarily via the Steam client's pid file
/// (~/.steam/steam.pid), which holds the live Steam PID. Falls back to matching the Steam
/// client processes by name.
/// </summary>
public sealed class SteamService : ISteamService
{
    private static readonly string[] SteamPidFileRelativePaths =
    [
        ".steam/steam.pid",
        ".steam/steam/steam.pid"
    ];

    private static readonly string[] SteamProcessNames = ["steam", "steamwebhelper"];

    public bool IsSteamLoginSupported => false;

    public bool IsSteamRunning()
    {
        return IsSteamRunningViaPidFile() ?? IsSteamRunningViaProcessName();
    }

    private static bool? IsSteamRunningViaPidFile()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (string.IsNullOrEmpty(home))
        {
            return default;
        }

        foreach (var relativePath in SteamPidFileRelativePaths)
        {
            var pidFilePath = Path.Combine(home, relativePath);
            if (!File.Exists(pidFilePath))
            {
                continue;
            }

            try
            {
                if (!int.TryParse(File.ReadAllText(pidFilePath).Trim(), out var pid) || pid <= 0)
                {
                    continue;
                }

                try
                {
                    using var _ = Process.GetProcessById(pid);
                    return true;
                }
                catch (ArgumentException)
                {
                    // No process with the recorded PID is running, the pid file is stale.
                    return false;
                }
            }
            catch
            {
                // Unreadable pid file, fall through to the next candidate / process-name check.
            }
        }

        return default;
    }

    private static bool IsSteamRunningViaProcessName()
    {
        return SteamProcessNames.Any(name => Process.GetProcessesByName(name).Length > 0);
    }
}
