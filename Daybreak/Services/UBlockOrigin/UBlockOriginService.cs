using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Services.BrowserExtensions;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Core.Extensions;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Linq;
using Daybreak.Services.Downloads;
using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Daybreak.Services.Notifications;

namespace Daybreak.Services.UBlockOrigin;
public sealed class UBlockOriginService : IBrowserExtension
{
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gorhill/uBlock/releases/download/[TAG_PLACEHOLDER]/uBlock0_[TAG_PLACEHOLDER].chromium.zip";
    private const string ReleasesUrl = "https://api.github.com/repos/gorhill/uBlock/git/refs/tags";
    private const string InstallationPath = "BrowserExtensions";
    private const string ZipName = "ublock.chromium.zip";
    private const string InstallationFolderName = "uBlock0.chromium";

    private static readonly SemaphoreSlim SemaphoreSlim = new(1);

    private static volatile bool VersionUpToDate;

    private readonly INotificationService notificationService;
    private readonly IDownloadService downloadService;
    private readonly IHttpClient<UBlockOriginService> httpClient;
    private readonly ILogger<UBlockOriginService> logger;

    public UBlockOriginService(
        INotificationService notificationService,
        IDownloadService downloadService,
        IHttpClient<UBlockOriginService> httpClient,
        ILogger<UBlockOriginService> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public string ExtensionId { get; } = "uBlock-Origin";

    public async Task CheckAndUpdate(string browserVersion)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdate), string.Empty);
        await SemaphoreSlim.WaitAsync();
        try
        {
            await this.CheckAndUpdateInternal(browserVersion);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
        }

        SemaphoreSlim.Release();
    }

    public Task<string> GetExtensionPath()
    {
        return Task.FromResult(Path.GetFullPath(Path.Combine(InstallationPath, InstallationFolderName)));
    }

    private async Task CheckAndUpdateInternal(string browserVersion)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdateInternal), string.Empty);
        if (VersionUpToDate)
        {
            return;
        }

        using var cancellationTokenSource = new CancellationTokenSource(5000);
        var currentVersionString = await this.GetCurrentVersion(cancellationTokenSource.Token);
        var latestVersionString = await this.GetLatestVersion(cancellationTokenSource.Token);
        if (latestVersionString is null)
        {
            scopedLogger.LogError("Failed to retrieve latest uBlock-Origin version");
            return;
        }

        if (currentVersionString is not null &&
            Models.Versioning.Version.TryParse(currentVersionString, out var currentVersion) &&
            Models.Versioning.Version.TryParse(latestVersionString, out var latestVersion) &&
            currentVersion.CompareTo(latestVersion) >= 0)
        {
            scopedLogger.LogInformation("uBlock-Origin is up to date");
            return;
        }

        this.notificationService.NotifyInformation(
            title:"Updating uBlock-Origin",
            description: "Updating uBlock-Origin browser extension");
        var zipFilePath = await this.DownloadVersion(latestVersionString, CancellationToken.None);
        if (zipFilePath is null)
        {
            scopedLogger.LogError("Failed to retrieve latest uBlock-Origin");
            this.notificationService.NotifyInformation(
            title: "Failed to update uBlock-Origin",
            description: "Failed to update uBlock-Origin");
            return;
        }

        using var zipFile = ZipFile.Read(zipFilePath);
        zipFile.ExtractAll(Path.GetFullPath(InstallationPath), ExtractExistingFileAction.OverwriteSilently);
        zipFile.Dispose();
        File.Delete(zipFilePath);
        VersionUpToDate = true;
        this.notificationService.NotifyInformation(
            title: "Updated uBlock-Origin",
            description: "Updated uBlock-Origin browser extension");
    }

    private async Task<string?> DownloadVersion(string version, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestVersion), version);
        scopedLogger.LogInformation($"Retrieving version {version}");
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, version);
        var destinationFolder = Path.GetFullPath(InstallationPath);
        var destinationPath = Path.Combine(destinationFolder, ZipName);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, new UpdateStatus(), cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download uMod version {version}");
        }

        return destinationPath;
    }

    private async Task<string?> GetCurrentVersion(CancellationToken cancellationToken)
    {
        var manifestFilePath = Path.GetFullPath(Path.Combine(InstallationPath, Path.Combine(InstallationFolderName, "manifest.json")));
        var fileInfo = new FileInfo(manifestFilePath);
        if (!fileInfo.Exists)
        {
            return default;
        }

        var manifest = JsonConvert.DeserializeObject<JObject>(await File.ReadAllTextAsync(fileInfo.FullName, cancellationToken));
        if (manifest?.TryGetValue("version", out var token) is not true ||
            token is not JValue tokenValue ||
            tokenValue.Value is not string value)
        {
            return default;
        }

        return value;
    }

    private async Task<string?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestVersion), string.Empty);
        scopedLogger.LogInformation("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return default;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync();
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .Where(v => !v.Contains("b") && !v.Contains("rc"))
            .LastOrDefault();
        if (latestRelease is not string tag)
        {
            scopedLogger.LogError("Could not parse version list. No latest version found");
            return default;
        }

        return latestRelease;
    }
}
