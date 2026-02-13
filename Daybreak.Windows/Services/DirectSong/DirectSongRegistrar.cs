using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Daybreak.Windows.Services.DirectSong;

/// <summary>
/// Windows implementation that registers DirectSong via the Windows registry.
/// Uses HKEY_CURRENT_USER to avoid requiring administrator privileges.
/// </summary>
internal sealed class DirectSongRegistrar(
    ILogger<DirectSongRegistrar> logger) : IDirectSongRegistrar
{
    // The Revival Pack extracts DirectSong files to a nested DirectSong subfolder
    private const string DirectSongSubdir = "DirectSong";

    // Registry paths for DirectSong
    // Using HKCU instead of HKLM to avoid requiring admin privileges
    private const string DirectSongRegistryKey = @"Software\DirectSong";
    private const string MusicPathValueName = "MusicPath";
    private const string DsGuildWarsDll = "ds_GuildWars.dll";

    private readonly ILogger<DirectSongRegistrar> logger = logger;

    public Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken)
    {
        // The actual DirectSong files are in the nested DirectSong subfolder
        var directSongDir = Path.Combine(Path.GetFullPath(installationDirectory), DirectSongSubdir);

        try
        {
            // Try HKEY_CURRENT_USER first (no admin required)
            using var hkcuKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(DirectSongRegistryKey);
            if (hkcuKey is not null)
            {
                hkcuKey.SetValue(MusicPathValueName, directSongDir);
                this.logger.LogDebug("Set DirectSong registry in HKCU: {Path}", directSongDir);
                return Task.FromResult(true);
            }

            this.logger.LogWarning("Failed to create registry key in HKCU");
            
            // Fallback to HKEY_LOCAL_MACHINE (requires admin, but try anyway)
            try
            {
                using var hklmKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(DirectSongRegistryKey);
                if (hklmKey is not null)
                {
                    hklmKey.SetValue(MusicPathValueName, directSongDir);
                    this.logger.LogDebug("Set DirectSong registry in HKLM: {Path}", directSongDir);
                    return Task.FromResult(true);
                }
            }
            catch (UnauthorizedAccessException)
            {
                this.logger.LogWarning("Cannot write to HKLM without administrator privileges");
            }

            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to set DirectSong registry");
            return Task.FromResult(false);
        }
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
        var dsGuildWarsPath = Path.Combine(directSongDir, DsGuildWarsDll);

        return File.Exists(dsGuildWarsPath);
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
        var dsGuildWarsSrc = Path.Combine(directSongDir, DsGuildWarsDll);
        var dsGuildWarsDst = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (!File.Exists(dsGuildWarsDst) && File.Exists(dsGuildWarsSrc))
        {
            File.Copy(dsGuildWarsSrc, dsGuildWarsDst);
            this.logger.LogDebug("Copied {Dll} to {Path}", DsGuildWarsDll, dsGuildWarsDst);
        }
    }

    public void RemoveFilesFromGuildWars(string gwDirectory)
    {
        var dsGuildWarsPath = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (File.Exists(dsGuildWarsPath))
        {
            File.Delete(dsGuildWarsPath);
            this.logger.LogDebug("Removed {Dll} from {Path}", DsGuildWarsDll, gwDirectory);
        }
    }
}
