namespace Daybreak.Shared.Services.Shell;

/// <summary>
/// Opens URLs and file-system paths using the operating system's default handler.
/// Provides a cross-platform abstraction over platform-specific shell invocations
/// (e.g. <c>explorer.exe</c> on Windows, <c>xdg-open</c> on Linux).
/// </summary>
public interface IShellExecutor
{
    /// <summary>
    /// Opens the given URL using the default web browser.
    /// </summary>
    /// <param name="url">The absolute URL to open.</param>
    void OpenUrl(string url);

    /// <summary>
    /// Opens the given file or folder using the default file manager or associated program.
    /// </summary>
    /// <param name="path">The file-system path to open.</param>
    void OpenPath(string path);
}
