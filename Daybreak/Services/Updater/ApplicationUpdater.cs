using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Services.Updater.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Data;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.Updater;

internal sealed class ApplicationUpdater(
    INotificationService notificationService,
    IRegistryService registryService,
    IDownloadService downloadService,
    IPostUpdateActionProvider postUpdateActionProvider,
    ILiveOptions<LauncherOptions> liveOptions,
    IHttpClient<ApplicationUpdater> httpClient,
    ILogger<ApplicationUpdater> logger) : IApplicationUpdater, IApplicationLifetimeService
{
    private const string UpdatePkgSubPath = "update.pkg";
    private const string TempInstallerFileNameSubPath = "Daybreak.Installer.Temp.exe";
    private const string InstallerFileNameSubPath = "Daybreak.Installer.exe";
    private const string UpdatedKey = "LauncherUpdating";
    private const string TempFileSubPath = "tempfile.zip";
    private const string VersionTag = "{VERSION}";
    private const string FileTag = "{FILE}";
    private const string RefTagPrefix = "/refs/tags/v";
    private const string VersionListUrl = "https://api.github.com/repos/gwdevhub/Daybreak/git/refs/tags";
    private const string Url = "https://github.com/gwdevhub/Daybreak/releases/latest";
    private const string DownloadUrl = $"https://github.com/gwdevhub/Daybreak/releases/download/v{VersionTag}/Daybreakv{VersionTag}.zip";
    private const string BlobStorageUrl = $"https://daybreak.blob.core.windows.net/v{VersionTag}/{FileTag}";
    private const int DownloadParallelTasks = 10;

    private readonly static string TempInstallerFileName = PathUtils.GetAbsolutePathFromRoot(TempInstallerFileNameSubPath);
    private readonly static string InstallerFileName = PathUtils.GetAbsolutePathFromRoot(InstallerFileNameSubPath);
    private readonly static string TempFile = PathUtils.GetAbsolutePathFromRoot(TempFileSubPath);
    private readonly static string UpdatePkg = PathUtils.GetAbsolutePathFromRoot(UpdatePkgSubPath);

    private readonly static ProgressUpdate ProgressInitialize = new(0, "Initializing update");
    private readonly static ProgressUpdate ProgressCheckLatest = new(0, "Checking latest version");
    private readonly static ProgressUpdate ProgressFinalize = new(1, "Downloaded update. Please restart Daybreak to finalize the update");
    private static ProgressUpdate ProgressDownload(double progress) => new(progress, "Downloading update");

    private readonly static TimeSpan DownloadInfoUpdateInterval = TimeSpan.FromMilliseconds(16);

    private readonly CancellationTokenSource updateCancellationTokenSource = new();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IRegistryService registryService = registryService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IPostUpdateActionProvider postUpdateActionProvider = postUpdateActionProvider.ThrowIfNull();
    private readonly ILiveOptions<LauncherOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly IHttpClient<ApplicationUpdater> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<ApplicationUpdater> logger = logger.ThrowIfNull();

    public Version CurrentVersion { get; } = ProjectConfiguration.CurrentVersion;

    public IProgressAsyncOperation<bool> DownloadUpdate(Version version, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        if (!this.liveOptions.Value.BetaUpdate)
        {
            return ProgressAsyncOperation.Create(async progress =>
            {
                return await Task.Factory.StartNew(() => this.DownloadUpdateInternalLegacy(version, progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
            }, cancellationToken);
        }

        return ProgressAsyncOperation.Create(async progress =>
        {
            try
            {
                progress.Report(ProgressInitialize);
                var maybeMetadataResponse = await this.httpClient.GetAsync(
                BlobStorageUrl
                .Replace(VersionTag, version.ToString().Replace(".", "-"))
                .Replace(FileTag, "Metadata.json"));
                if (maybeMetadataResponse.IsSuccessStatusCode)
                {
                    var metaData = await maybeMetadataResponse.Content.ReadFromJsonAsync<List<Metadata>>();
                    if (metaData is not null)
                    {
                        return await Task.Factory.StartNew(() => this.DownloadUpdateInternalBlob(metaData, version, progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
                    }
                }

                return await Task.Factory.StartNew(() => this.DownloadUpdateInternalLegacy(version, progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "Failed to download update for version {version}", version);
                return false;
            }
        }, cancellationToken);
    }

    public IProgressAsyncOperation<bool> DownloadLatestUpdate(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            progress.Report(ProgressCheckLatest);
            var latestVersion = await this.GetLatestVersion(cancellationToken);
            if (latestVersion is null)
            {
                this.logger.LogWarning("Failed to retrieve latest version. Aborting update");
                return false;
            }

            return await this.DownloadUpdate(latestVersion, cancellationToken);
        }, cancellationToken);
    }

    public async Task<bool> UpdateAvailable(CancellationToken cancellationToken)
    {
        var maybeLatestVersion = await this.GetLatestVersion(cancellationToken);
        if (maybeLatestVersion is null)
        {
            this.logger.LogWarning("Failed to retrieve latest version");
            return false;
        }

        return this.CurrentVersion.CompareTo(maybeLatestVersion) < 0;
    }

    public async Task<IEnumerable<Version>> GetVersions(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug($"Retrieving version list from {VersionListUrl}");
        try
        {
            var response = await this.httpClient.GetAsync(VersionListUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var serializedList = await response.Content.ReadAsStringAsync(cancellationToken);
                var versionList = serializedList.Deserialize<GithubRefTag[]>();
                return versionList!.Select(v => v.Ref![(RefTagPrefix.Length - 1)..])
                    .Select(v =>
                    {
                        var result = Version.TryParse(v, out var version);
                        return (result, version);
                    })
                    .Where(v => v.result)
                    .Select(v => v.version ?? throw new InvalidOperationException("Parsed version cannot be null"));
            }

            scopedLogger.LogError("Failed to retrieve version list. Status code: {statusCode}", response.StatusCode);
            return [];
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve version list from {url}", VersionListUrl);
            return [];
        }
    }

    public async Task<string?> GetChangelog(Version version, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        try
        {
            var changeLogResponse = await this.httpClient.GetAsync(
            BlobStorageUrl
                .Replace(VersionTag, version.ToString().Replace(".", "-"))
                .Replace(FileTag, "changelog.txt"), cancellationToken);

            if (!changeLogResponse.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to retrieve changelog for version {version}. Status code: {statusCode}", version, changeLogResponse.StatusCode);
                return default;
            }

            scopedLogger.LogDebug("Retrieved changelog for version {version}", version);
            return await changeLogResponse.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve changelog for version {version}", version);
            return default;
        }
    }

    public void PeriodicallyCheckForUpdates()
    {
        System.Extensions.TaskExtensions.RunPeriodicAsync(async () =>
        {
            if (this.liveOptions.Value.AutoCheckUpdate is false)
            {
                return;
            }

            if (await this.UpdateAvailable(CancellationToken.None))
            {
                var maybeLatestVersion = await this.GetLatestVersion(CancellationToken.None);
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

    private async Task<bool> DownloadUpdateInternalBlob(List<Metadata> metadata, Version version, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        progress.Report(ProgressInitialize);

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
                    var response = await this.httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
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
                if (fileDownloadResult is false ||
                    file is null ||
                    file.Name is null ||
                    file.RelativePath is null)
                {
                    scopedLogger.LogError($"{file?.RelativePath ?? string.Empty} failed to download. Cancelling update");
                    return false;
                }

                var fileNameBytes = Encoding.UTF8.GetBytes(file.Name);
                var relativePathBytes = Encoding.UTF8.GetBytes(file.RelativePath);
                await packageStream.WriteAsync(BitConverter.GetBytes(fileNameBytes.Length), cancellationToken);
                await packageStream.WriteAsync(fileNameBytes, cancellationToken);
                await packageStream.WriteAsync(BitConverter.GetBytes(relativePathBytes.Length), cancellationToken);
                await packageStream.WriteAsync(relativePathBytes, cancellationToken);
                await packageStream.WriteAsync(BitConverter.GetBytes(file.Size), cancellationToken);

                var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var maxMeasurements = 100;
                var fileSize = file.Size;
                while (fileSize > 0)
                {
                    var readBytes = await downloadStream.ReadAsync(downloadBuffer, cancellationToken);
                    await packageStream.WriteAsync(downloadBuffer.AsMemory(0, readBytes), cancellationToken);

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
                        // var etaMillis = averageSpeed > 0 ? remainingBytes / averageSpeed : 0;
                        // var eta = TimeSpan.FromMilliseconds(etaMillis);
                        progress.Report(ProgressDownload(downloaded / sizeToDownload));
                    }
                }
            }
        }

        scopedLogger.LogDebug("Prepared update package at {updatePkg}", UpdatePkg);
        progress.Report(ProgressFinalize);
        return true;
    }

    private async Task<bool> DownloadUpdateInternalLegacy(Version version, IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        progress.Report(ProgressInitialize);
        var uri = DownloadUrl.Replace(VersionTag, version.ToString());
        if (await this.downloadService.DownloadFile(uri, TempFile, progress, cancellationToken) is false)
        {
            scopedLogger.LogError("Failed to download update file");
            return false;
        }

        progress.Report(ProgressFinalize);
        scopedLogger.LogDebug("Downloaded update file");
        return true;
    }

    private async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            using var response = await this.httpClient.GetAsync(Url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var versionTag = response.RequestMessage?.RequestUri?.ToString().Split('/').Last().TrimStart('v');
                if (Version.TryParse(versionTag, out var parsedVersion))
                {
                    return parsedVersion;
                }

                scopedLogger.LogError("Failed to parse version from {versionTag}", versionTag ?? string.Empty);
                return default;
            }

            scopedLogger.LogError("Failed to retrieve latest version. Status code: {statusCode}", response.StatusCode);
            return default;
        }
        catch(Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve latest version from {url}", Url);
            return default;
        }
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
        this.logger.LogDebug("Launching installer");
        if (process.Start() is false)
        {
            throw new InvalidOperationException("Failed to launch installer");
        }
    }

    private async void ExecutePostUpdateActions()
    {
        this.logger.LogDebug("Executing post-update actions");
        foreach(var action in this.postUpdateActionProvider.GetPostUpdateActions())
        {
            this.logger.LogDebug("Starting [{actionName}]", action.GetType().Name);
            action.DoPostUpdateAction();
            await action.DoPostUpdateActionAsync();
            this.logger.LogDebug("Finished [{actionName}]", action.GetType().Name);
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
