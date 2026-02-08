using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.SevenZip;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Daybreak.Linux.Services.SevenZip;

/// <summary>
/// Linux implementation of ISevenZipExtractor that uses the native 7z command-line tool.
/// Requires p7zip-full (or equivalent) to be installed on the system.
/// </summary>
internal sealed class SevenZipArchiveExtractor(
    INotificationService notificationService,
    ILogger<SevenZipArchiveExtractor> logger) : ISevenZipExtractor
{
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<SevenZipArchiveExtractor> logger = logger;

    public async Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken)
    {
        if (!Is7zAvailable())
        {
            this.logger.LogError("7z is not installed. Cannot extract archives");
            this.notificationService.NotifyError(
                title: "7z not found",
                description: "The 7z command is required to extract archives on Linux. Please install p7zip-full (e.g. 'sudo apt install p7zip-full') and restart Daybreak.",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(15));
            return false;
        }

        try
        {
            var root = Path.GetFullPath(destinationDirectory);
            Directory.CreateDirectory(root);

            // First, list the archive to get entry count for progress tracking
            var entries = await this.ListArchiveEntries(sourceFile, cancellationToken);
            var total = Math.Max(entries.Count, 1);

            // Extract using 7z command
            var startInfo = new ProcessStartInfo
            {
                FileName = "7z",
                Arguments = $"x \"{sourceFile}\" -o\"{root}\" -y",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };

            var processed = 0;
            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data is null)
                {
                    return;
                }

                // 7z outputs lines like "- filename" for each extracted file
                if (e.Data.StartsWith("- "))
                {
                    var fileName = e.Data[2..].Trim();
                    processed++;
                    progressTracker((double)processed / total, fileName);
                }
            };

            process.Start();
            process.BeginOutputReadLine();

            var stderr = await process.StandardError.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                this.logger.LogError("7z extraction failed with exit code {ExitCode}. Error: {Error}", process.ExitCode, stderr);
                return false;
            }

            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            this.logger.LogError(ex, "Failed to extract archive {SourceFile} to {DestinationDirectory}", sourceFile, destinationDirectory);
            return false;
        }
    }

    private async Task<List<string>> ListArchiveEntries(string sourceFile, CancellationToken cancellationToken)
    {
        var entries = new List<string>();
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "7z",
                Arguments = $"l \"{sourceFile}\" -slt",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            await process.WaitForExitAsync(cancellationToken);

            foreach (var line in output.Split('\n'))
            {
                if (line.StartsWith("Path = "))
                {
                    entries.Add(line[7..].Trim());
                }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to list archive entries for progress tracking, progress will be approximate");
        }

        return entries;
    }

    private static bool Is7zAvailable()
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = "7z",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}
