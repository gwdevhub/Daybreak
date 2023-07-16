using Daybreak.Configuration.Options;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Privilege;
using Daybreak.Services.Registry;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Daybreak.Services.DSOAL;

/// <summary>
/// Service for managing DSOAL for GW1. Credits to: https://lemmy.wtf/post/27911
/// </summary>
public sealed class DSOALService : IDSOALService
{
    public const string DSOALFixAdminMessage = "Daybreak has detected an issue with the DSOAL installation. In order to fix this issue, Daybreak will need to restart as administrator. DSOAL will not work until then.";
    public const string DSOALFixRegistryKey = "DSOAL/FixSymbolicLink";
    private const string DownloadUrl = "https://github.com/ChthonVII/dsoal-GW1/releases/download/r420%2Bgw1_rev1/dsoal-GW1_r420+gw1_rev1.zip";
    private const string ArchiveName = "dsoal-GW1_r420+gw1_rev1.zip";
    private const string DSOALDirectory = "DSOAL";
    private const string HRTFArchiveName = "HRTF_OAL_1.19.0.zip";
    private const string DsoundDll = "dsound.dll";
    private const string DSOALAldrvDll = "dsoal-aldrv.dll";
    private const string AlsoftIni = "alsoft.ini";
    private const string OpenAlDirectory = "openal";

    private readonly IRegistryService registryService;
    private readonly IPrivilegeManager privilegeManager;
    private readonly IDownloadService downloadService;
    private readonly ILiveUpdateableOptions<DSOALOptions> options;
    private readonly ILogger<DSOALService> logger;

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
           File.Exists(Path.Combine(DSOALDirectory, DsoundDll)) &&
           File.Exists(Path.Combine(DSOALDirectory, AlsoftIni)) &&
           File.Exists(Path.Combine(DSOALDirectory, AlsoftIni));

    public DSOALService(
        IRegistryService registryService,
        IPrivilegeManager privilegeManager,
        IDownloadService downloadService,
        ILiveUpdateableOptions<DSOALOptions> options,
        ILogger<DSOALService> logger)
    {
        this.registryService = registryService.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public void EnsureDSOALSymbolicLinkExists()
    {
        this.EnsureSymbolicLinkExists();
    }

    public async Task<bool> SetupDSOAL(DSOALInstallationStatus dSOALInstallationStatus)
    {
        if (this.IsInstalled)
        {
            return true;
        }

        if (!this.privilegeManager.AdminPrivileges)
        {
            this.privilegeManager.RequestAdminPrivileges<LauncherView>("Administrator privileges are required to install DSOAL");
            return false;
        }

        if ((await this.downloadService.DownloadFile(DownloadUrl, ArchiveName, dSOALInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to install DSOAL");
            return false;
        }

        this.logger.LogInformation("Extracting DSOAL files");
        dSOALInstallationStatus.CurrentStep = DSOALInstallationStatus.ExtractingFiles;

        this.ExtractFiles();
        dSOALInstallationStatus.CurrentStep = DSOALInstallationStatus.SetupOpenALFiles;
        
        this.SetupHrtfAndPresetFiles();
        dSOALInstallationStatus.CurrentStep = DSOALInstallationStatus.Finished;
        return true;
    }

    public IEnumerable<string> GetCustomArguments()
    {
        if (this.options.Value.Enabled)
        {
            return new List<string> { "-dsound" };
        }
        else
        {
            return Enumerable.Empty<string>();
        }
    }

    public Task OnGuildwarsStarted(Process process)
    {
        return Task.CompletedTask;
    }

    public Task OnGuildwarsStarting(Process process)
    {
        var guildwarsDirectory = new FileInfo(process.StartInfo.FileName).Directory!.FullName;
        if (this.options.Value.Enabled)
        {
            this.EnsureSymbolicLinkExists();
            EnsureFileExistsInGuildwarsDirectory(DsoundDll, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(DSOALAldrvDll, guildwarsDirectory);
            EnsureFileExistsInGuildwarsDirectory(AlsoftIni, guildwarsDirectory);
        }
        else
        {
            EnsureFileDoesNotExistInGuildwarsDirectory(DsoundDll, guildwarsDirectory);
            EnsureFileDoesNotExistInGuildwarsDirectory(DSOALAldrvDll, guildwarsDirectory);
            EnsureFileDoesNotExistInGuildwarsDirectory(AlsoftIni, guildwarsDirectory);
        }

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

    private void EnsureSymbolicLinkExists()
    {
        var openalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), OpenAlDirectory);
        if (!Directory.Exists(openalPath))
        {
            if (!this.privilegeManager.AdminPrivileges)
            {
                this.registryService.SaveValue(DSOALFixRegistryKey, true);
                this.privilegeManager.RequestAdminPrivileges<LauncherView>(DSOALFixAdminMessage);
                return;
            }

            Directory.CreateSymbolicLink(openalPath, Path.GetFullPath(DSOALDirectory));
            return;
        }

        var fi = new FileInfo(openalPath);
        var desiredPath = Path.GetFullPath(DSOALDirectory);
        if (fi.LinkTarget == desiredPath)
        {
            return;
        }

        if (!this.privilegeManager.AdminPrivileges)
        {
            this.registryService.SaveValue(DSOALFixRegistryKey, true);
            this.privilegeManager.RequestAdminPrivileges<LauncherView>(DSOALFixAdminMessage);
            return;
        }

        Directory.Delete(openalPath);
        Directory.CreateSymbolicLink(openalPath, Path.GetFullPath(DSOALDirectory));
    }

    private void ExtractFiles()
    {
        ZipFile.ExtractToDirectory(ArchiveName, Path.Combine(Directory.GetCurrentDirectory(), DSOALDirectory), true);
        ZipFile.ExtractToDirectory(Path.Combine(DSOALDirectory, HRTFArchiveName), Path.Combine(Directory.GetCurrentDirectory(), DSOALDirectory), true);
        var options = this.options.Value;
        options.Path = Path.GetFullPath(DSOALDirectory);
        this.options.UpdateOption();
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

        Directory.CreateSymbolicLink(openalPath, Path.GetFullPath(DSOALDirectory));
    }
}
