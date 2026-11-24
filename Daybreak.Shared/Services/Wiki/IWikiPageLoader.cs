namespace Daybreak.Shared.Services.Wiki;

/// <summary>
/// Loads Guild Wars wiki pages so they can be shown inside Daybreak.
/// The wiki refuses to be framed (X-Frame-Options: SAMEORIGIN), so pages are fetched
/// and loaded into the iframe as documents instead of being navigated to directly.
/// </summary>
public interface IWikiPageLoader
{
    /// <summary>
    /// Returns true if <paramref name="url"/> points at the Guild Wars wiki.
    /// </summary>
    bool IsWikiUrl(Uri url);

    /// <summary>
    /// Fetches the page at <paramref name="url"/> and returns a document ready to be loaded
    /// into an iframe, or null if the page could not be loaded.
    /// </summary>
    Task<string?> LoadPage(Uri url, CancellationToken cancellationToken);
}
