namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Stateless translator between Wine-internal PIDs and Linux system PIDs.
/// Uses /proc scanning and winedbg to resolve mappings on demand.
/// </summary>
public interface IWinePidMapper
{
    /// <summary>
    /// Converts a Wine-internal PID to a Linux system PID by scanning /proc.
    /// When <paramref name="executable"/> is a full path it is matched against each
    /// candidate's full executable path, which uniquely identifies the process even
    /// when multiple Guild Wars instances (different install directories) run at once.
    /// When only a file name is supplied, the first matching process is returned.
    /// </summary>
    /// <param name="winePid">The Wine-internal process ID (unused for lookup, kept for logging).</param>
    /// <param name="executable">The executable to match: a full Linux path (preferred, for
    /// unambiguous matching) or a bare file name (e.g. "Gw.exe") as a best-effort fallback.</param>
    /// <returns>The Linux PID, or null if not found.</returns>
    int? WinePidToLinuxPid(int winePid, string executable);

    /// <summary>
    /// Converts a Linux system PID back to a Wine-internal PID.
    /// Reads the process's cmdline to determine the executable, then queries
    /// winedbg to find the corresponding Wine PID.
    /// </summary>
    /// <param name="linuxPid">The Linux system PID.</param>
    /// <returns>The Wine PID, or null if not found.</returns>
    int? LinuxPidToWinePid(int linuxPid);
}
