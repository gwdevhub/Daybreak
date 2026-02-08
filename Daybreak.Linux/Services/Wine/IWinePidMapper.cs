namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Stateless translator between Wine-internal PIDs and Linux system PIDs.
/// Uses /proc scanning and winedbg to resolve mappings on demand.
/// </summary>
public interface IWinePidMapper
{
    /// <summary>
    /// Converts a Wine-internal PID to a Linux system PID.
    /// Scans /proc for a process whose cmdline contains the executable name
    /// and whose environment references our Wine prefix.
    /// </summary>
    /// <param name="winePid">The Wine-internal process ID (unused for lookup, kept for logging).</param>
    /// <param name="executableName">The executable name (e.g. "Gw.exe") to search for in /proc.</param>
    /// <returns>The Linux PID, or null if not found.</returns>
    int? WinePidToLinuxPid(int winePid, string executableName);

    /// <summary>
    /// Converts a Linux system PID back to a Wine-internal PID.
    /// Reads the process's cmdline to determine the executable, then queries
    /// winedbg to find the corresponding Wine PID.
    /// </summary>
    /// <param name="linuxPid">The Linux system PID.</param>
    /// <returns>The Wine PID, or null if not found.</returns>
    int? LinuxPidToWinePid(int linuxPid);
}
