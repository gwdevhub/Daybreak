using Daybreak.Services.Guildwars.Models;
using Daybreak.Services.Guildwars.Utils;
using Daybreak.Services.GuildWars.Utils;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Runtime.CompilerServices;

namespace Daybreak.Services.GuildWars;
internal sealed class IntegratedGuildwarsInstaller(
    IGuildWarsExecutableManager guildWarsExecutableManager,
    INotificationService notificationService,
    ILogger<IntegratedGuildwarsInstaller> logger) : IGuildWarsInstaller
{
    private const string ExeName = "Gw.exe";
    private const string CompressedTempExeName = $"Gw.{VersionPlaceholder}.temp";
    private const string UncompressedTempExeName = $"Gw.{VersionPlaceholder}.exe";
    private const string VersionPlaceholder = "[VERSION]";
    private static readonly string StagingFolder = PathUtils.GetAbsolutePathFromRoot("GuildWarsCache");

    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<IntegratedGuildwarsInstaller> logger = logger.ThrowIfNull();

    public async IAsyncEnumerable<GuildWarsUpdateResponse> CheckAndUpdateGuildWarsExecutables(List<GuildWarsUpdateRequest> requests, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        requests.ThrowIfNull();
        var mainScopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdateGuildWarsExecutables), string.Empty);
        if (await this.GetLatestVersionId(CancellationToken.None) is not int latestVersion)
        {
            mainScopedLogger.LogError("Failed to fetch latest GuildWars version");
            throw new InvalidOperationException("Failed to fetch latest GuildWars version");
        }

        foreach(var request in requests)
        {
            if (request.ExecutablePath!.IsNullOrWhiteSpace() ||
                request.Status is null)
            {
                mainScopedLogger.LogError($"Invalid request for [{request.ExecutablePath}]");
                yield return new GuildWarsUpdateResponse { ExecutablePath = request.ExecutablePath, Result = false };
                continue;
            }

            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdateGuildWarsExecutables), request.ExecutablePath!);
            if (!File.Exists(request.ExecutablePath))
            {
                scopedLogger.LogError($"File not found at [{request.ExecutablePath}]");
                yield return new GuildWarsUpdateResponse { ExecutablePath = request.ExecutablePath, Result = false };
                continue;
            }

            if (await this.GetVersionId(request.ExecutablePath, request.CancellationToken) is int version &&
                    version == latestVersion)
            {
                scopedLogger.LogDebug("Executable is already latest");
                yield return new GuildWarsUpdateResponse { ExecutablePath = request.ExecutablePath, Result = true };
                continue;
            }

            var success = false;
            try
            {
                success = await this.UpdateGuildwars(request.ExecutablePath, request.Status, request.CancellationToken);
            }
            catch(Exception e)
            {
                scopedLogger.LogError(e, "Encountered exception while processing");
                success = false;
            }

            yield return new GuildWarsUpdateResponse { ExecutablePath = request.ExecutablePath, Result = success };
        }
    }

    public async Task<bool> UpdateGuildwars(string exePath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken)
    {
        return await new TaskFactory().StartNew(_ =>
        {
            return this.UpdateGuildwarsInternal(exePath, installationStatus, cancellationToken);
        }, TaskCreationOptions.LongRunning, cancellationToken).Unwrap();
    }

    public async Task<bool> InstallGuildwars(string destinationPath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken)
    {
        return await new TaskFactory().StartNew(_ => {
            return this.InstallGuildwarsInternal(destinationPath, installationStatus, cancellationToken);
        }, TaskCreationOptions.LongRunning, cancellationToken).Unwrap();
    }

    public async Task<int?> GetLatestVersionId(CancellationToken cancellationToken)
    {
        var guildWarsClient = new GuildWarsClient();
        var response = await guildWarsClient.Connect(cancellationToken);
        return response?.Item2.LatestExe;
    }

    public async Task<int?> GetVersionId(string executablePath, CancellationToken cancellationToken)
    {
        var parser = GuildWarsExecutableParser.TryParse(executablePath);
        if (parser is null)
        {
            return default;
        }

        try
        {
            return await parser.GetVersion(cancellationToken);
        }
        catch
        {
            return default;
        }
    }

    private async Task<bool> UpdateGuildwarsInternal(string exePath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken)
    {
        var downloadedPath = await this.FetchLatestGuildwarsInternal(installationStatus, cancellationToken);
        if (downloadedPath is null)
        {
            installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
            return false;
        }

        File.Copy(downloadedPath, exePath, true);
        installationStatus.CurrentStep = GuildwarsInstallationStatus.UpdateFinished;
        return true;
    }

    private async Task<string?> FetchLatestGuildwarsInternal(GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.FetchLatestGuildwarsInternal), string.Empty);
        GuildWarsClientContext? maybeContext = default;
        try
        {
            // Initialize the download client
            var guildWarsClient = new GuildWarsClient();
            var result = await guildWarsClient.Connect(cancellationToken);
            if (!result.HasValue)
            {
                scopedLogger.LogError("Failed to connect to ArenaNet servers");
                installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
                return default;
            }

            (var context, var manifest) = result.Value;
            maybeContext = context;
            var tempName = Path.Combine(StagingFolder, CompressedTempExeName.Replace(VersionPlaceholder, manifest.LatestExe.ToString()));
            var cacheName = Path.Combine(StagingFolder, UncompressedTempExeName.Replace(VersionPlaceholder, manifest.LatestExe.ToString()));
            if (File.Exists(cacheName) &&
                await this.GetVersionId(cacheName, cancellationToken) is int cacheVersion &&
                cacheVersion == manifest.LatestExe)
            {
                return cacheName;
            }

            (var downloadResult, var expectedFinalSize) = await this.DownloadCompressedExecutable(tempName, guildWarsClient, context, manifest, installationStatus, cancellationToken);
            if (!downloadResult)
            {
                scopedLogger.LogError("Failed to download compressed executable");
                installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
                return default;
            }

            if (!this.DecompressExecutable(tempName, cacheName, expectedFinalSize, installationStatus))
            {
                scopedLogger.LogError("Failed to decompress executable");
                installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
                return default;
            }

            File.Delete(tempName);
            return cacheName;
        }
        catch (Exception e)
        {
            this.notificationService.NotifyError(
                title: "Download exception",
                description: $"Encountered exception while downloading: {e}");
            installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
            this.logger.LogError(e, "Download failed. Encountered exception");
            return default;
        }
        finally
        {
            if (maybeContext.HasValue)
            {
                maybeContext.Value.Dispose();
            }
        }
    }

    private async Task<bool> InstallGuildwarsInternal(string destinationPath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.InstallGuildwarsInternal), destinationPath);
        var exePath = Path.Combine(destinationPath, ExeName);
        var latestGwPath = await this.FetchLatestGuildwarsInternal(installationStatus, cancellationToken);
        if (latestGwPath is null)
        {
            installationStatus.CurrentStep = GuildwarsInstallationStatus.Failed;
            return false;
        }

        var filePath = Path.GetFullPath(exePath);
        File.Copy(latestGwPath, filePath, true);
        installationStatus.CurrentStep = GuildwarsInstallationStatus.StartingExecutable;
        await Task.Delay(100, cancellationToken);
        using var process = Process.Start(filePath);
        scopedLogger.LogDebug("Starting executable. Waiting for the process to end before finishing installation");
        while (!process.HasExited)
        {
            await Task.Delay(1000, cancellationToken);
        }

        this.guildWarsExecutableManager.AddExecutable(filePath);
        installationStatus.CurrentStep = GuildwarsInstallationStatus.InstallFinished;
        return true;
    }

    private async Task<(bool Success, int ExpectedSize)> DownloadCompressedExecutable(
        string fileName,
        GuildWarsClient guildWarsClient,
        GuildWarsClientContext context,
        ManifestResponse manifest,
        GuildwarsInstallationStatus installationStatus,
        CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.DownloadCompressedExecutable), fileName);
        var maybeStream = await guildWarsClient.GetFileStream(context, manifest.LatestExe, 0, cancellationToken);
        if (maybeStream is null)
        {
            scopedLogger.LogError("Failed to get download stream");
            return (false, -1);
        }

        using var downloadStream = maybeStream;
        var directoryName = Path.GetDirectoryName(fileName);
        if (directoryName is null)
        {
            scopedLogger.LogError("Failed to create destination folder");
            return (false, -1);
        }

        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        using var writeFileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
        var expectedFinalSize = downloadStream.SizeDecompressed;
        var buffer = new Memory<byte>(new byte[2048]);
        var readBytes = 0;
        do
        {
            readBytes = await downloadStream.ReadAsync(buffer, cancellationToken);
            await writeFileStream.WriteAsync(buffer, cancellationToken);
            installationStatus.CurrentStep = GuildwarsInstallationStatus.Downloading((double)downloadStream.Position / downloadStream.Length, default);
        } while (readBytes > 0);

        return (true, expectedFinalSize);
    }

    private bool DecompressExecutable(
        string tempName,
        string exeName,
        int expectedFinalSize,
        GuildwarsInstallationStatus installationStatus)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.DecompressExecutable), tempName);
        try
        {
            var byteBuffer = new Memory<byte>(new byte[1]);
            using var readFileStream = new FileStream(tempName, FileMode.Open, FileAccess.Read);
            using var finalExeStream = new FileStream(exeName, FileMode.Create, FileAccess.ReadWrite);
            var bitStream = new BitStream(readFileStream);
            bitStream.Consume(4);
            var first4Bits = bitStream.Read(4);
            while (finalExeStream.Length < expectedFinalSize)
            {
                installationStatus.CurrentStep = GuildwarsInstallationStatus.Unpacking((double)finalExeStream.Length / expectedFinalSize, default);
                var litHuffman = HuffmanTable.BuildHuffmanTable(bitStream);
                var distHuffman = HuffmanTable.BuildHuffmanTable(bitStream);
                var blockSize = (bitStream.Read(4) + 1) * 4096;
                for (var i = 0; i < blockSize; i++)
                {
                    if (finalExeStream.Length == expectedFinalSize)
                    {
                        break;
                    }

                    var code = litHuffman.GetNextCode(bitStream);
                    if (code < 0x100)
                    {
                        finalExeStream.WriteByte((byte)code);
                    }
                    else
                    {
                        var blen = Huffman.ExtraBitsLength[code - 256];
                        code = Huffman.Table3[code - 256];
                        if (blen > 0)
                        {
                            code |= bitStream.Read((int)blen);
                        }

                        var backtrackCount = first4Bits + code + 1;
                        code = distHuffman.GetNextCode(bitStream);
                        blen = Huffman.ExtraBitsDistance[code];
                        var backtrack = Huffman.BacktrackTable[code];
                        if (blen > 0)
                        {
                            backtrack |= bitStream.Read((int)blen);
                        }

                        if (backtrack >= finalExeStream.Length)
                        {
                            throw new InvalidOperationException("Failed to decompress executable. backtrack >= finalExeStream.Length");
                        }

                        var src = finalExeStream.Length - (backtrack + 1);
                        for (var j = src; j < src + backtrackCount; j++)
                        {
                            finalExeStream.Seek(j, SeekOrigin.Begin);
                            var b = finalExeStream.ReadByte();
                            finalExeStream.Seek(0, SeekOrigin.End);
                            finalExeStream.WriteByte((byte)b);
                        }
                    }
                }
            }

            return true;
        }
        catch(Exception e)
        {
            scopedLogger.LogError(e, "Encountered exception when decompressing executable");
            this.notificationService.NotifyError(
                title: "Failed to decompress",
                description: $"Encountered exception while decompressing: {e}");
            return false;
        }
    }
}
