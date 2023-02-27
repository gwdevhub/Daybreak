using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Services.Navigation;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
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
    private const string TemporaryInstallerFileName = "Daybreak.Installer.Temp.exe";
    private const string InstallerFileName = "Daybreak.Installer.exe";
    private const string UpdatedKey = "Updating";
    private const string RegistryKey = "Daybreak";
    private const string TempFile = "tempfile.zip";
    private const string VersionTag = "{VERSION}";
    private const string RefTagPrefix = "/refs/tags";
    private const string VersionListUrl = "https://api.github.com/repos/AlexMacocian/Daybreak/git/refs/tags";
    private const string Url = "https://github.com/AlexMacocian/Daybreak/releases/latest";
    private const string DownloadUrl = $"https://github.com/AlexMacocian/Daybreak/releases/download/{VersionTag}/Daybreak{VersionTag}.zip";

    private readonly CancellationTokenSource updateCancellationTokenSource = new();
    private readonly IViewManager viewManager;
    private readonly IPostUpdateActionProvider postUpdateActionProvider;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
    private readonly IHttpClient<ApplicationUpdater> httpClient;
    private readonly ILogger<ApplicationUpdater> logger;

    public Version CurrentVersion { get; }

    public ApplicationUpdater(
        IViewManager viewManager,
        IPostUpdateActionProvider postUpdateActionProvider,
        ILiveOptions<ApplicationConfiguration> liveOptions,
        IHttpClient<ApplicationUpdater> httpClient,
        ILogger<ApplicationUpdater> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.postUpdateActionProvider = postUpdateActionProvider.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.httpClient.DefaultRequestHeaders.Add("user-agent", "Daybreak Client");

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
        updateStatus.CurrentStep = UpdateStatus.InitializingDownload;
        var uri = DownloadUrl.Replace(VersionTag, version.ToString());
        using var response = await this.httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        if (response.IsSuccessStatusCode is false)
        {
            updateStatus.CurrentStep = UpdateStatus.FailedDownload;
            this.logger.LogError($"Failed to download update. Details: {await response.Content.ReadAsStringAsync()}");
            return false;
        }

        using var downloadStream = await this.httpClient.GetStreamAsync(uri);
        this.logger.LogInformation("Beginning update download");
        var fileStream = File.OpenWrite(TempFile);
        var downloadSize = (double)response.Content!.Headers!.ContentLength!;
        var buffer = new byte[1024];
        var length = 0;
        double downloaded = 0;
        var tickTime = DateTime.Now;
        while (downloadStream.CanRead && (length = await downloadStream.ReadAsync(buffer)) > 0)
        {
            downloaded += length;
            await fileStream.WriteAsync(buffer, 0, length);
            if ((DateTime.Now - tickTime).TotalMilliseconds > 50)
            {
                tickTime = DateTime.Now;
                updateStatus.CurrentStep = UpdateStatus.Downloading(downloaded / downloadSize);
            }
        }

        updateStatus.CurrentStep = UpdateStatus.Downloading(1);
        updateStatus.CurrentStep = UpdateStatus.DownloadFinished;
        fileStream.Close();
        this.logger.LogInformation("Downloaded update");
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

        if (Version.TryParse(latestVersion, out var parsedVersion) is false)
        {
            throw new InvalidOperationException($"Could not parse retrieved version: {latestVersion}");
        }

        return await this.DownloadUpdate(parsedVersion, updateStatus);
    }

    public async Task<bool> UpdateAvailable()
    {
        var maybeLatestVersion = await this.GetLatestVersion();
        return maybeLatestVersion.Switch(
            onSome: latestVersion => string.Compare(this.CurrentVersion.ToString().Trim('v'), latestVersion, true) < 0,
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
        MarkUpdateInRegistry();
        this.LaunchExtractor();
    }

    public void OnStartup()
    {
        if (UpdateMarkedInRegistry())
        {
            UnmarkUpdateInRegistry();
            this.ExecutePostUpdateActions();
        }
    }

    public void OnClosing()
    {
    }

    private async Task<Optional<string>> GetLatestVersion()
    {
        using var response = await this.httpClient.GetAsync(Url);
        if (response.IsSuccessStatusCode)
        {
            var versionTag = response.RequestMessage!.RequestUri!.ToString().Split('/').Last().TrimStart('v');
            return versionTag;
        }

        return Optional.None<string>();
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

    private static void MarkUpdateInRegistry()
    {
        var homeRegistryKey = GetOrCreateHomeKey();
        homeRegistryKey.SetValue(UpdatedKey, true);
        homeRegistryKey.Close();
    }

    private static void UnmarkUpdateInRegistry()
    {
        var homeRegistryKey = GetOrCreateHomeKey();
        homeRegistryKey.SetValue(UpdatedKey, false);
        homeRegistryKey.Close();
    }

    private static bool UpdateMarkedInRegistry()
    {
        var homeRegistryKey = GetOrCreateHomeKey();
        var update = homeRegistryKey.GetValue(UpdatedKey);
        homeRegistryKey.Close();
        if (update is string updateString)
        {
            if (bool.TryParse(updateString, out var updateValue))
            {
                return updateValue;
            }
            else
            {
                throw new InvalidOperationException($"Found update value {updateString} in registry");
            }
        }

        return false;
    }

    private static RegistryKey GetOrCreateHomeKey()
    {
        var homeRegistryKey = Registry.CurrentUser.OpenSubKey("Software", true)?.OpenSubKey(RegistryKey, true);
        homeRegistryKey ??= Registry.CurrentUser.OpenSubKey("Software", true)!.CreateSubKey(RegistryKey, true);

        return homeRegistryKey;
    }
}
