using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Services.Downloads;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Net.Http;

namespace Daybreak.Services.Toolbox.Utilities;
internal sealed class ToolboxClient(
    IDownloadService downloadService,
    IHttpClient<ToolboxClient> httpClient,
    ILogger<ToolboxClient> logger) : IToolboxClient
{
    private const string DllName = "GWToolboxdll.dll";
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gwdevhub/GWToolboxpp/releases/download/[TAG_PLACEHOLDER]/GWToolboxdll.dll";
    private const string ReleasesUrl = "https://api.github.com/repos/gwdevhub/GWToolboxpp/git/refs/tags";

    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IHttpClient<ToolboxClient> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<ToolboxClient> logger = logger.ThrowIfNull();

    public async Task<DownloadLatestOperation> DownloadLatestDll(IProgress<ProgressUpdate> progress, string destinationFolder, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            return await this.DownloadLatestVersion(progress, destinationFolder, cancellationToken);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return new DownloadLatestOperation.ExceptionEncountered(ex);
        }
    }

    public async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            (var latest, _) = await this.GetLatestVersionTag(cancellationToken);
            if (latest is null)
            {
                scopedLogger.LogError("Failed to fetch latest version");
                return default;
            }

            return latest;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Encountered exception");
            return default;
        }
    }

    private async Task<DownloadLatestOperation> DownloadLatestVersion(IProgress<ProgressUpdate> progress, string destinationFolderPath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        (_, var tag) = await this.GetLatestVersionTag(cancellationToken);
        if (tag is null)
        {
            return new DownloadLatestOperation.NoVersionFound();
        }

        scopedLogger.LogDebug("Retrieving version {tag}", tag);
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, tag.ToString());
        var destinationFolder = Path.GetFullPath(destinationFolderPath);
        var destinationPath = Path.Combine(destinationFolder, DllName);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, progress, cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download GWToolboxdll version {tag}");
        }
        
        return new DownloadLatestOperation.Success(destinationPath);
    }

    private async Task<(Version? Version, string Literal)> GetLatestVersionTag(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return default;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync(cancellationToken);
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestReleaseTuple = releasesList?.Where(t => t.Ref?.Contains("Release") is true)
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .Select(t => (t, t.Replace("_Release", "")))
            .Select(t =>
            {
                var parseResult = Version.TryParse(t.Item2, out var parsedVersion);
                return (parseResult, parsedVersion, t.t);
            })
            .Where(t => t.parseResult)
            .OrderByDescending(t => t.parsedVersion)
            .FirstOrDefault();

        if (latestReleaseTuple is null)
        {
            scopedLogger.LogError("No valid releases found");
            return default;
        }

        var (parseResult, latestReleaseVersion, latestReleaseTag) = latestReleaseTuple.Value;
        return (latestReleaseVersion, latestReleaseTag);
    }
}
