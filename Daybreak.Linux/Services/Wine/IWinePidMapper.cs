namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Translates Wine-internal PIDs to Linux system PIDs by scanning /proc.
/// The reverse direction (Linux → Wine PID) is handled by the injector, which runs
/// inside Wine and can disambiguate concurrent instances by full image path.
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
}
