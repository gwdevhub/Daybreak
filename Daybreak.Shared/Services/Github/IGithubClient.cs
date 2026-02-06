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
}
