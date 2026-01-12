using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Microsoft.Extensions.Logging;
using Photino.NET;
using System.Core.Extensions;

namespace Daybreak.Services.Guildwars;

internal sealed class GuildWarsCopyService(
    PhotinoWindow photinoWindow,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    ILogger<GuildWarsCopyService> logger) : IGuildWarsCopyService
{
    private const string ExecutableName = "Gw.exe";

    private readonly static ProgressUpdate ProgressInitializing = new(0, "Initializing copy");
    private readonly static ProgressUpdate ProgressCompleted = new(1, "Copy completed");
    private readonly static ProgressUpdate ProgressFailed = new(1, "Copy failed");
    private readonly static ProgressUpdate ProgressCancelled = new(1, "Copy cancelled");
    private static ProgressUpdate ProgressCopying(double progress) => new(progress, "Copying files");

    private readonly static string[] FilesToCopy =
    [
        "Gw.dat",
        "Gw.exe"
    ];

    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly ILogger<GuildWarsCopyService> logger = logger.ThrowIfNull();

    public ProgressAsyncOperation<bool> CopyGuildwars(string existingExecutable, CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            progress.Report(ProgressInitializing);
            var sourceFolder = new FileInfo(existingExecutable).Directory;
            if (sourceFolder?.Exists is not true)
            {
                this.logger.LogError($"Source folder does not exist [{sourceFolder?.FullName}]");
                progress.Report(ProgressFailed);
                return false;
            }

            var totalBytesToCopy = 0d;
            foreach (var file in FilesToCopy)
            {
                var completePath = Path.Combine(sourceFolder.FullName, file);
                if (!File.Exists(completePath))
                {
                    this.logger.LogError($"Source folder does not contain required file [{file}]");
                    progress.Report(ProgressFailed);
                    return false;
                }

                var fi = new FileInfo(completePath);
                totalBytesToCopy += fi.Length;
            }

            if ((await this.photinoWindow.ShowOpenFolderAsync("Select Destination Folder")).FirstOrDefault() is not string destinationPath)
            {
                this.logger.LogDebug("Copy cancelled. Destination path selection cancelled");
                progress.Report(ProgressCancelled);
                return false;
            }

            var destinationFolder = new DirectoryInfo(destinationPath);
            Directory.CreateDirectory(destinationFolder.FullName);
            var buffer = new Memory<byte>(new byte[16777216]);
            var totalBytesCopied = 0d;
            foreach (var sourceFile in FilesToCopy)
            {
                var completeSourcePath = Path.Combine(sourceFolder.FullName, sourceFile);
                var completeDestinationPath = Path.Combine(destinationFolder.FullName, sourceFile);
                using var sourceFileStream = File.OpenRead(completeSourcePath);
                using var destinationFileStream = File.OpenWrite(completeDestinationPath);
                int bytesRead;
                do
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        this.logger.LogError("Copy cancelled. CancellationToken cancelled");
                        progress.Report(ProgressCancelled);
                        return false;
                    }

                    bytesRead = await sourceFileStream.ReadAsync(buffer, cancellationToken);
                    if (bytesRead > 0)
                    {
                        await destinationFileStream.WriteAsync(buffer, cancellationToken);
                    }

                    totalBytesCopied += bytesRead;
                    progress.Report(ProgressCopying(totalBytesCopied / totalBytesToCopy));
                } while (bytesRead > 0);
            }

            var finalPath = Path.Combine(destinationFolder.FullName, ExecutableName);
            this.guildWarsExecutableManager.AddExecutable(finalPath);

            this.logger.LogDebug("Copy succeeded");
            progress.Report(ProgressCompleted);
            await Task.Delay(100, cancellationToken);
            return true;
        }, cancellationToken);
    }
}
