using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Navigation;
using Daybreak.Services.Registry;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Version = Daybreak.Models.Versioning.Version;

namespace Daybreak.Services.Updater;

public sealed class ApplicationUpdater : IApplicationUpdater
{
    private const string InstallerFileName = "Daybreak.Installer.exe";
    private const string UpdatedKey = "LauncherUpdating";
    private const string TempFile = "tempfile.zip";
    private const string VersionTag = "{VERSION}";
    private const string RefTagPrefix = "/refs/tags";
    private const string VersionListUrl = "https://api.github.com/repos/AlexMacocian/Daybreak/git/refs/tags";
    private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
    private const string DownloadUrl = $"https://github.com/AlexMacocian/Daybreak/releases/download/{VersionTag}/Daybreak{VersionTag}.zip";

    private readonly CancellationTokenSource updateCancellationTokenSource = new();
    private readonly IRegistryService registryService;
    private readonly IViewManager viewManager;
    private readonly IDownloadService downloadService;
    private readonly IPostUpdateActionProvider postUpdateActionProvider;
    private readonly ILiveOptions<LauncherOptions> liveOptions;
    private readonly IHttpClient<ApplicationUpdater> httpClient;
    private readonly ILogger<ApplicationUpdater> logger;

    public Version CurrentVersion { get; }

    public ApplicationUpdater(
        IRegistryService registryService,
        IViewManager viewManager,
        IDownloadService downloadService,
        IPostUpdateActionProvider postUpdateActionProvider,
        ILiveOptions<LauncherOptions> liveOptions,
        IHttpClient<ApplicationUpdater> httpClient,
        ILogger<ApplicationUpdater> logger)
    {
        this.registryService = registryService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.postUpdateActionProvider = postUpdateActionProvider.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        if (Version.TryParse(Assembly.GetExecutingAssembly()!.GetName()!.Version!.ToString(), out var currentVersion))
        {
            if (currentVersion.HasPrefix is false)
            {
                currentVersion = Version.Parse("v" + currentVersion);
            }

            this.CurrentVersion = currentVersion;
        }
        else
        {
            throw new FatalException($"Application version is invalid: {Assembly.GetExecutingAssembly().GetName().Version}");
        }
    }

    public async Task<bool> DownloadUpdate(Version version, UpdateStatus updateStatus)
    {
        updateStatus.CurrentStep = DownloadStatus.InitializingDownload;
        var uri = DownloadUrl.Replace(VersionTag, version.ToString());
        if (await this.downloadService.DownloadFile(uri, TempFile, updateStatus) is false)
        {
            this.logger.LogError("Failed to download update file");
            return false;
        }

        updateStatus.CurrentStep = UpdateStatus.PendingRestart;
        this.logger.LogInformation("Downloaded update file");
        return true;
    }

    public async Task<bool> DownloadLatestUpdate(UpdateStatus updateStatus)
    {
        updateStatus.CurrentStep = UpdateStatus.CheckingLatestVersion;
        var latestVersion = (await this.GetLatestVersion()).ExtractValue();
        if (latestVersion is null)
        {
            this.logger.LogWarning("Failed to retrieve latest version. Aborting update");
            return false;
        }

        return await this.DownloadUpdate(latestVersion, updateStatus);
    }

    public async Task<bool> UpdateAvailable()
    {
        var maybeLatestVersion = await this.GetLatestVersion();
        return maybeLatestVersion.Switch(
            onSome: latestVersion => this.CurrentVersion.CompareTo(latestVersion) < 0,
            onNone: () =>
            {
                this.logger.LogWarning("Failed to retrieve latest version");
                return false;
            }).ExtractValue();
    }

    public async Task<IEnumerable<Version>> GetVersions()
    {
        this.logger.LogInformation($"Retrieving version list from {VersionListUrl}");
        var response = await this.httpClient.GetAsync(VersionListUrl);
        if (response.IsSuccessStatusCode)
        {
            var serializedList = await response.Content.ReadAsStringAsync();
            var versionList = serializedList.Deserialize<GithubRefTag[]>();
            return versionList.Select(v => v.Ref!.Remove(0, RefTagPrefix.Length)).Select(v => new Version(v));
        }

        return new List<Version>();
    }

    public void PeriodicallyCheckForUpdates()
    {
        System.Extensions.TaskExtensions.RunPeriodicAsync(async () =>
        {
            if (this.liveOptions.Value.AutoCheckUpdate is false)
            {
                this.updateCancellationTokenSource.Cancel();
                return;
            }

            if (await this.UpdateAvailable())
            {
                Application.Current.Dispatcher.Invoke(() => this.viewManager.ShowView<AskUpdateView>());
            }
        }, TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(15), this.updateCancellationTokenSource.Token);
    }

    public void FinalizeUpdate()
    {
        this.MarkUpdateInRegistry();
        this.LaunchExtractor();
    }

    public void OnStartup()
    {
        if (this.UpdateMarkedInRegistry())
        {
            this.UnmarkUpdateInRegistry();
            this.ExecutePostUpdateActions();
        }
    }

    public void OnClosing()
    {
    }

    private async Task<Optional<Version>> GetLatestVersion()
    {
        using var response = await this.httpClient.GetAsync(Url);
        if (response.IsSuccessStatusCode)
        {
            var versionTag = response.RequestMessage!.RequestUri!.ToString().Split('/').Last().TrimStart('v');
            if (Version.TryParse(versionTag, out var parsedVersion))
            {
                return parsedVersion;
            }

            return Optional.None<Version>();
        }

        return Optional.None<Version>();
    }

    private void LaunchExtractor()
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = InstallerFileName
            }
        };
        this.logger.LogInformation("Launching installer");
        if (process.Start() is false)
        {
            throw new InvalidOperationException("Failed to launch installer");
        }
    }

    private async void ExecutePostUpdateActions()
    {
        this.logger.LogInformation("Executing post-update actions");
        foreach(var action in this.postUpdateActionProvider.GetPostUpdateActions())
        {
            this.logger.LogInformation($"Starting [{action.GetType().Name}]");
            action.DoPostUpdateAction();
            await action.DoPostUpdateActionAsync();
            this.logger.LogInformation($"Finished [{action.GetType().Name}]");
        }
    }

    private void MarkUpdateInRegistry()
    {
        this.registryService.SaveValue(UpdatedKey, true);
    }

    private void UnmarkUpdateInRegistry()
    {
        this.registryService.DeleteValue(UpdatedKey);
    }

    private bool UpdateMarkedInRegistry()
    {
        if (this.registryService.TryGetValue<bool>(UpdatedKey, out _))
        {
            return true;
        }

        return false;
    }
}
