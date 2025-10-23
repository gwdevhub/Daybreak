using Daybreak.Configuration.Options;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Github;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Net.Http;

namespace Daybreak.Services.UMod;

internal sealed class UModService(
    IProcessInjector processInjector,
    INotificationService notificationService,
    IDownloadService downloadService,
    IHttpClient<UModService> httpClient,
    ILiveOptions<LauncherOptions> launcherOptions,
    ILiveUpdateableOptions<UModOptions> uModOptions,
    ILogger<UModService> logger) : IUModService
{
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gwdevhub/gMod/releases/download/[TAG_PLACEHOLDER]/gMod.dll";
    private const string ReleasesUrl = "https://api.github.com/repos/gwdevhub/gMod/git/refs/tags";
    private const string UModDirectorySubPath = "uMod";
    private const string UModDll = "uMod.dll";
    private const string UModModList = "modlist.txt";

    private static readonly ProgressUpdate ProgressFinished = new(1, "uMod installation finished");

    private static readonly string UModDirectory = PathUtils.GetAbsolutePathFromRoot(UModDirectorySubPath);

    private readonly IProcessInjector processInjector = processInjector.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IHttpClient<UModService> httpClient = httpClient.ThrowIfNull();
    private readonly ILiveOptions<LauncherOptions> launcherOptions = launcherOptions.ThrowIfNull();
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions = uModOptions.ThrowIfNull();
    private readonly ILogger<UModService> logger = logger.ThrowIfNull();

    public string Name => "gMod";
    public string Description => "gMod (formerly uMod) is a mod loader for Guild Wars that allows you to load custom textures and mods into the game";
    public bool IsVisible { get; }
    public bool IsEnabled
    {
        get => this.uModOptions.Value.Enabled;
        set
        {
            this.uModOptions.Value.Enabled = value;
            this.uModOptions.UpdateOption();
        }
    }

    public bool IsInstalled => File.Exists(Path.GetFullPath(Path.Combine(UModDirectory, UModDll)));

    public Version Version => File.Exists(Path.Combine(Path.GetFullPath(UModDirectory), UModDll)) ?
        Version.TryParse(FileVersionInfo.GetVersionInfo(Path.Combine(Path.GetFullPath(UModDirectory), UModDll)).FileVersion!, out var version) ?
            version :
            Version.Parse("0") :
        Version.Parse("0");

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await Task.Factory.StartNew(() => this.SetupUMod(progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        }, cancellationToken);
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        var modListFilePath = Path.Combine(UModDirectory, UModModList);
        var lines = this.uModOptions.Value.Mods.Where(e => e.Enabled && e.PathToFile is not null).Select(e => e.PathToFile).ToList();
        await File.WriteAllLinesAsync(modListFilePath, lines!, cancellationToken);
        var result = await this.processInjector.Inject(guildWarsCreatedContext.ApplicationLauncherContext.Process, Path.Combine(UModDirectory, UModDll), cancellationToken);
        if (result)
        {
            this.notificationService.NotifyInformation(
                title: "uMod loaded",
                description: "uMod.dll has been loaded. Textures will start loading asynchronously");
        }
        else
        {
            this.notificationService.NotifyError(
                title: "uMod failed to load",
                description: "uMod.dll has failed to load. Check logs for details");
        }
    }

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<bool> SetupUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        if ((await this.SetupUModDll(progress, cancellationToken)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        progress.Report(ProgressFinished);
        return true;
    }

    public bool AddMod(string pathToTpf, bool? imported = default)
    {
        var fullPath = Path.GetFullPath(pathToTpf);
        if (!File.Exists(pathToTpf))
        {
            return false;
        }

        if (this.uModOptions.Value.Mods.Any(m => m.PathToFile == fullPath))
        {
            return true;
        }

        var entry = new UModEntry
        {
            Enabled = this.uModOptions.Value.AutoEnableMods,
            PathToFile = fullPath,
            Name = Path.GetFileNameWithoutExtension(fullPath),
            Imported = imported is true
        };

        this.uModOptions.Value.Mods.Add(entry);
        this.uModOptions.UpdateOption();
        return true;
    }

    public bool RemoveMod(string pathToTpf)
    {
        var fullPath = Path.GetFullPath(pathToTpf);
        var mods = this.uModOptions.Value.Mods;
        var maybeMod = mods.FirstOrDefault(m => m.PathToFile == fullPath);
        if (maybeMod is null)
        {
            return true;
        }

        mods.Remove(maybeMod);
        this.SaveMods(mods);

        // If the mod was downloaded and managed entirely through Daybreak, we can safely delete it
        if (!maybeMod.Imported)
        {
            File.Delete(fullPath);
        }

        return true;
    }

    public List<UModEntry> GetMods()
    {
        return this.uModOptions.Value.Mods;
    }

    public void SaveMods(List<UModEntry> list)
    {
        this.uModOptions.Value.Mods = list;
        this.uModOptions.UpdateOption();
    }

    public async Task CheckAndUpdateUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdateUMod), string.Empty);
        var existingUMod = Path.Combine(UModDirectory, UModDll);
        if (!this.IsInstalled)
        {
            scopedLogger.LogDebug("UMod is not installed");
            return;
        }

        scopedLogger.LogDebug("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync(cancellationToken);
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault();

        if (!Version.TryParse(latestRelease ?? string.Empty, out var latestVersion))
        {
            scopedLogger.LogError($"Unable to parse latest version {latestRelease}");
            return;
        }

        if (this.Version.CompareTo(latestVersion) >= 0)
        {
            scopedLogger.LogDebug($"UMod is up to date");
            return;
        }

        await this.DownloadLatestDll(progress, cancellationToken);
        this.notificationService.NotifyInformation(
            title: "UMod updated",
            description: $"UMod has been updated to version {latestRelease}");
    }

    private void NotifyFaultyInstallation()
    {
        /*
             * Known issue where Guild Wars updater breaks the executable, which in turn breaks the integration with uMod.
             * Prompt the user to manually reinstall Guild Wars.
             */
        this.notificationService.NotifyInformation(
            title: "uMod failed to start",
            description: "uMod failed to start due to a known issue with Guild Wars updating process. Please manually re-install Guild Wars in order to restore uMod functionality");
    }

    private async Task<bool> SetupUModDll(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        if (this.IsInstalled)
        {
            return true;
        }

        if (await this.DownloadLatestDll(progress, cancellationToken) is not DownloadLatestOperation.Success)
        {
            this.logger.LogError("Failed to install uMod. Failed to download uMod");
            return false;
        }

        return true;
    }

    private async Task<DownloadLatestOperation> DownloadLatestDll(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        try
        {
            return await this.GetLatestVersion(progress, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Encountered exception");
            return new DownloadLatestOperation.ExceptionEncountered(ex);
        }
    }

    private async Task<DownloadLatestOperation> GetLatestVersion(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestVersion), string.Empty);
        scopedLogger.LogDebug("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return new DownloadLatestOperation.NonSuccessStatusCode((int)getListResponse.StatusCode);
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
            return new DownloadLatestOperation.NoVersionFound();
        }

        scopedLogger.LogDebug($"Retrieving version {tag}");
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, tag);
        var destinationFolder = UModDirectory;
        var destinationPath = Path.Combine(destinationFolder, UModDll);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, progress, cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download uMod version {tag}");
        }

        return new DownloadLatestOperation.Success(destinationPath);
    }
}
