using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Toolbox.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox.Utilities;
internal sealed class ToolboxClient : IToolboxClient
{
    private const string ToolboxDirectory = "GWToolbox";
    private const string DllName = "GWToolboxdll.dll";
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/HasKha/GWToolboxpp/releases/download/[TAG_PLACEHOLDER]/GWToolboxdll.dll";
    private const string ReleasesUrl = "https://api.github.com/repos/HasKha/GWToolboxpp/git/refs/tags";

    private readonly IDownloadService downloadService;
    private readonly IHttpClient<ToolboxClient> httpClient;
    private readonly ILogger<ToolboxClient> logger;

    public ToolboxClient(
        IDownloadService downloadService,
        IHttpClient<ToolboxClient> httpClient,
        ILogger<ToolboxClient> logger)
    {
        this.downloadService = downloadService.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, CancellationToken cancellationToken)
    {
        try
        {
            return await this.GetLatestVersion(toolboxInstallationStatus, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Encountered exception");
            return new DownloadLatestOperation.ExceptionEncountered(ex);
        }
    }

    private async Task<DownloadLatestOperation> GetLatestVersion(ToolboxInstallationStatus toolboxInstallationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestVersion), string.Empty);
        scopedLogger.LogInformation("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return new DownloadLatestOperation.NonSuccessStatusCode((int)getListResponse.StatusCode);
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync();
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?.Where(t => t.Ref?.Contains("Release") is true)
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault();
        if (latestRelease is not string tag)
        {
            scopedLogger.LogError("Could not parse version list. No latest version found");
            return new DownloadLatestOperation.NoVersionFound();
        }

        scopedLogger.LogInformation($"Retrieving version {tag}");
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, tag);
        var destinationFolder = Path.GetFullPath(ToolboxDirectory);
        var destinationPath = Path.Combine(destinationFolder, DllName);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, toolboxInstallationStatus);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download GWToolboxdll version {tag}");
        }
        
        return new DownloadLatestOperation.Success(destinationPath);
    }
}
