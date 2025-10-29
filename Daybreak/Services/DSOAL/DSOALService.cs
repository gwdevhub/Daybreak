using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.DSOAL;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.IO.Compression;

namespace Daybreak.Services.DSOAL;

/// <summary>
/// Service for managing DSOAL for GW1. Credits to: https://lemmy.wtf/post/27911
/// </summary>
internal sealed class DSOALService(
    INotificationService notificationService,
    IRegistryService registryService,
    IPrivilegeManager privilegeManager,
    IDownloadService downloadService,
    ILiveUpdateableOptions<DSOALOptions> options,
    ILogger<DSOALService> logger) : IDSOALService
{
    public const string DSOALFixAdminMessage = "Daybreak has detected an issue with the DSOAL installation. In order to fix this issue, Daybreak will need to restart as administrator. DSOAL will not work until then.";
    public const string DSOALFixRegistryKey = "DSOAL/FixSymbolicLink";
    private const string DownloadUrl = "https://github.com/ChthonVII/dsoal-GW1/releases/download/r420%2Bgw1_rev1/dsoal-GW1_r420+gw1_rev1.zip";
    private const string ArchiveName = "dsoal-GW1_r420+gw1_rev1.zip";
    private const string DSOALDirectorySubPath = "DSOAL";
    private const string HRTFArchiveName = "HRTF_OAL_1.19.0.zip";
    private const string DsoundDll = "dsound.dll";
    private const string DSOALAldrvDll = "dsoal-aldrv.dll";
    private const string AlsoftIni = "alsoft.ini";
    private const string OpenAlDirectory = "openal";

    private static readonly ProgressUpdate ProgressInitialize = new(0, "Initializing");
    private static readonly ProgressUpdate ProgressFailed = new(1, "Failed to install DSOAL");
    private static readonly ProgressUpdate ProgressUnpacking = new(1, "Unpacking DSOAL files");
    private static readonly ProgressUpdate ProgressSetupFiles = new(1, "Setting up DSOAL files");
    private static readonly ProgressUpdate ProgressSuccess = new(1, "DSOAL installation succeeded");

    private static readonly string DSOALDirectory = PathUtils.GetAbsolutePathFromRoot(DSOALDirectorySubPath);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IRegistryService registryService = registryService.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();
    private readonly IDownloadService downloadService = downloadService.ThrowIfNull();
    private readonly ILiveUpdateableOptions<DSOALOptions> options = options.ThrowIfNull();
    private readonly ILogger<DSOALService> logger = logger.ThrowIfNull();

    public string Name => "DSOAL";
    public string Description => "3D Positional Audio and EAX Effects";
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
    public bool IsInstalled => Directory.Exists(DSOALDirectory) &&
           File.Exists(Path.Combine(DSOALDirectory, DsoundDll)) &&
           File.Exists(Path.Combine(DSOALDirectory, AlsoftIni)) &&
           File.Exists(Path.Combine(DSOALDirectory, AlsoftIni));

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return this.SetupDSOAL(cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("DSOAL does not support custom management");
    }

    public void EnsureDSOALSymbolicLinkExists()
    {
        this.EnsureSymbolicLinkExists();
    }

    public ProgressAsyncOperation<bool> SetupDSOAL(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress =>
        {
            progress.Report(ProgressInitialize);
            if (this.IsInstalled)
            {
                return true;
            }

            if (!this.privilegeManager.AdminPrivileges)
            {
                await this.privilegeManager.RequestAdminPrivileges<LaunchView>("Administrator privileges are required to install DSOAL", cancelViewParams: default, cancellationToken);
                return false;
            }

            if ((await this.downloadService.DownloadFile(DownloadUrl, ArchiveName, progress, cancellationToken)) is false)
            {
                this.logger.LogError("Failed to install DSOAL");
                return false;
            }

            this.logger.LogDebug("Extracting DSOAL files");
            progress.Report(ProgressUnpacking);

            this.ExtractFiles();
            progress.Report(ProgressSetupFiles);

            this.SetupHrtfAndPresetFiles();
            progress.Report(ProgressSuccess);
            return true;
        }, cancellationToken);
    }

    public IEnumerable<string> GetCustomArguments()
    {
        if (this.options.Value.Enabled)
        {
            return [ "-dsound" ];
        }
        else
        {
            return [];
        }
    }

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
        var guildwarsDirectory = new FileInfo(guildWarsStartingContext.ApplicationLauncherContext.ExecutablePath).Directory!.FullName;
        if (this.IsInstalled)
        {
            if (!this.EnsureSymbolicLinkExists())
            {
                guildWarsStartingContext.CancelStartup = true;
            }

            EnsureFileExistsInGuildwarsDirectory(DsoundDll, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(DSOALAldrvDll, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(AlsoftIni, guildwarsDirectory);
            this.notificationService.NotifyInformation(
                title: "DSOAL started",
                description: "DSOAL files have been set up");
        }

        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var guildwarsDirectory = new FileInfo(guildWarsStartingDisabledContext.ApplicationLauncherContext.ExecutablePath).Directory!.FullName;
        EnsureFileDoesNotExistInGuildwarsDirectory(DsoundDll, guildwarsDirectory);
        EnsureFileDoesNotExistInGuildwarsDirectory(DSOALAldrvDll, guildwarsDirectory);
        EnsureFileDoesNotExistInGuildwarsDirectory(AlsoftIni, guildwarsDirectory);
        return Task.CompletedTask;
    }

    private static void EnsureFileExistsInGuildwarsDirectory(string fileName, string destinationDirectoryName)
    {
        var sourcePath = Path.Combine(DSOALDirectory, fileName);
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

    private bool EnsureSymbolicLinkExists()
    {
        var openalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), OpenAlDirectory);
        if (!Directory.Exists(openalPath))
        {
            if (!this.privilegeManager.AdminPrivileges)
            {
                this.registryService.SaveValue(DSOALFixRegistryKey, true);
                //this.privilegeManager.RequestAdminPrivileges<LauncherView>(DSOALFixAdminMessage);
                return false;
            }

            Directory.CreateSymbolicLink(openalPath, DSOALDirectory);
            return true;
        }

        var fi = new FileInfo(openalPath);
        var desiredPath = DSOALDirectory;
        if (fi.LinkTarget == desiredPath)
        {
            return true;
        }

        if (!this.privilegeManager.AdminPrivileges)
        {
            this.registryService.SaveValue(DSOALFixRegistryKey, true);
            //this.privilegeManager.RequestAdminPrivileges<LauncherView>(DSOALFixAdminMessage);
            return false;
        }

        Directory.Delete(openalPath);
        Directory.CreateSymbolicLink(openalPath, DSOALDirectory);
        return true;
    }

    private void ExtractFiles()
    {
        ZipFile.ExtractToDirectory(ArchiveName, DSOALDirectory, true);
        ZipFile.ExtractToDirectory(Path.Combine(DSOALDirectory, HRTFArchiveName), DSOALDirectory, true);
        File.Delete(ArchiveName);
        File.Delete(Path.Combine(DSOALDirectory, HRTFArchiveName));
    }

    private void SetupHrtfAndPresetFiles()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var openalPath = Path.Combine(appDataPath, OpenAlDirectory);
        if (Directory.Exists(openalPath))
        {
            this.logger.LogWarning($"Found existing openal symbolic link at [{openalPath}]. Deleting");
            Directory.Delete(openalPath);
        }

        Directory.CreateSymbolicLink(openalPath, DSOALDirectory);
    }
}
