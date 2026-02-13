using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Windows.Services.DirectSong;

/// <summary>
/// Windows implementation that runs the DirectSong registry editor natively.
/// </summary>
internal sealed class DirectSongRegistrar(
    ILogger<DirectSongRegistrar> logger) : IDirectSongRegistrar
{
    // The Revival Pack extracts DirectSong files to a nested DirectSong subfolder
    private const string DirectSongSubdir = "DirectSong";
    // Windows 10 requires a specific WMVCORE.DLL from the archive
    private const string Windows10WmvcoreSubdir = "wmvcore_dll_for_Windows10";
    private const string Windows10WmvcoreVersion = "11.0.6001.7006";

    private const string WMVCOREDll = "WMVCORE.DLL";
    private const string DsGuildWarsDll = "ds_GuildWars.dll";

    private readonly ILogger<DirectSongRegistrar> logger = logger;

    public async Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken)
    {
        // The actual DirectSong files are in the nested DirectSong subfolder
        var directSongDir = Path.Combine(Path.GetFullPath(installationDirectory), DirectSongSubdir);
        using var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(directSongDir, registryEditorName),
                WorkingDirectory = directSongDir,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var success = false;
        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data == directSongDir)
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

    public Task<bool> SetupDllOverrides(CancellationToken cancellationToken)
    {
        // On Windows, no DLL overrides are needed
        return Task.FromResult(true);
    }

    public bool IsGStreamerAvailable()
    {
        // On Windows, gstreamer is not required
        return true;
    }

    public string GetGStreamerInstallInstructions()
    {
        return string.Empty;
    }

    public bool ArePlatformFilesInstalled(string installationDirectory)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var directSongDir = Path.Combine(fullInstallDir, DirectSongSubdir);
        var wmvcoreDir = Path.Combine(fullInstallDir, Windows10WmvcoreSubdir, Windows10WmvcoreVersion);

        var wmcorePath = Path.Combine(wmvcoreDir, WMVCOREDll);
        var dsGuildWarsPath = Path.Combine(directSongDir, DsGuildWarsDll);

        return File.Exists(wmcorePath) && File.Exists(dsGuildWarsPath);
    }

    public Task<bool> SetupPlatformFiles(string installationDirectory, IDownloadService downloadService, IProgress<ProgressUpdate>? progress, CancellationToken cancellationToken)
    {
        // On Windows, all files come from the DirectSong.7z archive - nothing extra to download
        return Task.FromResult(true);
    }

    public void CopyFilesToGuildWars(string installationDirectory, string gwDirectory)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var directSongDir = Path.Combine(fullInstallDir, DirectSongSubdir);
        var wmvcoreDir = Path.Combine(fullInstallDir, Windows10WmvcoreSubdir, Windows10WmvcoreVersion);

        var wmcoreSrc = Path.Combine(wmvcoreDir, WMVCOREDll);
        var dsGuildWarsSrc = Path.Combine(directSongDir, DsGuildWarsDll);

        var wmcoreDst = Path.Combine(gwDirectory, WMVCOREDll);
        var dsGuildWarsDst = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (!File.Exists(wmcoreDst) && File.Exists(wmcoreSrc))
        {
            File.Copy(wmcoreSrc, wmcoreDst);
            this.logger.LogDebug("Copied {Dll} to {Path}", WMVCOREDll, wmcoreDst);
        }

        if (!File.Exists(dsGuildWarsDst) && File.Exists(dsGuildWarsSrc))
        {
            File.Copy(dsGuildWarsSrc, dsGuildWarsDst);
            this.logger.LogDebug("Copied {Dll} to {Path}", DsGuildWarsDll, dsGuildWarsDst);
        }
    }

    public void RemoveFilesFromGuildWars(string gwDirectory)
    {
        var wmcorePath = Path.Combine(gwDirectory, WMVCOREDll);
        var dsGuildWarsPath = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (File.Exists(wmcorePath))
        {
            File.Delete(wmcorePath);
            this.logger.LogDebug("Removed {Dll} from {Path}", WMVCOREDll, gwDirectory);
        }

        if (File.Exists(dsGuildWarsPath))
        {
            File.Delete(dsGuildWarsPath);
            this.logger.LogDebug("Removed {Dll} from {Path}", DsGuildWarsDll, gwDirectory);
        }
    }
}
