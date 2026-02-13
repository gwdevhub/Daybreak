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
    private const string RawFileUrlTemplate = "https://raw.githubusercontent.com/{0}/{1}/{2}/{3}";
    private const string CommitsAtomUrlTemplate = "https://github.com/{0}/{1}/commits/{2}.atom";
    private const string ContentsUrlTemplate = "https://api.github.com/repos/{0}/{1}/contents/{2}?ref={3}";

    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan DownloadTimeout = TimeSpan.FromMinutes(5);

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

    public async Task<bool> DownloadRawFile(string owner, string repo, string branch, string filePath, string destinationPath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(RawFileUrlTemplate, owner, repo, branch, filePath);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(DownloadTimeout);

            scopedLogger.LogDebug("Downloading raw file from {Url}", url);
            using var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to download raw file. Status code: {StatusCode}", response.StatusCode);
                return false;
            }

            // Ensure directory exists
            var directory = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var fileStream = File.Create(destinationPath);
            await response.Content.CopyToAsync(fileStream, timeoutCts.Token);

            scopedLogger.LogDebug("Successfully downloaded {FilePath} to {DestinationPath}", filePath, destinationPath);
            return true;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Download from {Url} timed out", url);
            return false;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to download raw file from {Url}", url);
            return false;
        }
    }

    public async Task<string?> GetLatestCommitSha(string owner, string repo, string branch, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(CommitsAtomUrlTemplate, owner, repo, branch);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(RequestTimeout);

            scopedLogger.LogDebug("Fetching commits atom feed from {Url}", url);
            using var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to fetch commits atom feed. Status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(timeoutCts.Token);
            
            // Parse the Atom feed to extract the latest commit SHA from the first <id> tag
            // Format: tag:github.com,2008:Grit::Commit/{sha}
            var idStartIndex = content.IndexOf("<id>tag:github.com,2008:Grit::Commit/", StringComparison.Ordinal);
            if (idStartIndex < 0)
            {
                scopedLogger.LogWarning("Could not find commit ID in atom feed");
                return null;
            }

            var shaStart = idStartIndex + "<id>tag:github.com,2008:Grit::Commit/".Length;
            var shaEnd = content.IndexOf("</id>", shaStart, StringComparison.Ordinal);
            if (shaEnd < 0)
            {
                scopedLogger.LogWarning("Could not parse commit SHA from atom feed");
                return null;
            }

            var sha = content[shaStart..shaEnd];
            scopedLogger.LogDebug("Latest commit SHA: {Sha}", sha);
            return sha;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Request to {Url} timed out", url);
            return null;
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to fetch commits from {Url}", url);
            return null;
        }
    }

    public async Task<IEnumerable<string>> GetDirectoryContents(string owner, string repo, string path, string branch, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var url = string.Format(ContentsUrlTemplate, owner, repo, path, branch);
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(RequestTimeout);

            scopedLogger.LogDebug("Fetching directory contents from {Url}", url);
            using var response = await this.httpClient.GetAsync(url, timeoutCts.Token);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError("Failed to fetch directory contents. Status code: {StatusCode}", response.StatusCode);
                return [];
            }

            var items = await response.Content.ReadFromJsonAsync<List<GithubContentItem>>(timeoutCts.Token);
            if (items is null)
            {
                scopedLogger.LogWarning("Failed to deserialize directory contents");
                return [];
            }

            var files = items
                .Where(item => item.IsFile && !string.IsNullOrEmpty(item.Name))
                .Select(item => item.Name!)
                .ToList();

            foreach (var fileName in files)
            {
                scopedLogger.LogDebug("Found file: {FileName}", fileName);
            }

            return files;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            scopedLogger.LogWarning("Request to {Url} timed out", url);
            return [];
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to fetch directory contents from {Url}", url);
            return [];
        }
    }
}
