using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.IO.Compression;
using System.IO;
using Daybreak.Shared.Services.DXVK;
using System.Core.Extensions;
using System.Extensions.Core;
using Daybreak.Shared.Models.Github;
using System.Net.Http;
using System.Extensions;
using System.Formats.Tar;
using Daybreak.Shared.Models.Async;

namespace Daybreak.Services.DXVK;
/// <summary>
/// Service for managing DXVK for GW1. https://github.com/doitsujin/dxvk
/// </summary>
internal sealed class DXVKService(
    INotificationService notificationService,
    IDownloadService downloadService,
    IHttpClient<DXVKService> httpClient,
    ILiveUpdateableOptions<DXVKOptions> options,
    ILogger<DXVKService> logger) : IDXVKService
{
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = $"https://github.com/doitsujin/dxvk/releases/download/v{TagPlaceholder}/dxvk-{TagPlaceholder}.tar.gz";
    private const string ReleasesUrl = "https://api.github.com/repos/doitsujin/dxvk/git/refs/tags";
    private const string ArchiveName = "dxvk.tar.gz";
    private const string DxvkArchivedFile = $"dxvk-{TagPlaceholder}\\x32\\d3d9.dll";
    private const string DXVKDirectorySubpath = "DXVK";
    private const string D3D9Dll = "d3d9.dll";

    private static readonly ProgressUpdate ProgressFailedToGetLatest = new(0, "Failed to get latest version");
    private static readonly ProgressUpdate ProgressFailedDownload = new(0, "Failed to download files");
    private static readonly ProgressUpdate ProgressUnpackingFiles = new(0, "Unpacking files");
    private static readonly ProgressUpdate ProgressFinished = new(0, "Finished");

    private static readonly string DXVKDirectory = PathUtils.GetAbsolutePathFromRoot(DXVKDirectorySubpath);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly ILiveUpdateableOptions<DXVKOptions> options = options.ThrowIfNull();
    private readonly IHttpClient<DXVKService> httpClient = httpClient.ThrowIfNull();
    private readonly ILogger<DXVKService> logger = logger.ThrowIfNull();

    public string Name => "DXVK";
    public string Description => "Translation layer which converts DX9 3D calls to Vulkan. Can improve performance for some users";
    public bool IsVisible => true;
    public bool CanCustomManage => false;
    public bool IsEnabled
    {
        get => this.options.Value.Enabled;
        set
        {
            this.options.Value.Enabled = value;
            this.options.UpdateOption();
        }
    }
    public bool IsInstalled => Directory.Exists(this.options.Value.Path) &&
           File.Exists(Path.Combine(DXVKDirectory, D3D9Dll));

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await this.SetupDXVK(progress, cancellationToken);
        }, cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DXVK mod does not support custom management");
    }

    public async Task<bool> SetupDXVK(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        if (this.IsInstalled)
        {
            return true;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        
        var latestVersion = await this.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            scopedLogger.LogError("Failed to get latest version");
            progress.Report(ProgressFailedToGetLatest);
            return false;
        }

        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, latestVersion.ToString());
        if ((await this.downloadService.DownloadFile(downloadUrl, ArchiveName, progress, cancellationToken)) is false)
        {
            scopedLogger.LogError("Failed to install DXVK");
            progress.Report(ProgressFailedDownload);
            return false;
        }

        scopedLogger.LogDebug("Extracting DXVK files");
        progress.Report(ProgressUnpackingFiles);
        await this.ExtractFiles(latestVersion, cancellationToken);

        progress.Report(ProgressFinished);
        return true;
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        var guildwarsDirectory = new FileInfo(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath).Directory?.FullName;
        if (guildwarsDirectory is null)
        {
            this.notificationService.NotifyError(
                title: "DXVK failed to start",
                description: "Guild Wars directory not found");
            return Task.CompletedTask;
        }

        if (this.IsInstalled)
        {
            EnsureFileExistsInGuildwarsDirectory(D3D9Dll, guildwarsDirectory);
            this.notificationService.NotifyInformation(
                title: "DXVK started",
                description: "DXVK files have been set up");
        }

        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var guildwarsDirectory = new FileInfo(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath).Directory?.FullName;
        if (guildwarsDirectory is null)
        {
            return Task.CompletedTask;
        }

        EnsureFileDoesNotExistInGuildwarsDirectory(D3D9Dll, guildwarsDirectory);
        return Task.CompletedTask;
    }

    private static void EnsureFileExistsInGuildwarsDirectory(string fileName, string destinationDirectoryName)
    {
        var sourcePath = Path.Combine(DXVKDirectory, fileName);
        var destinationPath = Path.Combine(destinationDirectoryName, fileName);
        if (File.Exists(destinationPath))
        {
            return;
        }

        File.Copy(sourcePath, destinationPath, true);
    }

    private static void EnsureFileDoesNotExistInGuildwarsDirectory(string fileName, string destinationDirectoryName)
    {
        var destinationPath = Path.Combine(destinationDirectoryName, fileName);
        if (!File.Exists(destinationPath))
        {
            return;
        }

        File.Delete(destinationPath);
    }

    private async ValueTask ExtractFiles(Version version, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(DXVKDirectory))
        {
            Directory.CreateDirectory(DXVKDirectory);
        }

        using var fileStream = File.OpenRead(ArchiveName);
        using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
        await TarFile.ExtractToDirectoryAsync(gzipStream, DXVKDirectory, overwriteFiles: true, cancellationToken);
        gzipStream.Close();
        fileStream.Close();

        var destFile = Path.Combine(DXVKDirectory, Path.GetFileName(D3D9Dll));
        var sourceFile = Path.Combine(DXVKDirectory, $"{DxvkArchivedFile.Replace(TagPlaceholder, version.ToString())}");
        File.Move(sourceFile, destFile, true);
        foreach (var subDir in Directory.GetDirectories(DXVKDirectory))
        {
            Directory.Delete(subDir, true);
        }

        var options = this.options.Value;
        options.Path = DXVKDirectory;
        options.Version = version.ToString();
        this.options.UpdateOption();
        File.Delete(ArchiveName);
    }

    private async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError("Received non success status code [{statusCode}]", getListResponse.StatusCode);
            return default;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync(cancellationToken);
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault();
        if (latestRelease is not string tag)
        {
            scopedLogger.LogError("Could not parse version list. No latest version found");
            return default;
        }

        if (!Version.TryParse(tag, out var version))
        {
            scopedLogger.LogError("Could not parse version from tag {tag}", tag);
            return default;
        }

        return version;
    }
}
