using Daybreak.Shared.Services.TarGz;
using Microsoft.Extensions.Logging;
using SharpCompress.Common;
using SharpCompress.Readers;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.TarGz;

/// <summary>
/// Cross-platform implementation of ITarGzExtractor using SharpCompress library.
/// Supports .tar.gz, .tgz, and other archive formats supported by SharpCompress.
/// </summary>
internal sealed class TarGzExtractor(
    ILogger<TarGzExtractor> logger) : ITarGzExtractor
{
    private readonly ILogger<TarGzExtractor> logger = logger.ThrowIfNull();

    public async Task<bool> ExtractToDirectory(
        string sourceFile,
        string destinationDirectory,
        Action<double, string> progressTracker,
        CancellationToken cancellationToken)
    {
        return await Task.Run(() => this.ExtractToDirectoryInternal(sourceFile, destinationDirectory, progressTracker, cancellationToken), cancellationToken);
    }

    private bool ExtractToDirectoryInternal(
        string sourceFile,
        string destinationDirectory,
        Action<double, string> progressTracker,
        CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            if (!File.Exists(sourceFile))
            {
                scopedLogger.LogError("Source file does not exist: {SourceFile}", sourceFile);
                return false;
            }

            Directory.CreateDirectory(destinationDirectory);

            // Use ReaderFactory which properly handles nested archives like .tar.gz
            using var stream = File.OpenRead(sourceFile);
            using var reader = ReaderFactory.OpenReader(stream, new ReaderOptions { ExtractFullPath = true, Overwrite = true });

            var processedEntries = 0;

            while (reader.MoveToNextEntry())
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entry = reader.Entry;
                if (entry.IsDirectory)
                {
                    continue;
                }

                var entryKey = entry.Key;
                if (string.IsNullOrEmpty(entryKey))
                {
                    continue;
                }

                // Report progress (we don't know total count upfront with streaming reader)
                progressTracker(0.5, entryKey);

                // Extract the entry
                reader.WriteEntryToDirectory(destinationDirectory);

                processedEntries++;
            }

            // Final progress update
            progressTracker(1.0, "Extraction complete");

            scopedLogger.LogInformation(
                "Successfully extracted {EntryCount} files from {SourceFile} to {DestinationDirectory}",
                processedEntries, sourceFile, destinationDirectory);

            return true;
        }
        catch (OperationCanceledException)
        {
            scopedLogger.LogInformation("Archive extraction was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to extract archive {SourceFile} to {DestinationDirectory}", sourceFile, destinationDirectory);
            return false;
        }
    }
}
