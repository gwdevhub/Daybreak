using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Daybreak.Shared.Services.DXVK;
using System.Core.Extensions;
using Daybreak.Shared.Models;
using System.Extensions.Core;

namespace Daybreak.Services.DXVK;
/// <summary>
/// Service for managing DXVK for GW1. https://github.com/Deadsimon/dxvk-for-guildwars
/// </summary>
internal sealed class DXVKService(
    INotificationService notificationService,
    IDownloadService downloadService,
    ILiveUpdateableOptions<DXVKOptions> options,
    ILogger<DXVKService> logger) : IDXVKService
{
    private const string DownloadUrlAMDCapped = "https://github.com/Deadsimon/dxvk-for-guildwars/releases/download/V1.21/dxvk_for_GW_V1.21_amd-FPS_Capped.zip";
    private const string DownloadUrlAMD = "https://github.com/Deadsimon/dxvk-for-guildwars/releases/download/V1.21/dxvk_for_GW_V1.21_amd.zip";
    private const string DownloadUrlNvidiaCapped = "https://github.com/Deadsimon/dxvk-for-guildwars/releases/download/V1.21/dxvk_for_GW_V1.21_nvidia-FPS_capped.zip";
    private const string DownloadUrlNvidia = "https://github.com/Deadsimon/dxvk-for-guildwars/releases/download/V1.21/dxvk_for_GW_V1.21_nvidia.zip";
    private const string ArchiveName = "dxvk.zip";
    private const string DXVKDirectorySubpath = "DXVK";
    private const string D3D9Dll = "d3d9.dll";
    private const string DXGIDll = "dxgi.dll";
    private const string DXVKConf = "DXVK.conf";
    private const string DXVKCache = "Gw.dxvk-cache";

    private static readonly string DXVKDirectory = PathUtils.GetAbsolutePathFromRoot(DXVKDirectorySubpath);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly ILiveUpdateableOptions<DXVKOptions> options = options.ThrowIfNull();
    private readonly ILogger<DXVKService> logger = logger.ThrowIfNull();

    public string Name => "DSOAL";
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
           File.Exists(Path.Combine(DXVKDirectory, D3D9Dll)) &&
           File.Exists(Path.Combine(DXVKDirectory, DXGIDll)) &&
           File.Exists(Path.Combine(DXVKDirectory, DXVKConf)) &&
           File.Exists(Path.Combine(DXVKDirectory, DXVKCache));

    public async Task<bool> SetupDXVK(DXVKInstallationStatus dXVKInstallationStatus, DXVKInstallationChoice dXVKInstallationChoice, CancellationToken cancellationToken)
    {
        if (this.IsInstalled)
        {
            return true;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        var downloadUrl = dXVKInstallationChoice switch
        {
            DXVKInstallationChoice.AMD => DownloadUrlAMD,
            DXVKInstallationChoice.AMDCapped => DownloadUrlAMDCapped,
            DXVKInstallationChoice.Nvidia => DownloadUrlNvidia,
            DXVKInstallationChoice.NvidiaCapped => DownloadUrlNvidiaCapped,
            _ => throw new ArgumentOutOfRangeException(nameof(dXVKInstallationChoice))
        };

        if ((await this.downloadService.DownloadFile(downloadUrl, ArchiveName, dXVKInstallationStatus, cancellationToken)) is false)
        {
            scopedLogger.LogError("Failed to install DXVK");
            return false;
        }

        scopedLogger.LogInformation("Extracting DXVK files");
        dXVKInstallationStatus.CurrentStep = DXVKInstallationStatus.ExtractingFiles;
        this.ExtractFiles();

        dXVKInstallationStatus.CurrentStep = DXVKInstallationStatus.Finished;
        return true;
    }

    public IEnumerable<string> GetCustomArguments() => [];

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
            EnsureFileExistsInGuildwarsDirectory(DXGIDll, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(DXVKCache, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(DXVKConf, guildwarsDirectory);
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
        EnsureFileDoesNotExistInGuildwarsDirectory(DXGIDll, guildwarsDirectory);
        EnsureFileDoesNotExistInGuildwarsDirectory(DXVKCache, guildwarsDirectory);
        EnsureFileDoesNotExistInGuildwarsDirectory(DXVKConf, guildwarsDirectory);
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

    private void ExtractFiles()
    {
        ZipFile.ExtractToDirectory(ArchiveName, DXVKDirectory, true);
        foreach (var subDir in Directory.GetDirectories(DXVKDirectory))
        {
            foreach (var file in Directory.GetFiles(subDir))
            {
                var destFile = Path.Combine(DXVKDirectory, Path.GetFileName(file));
                File.Move(file, destFile, true);
            }

            Directory.Delete(subDir, true);
        }

        var options = this.options.Value;
        options.Path = DXVKDirectory;
        this.options.UpdateOption();
        File.Delete(ArchiveName);
    }
}
