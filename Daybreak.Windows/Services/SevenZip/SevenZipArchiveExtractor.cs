using Daybreak.Shared.Services.SevenZip;
using Microsoft.Extensions.Logging;
using SevenZipExtractor;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Windows.Services.SevenZip;

internal sealed class SevenZipArchiveExtractor(
    ILogger<SevenZipArchiveExtractor> logger) : ISevenZipExtractor
{
    private readonly ILogger<SevenZipArchiveExtractor> logger = logger.ThrowIfNull();

    public async Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ExtractToDirectory), sourceFile);

        try
        {
            var root = Path.GetFullPath(destinationDirectory);
            using var archive = new ArchiveFile(sourceFile);
            var total = archive.Entries.Count;
            var processed = 0;
            await Task.Factory.StartNew(() =>
            {
                archive.Extract(entry =>
                {
                    progressTracker((double)processed / total, entry.FileName);
                    processed++;
                    return Path.Combine(root, entry.FileName);
                });
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return true;
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to extract archive {SourceFile} to {DestinationDirectory}", sourceFile, destinationDirectory);
            return false;
        }
    }
}
