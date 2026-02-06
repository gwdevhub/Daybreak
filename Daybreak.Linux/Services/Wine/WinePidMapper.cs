using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Stateless translator between Wine-internal PIDs and Linux system PIDs.
/// Uses /proc scanning for Wine→Linux and winedbg for Linux→Wine.
/// </summary>
public sealed class WinePidMapper(
    IWinePrefixManager winePrefixManager,
    ILogger<WinePidMapper> logger
) : IWinePidMapper
{
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly ILogger<WinePidMapper> logger = logger;

    /// <inheritdoc />
    public int? WinePidToLinuxPid(int winePid, string executableName)
    {
        var prefixPath = this.winePrefixManager.GetWinePrefixPath();

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
                    if (!cmdline.Contains(executableName, StringComparison.OrdinalIgnoreCase))
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

                    this.logger.LogDebug(
                        "Resolved Wine PID {WinePid} -> Linux PID {LinuxPid} for {ExeName}",
                        winePid, pid, executableName
                    );

                    return pid;
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error scanning /proc for Wine process {ExeName}", executableName);
        }

        this.logger.LogWarning("Could not find Linux PID for Wine PID {WinePid} ({ExeName})", winePid, executableName);
        return null;
    }

    /// <inheritdoc />
    public int? LinuxPidToWinePid(int linuxPid)
    {
        // Step 1: Read the executable name from /proc/<pid>/cmdline
        string? executableName;
        try
        {
            var cmdline = File.ReadAllText($"/proc/{linuxPid}/cmdline");
            // cmdline is null-separated; find the segment containing .exe
            var segments = cmdline.Split('\0', StringSplitOptions.RemoveEmptyEntries);
            var exeSegment = segments.FirstOrDefault(s => s.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                          ?? segments.FirstOrDefault(s => s.Contains(".exe", StringComparison.OrdinalIgnoreCase));

            if (exeSegment is null)
            {
                this.logger.LogWarning("Could not determine executable name for Linux PID {LinuxPid}", linuxPid);
                return null;
            }

            // Extract just the filename (e.g. "Gw.exe" from "Z:\mnt\...\Gw.exe")
            executableName = exeSegment.Split('\\').Last().Split('/').Last();
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Could not read cmdline for Linux PID {LinuxPid}", linuxPid);
            return null;
        }

        // Step 2: Query winedbg for the Wine PID of that executable
        return this.QueryWineDbgForPid(executableName);
    }

    /// <summary>
    /// Runs <c>winedbg --command "info proc"</c> in our prefix and parses the output
    /// to find the Wine PID for the given executable name.
    /// Output format: " 0000021c 1        'Gw.exe'"
    /// </summary>
    private int? QueryWineDbgForPid(string executableName)
    {
        var prefixPath = this.winePrefixManager.GetWinePrefixPath();

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "winedbg",
                Arguments = "--command \"info proc\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            startInfo.Environment["WINEPREFIX"] = prefixPath;

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(TimeSpan.FromSeconds(5));

            // Parse lines like:
            //  0000021c 1        'Gw.exe'
            //  00000038 12       \_ 'services.exe'
            foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmed = line.Trim();

                // Skip header line
                if (trimmed.StartsWith("pid", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Check if this line contains our executable
                if (!trimmed.Contains(executableName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Strip tree prefixes like "\_ " or "= "
                var cleaned = trimmed.TrimStart('=', ' ');
                if (cleaned.StartsWith("\\_"))
                {
                    cleaned = cleaned[2..].TrimStart();
                }

                // First token is the hex PID
                var hexPid = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (hexPid is not null && int.TryParse(hexPid, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var winePid))
                {
                    this.logger.LogDebug(
                        "Resolved Linux PID -> Wine PID {WinePid} (0x{WinePidHex}) for {ExeName}",
                        winePid, hexPid, executableName
                    );

                    return winePid;
                }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to query winedbg for {ExeName}", executableName);
        }

        this.logger.LogWarning("Could not find Wine PID for {ExeName} via winedbg", executableName);
        return null;
    }
}
