using Daybreak.Shared.Services.DirectSong;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Windows.Services.DirectSong;

/// <summary>
/// Windows implementation that runs the DirectSong registry editor natively.
/// </summary>
internal sealed class DirectSongRegistrar(
    ILogger<DirectSongRegistrar> logger) : IDirectSongRegistrar
{
    private readonly ILogger<DirectSongRegistrar> logger = logger;

    public async Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        using var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(fullInstallDir, registryEditorName),
                WorkingDirectory = fullInstallDir,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var success = false;
        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data == fullInstallDir)
            {
                success = true;
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        while (!process.HasExited && !success)
        {
            await Task.Delay(1000, cancellationToken);
        }

        process.CancelOutputRead();
        if (!process.HasExited)
        {
            process.Close();
        }

        this.logger.LogDebug("DirectSong registry registration result: {Success}", success);
        return success;
    }
}
