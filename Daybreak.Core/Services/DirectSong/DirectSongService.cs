using CG.Web.MegaApiClient;
using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Github;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.DirectSong;

internal sealed class DirectSongService(
    IOptionsProvider optionsProvider,
    INotificationService notificationService,
    ISevenZipExtractor sevenZipExtractor,
    IDownloadService downloadService,
    IDirectSongRegistrar directSongRegistrar,
    IGithubClient githubClient,
    IOptionsMonitor<DirectSongOptions> options,
    ILogger<DirectSongService> logger) : IDirectSongService
{
    // Mega URL for DirectSong Revival Pack (more complete than the legacy pack)
    private const string MegaDownloadUrl = "https://mega.nz/file/P2pWGK7C#FLZZOOWE1c5gYSgCqD4MC464m6ZvK1oGTlS08hLpnKw";
    private const string RegistryEditorName = "RegisterDirectSongDirectory.exe";
    private const string InstallationDirectorySubPath = "DirectSong";
    private const string DestinationZipFile = "DirectSong.7z";

    // DirectSong Remix from ChthonVII - enhanced GuildWars.ds playlist
    private const string RemixRepoOwner = "ChthonVII";
    private const string RemixRepoName = "gwdirectsongremix";
    private const string RemixBranch = "main";
    private const string RemixFilePath = "GuildWars.ds";
    private const string RemixVersionFile = "GuildWars.ds.version";
    private const string RemixAudioFolder = "GWDirectSongRemix";
    private const string DirectSongSubdir = "DirectSong";

    private static readonly ProgressUpdate ProgressStarting = new(0, "Starting DirectSong installation");
    private static readonly ProgressUpdate ProgressPlatformFiles = new(0.78, "Setting up platform-specific files");
    private static readonly ProgressUpdate ProgressFailed = new(1, "DirectSong installation failed");
    private static readonly ProgressUpdate ProgressCompleted = new(1, "DirectSong installation completed");
    private static readonly ProgressUpdate ProgressRegistry = new(0.85, "Setting up DirectSong registry entries");
    private static readonly ProgressUpdate ProgressDllOverrides = new(0.90, "Setting up DLL overrides");
    private static readonly ProgressUpdate ProgressRemix = new(0.95, "Downloading DirectSong Remix playlist");
    private static ProgressUpdate ProgressDownloading(double progress) => new(Math.Clamp(progress * 0.50, 0, 0.50), $"Downloading DirectSong Revival Pack: {progress:P0}");
    private static ProgressUpdate ProgressUnpacking(double progress) => new(Math.Clamp((progress * 0.25) + 0.50, 0.50, 0.75), "Unpacking DirectSong files");

    private static readonly string InstallationDirectory = PathUtils.GetAbsolutePathFromRoot(InstallationDirectorySubPath);

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ISevenZipExtractor sevenZipExtractor = sevenZipExtractor.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IDirectSongRegistrar directSongRegistrar = directSongRegistrar.ThrowIfNull();
    private readonly IGithubClient githubClient = githubClient.ThrowIfNull();
    private readonly IOptionsMonitor<DirectSongOptions> options = options.ThrowIfNull();
    private readonly ILogger<DirectSongService> logger = logger.ThrowIfNull();

    public string Name => "DirectSong";
    public string Description => "Enables custom Guild Wars soundtrack composed by Jeremy Soule";
    public bool IsVisible => true;
    public bool CanCustomManage => false;
    public bool CanUninstall => true;
    public bool CanDisable => true;
    public bool IsEnabled
    {
        get => this.options.CurrentValue.Enabled;
        set
        {
            var options = this.options.CurrentValue;
            options.Enabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }
    public bool IsInstalled =>
        Directory.Exists(InstallationDirectory) &&
        this.directSongRegistrar.ArePlatformFilesInstalled(InstallationDirectory);

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create<bool>(progress =>
        {
            progress.Report(new ProgressUpdate(0, "Uninstalling DirectSong"));
            if (!this.IsInstalled)
            {
                progress.Report(new ProgressUpdate(1, "DirectSong is not installed"));
                return Task.FromResult(true);
            }

            if (!Directory.Exists(InstallationDirectory))
            {
                progress.Report(new ProgressUpdate(1, "DirectSong is not installed"));
                return Task.FromResult(true);
            }

            Directory.Delete(InstallationDirectory, true);
            progress.Report(new ProgressUpdate(1, "DirectSong uninstalled successfully"));
            return Task.FromResult(true);
        }, cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await Task.Factory.StartNew(() => this.SetupDirectSong(progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        }, cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DirectSong does not support custom management");
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public async Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        return await this.IsRemixUpdateAvailable(cancellationToken);
    }

    public async Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return false;
        }

        return await this.UpdateRemixPlaylist(cancellationToken);
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return Task.CompletedTask;
        }

        var gwPath = Path.GetFullPath(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath);
        var gwDirectory = Path.GetDirectoryName(gwPath)!;

        this.directSongRegistrar.CopyFilesToGuildWars(InstallationDirectory, gwDirectory);

        this.notificationService.NotifyInformation(
                title: "DirectSong started",
                description: "DirectSong has been set up");
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var gwPath = Path.GetFullPath(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath);
        var gwDirectory = Path.GetDirectoryName(gwPath)!;

        this.directSongRegistrar.RemoveFilesFromGuildWars(gwDirectory);

        return Task.CompletedTask;
    }

    public Task<bool> SetupDirectSong(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        return this.SetupDirectSongInternal(progress, cancellationToken);
    }

    private async Task<bool> SetupDirectSongInternal(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.IsInstalled)
        {
            scopedLogger.LogDebug("Already installed");
            return true;
        }

        // Check for gstreamer availability (no-op on Windows)
        if (!this.directSongRegistrar.IsGStreamerAvailable())
        {
            var instructions = this.directSongRegistrar.GetGStreamerInstallInstructions();
            this.notificationService.NotifyError(
                title: "GStreamer libav plugin missing",
                description: instructions,
                expirationTime: DateTime.UtcNow + TimeSpan.FromMinutes(5));
            scopedLogger.LogWarning("GStreamer libav plugin not found. DirectSong WMA playback may not work.");
            // Continue with installation - the user can install gstreamer later
        }

        progress.Report(ProgressStarting);
        var destinationPath = Path.Combine(Path.GetFullPath(InstallationDirectory), DestinationZipFile);
        if (!File.Exists(destinationPath))
        {
            Directory.CreateDirectory(Path.GetFullPath(InstallationDirectory));
            scopedLogger.LogDebug($"Downloading DirectSong Revival Pack from Mega");
            progress.Report(ProgressDownloading(0));
            
            try
            {
                var megaClient = new MegaApiClient();
                await megaClient.LoginAnonymousAsync();
                
                var fileUri = new Uri(MegaDownloadUrl);
                var node = await megaClient.GetNodeFromLinkAsync(fileUri);
                
                scopedLogger.LogDebug($"Downloading {node.Name} ({node.Size} bytes)");
                
                using var downloadStream = await megaClient.DownloadAsync(fileUri, new Progress<double>(p =>
                {
                    // MegaApiClient reports progress as 0-100, normalize to 0-1
                    var normalizedProgress = p / 100.0;
                    progress.Report(ProgressDownloading(normalizedProgress));
                }), cancellationToken);
                
                using var fileStream = File.Create(destinationPath);
                await downloadStream.CopyToAsync(fileStream, cancellationToken);
                
                await megaClient.LogoutAsync();
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Failed to download from Mega");
                progress.Report(ProgressFailed);
                return false;
            }
        }

        scopedLogger.LogDebug("Extracting DirectSong files");
        if (!await this.sevenZipExtractor.ExtractToDirectory(destinationPath, InstallationDirectory, (progressValue, fileName) =>
        {
            scopedLogger.LogDebug($"Extracted {fileName}");
            progress.Report(ProgressUnpacking(progressValue));
        }, cancellationToken))
        {
            //TODO: Better handle corrupted downloaded archives
            scopedLogger.LogError("Extraction failed");
            progress.Report(ProgressFailed);
            File.Delete(destinationPath);
            return false;
        }

        // Set up platform-specific files (downloads extra DLLs on Linux, no-op on Windows)
        scopedLogger.LogDebug("Setting up platform-specific files");
        progress.Report(ProgressPlatformFiles);
        if (!await this.directSongRegistrar.SetupPlatformFiles(InstallationDirectory, this.downloadService, progress, cancellationToken))
        {
            scopedLogger.LogError("Failed to set up platform-specific files");
            progress.Report(ProgressFailed);
            return false;
        }

        scopedLogger.LogDebug("Setting up registry entries");
        progress.Report(ProgressRegistry);
        if (!await this.directSongRegistrar.RegisterDirectSongDirectory(InstallationDirectory, RegistryEditorName, cancellationToken))
        {
            scopedLogger.LogError("Failed to set up registry entries");
            progress.Report(ProgressFailed);
            return false;
        }

        // Set up DLL overrides (wmvcore, wmasf to native,builtin) - no-op on Windows
        scopedLogger.LogDebug("Setting up DLL overrides");
        progress.Report(ProgressDllOverrides);
        if (!await this.directSongRegistrar.SetupDllOverrides(cancellationToken))
        {
            scopedLogger.LogError("Failed to set up DLL overrides");
            progress.Report(ProgressFailed);
            return false;
        }

        // Download DirectSong Remix GuildWars.ds from ChthonVII's repo
        scopedLogger.LogDebug("Downloading DirectSong Remix playlist");
        progress.Report(ProgressRemix);
        var directSongDir = Path.Combine(Path.GetFullPath(InstallationDirectory), DirectSongSubdir);
        var guildWarsDsPath = Path.Combine(directSongDir, RemixFilePath);
        var versionFilePath = Path.Combine(directSongDir, RemixVersionFile);

        if (await this.githubClient.DownloadRawFile(RemixRepoOwner, RemixRepoName, RemixBranch, RemixFilePath, guildWarsDsPath, cancellationToken))
        {
            // Save the commit SHA for version tracking
            var commitSha = await this.githubClient.GetLatestCommitSha(RemixRepoOwner, RemixRepoName, RemixBranch, cancellationToken);
            if (!string.IsNullOrEmpty(commitSha))
            {
                await File.WriteAllTextAsync(versionFilePath, commitSha, cancellationToken);
                scopedLogger.LogDebug("Downloaded DirectSong Remix (commit {CommitSha})", commitSha);
            }
            else
            {
                scopedLogger.LogDebug("Downloaded DirectSong Remix (commit SHA unavailable)");
            }
        }
        else
        {
            scopedLogger.LogWarning("Failed to download DirectSong Remix, using default GuildWars.ds from Revival Pack");
            // Not a fatal error - the original GuildWars.ds from the Revival Pack will still work
        }

        // Sync audio files from the GWDirectSongRemix folder
        await this.SyncRemixAudioFiles(cancellationToken);

        scopedLogger.LogDebug($"Deleting archive {destinationPath}");
        File.Delete(destinationPath);
        progress.Report(ProgressCompleted);
        return true;
    }

    /// <summary>
    /// Gets the currently installed DirectSong Remix commit SHA from the version file.
    /// </summary>
    private string? GetInstalledRemixCommitSha()
    {
        var directSongDir = Path.Combine(Path.GetFullPath(InstallationDirectory), DirectSongSubdir);
        var versionFilePath = Path.Combine(directSongDir, RemixVersionFile);

        if (!File.Exists(versionFilePath))
        {
            return null;
        }

        try
        {
            // Version file contains just the commit SHA
            return File.ReadAllText(versionFilePath).Trim();
        }
        catch (Exception ex)
        {
            this.logger.LogDebug(ex, "Failed to read remix version file");
            return null;
        }
    }

    /// <summary>
    /// Checks if a newer version of the DirectSong Remix playlist is available.
    /// </summary>
    private async Task<bool> IsRemixUpdateAvailable(CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return false;
        }

        var installedSha = this.GetInstalledRemixCommitSha();
        if (string.IsNullOrEmpty(installedSha))
        {
            // No version file means we should try to download/update
            return true;
        }

        var latestSha = await this.githubClient.GetLatestCommitSha(RemixRepoOwner, RemixRepoName, RemixBranch, cancellationToken);
        if (string.IsNullOrEmpty(latestSha))
        {
            // Can't determine latest version, assume no update
            return false;
        }

        // Compare SHAs (case-insensitive since git SHAs are hex)
        var updateAvailable = !installedSha.Equals(latestSha, StringComparison.OrdinalIgnoreCase);
        if (updateAvailable)
        {
            this.logger.LogDebug("DirectSong Remix update available. Installed: {Installed}, Latest: {Latest}", installedSha, latestSha);
        }

        return updateAvailable;
    }

    /// <summary>
    /// Updates the DirectSong Remix playlist to the latest version.
    /// </summary>
    private async Task<bool> UpdateRemixPlaylist(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var directSongDir = Path.Combine(Path.GetFullPath(InstallationDirectory), DirectSongSubdir);
        var guildWarsDsPath = Path.Combine(directSongDir, RemixFilePath);
        var versionFilePath = Path.Combine(directSongDir, RemixVersionFile);

        scopedLogger.LogDebug("Updating DirectSong Remix playlist");

        if (!await this.githubClient.DownloadRawFile(RemixRepoOwner, RemixRepoName, RemixBranch, RemixFilePath, guildWarsDsPath, cancellationToken))
        {
            scopedLogger.LogError("Failed to download DirectSong Remix update");
            return false;
        }

        // Sync audio files from the GWDirectSongRemix folder
        await this.SyncRemixAudioFiles(cancellationToken);

        // Save the new commit SHA
        var commitSha = await this.githubClient.GetLatestCommitSha(RemixRepoOwner, RemixRepoName, RemixBranch, cancellationToken);
        if (!string.IsNullOrEmpty(commitSha))
        {
            await File.WriteAllTextAsync(versionFilePath, commitSha, cancellationToken);
            scopedLogger.LogDebug("Updated DirectSong Remix to commit {CommitSha}", commitSha);
            this.notificationService.NotifyInformation(
                title: "DirectSong Remix updated",
                description: $"Updated to commit {commitSha[..7]}");
        }
        else
        {
            scopedLogger.LogWarning("Updated DirectSong Remix but could not determine commit SHA");
            this.notificationService.NotifyInformation(
                title: "DirectSong Remix updated",
                description: "Downloaded latest version");
        }

        return true;
    }

    /// <summary>
    /// Syncs the GWDirectSongRemix audio files from the GitHub repository.
    /// Downloads any missing files (except readme.txt) to the DirectSong folder.
    /// </summary>
    private async Task<int> SyncRemixAudioFiles(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var directSongDir = Path.Combine(Path.GetFullPath(InstallationDirectory), DirectSongSubdir);
        var remixAudioDir = Path.Combine(directSongDir, RemixAudioFolder);

        // Ensure the directory exists
        Directory.CreateDirectory(remixAudioDir);

        // Get list of files from GitHub
        var repoFiles = await this.githubClient.GetDirectoryContents(
            RemixRepoOwner, RemixRepoName, RemixAudioFolder, RemixBranch, cancellationToken);

        var filesToDownload = repoFiles
            .Where(f => !f.Equals("readme.txt", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (filesToDownload.Count == 0)
        {
            scopedLogger.LogDebug("No audio files found in GWDirectSongRemix folder");
            return 0;
        }

        var downloadedCount = 0;
        foreach (var fileName in filesToDownload)
        {
            var localPath = Path.Combine(remixAudioDir, fileName);
            if (File.Exists(localPath))
            {
                scopedLogger.LogDebug("File already exists: {FileName}", fileName);
                continue;
            }

            var repoPath = $"{RemixAudioFolder}/{fileName}";
            scopedLogger.LogDebug("Downloading missing audio file: {FileName}", fileName);

            if (await this.githubClient.DownloadRawFile(
                RemixRepoOwner, RemixRepoName, RemixBranch, repoPath, localPath, cancellationToken))
            {
                downloadedCount++;
                scopedLogger.LogInformation("Downloaded audio file: {FileName}", fileName);
            }
            else
            {
                scopedLogger.LogWarning("Failed to download audio file: {FileName}", fileName);
            }
        }

        if (downloadedCount > 0)
        {
            this.notificationService.NotifyInformation(
                title: "DirectSong Remix audio files synced",
                description: $"Downloaded {downloadedCount} new audio file(s)");
        }

        return downloadedCount;
    }
}
