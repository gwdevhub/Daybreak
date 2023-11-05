using Daybreak.Configuration.Options;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Notifications;
using Daybreak.Services.Privilege;
using Daybreak.Services.SevenZip;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.DirectSong;
internal sealed class DirectSongService : IDirectSongService
{
    private const string DownloadUrl = "https://guildwarslegacy.com/DirectSong.7z";
    private const string RegistryEditorName = "RegisterDirectSongDirectory.exe";
    private const string InstallationDirectory = "DirectSong";
    private const string DestinationZipFile = "DirectSong.7z";
    private const string WMVCOREDll = "WMVCORE.DLL";
    private const string DsGuildwarsDll = "ds_GuildWars.dll";

    private readonly INotificationService notificationService;
    private readonly IPrivilegeManager privilegeManager;
    private readonly ISevenZipExtractor sevenZipExtractor;
    private readonly IDownloadService downloadService;
    private readonly ILiveUpdateableOptions<DirectSongOptions> options;
    private readonly ILogger<DirectSongService> logger;

    public string Name => "DirectSong";
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
    public DirectSongInstallationStatus? CachedInstallationStatus { get; private set; }
    public Task<bool>? InstallationTask { get; private set; }

    public DirectSongService(
        INotificationService notificationService,
        IPrivilegeManager privilegeManager,
        ISevenZipExtractor sevenZipExtractor,
        IDownloadService downloadService,
        ILiveUpdateableOptions<DirectSongOptions> options,
        ILogger<DirectSongService> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.sevenZipExtractor = sevenZipExtractor.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<string> GetCustomArguments() => Array.Empty<string>();

    public Task OnGuildWarsCreated(Process process, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(Process process, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(Process process, CancellationToken cancellationToken)
    {
        if (!this.IsInstalled)
        {
            return Task.CompletedTask;
        }

        var gwPath = Path.GetFullPath(process.StartInfo.FileName);
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

    public Task OnGuildWarsStartingDisabled(Process process, CancellationToken cancellationToken)
    {
        var gwPath = Path.GetFullPath(process.StartInfo.FileName);
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

    public Task<bool> SetupDirectSong(DirectSongInstallationStatus directSongInstallationStatus, CancellationToken cancellationToken)
    {
        if (this.InstallationTask is not null)
        {
            return this.InstallationTask;
        }

        this.InstallationTask = Task.Run(() => this.SetupDirectSongInternal(directSongInstallationStatus, cancellationToken));
        return this.InstallationTask;
    }

    private async Task<bool> SetupDirectSongInternal(DirectSongInstallationStatus directSongInstallationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.SetupDirectSongInternal), string.Empty);
        if (this.CachedInstallationStatus is not null)
        {
            scopedLogger.LogWarning($"Another installation already in progress. Check {nameof(this.CachedInstallationStatus)}");
            return false;
        }

        if (this.IsInstalled)
        {
            scopedLogger.LogInformation("Already installed");
            return true;
        }

        if (!this.privilegeManager.AdminPrivileges)
        {
            this.privilegeManager.RequestAdminPrivileges<LauncherView>("DirectSong installation requires Administrator privileges in order to set up the registry entries");
            return false;
        }

        this.CachedInstallationStatus = directSongInstallationStatus;
        directSongInstallationStatus.CurrentStep = DirectSongInstallationStatus.StartingStep;
        var destinationPath = Path.Combine(Path.GetFullPath(InstallationDirectory), DestinationZipFile);
        if (!File.Exists(destinationPath))
        {
            Directory.CreateDirectory(Path.GetFullPath(InstallationDirectory));
            scopedLogger.LogInformation($"Downloading {DestinationZipFile}");
            directSongInstallationStatus.CurrentStep = DirectSongInstallationStatus.InitializingDownload;
            if (!await this.downloadService.DownloadFile(DownloadUrl, destinationPath, directSongInstallationStatus, cancellationToken))
            {
                scopedLogger.LogError("Download failed. Check logs");
                return false;
            }
        }

        scopedLogger.LogInformation("Extracting DirectSong files");
        if (!await this.sevenZipExtractor.ExtractToDirectory(destinationPath, InstallationDirectory, (progress, fileName) =>
        {
            scopedLogger.LogInformation($"Extracted {fileName}");
            directSongInstallationStatus.CurrentStep = DirectSongInstallationStatus.Extracting(progress);
        }, cancellationToken))
        {
            scopedLogger.LogError("Extraction failed");
            return false;
        }

        scopedLogger.LogInformation("Extracted files. Setting up registry entries");
        directSongInstallationStatus.CurrentStep = DirectSongInstallationStatus.SetupRegistry;
        if (!await RunRegisterDirectSongDirectory(cancellationToken))
        {
            scopedLogger.LogError("Failed to set up registry entries");
            return false;
        }

        scopedLogger.LogInformation($"Deleting archive {destinationPath}");
        File.Delete(destinationPath);
        directSongInstallationStatus.CurrentStep = DirectSongInstallationStatus.Finished;
        this.InstallationTask = default;
        this.CachedInstallationStatus = default;
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
