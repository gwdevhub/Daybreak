using System.Diagnostics;

namespace Daybreak.Shared.Services.ApplicationLauncher;

/// <summary>
/// Platform-specific checker that determines when a Guild Wars process
/// is fully ready (main game window visible and responsive).
/// </summary>
public interface IGuildWarsReadyChecker
{
    /// <summary>
    /// Waits until the Guild Wars process is ready, or returns null if it fails.
    /// </summary>
    /// <param name="process">The Guild Wars process to monitor.</param>
    /// <param name="timeout">Maximum time to wait.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the process is ready, false if it exited or timed out.</returns>
    Task<bool> WaitForReady(Process process, TimeSpan timeout, CancellationToken cancellationToken);
}
