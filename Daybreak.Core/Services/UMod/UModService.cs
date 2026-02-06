using Daybreak.Configuration.Options;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Github;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Utils;
using Daybreak.Views.Mods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using TrailBlazr.Services;

namespace Daybreak.Services.UMod;

internal sealed class UModService(
    PhotinoWindow photinoWindow,
    IOptionsProvider optionsProvider,
    IViewManager viewManager,
    IProcessInjector processInjector,
    IModPathResolver modPathResolver,
    INotificationService notificationService,
    IDownloadService downloadService,
    IGithubClient githubClient,
    IOptionsMonitor<UModOptions> uModOptions,
    ILogger<UModService> logger) : IUModService
{
    private const string GithubOwner = "gwdevhub";
    private const string GithubRepo = "gMod";
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gwdevhub/gMod/releases/download/[TAG_PLACEHOLDER]/gMod.dll";
    private const string UModDirectorySubPath = "uMod";
    private const string UModDll = "uMod.dll";
    private const string UModModList = "modlist.txt";

    private static readonly ProgressUpdate ProgressFinished = new(1, "uMod installation finished");

    private static readonly string UModDirectory = PathUtils.GetAbsolutePathFromRoot(UModDirectorySubPath);

    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly IProcessInjector processInjector = processInjector.ThrowIfNull();
    private readonly IModPathResolver modPathResolver = modPathResolver.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly IGithubClient githubClient = githubClient.ThrowIfNull();
    private readonly IOptionsMonitor<UModOptions> uModOptions = uModOptions.ThrowIfNull();
    private readonly ILogger<UModService> logger = logger.ThrowIfNull();

    public string Name => "gMod";
    public string Description => "gMod (formerly uMod) is a texture mod loader for Guild Wars that allows you to load custom textures and mods into the game";
    public bool IsVisible => true;
    public bool CanCustomManage => true;
    public bool CanUninstall => true;
    public bool IsEnabled
    {
        get => this.uModOptions.CurrentValue.Enabled;
        set
        {
            var options = this.uModOptions.CurrentValue;
            options.Enabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }

    public bool IsInstalled => File.Exists(Path.GetFullPath(Path.Combine(UModDirectory, UModDll)));

    public Version Version => File.Exists(Path.Combine(Path.GetFullPath(UModDirectory), UModDll)) ?
        Version.TryParse(FileVersionInfo.GetVersionInfo(Path.Combine(Path.GetFullPath(UModDirectory), UModDll)).FileVersion!, out var version) ?
            version :
            Version.Parse("0") :
        Version.Parse("0");

    public async Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        var latestVersion = await this.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            return false;
        }

        return this.Version < latestVersion;
    }

    public async Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        return await this.PerformInstallation(cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            return await Task.Factory.StartNew(() => this.SetupUMod(progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        }, cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create<bool>(progress =>
        {
            progress.Report(new ProgressUpdate(0, "Uninstalling gMod"));
            if (!this.IsInstalled)
            {
                progress.Report(new ProgressUpdate(1, "gMod is not installed"));
                return Task.FromResult(true);
            }

            var gModPath = Path.GetFullPath(Path.Combine(UModDirectory, UModDll));
            if (!File.Exists(gModPath))
            {
                progress.Report(new ProgressUpdate(1, "gMod is not installed"));
                return Task.FromResult(true);
            }

            File.Delete(gModPath);
            progress.Report(new ProgressUpdate(1, "gMod uninstalled successfully"));
            return Task.FromResult(true);
        }, cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        this.viewManager.ShowView<UModManagementView>();
        return Task.CompletedTask;
    }

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var modListFilePath = Path.Combine(UModDirectory, UModModList);
        var lines = this.uModOptions.CurrentValue.Mods
            .Where(e => e.Enabled && e.PathToFile is not null)
            .Select(e => e.PathToFile)
            .OfType<string>()
            .Where(e =>
            {
                if (!File.Exists(e))
                {
                    scopedLogger.LogWarning("Mod file {ModFile} does not exist", e);
                    this.notificationService.NotifyError(
                        title: "gMod Mod Missing",
                        description: $"The mod file {e} could not be found. gMod will not load this mod");

                    return false;
                }

                return true;
            })
            .Select(this.modPathResolver.ResolveForModLoader)
            .ToList();

        await File.WriteAllLinesAsync(modListFilePath, lines, cancellationToken);
        var result = await this.processInjector.Inject(guildWarsCreatedContext.ApplicationLauncherContext.Process, Path.Combine(UModDirectory, UModDll), cancellationToken);
        if (result)
        {
            this.notificationService.NotifyInformation(
                title: "gMod loaded",
                description: "gMod.dll has been loaded. Textures will start loading asynchronously");
        }
        else
        {
            this.notificationService.NotifyError(
                title: "gMod failed to load",
                description: "gMod.dll has failed to load. Check logs for details");
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

        if (this.uModOptions.CurrentValue.Mods.Any(m => m.PathToFile == fullPath))
        {
            return true;
        }

        var entry = new UModEntry
        {
            Enabled = this.uModOptions.CurrentValue.AutoEnableMods,
            PathToFile = fullPath,
            Name = Path.GetFileNameWithoutExtension(fullPath),
            Imported = imported is true
        };

        var options = this.uModOptions.CurrentValue;
        options.Mods.Add(entry);
        this.optionsProvider.SaveOption(options);
        return true;
    }

    public bool RemoveMod(string pathToTpf)
    {
        var fullPath = Path.GetFullPath(pathToTpf);
        var mods = this.uModOptions.CurrentValue.Mods;
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
        return this.uModOptions.CurrentValue.Mods;
    }

    public void SaveMods(List<UModEntry> list)
    {
        var options = this.uModOptions.CurrentValue;
        options.Mods = list;
        this.optionsProvider.SaveOption(options);
    }

    public async Task<IReadOnlyCollection<string>> LoadModsFromDisk(CancellationToken cancellationToken)
    {
        var files = await this.photinoWindow.ShowOpenFileAsync("Select Mod Files", multiSelect: true, filters: [("Zip Files", ["zip"]), ("TPF Files", ["tpf"])]);
        return [.. files.Where(f => this.AddMod(f, imported: true))];
    }

    public async Task CheckAndUpdateUMod(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.IsInstalled)
        {
            scopedLogger.LogInformation("uMod is not installed. Skipping update check");
            return;
        }

        var latestVersion = await this.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            scopedLogger.LogError("Could not retrieve latest uMod version. Skipping update");
            return;
        }

        if (this.Version >= latestVersion)
        {
            scopedLogger.LogInformation("uMod is up to date. Current version: {currentVersion}, Latest version: {latestVersion}", this.Version, latestVersion);
            return;
        }

        await this.DownloadLatestDll(progress, cancellationToken);
        this.notificationService.NotifyInformation(
            title: "UMod updated",
            description: $"UMod has been updated to version {latestVersion}");
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
            return await this.GetLatestVersionDll(progress, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Encountered exception");
            return new DownloadLatestOperation.ExceptionEncountered(ex);
        }
    }

    private async Task<DownloadLatestOperation> GetLatestVersionDll(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving version list");
        var tags = await this.githubClient.GetRefTags(GithubOwner, GithubRepo, cancellationToken);
        var latestRelease = tags
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

    private async Task<Version?> GetLatestVersion(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug("Retrieving version list");
        var tags = await this.githubClient.GetRefTags(GithubOwner, GithubRepo, cancellationToken);
        var latestRelease = tags
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault()?
            .TrimStart('v');

        if (!Version.TryParse(latestRelease ?? string.Empty, out var latestVersion))
        {
            scopedLogger.LogError("Unable to parse latest version {latestRelease}", latestRelease ?? string.Empty);
            return default;
        }

        return latestVersion;
    }
}
