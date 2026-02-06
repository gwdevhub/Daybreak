using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Manages a dedicated Wine prefix for running Windows executables on Linux.
/// Also implements IModService to appear in the mod manager UI.
/// </summary>
public interface IWinePrefixManager : IModService
{
    /// <summary>
    /// Checks if Wine is installed and available on the system.
    /// </summary>
    bool IsAvailable();

    /// <summary>
    /// Checks if the Wine prefix has been initialized.
    /// </summary>
    bool IsInitialized();

    /// <summary>
    /// Initializes the Wine prefix if it doesn't exist (wineboot --init).
    /// Returns a progress-reporting async operation.
    /// </summary>
    IProgressAsyncOperation<bool> Install(CancellationToken cancellationToken);

    /// <summary>
    /// Returns the path to Daybreak's Wine prefix directory.
    /// </summary>
    string GetWinePrefixPath();

    /// <summary>
    /// Launches a Windows executable through Wine with the managed prefix.
    /// </summary>
    /// <param name="exePath">Path to the Windows executable (Linux path, will be converted to Wine path).</param>
    /// <param name="workingDirectory">Working directory for the process (Linux path).</param>
    /// <param name="arguments">Arguments to pass to the executable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Output, error, and exit code from the process.</returns>
    Task<(string? Output, string? Error, int ExitCode)> LaunchProcess(
        string exePath,
        string workingDirectory,
        string[] arguments,
        CancellationToken cancellationToken);
}
