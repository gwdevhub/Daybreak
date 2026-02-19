namespace Daybreak.Shared.Services.Api;

/// <summary>
/// Service for discovering running Daybreak API instances by scanning known ports.
/// Replaces mDNS-based service discovery for cross-platform compatibility.
/// </summary>
public interface IApiScanningService
{
    /// <summary>
    /// Gets the URI for a Daybreak API instance associated with the given process ID.
    /// </summary>
    /// <param name="processId">The process ID of the Guild Wars instance.</param>
    /// <returns>The API URI if found, null otherwise.</returns>
    Uri? GetApiUriByProcessId(int processId);

    /// <summary>
    /// Gets all discovered Daybreak API URIs matching a predicate.
    /// </summary>
    /// <param name="predicate">A predicate to filter discovered services by process ID.</param>
    /// <returns>A list of matching URIs, or null if none found.</returns>
    IReadOnlyList<(int ProcessId, Uri Uri)>? QueryByProcessId(Func<int, bool> predicate);

    /// <summary>
    /// Triggers an immediate scan for Daybreak API instances.
    /// </summary>
    void RequestScan();
}
