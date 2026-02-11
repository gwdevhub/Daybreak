using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Github;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.TarGz;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions.Core;
using Daybreak.Shared.Services.DXVK;

namespace Daybreak.Services.DXVK;

internal sealed class DXVKService(
    IOptionsProvider optionsProvider,
    INotificationService notificationService,
    IDownloadService downloadService,
    ITarGzExtractor tarGzExtractor,
    IGithubClient githubClient,
    IOptionsMonitor<DXVKOptions> dxvkOptions,
    ILogger<DXVKService> logger) : IDXVKService
{
    private const string GithubOwner = "doitsujin";
    private const string GithubRepo = "dxvk";
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = $"https://github.com/{GithubOwner}/{GithubRepo}/releases/download/v{TagPlaceholder}/dxvk-{TagPlaceholder}.tar.gz";
    private const string DirectorySubPath = "DXVK";
    private const string D3D9Dll = "d3d9.dll";
    private const string D3D9DllPattern = "d3d9-*.dll";
    private const string X32SubFolder = "x32";
    private const string X64SubFolder = "x64";

    private static readonly ProgressUpdate ProgressFinished = new(1, "DXVK installation finished");

    private static readonly string DXVKDirectory = PathUtils.GetAbsolutePathFromRoot(DirectorySubPath);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly ITarGzExtractor tarGzExtractor = tarGzExtractor.ThrowIfNull();
    private readonly IGithubClient githubClient = githubClient.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<DXVKOptions> dxvkOptions = dxvkOptions.ThrowIfNull();
    private readonly ILogger<DXVKService> logger = logger.ThrowIfNull();

    public string Name => "DXVK";
    public string Description => "DXVK is a Vulkan-based compatibility layer for Direct3D 9/10/11 which improves performance and reduces stuttering";
    public bool IsVisible => true;
    public bool CanCustomManage => false;
    public bool CanUninstall => true;
    public bool CanDisable => true;

    public bool IsEnabled
    {
        get => this.dxvkOptions.CurrentValue.Enabled;
        set
        {
            var options = this.dxvkOptions.CurrentValue;
            options.Enabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }

    public bool IsInstalled
    {
        get
        {
            // Check for versioned dll file (used by Windows installer)
            if (this.GetInstalledDxvkFile() is null)
            {
                return false;
            }

            // Also check for x32 and x64 folders (used by Linux installer)
            var x32Folder = Path.Combine(DXVKDirectory, X32SubFolder);
            var x64Folder = Path.Combine(DXVKDirectory, X64SubFolder);
            return Directory.Exists(x32Folder) && Directory.Exists(x64Folder);
        }
    }

    public Version Version
    {
        get
        {
            var installedFile = this.GetInstalledDxvkFile();
            if (installedFile is null)
            {
                return new Version(0, 0, 0, 0);
            }

            // Extract version from filename: d3d9-2.7.1.dll -> 2.7.1
            var fileName = Path.GetFileNameWithoutExtension(installedFile);
            var versionString = fileName.Replace("d3d9-", "");
            if (Version.TryParse(versionString, out var version))
            {
                return version;
            }

            return new Version(0, 0, 0, 0);
        }
    }

    public async Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return false;
        }

        var latestVersion = await this.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            return false;
        }

        return this.Version < latestVersion;
    }

    public async Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        return await this.PerformInstallation(cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await Task.Factory.StartNew(
                () => this.SetupDXVKInternal(progress, cancellationToken),
                cancellationToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current).Unwrap();
        }, cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create<bool>(progress =>
        {
            progress.Report(new ProgressUpdate(0, "Uninstalling DXVK"));

            var installedFile = this.GetInstalledDxvkFile();
            if (installedFile is null)
            {
                progress.Report(new ProgressUpdate(1, "DXVK is not installed"));
                return Task.FromResult(true);
            }

            try
            {
                File.Delete(installedFile);

                // Clean up the directory if empty
                if (Directory.Exists(DXVKDirectory) && !Directory.EnumerateFileSystemEntries(DXVKDirectory).Any())
                {
                    Directory.Delete(DXVKDirectory, recursive: true);
                }

                progress.Report(new ProgressUpdate(1, "DXVK uninstalled successfully"));
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to uninstall DXVK");
                progress.Report(new ProgressUpdate(1, $"Failed to uninstall DXVK: {ex.Message}"));
                return Task.FromResult(false);
            }
        }, cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DXVK does not support custom management");
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
        => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        // Copy DXVK d3d9.dll to the Guild Wars folder before the process is created
        // Windows/Wine will load DLLs from the application folder first
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.IsInstalled)
        {
            scopedLogger.LogWarning("DXVK is enabled but not installed. Skipping DXVK setup");
            return Task.CompletedTask;
        }

        var gwFolder = Path.GetDirectoryName(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath);
        if (string.IsNullOrEmpty(gwFolder))
        {
            scopedLogger.LogError("Could not determine Guild Wars folder");
            return Task.CompletedTask;
        }

        // Find the DXVK d3d9.dll (from x32 folder for 32-bit Guild Wars)
        var sourceDll = Path.Combine(DXVKDirectory, X32SubFolder, D3D9Dll);
        if (!File.Exists(sourceDll))
        {
            // Fall back to versioned file
            var versionedFile = this.GetInstalledDxvkFile();
            if (versionedFile is not null)
            {
                sourceDll = versionedFile;
            }
            else
            {
                scopedLogger.LogError("DXVK d3d9.dll not found");
                return Task.CompletedTask;
            }
        }

        var targetDll = Path.Combine(gwFolder, D3D9Dll);

        try
        {
            File.Copy(sourceDll, targetDll, overwrite: true);
            scopedLogger.LogInformation("Copied DXVK d3d9.dll to {TargetPath}", targetDll);
            this.notificationService.NotifyInformation(
                title: "DXVK enabled",
                description: "DXVK d3d9.dll has been copied to the Guild Wars folder");
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to copy DXVK d3d9.dll to Guild Wars folder");
            this.notificationService.NotifyError(
                title: "DXVK setup failed",
                description: $"Failed to copy d3d9.dll: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        // Remove DXVK d3d9.dll from the Guild Wars folder when disabled
        var scopedLogger = this.logger.CreateScopedLogger();

        var gwFolder = Path.GetDirectoryName(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath);
        if (string.IsNullOrEmpty(gwFolder))
        {
            return Task.CompletedTask;
        }

        var targetDll = Path.Combine(gwFolder, D3D9Dll);
        if (File.Exists(targetDll))
        {
            try
            {
                File.Delete(targetDll);
                scopedLogger.LogInformation("Removed d3d9.dll from {TargetPath}", targetDll);
            }
            catch (Exception ex)
            {
                scopedLogger.LogWarning(ex, "Failed to remove d3d9.dll from Guild Wars folder");
            }
        }

        return Task.CompletedTask;
    }

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public async Task<bool> SetupDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        return await this.SetupDXVKInternal(progress, cancellationToken);
    }

    public async Task CheckAndUpdateDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.IsInstalled)
        {
            scopedLogger.LogInformation("DXVK is not installed. Skipping update check");
            return;
        }

        var latestVersion = await this.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            scopedLogger.LogError("Could not retrieve latest DXVK version. Skipping update");
            return;
        }

        if (this.Version >= latestVersion)
        {
            scopedLogger.LogInformation("DXVK is up to date. Current version: {CurrentVersion}, Latest version: {LatestVersion}",
                this.Version, latestVersion);
            return;
        }

        scopedLogger.LogInformation("Updating DXVK from {CurrentVersion} to {LatestVersion}", this.Version, latestVersion);
        var success = await this.DownloadAndExtractDXVK(progress, cancellationToken);
        if (success)
        {
            this.notificationService.NotifyInformation(
                title: "DXVK updated",
                description: $"DXVK has been updated to version {latestVersion}");
        }
    }

    private async Task<bool> SetupDXVKInternal(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        if (this.IsInstalled)
        {
            progress.Report(ProgressFinished);
            return true;
        }

        var success = await this.DownloadAndExtractDXVK(progress, cancellationToken);
        if (!success)
        {
            this.logger.LogError("Failed to setup DXVK");
            return false;
        }

        progress.Report(ProgressFinished);
        return true;
    }

    private async Task<bool> DownloadAndExtractDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        // Get the latest version using redirect (avoids API rate limits)
        progress.Report(new ProgressUpdate(0, "Checking for latest DXVK version"));
        var latestVersion = await this.githubClient.GetLatestVersionFromRedirect(GithubOwner, GithubRepo, cancellationToken);

        if (latestVersion is null)
        {
            scopedLogger.LogError("Could not find latest DXVK version");
            return false;
        }

        var latestTag = latestVersion.ToString();
        scopedLogger.LogInformation("Latest DXVK version: {Version}", latestTag);

        // Download the tar.gz file
        progress.Report(new ProgressUpdate(0.1, $"Downloading DXVK v{latestTag}"));
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, latestTag);
        var tempFile = Path.Combine(Path.GetTempPath(), $"dxvk-{latestTag}.tar.gz");

        try
        {
            var downloadProgress = new Progress<ProgressUpdate>(update =>
            {
                // Map download progress to 10-50% of total progress
                var mappedProgress = 0.1 + (update.Percentage * 0.4);
                progress.Report(new ProgressUpdate(mappedProgress, update.StatusMessage));
            });

            var downloadSuccess = await this.downloadService.DownloadFile(downloadUrl, tempFile, downloadProgress, cancellationToken);
            if (!downloadSuccess)
            {
                scopedLogger.LogError("Failed to download DXVK from {Url}", downloadUrl);
                return false;
            }

            // Extract the tar.gz file
            progress.Report(new ProgressUpdate(0.5, "Extracting DXVK"));
            var extractDir = Path.Combine(Path.GetTempPath(), $"dxvk-{latestTag}-extract");

            var extractSuccess = await this.tarGzExtractor.ExtractToDirectory(
                tempFile,
                extractDir,
                (extractProgress, fileName) =>
                {
                    // Map extraction progress to 50-90% of total progress
                    var mappedProgress = 0.5 + (extractProgress * 0.4);
                    progress.Report(new ProgressUpdate(mappedProgress, $"Extracting: {fileName}"));
                },
                cancellationToken);

            if (!extractSuccess)
            {
                scopedLogger.LogError("Failed to extract DXVK archive");
                return false;
            }

            // Find and copy the x32 d3d9.dll (Guild Wars is 32-bit)
            progress.Report(new ProgressUpdate(0.9, "Installing DXVK"));
            var dxvkExtractRoot = Path.Combine(extractDir, $"dxvk-{latestTag}");
            var sourceDll = Path.Combine(dxvkExtractRoot, X32SubFolder, D3D9Dll);
            if (!File.Exists(sourceDll))
            {
                scopedLogger.LogError("Could not find d3d9.dll in extracted archive at {Path}", sourceDll);
                return false;
            }

            Directory.CreateDirectory(DXVKDirectory);

            // Delete any existing versioned dll files
            foreach (var existingFile in Directory.GetFiles(DXVKDirectory, D3D9DllPattern))
            {
                try
                {
                    File.Delete(existingFile);
                    scopedLogger.LogDebug("Deleted old DXVK file: {Path}", existingFile);
                }
                catch (Exception ex)
                {
                    scopedLogger.LogWarning(ex, "Failed to delete old DXVK file: {Path}", existingFile);
                }
            }

            // Save with versioned filename: d3d9-2.7.1.dll
            var targetDll = Path.Combine(DXVKDirectory, $"d3d9-{latestTag}.dll");
            File.Copy(sourceDll, targetDll, overwrite: true);

            // Copy x32 and x64 folders for Linux Wine support
            var x32Source = Path.Combine(dxvkExtractRoot, X32SubFolder);
            var x64Source = Path.Combine(dxvkExtractRoot, X64SubFolder);
            var x32Target = Path.Combine(DXVKDirectory, X32SubFolder);
            var x64Target = Path.Combine(DXVKDirectory, X64SubFolder);

            // Clean up old x32/x64 folders
            if (Directory.Exists(x32Target))
            {
                Directory.Delete(x32Target, recursive: true);
            }
            if (Directory.Exists(x64Target))
            {
                Directory.Delete(x64Target, recursive: true);
            }

            // Copy x32 folder
            if (Directory.Exists(x32Source))
            {
                CopyDirectory(x32Source, x32Target);
                scopedLogger.LogDebug("Copied x32 folder for Linux Wine support");
            }

            // Copy x64 folder if it exists
            if (Directory.Exists(x64Source))
            {
                CopyDirectory(x64Source, x64Target);
                scopedLogger.LogDebug("Copied x64 folder for Linux Wine support");
            }

            scopedLogger.LogInformation("DXVK v{Version} installed successfully to {Path}", latestTag, targetDll);

            // Clean up temp files
            try
            {
                File.Delete(tempFile);
                Directory.Delete(extractDir, recursive: true);
            }
            catch (Exception ex)
            {
                scopedLogger.LogWarning(ex, "Failed to clean up temporary files");
            }

            return true;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to download and extract DXVK");
            return false;
        }
    }

    private async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving DXVK latest version");

        // Use redirect method to avoid API rate limits
        var latestVersion = await this.githubClient.GetLatestVersionFromRedirect(GithubOwner, GithubRepo, cancellationToken);
        if (latestVersion is null)
        {
            scopedLogger.LogError("Unable to retrieve latest DXVK version");
            return default;
        }

        return latestVersion;
    }

    /// <summary>
    /// Gets the path to the installed DXVK dll file, or null if not installed.
    /// Looks for files matching the pattern d3d9-*.dll in the DXVK directory.
    /// </summary>
    private string? GetInstalledDxvkFile()
    {
        if (!Directory.Exists(DXVKDirectory))
        {
            return null;
        }

        // Look for versioned dll files: d3d9-2.7.1.dll
        var files = Directory.GetFiles(DXVKDirectory, D3D9DllPattern);
        return files.FirstOrDefault();
    }

    /// <summary>
    /// Recursively copies a directory and its contents.
    /// </summary>
    private static void CopyDirectory(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var fileName = Path.GetFileName(file);
            File.Copy(file, Path.Combine(targetDir, fileName), overwrite: true);
        }

        foreach (var subDir in Directory.GetDirectories(sourceDir))
        {
            var dirName = Path.GetFileName(subDir);
            CopyDirectory(subDir, Path.Combine(targetDir, dirName));
        }
    }
}
