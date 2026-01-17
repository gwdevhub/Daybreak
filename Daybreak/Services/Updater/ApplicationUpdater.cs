using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Data;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.Net.Http.Json;

namespace Daybreak.Services.Updater;

internal sealed class ApplicationUpdater(
    INotificationService notificationService,
    IRegistryService registryService,
    IDownloadService downloadService,
    IEnumerable<PostUpdateActionBase> postUpdateActions,
    IOptionsMonitor<LauncherOptions> liveOptions,
    IHttpClient<ApplicationUpdater> httpClient,
    ILogger<ApplicationUpdater> logger) : IApplicationUpdater, IHostedService
{
    private const string UpdatePkgSubPath = "update.pkg";
    private const string TempInstallerFileNameSubPath = "Installer/Daybreak.Installer.Temp.exe";
    private const string InstallerFileNameSubPath = "Installer/Daybreak.Installer.exe";
    private const string UpdatedKey = "LauncherUpdating";
    private const string TempFileSubPath = "tempfile.zip";
    private const string VersionTag = "{VERSION}";
    private const string RefTagPrefix = "/refs/tags/v";
    private const string VersionListUrl = "https://api.github.com/repos/gwdevhub/Daybreak/git/refs/tags";
    private const string Url = "https://github.com/gwdevhub/Daybreak/releases/latest";
    private const string DownloadUrl = $"https://github.com/gwdevhub/Daybreak/releases/download/v{VersionTag}/Daybreakv{VersionTag}.zip";
    private const string GithubReleasesUrl = $"https://api.github.com/repos/gwdevhub/daybreak/releases";

    private readonly static string TempInstallerFileName = PathUtils.GetAbsolutePathFromRoot(TempInstallerFileNameSubPath);
    private readonly static string InstallerFileName = PathUtils.GetAbsolutePathFromRoot(InstallerFileNameSubPath);
    private readonly static string TempFile = PathUtils.GetAbsolutePathFromRoot(TempFileSubPath);
    private readonly static string UpdatePkg = PathUtils.GetAbsolutePathFromRoot(UpdatePkgSubPath);

    private readonly static ProgressUpdate ProgressInitialize = new(0, "Initializing update");
    private readonly static ProgressUpdate ProgressCheckLatest = new(0, "Checking latest version");
    private readonly static ProgressUpdate ProgressFinalize = new(1, "Downloaded update. Please close any running Guild Wars instances and restart Daybreak to finalize the update");
    private static ProgressUpdate ProgressDownload(double progress) => new(progress, "Downloading update");

    private readonly static TimeSpan DownloadInfoUpdateInterval = TimeSpan.FromMilliseconds(16);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IRegistryService registryService = registryService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IEnumerable<PostUpdateActionBase> postUpdateActions = postUpdateActions.ThrowIfNull();
    private readonly IOptionsMonitor<LauncherOptions> liveOptions = liveOptions.ThrowIfNull();
    private readonly IHttpClient<ApplicationUpdater> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<ApplicationUpdater> logger = logger.ThrowIfNull();

    public Version CurrentVersion { get; } = ProjectConfiguration.CurrentVersion;

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        if (this.UpdateMarkedInRegistry())
        {
            this.UnmarkUpdateInRegistry();
            await this.ExecutePostUpdateActions(cancellationToken);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (this.liveOptions.CurrentValue.AutoCheckUpdate is false)
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
            }
            finally
            {
                await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken);
            }
        }
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IProgressAsyncOperation<bool> DownloadUpdate(Version version, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await Task.Factory.StartNew(() => this.DownloadUpdateInternalLegacy(version, progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
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
                var versionList = await response.Content.ReadFromJsonAsync<GithubRefTag[]>(cancellationToken);
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
            var githubChangeLogResponse = await this.GetChangeLogFromGithub(version, cancellationToken);
            if (githubChangeLogResponse is null)
            {
                scopedLogger.LogError("Failed to retrieve changelog from github for version {version}", version);
                return default;
            }

            scopedLogger.LogDebug("Retrieved changelog from github for version {version}", version);
            return githubChangeLogResponse;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve changelog for version {version}", version);
            return default;
        }
    }

    public void FinalizeUpdate()
    {
        this.MarkUpdateInRegistry();
        this.LaunchExtractor();
    }

    private async Task<string?> GetChangeLogFromGithub(Version version, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(flowIdentifier: version.ToString());
        try
        {
            using var response = await this.httpClient.GetAsync(GithubReleasesUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to retrieve releases list from github for version. Status code: {statusCode}", response.StatusCode);
                return default;
            }

            var releases = await response.Content.ReadFromJsonAsync<List<GithubRelease>>(cancellationToken);
            var release = releases?.FirstOrDefault(r => r.TagName == $"v{version}");
            if (release is null)
            {
                scopedLogger.LogError("Failed to find release info for version {version} on github", version);
                return default;
            }

            return release.Body.Replace("<br />", $"<br />{Environment.NewLine}");
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve changelog from github for version {version}", version);
            return default;
        }
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
                FileName = InstallerFileName,
                Arguments = $"update \"{PathUtils.GetRootFolder()}\""
            }
        };
        this.logger.LogDebug("Launching installer");
        if (process.Start() is false)
        {
            throw new InvalidOperationException("Failed to launch installer");
        }
    }

    private async Task ExecutePostUpdateActions(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        this.logger.LogDebug("Executing post-update actions");
        foreach(var action in this.postUpdateActions)
        {
            scopedLogger.LogDebug("Starting [{actionName}]", action.GetType().Name);
            action.DoPostUpdateAction();
            await action.DoPostUpdateActionAsync(cancellationToken);
            scopedLogger.LogDebug("Finished [{actionName}]", action.GetType().Name);
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
