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
    Task<(string? Output, string? Error, int ExitCode)> LaunchProcess(
        string exePath,
        string workingDirectory,
        string[] arguments,
        CancellationToken cancellationToken,
        Func<IReadOnlyList<string>, IReadOnlyList<string>, (bool IsComplete, int ExitCode)>? outputCompletionChecker = null,
        TimeSpan? timeout = null);
}
