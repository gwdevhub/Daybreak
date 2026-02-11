namespace Daybreak.Shared.Services.TarGz;

/// <summary>
/// Interface for extracting .tar.gz archives in a cross-platform manner.
/// </summary>
public interface ITarGzExtractor
{
    /// <summary>
    /// Extracts a .tar.gz archive to the specified directory.
    /// </summary>
    /// <param name="sourceFile">Path to the .tar.gz file to extract.</param>
    /// <param name="destinationDirectory">Directory to extract files to.</param>
    /// <param name="progressTracker">Callback for progress updates (progress 0-1, current file name).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if extraction was successful, false otherwise.</returns>
    Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken);
}
