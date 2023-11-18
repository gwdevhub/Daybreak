﻿using Daybreak.Configuration.Options;
using Daybreak.Models;
using Daybreak.Models.Github;
using Daybreak.Models.Progress;
using Daybreak.Models.UMod;
using Daybreak.Services.Downloads;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.Toolbox.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;

public sealed class UModService : IUModService
{
    private const string TagPlaceholder = "[TAG_PLACEHOLDER]";
    private const string ReleaseUrl = "https://github.com/gwdevhub/gMod/releases/download/[TAG_PLACEHOLDER]/gMod.dll";
    private const string ReleasesUrl = "https://api.github.com/repos/gwdevhub/gMod/git/refs/tags";
    private const string UModDirectory = "uMod";
    private const string UModDll = "uMod.dll";
    private const string UModModList = "modlist.txt";

    private readonly IProcessInjector processInjector;
    private readonly INotificationService notificationService;
    private readonly IDownloadService downloadService;
    private readonly IHttpClient<UModService> httpClient;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions;
    private readonly ILogger<UModService> logger;

    public string Name => "uMod";

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

    public UModService(
        IProcessInjector processInjector,
        INotificationService notificationService,
        IDownloadService downloadService,
        IHttpClient<UModService> httpClient,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<UModOptions> uModOptions,
        ILogger<UModService> logger)
    {
        this.processInjector = processInjector.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.uModOptions = uModOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<string> GetCustomArguments()
    {
        return Enumerable.Empty<string>();
    }

    public Task OnGuildWarsStarting(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken)
    {
        var modListFilePath = Path.Combine(Path.GetFullPath(UModDirectory), UModModList);
        var lines = this.uModOptions.Value.Mods.Where(e => e.Enabled && e.PathToFile is not null).Select(e => e.PathToFile).ToList();
        await File.WriteAllLinesAsync(modListFilePath, lines!, cancellationToken);
        var result = await this.processInjector.Inject(applicationLauncherContext.Process, Path.Combine(Path.GetFullPath(UModDirectory), UModDll), cancellationToken);
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

    public Task OnGuildWarsStarted(ApplicationLauncherContext applicationLauncherContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus)
    {
        if ((await this.SetupUModDll(uModInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        uModInstallationStatus.CurrentStep = UModInstallationStatus.Finished;
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

    public async Task CheckAndUpdateUMod(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAndUpdateUMod), string.Empty);
        var existingUMod = Path.Combine(Path.GetFullPath(UModDirectory), UModDll);
        if (!this.IsInstalled)
        {
            scopedLogger.LogInformation("UMod is not installed");
            return;
        }

        scopedLogger.LogInformation("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return;
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync();
        var releasesList = responseString.Deserialize<List<GithubRefTag>>();
        var latestRelease = releasesList?
            .Select(t => t.Ref?.Replace("refs/tags/", ""))
            .OfType<string>()
            .LastOrDefault();

        if (!Daybreak.Models.Versioning.Version.TryParse(latestRelease ?? string.Empty, out var latestVersion))
        {
            scopedLogger.LogError($"Unable to parse latest version {latestRelease}");
            return;
        }

        var currentRelease = FileVersionInfo.GetVersionInfo(existingUMod).FileVersion;
        if (!Daybreak.Models.Versioning.Version.TryParse(currentRelease ?? string.Empty, out var currentVersion))
        {
            scopedLogger.LogError($"Unable to parse current version {currentRelease}");
            return;
        }

        if (currentVersion.CompareTo(latestVersion) >= 0)
        {
            scopedLogger.LogError($"UMod is up to date");
            return;
        }

        await this.DownloadLatestDll(new UModInstallationStatus(), cancellationToken);
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

    private async Task<bool> SetupUModDll(UModInstallationStatus uModInstallationStatus)
    {
        if (this.IsInstalled)
        {
            return true;
        }

        if (await this.DownloadLatestDll(uModInstallationStatus, CancellationToken.None) is not DownloadLatestOperation.Success)
        {
            this.logger.LogError("Failed to install uMod. Failed to download uMod");
            return false;
        }

        return true;
    }

    private async Task<DownloadLatestOperation> DownloadLatestDll(UModInstallationStatus uModInstallationStatus, CancellationToken cancellationToken)
    {
        try
        {
            return await this.GetLatestVersion(uModInstallationStatus, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Encountered exception");
            return new DownloadLatestOperation.ExceptionEncountered(ex);
        }
    }

    private async Task<DownloadLatestOperation> GetLatestVersion(UModInstallationStatus uModInstallationStatus, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetLatestVersion), string.Empty);
        scopedLogger.LogInformation("Retrieving version list");
        var getListResponse = await this.httpClient.GetAsync(ReleasesUrl, cancellationToken);
        if (!getListResponse.IsSuccessStatusCode)
        {
            scopedLogger.LogError($"Received non success status code [{getListResponse.StatusCode}]");
            return new DownloadLatestOperation.NonSuccessStatusCode((int)getListResponse.StatusCode);
        }

        var responseString = await getListResponse.Content.ReadAsStringAsync();
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

        scopedLogger.LogInformation($"Retrieving version {tag}");
        var downloadUrl = ReleaseUrl.Replace(TagPlaceholder, tag);
        var destinationFolder = Path.GetFullPath(UModDirectory);
        var destinationPath = Path.Combine(destinationFolder, UModDll);
        var success = await this.downloadService.DownloadFile(downloadUrl, destinationPath, uModInstallationStatus, cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to download uMod version {tag}");
        }

        return new DownloadLatestOperation.Success(destinationPath);
    }
}
