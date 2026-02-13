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
    /// Uses event-based stdout/stderr reading to avoid pipe deadlocks.
    /// Because Wine's wrapper process may not exit even after the Windows exe finishes
    /// (due to lingering child processes like Guild Wars), an optional completionChecker
    /// callback can be provided. When supplied, stdout lines are passed to the checker;
    /// once it returns true, the method returns immediately without waiting for Wine to exit.
    /// If no checker is provided, falls back to WaitForExitAsync.
    /// </summary>
    /// <param name="exePath">Path to the Windows executable (Linux path, will be converted to Wine path).</param>
    /// <param name="workingDirectory">Working directory for the process (Linux path).</param>
    /// <param name="arguments">Arguments to pass to the executable.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="completionChecker">Optional callback invoked with each stdout line and all lines so far.
    /// Return true to signal the process output is complete and stop waiting.</param>
    /// <returns>Output, error, and exit code from the process.</returns>
    Task<(string? Output, string? Error, int ExitCode)> LaunchProcess(
        string exePath,
        string workingDirectory,
        string[] arguments,
        CancellationToken cancellationToken,
        Func<string, IReadOnlyList<string>, bool>? completionChecker = null);

    /// <summary>
    /// Sets a DLL override in the Wine registry.
    /// </summary>
    /// <param name="dllName">Name of the DLL (without .dll extension).</param>
    /// <param name="mode">Override mode (e.g., "native,builtin").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> SetDllOverride(string dllName, string mode, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a registry value to the Wine prefix registry using 'wine reg add'.
    /// </summary>
    /// <param name="keyPath">Registry key path (e.g., "HKLM\\Software\\DirectSong").</param>
    /// <param name="valueName">Name of the value to set.</param>
    /// <param name="value">The value data.</param>
    /// <param name="valueType">Registry value type (e.g., "REG_SZ" for string).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> AddRegistryValue(string keyPath, string valueName, string value, string valueType, CancellationToken cancellationToken);
}
