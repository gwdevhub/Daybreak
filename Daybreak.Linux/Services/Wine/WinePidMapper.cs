using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Translates Wine-internal PIDs to Linux system PIDs by scanning /proc and matching the
/// full executable path. The reverse direction (Linux → Wine PID) lives in the injector,
/// which runs inside Wine and can disambiguate concurrent instances by image path.
/// </summary>
public sealed class WinePidMapper(
    ILogger<WinePidMapper> logger
) : IWinePidMapper
{
    private readonly ILogger<WinePidMapper> logger = logger;

    /// <inheritdoc />
    public int? WinePidToLinuxPid(int winePid, string executable)
    {
        var targetFileName = Path.GetFileName(executable);
        var matchByFullPath = executable.Contains('/') || executable.Contains('\\');
        var targetFullPath = matchByFullPath ? TryGetFullPath(executable) : null;

        try
        {
            foreach (var procDir in Directory.EnumerateDirectories("/proc"))
            {
                var dirName = Path.GetFileName(procDir);
                if (!int.TryParse(dirName, out var pid))
                {
                    continue;
                }

                try
                {
                    var cmdlinePath = Path.Combine(procDir, "cmdline");
                    if (!File.Exists(cmdlinePath))
                    {
                        continue;
                    }

                    var cmdline = File.ReadAllText(cmdlinePath);
                    if (cmdline.Length == 0)
                    {
                        continue;
                    }

                    // cmdline is null-separated; find the segment pointing at the executable.
                    var segments = cmdline.Split('\0', StringSplitOptions.RemoveEmptyEntries);
                    var exeSegment = segments.FirstOrDefault(s =>
                        s.EndsWith(targetFileName, StringComparison.OrdinalIgnoreCase));
                    if (exeSegment is null)
                    {
                        continue;
                    }

                    // When a full path was supplied, require an exact path match so that
                    // concurrent Guild Wars instances in different directories are not confused.
                    if (matchByFullPath)
                    {
                        var candidateFullPath = TryGetFullPath(WinePathToLinuxPath(exeSegment));
                        if (candidateFullPath is null ||
                            !string.Equals(candidateFullPath, targetFullPath, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    this.logger.LogDebug(
                        "Resolved Wine PID {WinePid} -> Linux PID {LinuxPid} for {Executable}",
                        winePid, pid, executable
                    );

                    return pid;
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error scanning /proc for Wine process {Executable}", executable);
        }

        this.logger.LogWarning("Could not find Linux PID for Wine PID {WinePid} ({Executable})", winePid, executable);
        return null;
    }

    private static string? TryGetFullPath(string path)
    {
        try
        {
            return Path.GetFullPath(path);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Converts a Wine Z: drive path back to a Linux path.
    /// "Z:\home\user\Guild Wars\Gw.exe" -> "/home/user/Guild Wars/Gw.exe".
    /// </summary>
    private static string WinePathToLinuxPath(string winePath)
    {
        if (winePath.StartsWith("Z:", StringComparison.OrdinalIgnoreCase))
        {
            winePath = winePath[2..];
        }

        return winePath.Replace('\\', '/');
    }
}
