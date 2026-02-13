using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Linux.Services.DirectSong;

/// <summary>
/// Linux implementation that sets up DirectSong through Wine registry and DLL overrides.
/// Based on: https://github.com/ChthonVII/guildwarslinuxinstallguide#part-10-directsong
/// </summary>
internal sealed class DirectSongRegistrar(
    IWinePrefixManager winePrefixManager,
    ILogger<DirectSongRegistrar> logger) : IDirectSongRegistrar
{
    // Registry paths for DirectSong
    // Win64 prefix uses Wow6432Node for 32-bit applications
    private const string DirectSongRegistryKeyWin64 = @"HKLM\Software\Wow6432Node\DirectSong";
    private const string DirectSongRegistryKeyWin32 = @"HKLM\Software\DirectSong";
    private const string MusicPathValueName = "MusicPath";

    // The Revival Pack extracts DirectSong files to a nested DirectSong subfolder
    private const string DirectSongSubdir = "DirectSong";

    // DLL names
    private const string WMVCOREDll = "WMVCORE.DLL";
    private const string WMASFDll = "WMASF.DLL";
    private const string DsGuildWarsDll = "ds_GuildWars.dll";

    // Subdirectory for Linux-specific DLLs (from ChthonVII's repo)
    private const string LinuxDllSubdir = "LinuxDlls";

    // Download URLs for Linux-specific DLLs
    private const string GitHubWMVCOREUrl = "https://github.com/ChthonVII/guildwarslinuxinstallguide/raw/main/extras/DirectSongDLLS/WMVCORE.DLL";
    private const string GitHubWMASFUrl = "https://github.com/ChthonVII/guildwarslinuxinstallguide/raw/main/extras/DirectSongDLLS/WMASF.DLL";

    // Common gstreamer library paths to check
    private static readonly string[] GStreamerLibPaths =
    [
        // 32-bit paths (for standard Wine)
        "/usr/lib/i386-linux-gnu/gstreamer-1.0/libgstlibav.so",
        "/usr/lib32/gstreamer-1.0/libgstlibav.so",
        "/usr/lib/gstreamer-1.0/libgstlibav.so",
        // 64-bit paths (for Wine wow64 mode)
        "/usr/lib/x86_64-linux-gnu/gstreamer-1.0/libgstlibav.so",
        "/usr/lib64/gstreamer-1.0/libgstlibav.so",
        // Arch/Manjaro/CachyOS paths
        "/usr/lib/gstreamer-1.0/libgstlibav.so",
        "/usr/lib32/gstreamer-1.0/libgstlibav.so",
    ];

    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly ILogger<DirectSongRegistrar> logger = logger;

    public async Task<bool> RegisterDirectSongDirectory(string installationDirectory, string registryEditorName, CancellationToken cancellationToken)
    {
        // The actual DirectSong files are in the nested DirectSong subfolder
        var directSongDir = Path.Combine(Path.GetFullPath(installationDirectory), DirectSongSubdir);
        var wineInstallDir = PathUtils.ToWinePath(directSongDir);

        this.logger.LogDebug("Registering DirectSong directory via Wine registry. Native path: {NativePath}, Wine path: {WinePath}", 
            directSongDir, wineInstallDir);

        // Wine prefixes are typically 64-bit (win64), so we use the Wow6432Node path
        // for 32-bit applications like Guild Wars
        var registryKey = DirectSongRegistryKeyWin64;
        
        var success = await this.winePrefixManager.AddRegistryValue(
            registryKey,
            MusicPathValueName,
            wineInstallDir,
            "REG_SZ",
            cancellationToken);

        if (!success)
        {
            this.logger.LogWarning("Failed to set registry in {Key}, trying 32-bit key", registryKey);
            // Fallback to 32-bit registry path in case of a 32-bit prefix
            success = await this.winePrefixManager.AddRegistryValue(
                DirectSongRegistryKeyWin32,
                MusicPathValueName,
                wineInstallDir,
                "REG_SZ",
                cancellationToken);
        }

        this.logger.LogDebug("DirectSong registry registration result: {Success}", success);
        return success;
    }

    public async Task<bool> SetupDllOverrides(CancellationToken cancellationToken)
    {
        this.logger.LogDebug("Setting up DLL overrides for DirectSong (wmvcore, wmasf)");

        // Set wmvcore to native,builtin - required for WMA decoding
        var wmvcoreSuccess = await this.winePrefixManager.SetDllOverride("wmvcore", "native,builtin", cancellationToken);
        if (!wmvcoreSuccess)
        {
            this.logger.LogError("Failed to set wmvcore DLL override");
            return false;
        }

        // Set wmasf to native,builtin - required for WMA decoding  
        var wmasfSuccess = await this.winePrefixManager.SetDllOverride("wmasf", "native,builtin", cancellationToken);
        if (!wmasfSuccess)
        {
            this.logger.LogError("Failed to set wmasf DLL override");
            return false;
        }

        this.logger.LogDebug("DLL overrides set successfully");
        return true;
    }

    public bool IsGStreamerAvailable()
    {
        // Check if any of the known gstreamer libav paths exist
        foreach (var path in GStreamerLibPaths)
        {
            if (File.Exists(path))
            {
                this.logger.LogDebug("Found gstreamer libav at: {Path}", path);
                return true;
            }
        }

        // Try using gst-inspect to check if libav plugin is available
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "gst-inspect-1.0",
                    Arguments = "libav",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit(5000);

            if (process.ExitCode == 0)
            {
                this.logger.LogDebug("gst-inspect-1.0 found libav plugin");
                return true;
            }
        }
        catch (Exception ex)
        {
            this.logger.LogDebug(ex, "Failed to run gst-inspect-1.0");
        }

        this.logger.LogWarning("GStreamer libav plugin not found. DirectSong WMA playback may not work.");
        return false;
    }

    public string GetGStreamerInstallInstructions()
    {
        return """
            DirectSong requires the GStreamer libav plugin for WMA audio playback.
            
            Install it using your package manager:
            
            Debian/Ubuntu (32-bit Wine):
              sudo apt-get install gstreamer1.0-libav:i386
            
            Debian/Ubuntu (Wine wow64 mode):
              sudo apt-get install gstreamer1.0-libav
            
            Arch/Manjaro/CachyOS:
              sudo pacman -S gst-libav lib32-gst-libav
            
            Fedora:
              sudo dnf install gstreamer1-libav.i686
            
            After installation, restart Daybreak.
            """;
    }

    public bool ArePlatformFilesInstalled(string installationDirectory)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var directSongDir = Path.Combine(fullInstallDir, DirectSongSubdir);
        var linuxDllDir = Path.Combine(fullInstallDir, LinuxDllSubdir);

        // Check for Linux-specific DLLs in the LinuxDlls subdirectory
        var wmcorePath = Path.Combine(linuxDllDir, WMVCOREDll);
        var wmasfPath = Path.Combine(linuxDllDir, WMASFDll);
        // ds_GuildWars.dll comes from the nested DirectSong subdirectory
        var dsGuildWarsPath = Path.Combine(directSongDir, DsGuildWarsDll);

        var installed = File.Exists(wmcorePath) && File.Exists(wmasfPath) && File.Exists(dsGuildWarsPath);
        this.logger.LogDebug("ArePlatformFilesInstalled: wmcore={WmCore}, wmasf={WmAsf}, dsGW={DsGW}, result={Result}",
            File.Exists(wmcorePath), File.Exists(wmasfPath), File.Exists(dsGuildWarsPath), installed);

        return installed;
    }

    public async Task<bool> SetupPlatformFiles(string installationDirectory, IDownloadService downloadService, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var linuxDllDir = Path.Combine(fullInstallDir, LinuxDllSubdir);

        // Create the Linux DLL subdirectory
        Directory.CreateDirectory(linuxDllDir);

        var wmcorePath = Path.Combine(linuxDllDir, WMVCOREDll);
        var wmasfPath = Path.Combine(linuxDllDir, WMASFDll);

        // Download WMVCORE.DLL from ChthonVII's repo
        this.logger.LogDebug("Downloading WMVCORE.DLL from GitHub to {Path}", wmcorePath);
        if (!await downloadService.DownloadFile(GitHubWMVCOREUrl, wmcorePath, progress, cancellationToken))
        {
            this.logger.LogError("Failed to download WMVCORE.DLL from GitHub");
            return false;
        }

        // Download WMASF.DLL from ChthonVII's repo
        this.logger.LogDebug("Downloading WMASF.DLL from GitHub to {Path}", wmasfPath);
        if (!await downloadService.DownloadFile(GitHubWMASFUrl, wmasfPath, progress, cancellationToken))
        {
            this.logger.LogError("Failed to download WMASF.DLL from GitHub");
            return false;
        }

        this.logger.LogDebug("Platform-specific files downloaded successfully");
        return true;
    }

    public void CopyFilesToGuildWars(string installationDirectory, string gwDirectory)
    {
        var fullInstallDir = Path.GetFullPath(installationDirectory);
        var directSongDir = Path.Combine(fullInstallDir, DirectSongSubdir);
        var linuxDllDir = Path.Combine(fullInstallDir, LinuxDllSubdir);

        // Source paths - Linux DLLs from ChthonVII's repo
        var wmcoreSrc = Path.Combine(linuxDllDir, WMVCOREDll);
        var wmasfSrc = Path.Combine(linuxDllDir, WMASFDll);
        // ds_GuildWars.dll from the nested DirectSong subdirectory
        var dsGuildWarsSrc = Path.Combine(directSongDir, DsGuildWarsDll);

        // Destination paths
        var wmcoreDst = Path.Combine(gwDirectory, WMVCOREDll);
        var wmasfDst = Path.Combine(gwDirectory, WMASFDll);
        var dsGuildWarsDst = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (!File.Exists(wmcoreDst) && File.Exists(wmcoreSrc))
        {
            File.Copy(wmcoreSrc, wmcoreDst);
            this.logger.LogDebug("Copied {Dll} to {Path}", WMVCOREDll, wmcoreDst);
        }

        if (!File.Exists(wmasfDst) && File.Exists(wmasfSrc))
        {
            File.Copy(wmasfSrc, wmasfDst);
            this.logger.LogDebug("Copied {Dll} to {Path}", WMASFDll, wmasfDst);
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
        var wmasfPath = Path.Combine(gwDirectory, WMASFDll);
        var dsGuildWarsPath = Path.Combine(gwDirectory, DsGuildWarsDll);

        if (File.Exists(wmcorePath))
        {
            File.Delete(wmcorePath);
            this.logger.LogDebug("Removed {Dll} from {Path}", WMVCOREDll, gwDirectory);
        }

        if (File.Exists(wmasfPath))
        {
            File.Delete(wmasfPath);
            this.logger.LogDebug("Removed {Dll} from {Path}", WMASFDll, gwDirectory);
        }

        if (File.Exists(dsGuildWarsPath))
        {
            File.Delete(dsGuildWarsPath);
            this.logger.LogDebug("Removed {Dll} from {Path}", DsGuildWarsDll, gwDirectory);
        }
    }
}
