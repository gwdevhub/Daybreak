using Daybreak.Shared.Models.Github;

namespace Daybreak.Shared.Services.Github;

/// <summary>
/// Shared GitHub API client for retrieving version tags, releases, and changelogs.
/// Provides a common interface for all services that interact with GitHub repositories.
/// </summary>
public interface IGithubClient
{
    /// <summary>
    /// Gets all version tags from a GitHub repository's refs/tags endpoint.
    /// </summary>
    Task<IEnumerable<GithubRefTag>> GetRefTags(string owner, string repo, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all releases from a GitHub repository.
    /// </summary>
    Task<IEnumerable<GithubRelease>> GetReleases(string owner, string repo, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the latest version by following the /releases/latest redirect URL.
    /// </summary>
    Task<Version?> GetLatestVersionFromRedirect(string owner, string repo, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads a raw file from a GitHub repository without using the API.
    /// Uses raw.githubusercontent.com which has no rate limiting.
    /// </summary>
    /// <param name="owner">Repository owner</param>
    /// <param name="repo">Repository name</param>
    /// <param name="branch">Branch name (e.g., "main")</param>
    /// <param name="filePath">Path to the file in the repository</param>
    /// <param name="destinationPath">Local path to save the file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if download succeeded, false otherwise</returns>
    Task<bool> DownloadRawFile(string owner, string repo, string branch, string filePath, string destinationPath, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the latest commit SHA for a specific file path from a GitHub repository.
    /// Uses the Atom feed to avoid API rate limiting.
    /// </summary>
    /// <param name="owner">Repository owner</param>
    /// <param name="repo">Repository name</param>
    /// <param name="branch">Branch name (e.g., "main")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The commit SHA or null if failed</returns>
    Task<string?> GetLatestCommitSha(string owner, string repo, string branch, CancellationToken cancellationToken);
}
