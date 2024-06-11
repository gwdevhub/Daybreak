using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.SevenZip;
internal sealed class SevenZipExtractor : ISevenZipExtractor
{
    private const string ExtractorExeName = "Daybreak.7ZipExtractor.exe";

    private readonly ILogger<SevenZipExtractor> logger;

    public SevenZipExtractor(
        ILogger<SevenZipExtractor> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public async Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ExtractToDirectory), sourceFile);

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = PathUtils.GetAbsolutePathFromRoot(ExtractorExeName),
                Arguments = $"\"{sourceFile}\" \"{destinationDirectory}\"",
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                UseShellExecute = false
            },
            EnableRaisingEvents = true
        };

        process!.OutputDataReceived += (sender, args) =>
        {
            if (args.Data is null)
            {
                return;
            }

            var tokens = args.Data.Split(' ');
            if (!double.TryParse(tokens[0], out var progress))
            {
            }

            var fileName = string.Join(' ', tokens.Skip(1));
            progressTracker(progress, fileName);
        };

        process.Start();
        process.BeginOutputReadLine();
        await process.WaitForExitAsync(cancellationToken);
        process.CancelOutputRead();
        return process.ExitCode == 0;
    }
}
