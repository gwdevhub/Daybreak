using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.DirectSong;

/// <summary>
/// Linux implementation that runs the DirectSong registry editor through Wine.
/// </summary>
internal sealed class DirectSongRegistrar(
    IWinePrefixManager winePrefixManager,
    ILogger<DirectSongRegistrar> logger) : IDirectSongRegistrar
{
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly ILogger<DirectSongRegistrar> logger = logger;

    public async Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var wineInstallDir = PathUtils.ToWinePath(fullInstallDir);
        var exePath = Path.Combine(fullInstallDir, registryEditorName);

        this.logger.LogDebug("Running DirectSong registry editor through Wine: {ExePath}. Expecting output: {NativePath} or {WinePath}", exePath, fullInstallDir, wineInstallDir);

        var (output, error, exitCode) = await this.winePrefixManager.LaunchProcess(
            exePath,
            fullInstallDir,
            [],
            cancellationToken,
            completionChecker: (line, allLines) =>
                line.Trim().Equals(fullInstallDir, StringComparison.OrdinalIgnoreCase) ||
                line.Trim().Equals(wineInstallDir, StringComparison.OrdinalIgnoreCase));

        var success = output is not null &&
            (output.Contains(fullInstallDir, StringComparison.OrdinalIgnoreCase) ||
             output.Contains(wineInstallDir, StringComparison.OrdinalIgnoreCase));
        this.logger.LogDebug("DirectSong registry registration via Wine result: {Success}, ExitCode: {ExitCode}", success, exitCode);
        return success;
    }
}
