namespace Daybreak.Shared.Services.Api;

/// <summary>
/// Platform-specific provider for converting process IDs.
/// On Windows, this is a pass-through. On Linux, this maps Wine PIDs to Linux system PIDs.
/// </summary>
public interface IPidProvider
{
    /// <summary>
    /// Converts a reported process ID (e.g., from the Daybreak API health endpoint)
    /// to the actual system process ID.
    /// </summary>
    /// <param name="reportedPid">The process ID reported by the API (may be a Wine PID on Linux).</param>
    /// <param name="executableName">The executable name to search for (e.g., "Gw.exe").</param>
    /// <returns>The system process ID, or the original PID if conversion fails or is not needed.</returns>
    int ResolveSystemPid(int reportedPid, string executableName);
}
