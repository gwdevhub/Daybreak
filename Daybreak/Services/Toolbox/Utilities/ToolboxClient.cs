﻿using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Toolbox.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Version = Daybreak.Models.Versioning.Version;

namespace Daybreak.Services.Toolbox.Utilities;
internal sealed class ToolboxClient : IToolboxClient
{
    private const string DllName = "GWToolboxdll.dll";
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gwdevhub/GWToolboxpp/releases/download/[TAG_PLACEHOLDER]/GWToolboxdll.dll";
    private const string ReleasesUrl = "https://api.github.com/repos/gwdevhub/GWToolboxpp/git/refs/tags";

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

    public async Task<DownloadLatestOperation> DownloadLatestDll(ToolboxInstallationStatus toolboxInstallationStatus, string destinationFolder, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            return await this.DownloadLatestVersion(toolboxInstallationStatus, destinationFolder, cancellationToken);
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

    private async Task<DownloadLatestOperation> DownloadLatestVersion(ToolboxInstallationStatus toolboxInstallationStatus, string destinationFolderPath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        (_, var tag) = await this.GetLatestVersionTag(cancellationToken);
        if (tag is null)
        {
            return new DownloadLatestOperation.NoVersionFound();
        }

        scopedLogger.LogInformation($"Retrieving version {tag}");
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, tag.ToString());
        var destinationFolder = Path.GetFullPath(destinationFolderPath);
        var destinationPath = Path.Combine(destinationFolder, DllName);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, toolboxInstallationStatus, cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download GWToolboxdll version {tag}");
        }
        
        return new DownloadLatestOperation.Success(destinationPath);
    }

    private async Task<(Version? Version, string Literal)> GetLatestVersionTag(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return default;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync();
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?.Where(t => t.Ref?.Contains("Release") is true)
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault(); // Replace _Release with -Release for the Version parser
        if (latestRelease?.Replace('_', '-').ToLowerInvariant() is not string tag)
        {
            scopedLogger.LogError("Could not parse version list. No latest version found");
            return default;
        }

        if (!Version.TryParse(tag, out var toolboxVersion))
        {
            scopedLogger.LogError("Could not parse version. No latest version found");
        }

        return (toolboxVersion, latestRelease);
    }
}
