using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Core.Extensions;
using System.IO;

namespace Daybreak.Services.Guildwars;

internal sealed class GuildWarsCopyService(
    IGuildWarsExecutableManager guildWarsExecutableManager,
    ILogger<GuildWarsCopyService> logger) : IGuildWarsCopyService
{
    private const string ExecutableName = "Gw.exe";

    private static readonly string[] FilesToCopy =
    [
        "Gw.dat",
        "Gw.exe",
        "GwLoginClient.dll"
    ];

    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly ILogger<GuildWarsCopyService> logger = logger.ThrowIfNull();

    public async Task CopyGuildwars(string existingExecutable, CopyStatus copyStatus, CancellationToken cancellationToken)
    {
        copyStatus.CurrentStep = CopyStatus.InitializingCopy;
        var sourceFolder = new FileInfo(existingExecutable).Directory;
        if (sourceFolder?.Exists is not true)
        {
            this.logger.LogError($"Source folder does not exist [{sourceFolder?.FullName}]");
            copyStatus.CurrentStep = CopyStatus.CopyFailed;
            return;
        }

        var totalBytesToCopy = 0d;
        foreach(var file in FilesToCopy)
        {
            var completePath = Path.Combine(sourceFolder.FullName, file);
            if (!File.Exists(completePath))
            {
                this.logger.LogError($"Source folder does not contain required file [{file}]");
                copyStatus.CurrentStep = CopyStatus.CopyFailed;
                return;
            }

            var fi = new FileInfo(completePath);
            totalBytesToCopy += fi.Length;
        }

        if (!TryGetDestinationPath(out var destinationPath))
        {
            this.logger.LogDebug("Copy cancelled. Destination path selection cancelled");
            copyStatus.CurrentStep = CopyStatus.CopyFailed;
            return;
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
                    copyStatus.CurrentStep = CopyStatus.CopyCancelled;
                    return;
                }

                bytesRead = await sourceFileStream.ReadAsync(buffer, cancellationToken);
                if (bytesRead > 0)
                {
                    await destinationFileStream.WriteAsync(buffer, cancellationToken);
                }

                totalBytesCopied += bytesRead;
                copyStatus.CurrentStep = CopyStatus.Copying(totalBytesCopied / totalBytesToCopy);
            } while (bytesRead > 0);
        }

        var finalPath = Path.Combine(destinationFolder.FullName, ExecutableName);
        this.guildWarsExecutableManager.AddExecutable(finalPath);

        this.logger.LogDebug("Copy succeeded");
        copyStatus.CurrentStep = CopyStatus.CopyFinished;
    }

    private static bool TryGetDestinationPath(out string path)
    {
        path = string.Empty;
        var folderPicker = new OpenFolderDialog()
        {
            Title = "Select Destination Folder",
            Multiselect = false,
            ValidateNames = true
        };

        if (folderPicker.ShowDialog() is not true)
        {
            return false;
        }

        path = Path.GetFullPath(folderPicker.FolderName);
        return true;
    }
}
