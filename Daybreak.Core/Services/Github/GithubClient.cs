using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Services.Github;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Net.Http.Json;

namespace Daybreak.Services.Github;

internal sealed class GithubClient(
    IHttpClient<GithubClient> httpClient,
    ILogger<GithubClient> logger) : IGithubClient
{
    private const string RefTagsUrlTemplate = "https://api.github.com/repos/{0}/{1}/git/refs/tags";
    private const string ReleasesUrlTemplate = "https://api.github.com/repos/{0}/{1}/releases";
    private const string LatestReleaseUrlTemplate = "https://github.com/{0}/{1}/releases/latest";

    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);

    private readonly IHttpClient<GithubClient> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<GithubClient> logger = logger.ThrowIfNull();

    public async Task<IEnumerable<GithubRefTag>> GetRefTags(string owner, string repo, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(RefTagsUrlTemplate, owner, repo);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(RequestTimeout);

            scopedLogger.LogDebug("Retrieving ref tags from {Url}", url);
            var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to retrieve ref tags. Status code: {StatusCode}", response.StatusCode);
                return [];
            }

            var tags = await response.Content.ReadFromJsonAsync<List<GithubRefTag>>(timeoutCts.Token);
            return tags ?? [];
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Request to {Url} timed out", url);
            return [];
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve ref tags from {Url}", url);
            return [];
        }
    }

    public async Task<IEnumerable<GithubRelease>> GetReleases(string owner, string repo, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(ReleasesUrlTemplate, owner, repo);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(RequestTimeout);

            scopedLogger.LogDebug("Retrieving releases from {Url}", url);
            using var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to retrieve releases. Status code: {StatusCode}", response.StatusCode);
                return [];
            }

            var releases = await response.Content.ReadFromJsonAsync<List<GithubRelease>>(timeoutCts.Token);
            return releases ?? [];
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Request to {Url} timed out", url);
            return [];
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve releases from {Url}", url);
            return [];
        }
    }

    public async Task<Version?> GetLatestVersionFromRedirect(string owner, string repo, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(LatestReleaseUrlTemplate, owner, repo);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(RequestTimeout);

            using var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to retrieve latest version. Status code: {StatusCode}", response.StatusCode);
                return default;
            }

            var versionTag = response.RequestMessage?.RequestUri?.ToString().Split('/').Last().TrimStart('v');
            if (Version.TryParse(versionTag, out var parsedVersion))
            {
                return parsedVersion;
            }

            scopedLogger.LogError("Failed to parse version from {VersionTag}", versionTag ?? string.Empty);
            return default;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Request to {Url} timed out", url);
            return default;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to retrieve latest version from {Url}", url);
            return default;
        }
    }
}
