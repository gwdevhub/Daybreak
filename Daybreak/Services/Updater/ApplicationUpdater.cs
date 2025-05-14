using Daybreak.Configuration.Options;
using Daybreak.Services.Updater.Models;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Data;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpdateStatus = Daybreak.Shared.Models.Progress.UpdateStatus;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Services.Updater;

internal sealed class ApplicationUpdater : IApplicationUpdater
{
    private const string UpdatePkgSubPath = "update.pkg";
    private const string TempInstallerFileNameSubPath = "Daybreak.Installer.Temp.exe";
    private const string InstallerFileNameSubPath = "Daybreak.Installer.exe";
    private const string UpdatedKey = "LauncherUpdating";
    private const string TempFileSubPath = "tempfile.zip";
    private const string VersionTag = "{VERSION}";
    private const string FileTag = "{FILE}";
    private const string RefTagPrefix = "/refs/tags";
    private const string VersionListUrl = "https://api.github.com/repos/AlexMacocian/Daybreak/git/refs/tags";
    private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
    private const string DownloadUrl = $"https://github.com/AlexMacocian/Daybreak/releases/download/{VersionTag}/Daybreak{VersionTag}.zip";
    private const string BlobStorageUrl = $"https://daybreak.blob.core.windows.net/{VersionTag}/{FileTag}";
    private const int DownloadParallelTasks = 10;

    private readonly static string TempInstallerFileName = PathUtils.GetAbsolutePathFromRoot(TempInstallerFileNameSubPath);
    private readonly static string InstallerFileName = PathUtils.GetAbsolutePathFromRoot(InstallerFileNameSubPath);
    private readonly static string TempFile = PathUtils.GetAbsolutePathFromRoot(TempFileSubPath);
    private readonly static string UpdatePkg = PathUtils.GetAbsolutePathFromRoot(UpdatePkgSubPath);

    private readonly static TimeSpan DownloadInfoUpdateInterval = TimeSpan.FromMilliseconds(16);

    private readonly CancellationTokenSource updateCancellationTokenSource = new();
    private readonly INotificationService notificationService;
    private readonly IRegistryService registryService;
    private readonly IDownloadService downloadService;
    private readonly IPostUpdateActionProvider postUpdateActionProvider;
    private readonly ILiveOptions<LauncherOptions> liveOptions;
    private readonly IHttpClient<ApplicationUpdater> httpClient;
    private readonly ILogger<ApplicationUpdater> logger;

    public Version CurrentVersion { get; }

    public ApplicationUpdater(
        INotificationService notificationService,
        IRegistryService registryService,
        IDownloadService downloadService,
        IPostUpdateActionProvider postUpdateActionProvider,
        ILiveOptions<LauncherOptions> liveOptions,
        IHttpClient<ApplicationUpdater> httpClient,
        ILogger<ApplicationUpdater> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.registryService = registryService.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.postUpdateActionProvider = postUpdateActionProvider.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        if (Version.TryParse(Assembly.GetExecutingAssembly()!.GetName()!.Version!.ToString(), out var currentVersion))
        {
            if (currentVersion.HasPrefix is false)
            {
                currentVersion = Version.Parse("v" + currentVersion);
            }

            this.CurrentVersion = currentVersion;
        }
        else
        {
            throw new FatalException($"Application version is invalid: {Assembly.GetExecutingAssembly().GetName().Version}");
        }
    }

    public async Task<bool> DownloadUpdate(Version version, UpdateStatus updateStatus)
    {
        if (version.HasPrefix is false)
        {
            version.HasPrefix = true;
        }

        if (!this.liveOptions.Value.BetaUpdate)
        {
            return await this.DownloadUpdateInternalLegacy(version, updateStatus);
        }

        var maybeMetadataResponse = await this.httpClient.GetAsync(
            BlobStorageUrl
                .Replace(VersionTag, version.ToString().Replace(".", "-"))
                .Replace(FileTag, "Metadata.json"));
        if (maybeMetadataResponse.IsSuccessStatusCode)
        {
            var metaData = await maybeMetadataResponse.Content.ReadFromJsonAsync<List<Metadata>>();
            if (metaData is not null)
            {
                return await
                    await new TaskFactory().StartNew(() => this.DownloadUpdateInternalBlob(metaData, version, updateStatus), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            }
        }

        return await this.DownloadUpdateInternalLegacy(version, updateStatus);
    }

    public async Task<bool> DownloadLatestUpdate(UpdateStatus updateStatus)
    {
        updateStatus.CurrentStep = UpdateStatus.CheckingLatestVersion;
        var latestVersion = await this.GetLatestVersion();
        if (latestVersion is null)
        {
            this.logger.LogWarning("Failed to retrieve latest version. Aborting update");
            return false;
        }

        return await this.DownloadUpdate(latestVersion, updateStatus);
    }

    public async Task<bool> UpdateAvailable()
    {
        var maybeLatestVersion = await this.GetLatestVersion();
        if (maybeLatestVersion is null)
        {
            this.logger.LogWarning("Failed to retrieve latest version");
            return false;
        }

        return this.CurrentVersion.CompareTo(maybeLatestVersion) < 0;
    }

    public async Task<IEnumerable<Version>> GetVersions()
    {
        this.logger.LogInformation($"Retrieving version list from {VersionListUrl}");
        var response = await this.httpClient.GetAsync(VersionListUrl);
        if (response.IsSuccessStatusCode)
        {
            var serializedList = await response.Content.ReadAsStringAsync();
            var versionList = serializedList.Deserialize<GithubRefTag[]>();
            return versionList!.Select(v => v.Ref!.Remove(0, RefTagPrefix.Length)).Select(v => new Version(v));
        }

        return new List<Version>();
    }

    public async Task<string?> GetChangelog(Version version)
    {
        var changeLogResponse = await this.httpClient.GetAsync(
            BlobStorageUrl
                .Replace(VersionTag, version.ToString().Replace(".", "-"))
                .Replace(FileTag, "changelog.txt"));

        if (!changeLogResponse.IsSuccessStatusCode)
        {
            return default;
        }

        return await changeLogResponse.Content.ReadAsStringAsync();
    }

    public void PeriodicallyCheckForUpdates()
    {
        System.Extensions.TaskExtensions.RunPeriodicAsync(async () =>
        {
            if (this.liveOptions.Value.AutoCheckUpdate is false)
            {
                return;
            }

            if (await this.UpdateAvailable())
            {
                var maybeLatestVersion = await this.GetLatestVersion();
                if (maybeLatestVersion is null)
                {
                    return;
                }

                this.notificationService.NotifyInformation<UpdateNotificationHandler>(
                    title: "Daybreak Update Available",
                    description: $"Version v{maybeLatestVersion} of Daybreak is available.\nClick this notification to start the update process",
                    metaData: maybeLatestVersion.ToString(),
                    persistent: true);
            }
        }, TimeSpan.FromSeconds(0), TimeSpan.FromMinutes(15), this.updateCancellationTokenSource.Token);
    }

    public void FinalizeUpdate()
    {
        this.MarkUpdateInRegistry();
        this.LaunchExtractor();
    }

    public void OnStartup()
    {
        if (this.UpdateMarkedInRegistry())
        {
            this.UnmarkUpdateInRegistry();
            Task.Factory.StartNew(this.ExecutePostUpdateActions, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        this.PeriodicallyCheckForUpdates();
    }

    public void OnClosing()
    {
    }

    private async Task<bool> DownloadUpdateInternalBlob(List<Metadata> metadata, Version version, UpdateStatus updateStatus)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.DownloadUpdateInternalBlob), version.ToString());
        updateStatus.CurrentStep = DownloadStatus.InitializingDownload;

        // Exclude daybreak packed files
        var daybreakArchive = $"daybreak{version}.zip";
        var filesToDownload = metadata
            .Where(m => m.Name != daybreakArchive)
            .Where(m =>
            {
                var fileInfo = new FileInfo(m.RelativePath!);
                if (!fileInfo.Exists)
                {
                    return true;
                }

                if (fileInfo.Length != m.Size)
                {
                    return true;
                }

                try
                {
                    var info = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
                    if (info is null && m.VersionInfo is null)
                    {
                        return false;
                    }

                    if (info is null && m.VersionInfo is not null)
                    {
                        return true;
                    }

                    if (info is not null && m.VersionInfo is null)
                    {
                        return false;
                    }

                    return m.VersionInfo!.CompareTo(info!.ProductVersion) > 0;
                }
                catch
                {
                    return false;
                }

            })
            .Where(m =>
            {
                // Special case for Daybreak.Installer
                if (m.Name != TempInstallerFileName)
                {
                    return true;
                }

                var existingInstaller = new FileInfo(InstallerFileName);
                return !existingInstaller.Exists || existingInstaller.Length != m.Size;
            })
            .ToList();

        using var packageStream = new FileStream(UpdatePkg, FileMode.Create);
        var downloaded = 0d;
        var downloadBuffer = new byte[8192];
        var sizeToDownload = (double)filesToDownload.Sum(m => m.Size);
        var sw = Stopwatch.StartNew();
        var lastUpdate = DateTime.Now;
        var speedMeasurements = new List<double>();
        var fileQueue = filesToDownload;
        var pendingDownloads = fileQueue.AsEnumerable();
        while(pendingDownloads.Any())
        {
            var parallelDownloads = pendingDownloads.Take(DownloadParallelTasks);
            pendingDownloads = pendingDownloads.Skip(DownloadParallelTasks);
            // Setup the download streams for all the parallel downloads
            var parallelResponses = parallelDownloads.Select(file =>
            {
                return Task.Run(async () =>
                {
                    var downloadUrl = BlobStorageUrl.Replace(VersionTag, version.ToString().Replace('.', '-')).Replace(FileTag, file.RelativePath);
                    var response = await this.httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                    if (!response.IsSuccessStatusCode)
                    {
                        scopedLogger.LogError($"Error {response.StatusCode} when downloading {file.RelativePath}");
                        return (file, false, response);
                    }

                    return (file, true, response);
                });
            });

            foreach(var result in parallelResponses)
            {
                (var file, var fileDownloadResult, var response) = await result;
                if (fileDownloadResult is false)
                {
                    scopedLogger.LogError($"{file.RelativePath} failed to download. Cancelling update");
                    return false;
                }

                var fileNameBytes = Encoding.UTF8.GetBytes(file.Name!);
                var relativePathBytes = Encoding.UTF8.GetBytes(file.RelativePath!);
                await packageStream.WriteAsync(BitConverter.GetBytes(fileNameBytes.Length));
                await packageStream.WriteAsync(fileNameBytes);
                await packageStream.WriteAsync(BitConverter.GetBytes(relativePathBytes.Length));
                await packageStream.WriteAsync(relativePathBytes);
                await packageStream.WriteAsync(BitConverter.GetBytes(file.Size));

                var downloadStream = await response.Content.ReadAsStreamAsync();
                var maxMeasurements = 100;
                var fileSize = file.Size;
                while (fileSize > 0)
                {
                    var readBytes = await downloadStream.ReadAsync(downloadBuffer, 0, downloadBuffer.Length);
                    await packageStream.WriteAsync(downloadBuffer, 0, readBytes);

                    fileSize -= readBytes;
                    downloaded += readBytes;
                    if (DateTime.Now - lastUpdate > DownloadInfoUpdateInterval)
                    {
                        lastUpdate = DateTime.Now;
                        var currentSpeed = downloaded / sw.ElapsedMilliseconds;
                        if (speedMeasurements.Count > maxMeasurements)
                        {
                            speedMeasurements.Remove(0);
                        }

                        speedMeasurements.Add(currentSpeed);
                        var averageSpeed = speedMeasurements.Average();
                        var remainingBytes = sizeToDownload - downloaded;
                        var etaMillis = averageSpeed > 0 ? remainingBytes / averageSpeed : 0;
                        updateStatus.CurrentStep = DownloadStatus.Downloading(downloaded / sizeToDownload, TimeSpan.FromMilliseconds(etaMillis));
                    }
                }
            }
        }

        updateStatus.CurrentStep = DownloadStatus.Downloading(1, TimeSpan.Zero);

        scopedLogger.LogInformation($"Prepared update package at {UpdatePkg}");
        updateStatus.CurrentStep = UpdateStatus.PendingRestart;
        return true;
    }

    private async Task<bool> DownloadUpdateInternalLegacy(Version version, UpdateStatus updateStatus)
    {
        updateStatus.CurrentStep = DownloadStatus.InitializingDownload;
        var uri = DownloadUrl.Replace(VersionTag, version.ToString());
        if (await this.downloadService.DownloadFile(uri, TempFile, updateStatus) is false)
        {
            this.logger.LogError("Failed to download update file");
            return false;
        }

        updateStatus.CurrentStep = UpdateStatus.PendingRestart;
        this.logger.LogInformation("Downloaded update file");
        return true;
    }

    private async Task<Version?> GetLatestVersion()
    {
        using var response = await this.httpClient.GetAsync(Url);
        if (response.IsSuccessStatusCode)
        {
            var versionTag = response.RequestMessage!.RequestUri!.ToString().Split('/').Last().TrimStart('v');
            if (Version.TryParse(versionTag, out var parsedVersion))
            {
                return parsedVersion;
            }

            return default;
        }

        return default;
    }

    private void LaunchExtractor()
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = InstallerFileName
            }
        };
        this.logger.LogInformation("Launching installer");
        if (process.Start() is false)
        {
            throw new InvalidOperationException("Failed to launch installer");
        }
    }

    private async void ExecutePostUpdateActions()
    {
        this.logger.LogInformation("Executing post-update actions");
        foreach(var action in this.postUpdateActionProvider.GetPostUpdateActions())
        {
            this.logger.LogInformation($"Starting [{action.GetType().Name}]");
            action.DoPostUpdateAction();
            await action.DoPostUpdateActionAsync();
            this.logger.LogInformation($"Finished [{action.GetType().Name}]");
        }
    }

    private void MarkUpdateInRegistry()
    {
        this.registryService.SaveValue(UpdatedKey, true);
    }

    private void UnmarkUpdateInRegistry()
    {
        this.registryService.DeleteValue(UpdatedKey);
    }

    private bool UpdateMarkedInRegistry()
    {
        if (this.registryService.TryGetValue<bool>(UpdatedKey, out _))
        {
            return true;
        }

        return false;
    }
}
