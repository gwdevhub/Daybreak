using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.IO;

namespace Daybreak.Services.DirectSong;
internal sealed class DirectSongService(
    INotificationService notificationService,
    IPrivilegeManager privilegeManager,
    ISevenZipExtractor sevenZipExtractor,
    IDownloadService downloadService,
    ILiveUpdateableOptions<DirectSongOptions> options,
    ILogger<DirectSongService> logger) : IDirectSongService
{
    private const string DownloadUrl = "https://guildwarslegacy.com/DirectSong.7z";
    private const string RegistryEditorName = "RegisterDirectSongDirectory.exe";
    private const string InstallationDirectorySubPath = "DirectSong";
    private const string DestinationZipFile = "DirectSong.7z";
    private const string WMVCOREDll = "WMVCORE.DLL";
    private const string DsGuildwarsDll = "ds_GuildWars.dll";

    private static readonly ProgressUpdate ProgressStarting = new(0, "Starting DirectSong installation");
    private static readonly ProgressUpdate ProgressInitializingDownload = new(0, "Initializing DirectSong download");
    private static readonly ProgressUpdate ProgressFailed = new(1, "DirectSong installation failed");
    private static readonly ProgressUpdate ProgressCompleted = new(1, "DirectSong installation completed");
    private static readonly ProgressUpdate ProgressRegistry = new(1, "Setting up DirectSong registry entries");
    private static ProgressUpdate ProgressUnpacking(double progress) => new(progress, "Unpacking DirectSong files");

    private static readonly string InstallationDirectory = PathUtils.GetAbsolutePathFromRoot(InstallationDirectorySubPath);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();
    private readonly ISevenZipExtractor sevenZipExtractor = sevenZipExtractor.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly ILiveUpdateableOptions<DirectSongOptions> options = options.ThrowIfNull();
    private readonly ILogger<DirectSongService> logger = logger.ThrowIfNull();

    public string Name => "DirectSong";
    public string Description => "Enables custom Guild Wars soundtrack composed by Jeremy Soule";
    public bool IsVisible => true;
    public bool CanCustomManage => false;
    public bool CanUninstall => true;
    public bool IsEnabled
    {
        get => this.options.Value.Enabled;
        set
        {
            this.options.Value.Enabled = value;
            this.options.UpdateOption();
        }
    }
    public bool IsInstalled =>
        Directory.Exists(InstallationDirectory) &&
        File.Exists(Path.Combine(Path.GetFullPath(InstallationDirectory), WMVCOREDll)) &&
        File.Exists(Path.Combine(Path.GetFullPath(InstallationDirectory), DsGuildwarsDll));

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

    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken) => Task.FromResult(false);

    public Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DirectSong mod does not support manual updates");
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
        var wmCorePath = Path.Combine(gwDirectory, WMVCOREDll);
        var dsGuildWarsDll = Path.Combine(gwDirectory, DsGuildwarsDll);
        if (!File.Exists(wmCorePath))
        {
            File.Copy(Path.Combine(Path.GetFullPath(InstallationDirectory), WMVCOREDll), wmCorePath);
        }

        if (!File.Exists(dsGuildWarsDll))
        {
            File.Copy(Path.Combine(Path.GetFullPath(InstallationDirectory), DsGuildwarsDll), dsGuildWarsDll);
        }

        this.notificationService.NotifyInformation(
                title: "DirectSong started",
                description: "DirectSong has been set up");
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var gwPath = Path.GetFullPath(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath);
        var gwDirectory = Path.GetDirectoryName(gwPath)!;
        var wmCorePath = Path.Combine(gwDirectory, WMVCOREDll);
        var dsGuildWarsDll = Path.Combine(gwDirectory, DsGuildwarsDll);

        if (File.Exists(wmCorePath))
        {
            File.Delete(wmCorePath);
        }

        if (File.Exists(dsGuildWarsDll))
        {
            File.Delete(dsGuildWarsDll);
        }

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

        if (!this.privilegeManager.AdminPrivileges)
        {
            await this.privilegeManager.RequestAdminPrivileges<LaunchView>("DirectSong installation requires Administrator privileges in order to set up the registry entries", cancelViewParams: default, cancellationToken);
            return false;
        }

        progress.Report(ProgressStarting);
        var destinationPath = Path.Combine(Path.GetFullPath(InstallationDirectory), DestinationZipFile);
        if (!File.Exists(destinationPath))
        {
            Directory.CreateDirectory(Path.GetFullPath(InstallationDirectory));
            scopedLogger.LogDebug($"Downloading {DestinationZipFile}");
            progress.Report(ProgressInitializingDownload);
            if (!await this.downloadService.DownloadFile(DownloadUrl, destinationPath, progress, cancellationToken))
            {
                scopedLogger.LogError("Download failed. Check logs");
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

        scopedLogger.LogDebug("Extracted files. Setting up registry entries");
        progress.Report(ProgressRegistry);
        if (!await RunRegisterDirectSongDirectory(cancellationToken))
        {
            scopedLogger.LogError("Failed to set up registry entries");
            progress.Report(ProgressFailed);
            return false;
        }

        scopedLogger.LogDebug($"Deleting archive {destinationPath}");
        File.Delete(destinationPath);
        progress.Report(ProgressCompleted);
        return true;
    }

    private static async Task<bool> RunRegisterDirectSongDirectory(CancellationToken cancellationToken)
    {
        using var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Path.GetFullPath(InstallationDirectory), RegistryEditorName),
                WorkingDirectory = Path.GetFullPath(InstallationDirectory),
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        var success = false;
        process.OutputDataReceived += (s, e) =>
        {
            if (e.Data == Path.GetFullPath(InstallationDirectory))
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

        return success;
    }
}
