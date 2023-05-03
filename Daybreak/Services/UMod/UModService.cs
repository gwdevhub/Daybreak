using Daybreak.Configuration.Options;
using Daybreak.Models;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;

public sealed class UModService : IUModService
{
    private const string DownloadUrl = "https://storage.googleapis.com/google-code-archive-downloads/v2/code.google.com/texmod/uMod_v1_r44.zip";
    private const string ArchiveName = "uMod_v1_r44.zip";
    private const string UModDirectory = "uMod";
    private const string D3D9Dll = "d3d9.dll";
    private const string D3D9DllBackup = "d3d9.dll.backup";
    private const string UModExecutable = "uMod.exe";
    private const string UModDefaultTemplateFile = "uMod_SaveFiles.txt";
    private const string UModModListFile = "ModList.txt";

    private readonly IDownloadService downloadService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<UModOptions> uModOptions;
    private readonly ILogger<UModService> logger;

    public bool UModExists => File.Exists(this.uModOptions.Value.Path);

    public bool Enabled
    {
        get => this.uModOptions.Value.Enabled;
        set
        {
            this.uModOptions.Value.Enabled = value;
            this.uModOptions.UpdateOption();
        }
    }

    public UModService(
        IDownloadService downloadService,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<UModOptions> uModOptions,
        ILogger<UModService> logger)
    {
        this.downloadService = downloadService.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.uModOptions = uModOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public bool LoadUModFromDisk()
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "Executable Files (*.exe)|*.exe",
            Multiselect = false,
            RestoreDirectory = true,
            Title = "Please select the uMod executable"
        };
        if (filePicker.ShowDialog() is false)
        {
            return false;
        }

        var fileName = filePicker.FileName;
        this.uModOptions.Value.Path = Path.GetFullPath(fileName);
        this.uModOptions.UpdateOption();
        return true;
    }

    public async Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus)
    {
        if ((await this.SetupUModExecutable(uModInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        uModInstallationStatus.CurrentStep = UModInstallationStatus.Installing;
        var maybeGuildwarsPath = this.launcherOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
        if (maybeGuildwarsPath is not GuildwarsPath guildwarsPath)
        {
            this.logger.LogError("No selected Guild Wars executable was found");
            return false;
        }

        var guildWarsDirectory = Path.GetDirectoryName(guildwarsPath.Path);
        var d3d9SourceFilePath = Path.Combine(UModDirectory, D3D9Dll);
        var d3d9DestinationFilePath = Path.Combine(guildWarsDirectory, D3D9Dll);
        if (MustBackupD3D9Dll(d3d9SourceFilePath, d3d9DestinationFilePath))
        {
            this.logger.LogInformation($"Found an existing {D3D9Dll} file. Saving it as {D3D9DllBackup}");
            var d3d9DestinationBackupFilePath = Path.Combine(guildWarsDirectory, D3D9DllBackup);
            if (File.Exists(d3d9DestinationBackupFilePath))
            {
                File.Delete(d3d9DestinationBackupFilePath);
            }

            File.Move(d3d9DestinationFilePath, d3d9DestinationBackupFilePath);
        }

        this.logger.LogInformation($"Copying {d3d9SourceFilePath} to {d3d9DestinationFilePath}");
        File.Copy(d3d9SourceFilePath, d3d9DestinationFilePath);

        uModInstallationStatus.CurrentStep = UModInstallationStatus.Finished;
        return true;
    }

    public async Task<bool> AddMod(string pathToTpf)
    {
        if (this.uModOptions.Value.AutoEnableMods is false)
        {
            return false;
        }

        var modListPath = Path.Combine(UModDirectory, UModModListFile);
        var tpfPath = Path.GetFullPath(pathToTpf);
        await File.AppendAllLinesAsync(modListPath, new string[] { $"Add_true:{tpfPath}" });
        return true;
    }

    private async Task<bool> SetupUModExecutable(UModInstallationStatus uModInstallationStatus)
    {
        if (File.Exists(this.uModOptions.Value.Path))
        {
            return true;
        }

        if ((await this.downloadService.DownloadFile(DownloadUrl, ArchiveName, uModInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to install uMod");
            return false;
        }

        ZipFile.ExtractToDirectory(ArchiveName, Directory.GetCurrentDirectory(), true);
        var uModOptions = this.uModOptions.Value;
        uModOptions.Path = Path.GetFullPath(Path.Combine(UModDirectory, UModExecutable));
        this.uModOptions.UpdateOption();
        await this.SetupDefaultTemplateFile();
        await SetupModListFile();
        return true;
    }

    private async Task SetupDefaultTemplateFile()
    {
        var maybeGuildwarsPath = this.launcherOptions.Value.GuildwarsPaths.Where(a => a.Default).FirstOrDefault();
        if (maybeGuildwarsPath is not GuildwarsPath guildwarsPath)
        {
            this.logger.LogError("Unable to create default template file. No guild wars executable found");
            return;
        }

        /*
         * uMod expects a modlist file with Add_true:Path_To_Tbf for each tbf.
         * To make uMod auto-load the template, it needs to be saved in uMod_SaveFiles.txt.
         * uMod_SaveFiles.txt has a weird format where each character is appended with \0.
         */
        var defaultTemplateFile = Path.Combine(UModDirectory, UModDefaultTemplateFile);
        var modListPath = Path.GetFullPath(Path.Combine(UModDirectory, UModModListFile));
        var gwPath = Path.GetFullPath(guildwarsPath.Path!);
        var entry = $"{gwPath}|{modListPath}\r\n";
        var finalSb = new StringBuilder();
        foreach(var c in entry)
        {
            finalSb.Append(c).Append('\0');
        }

        await File.WriteAllLinesAsync(defaultTemplateFile, new string[] { finalSb.ToString() } );
    }

    private static async Task SetupModListFile()
    {
        var modListPath = Path.Combine(UModDirectory, UModModListFile);
        await File.WriteAllTextAsync(modListPath, string.Empty);
    }

    private static bool MustBackupD3D9Dll(string sourcePath, string destinationPath)
    {
        if (!File.Exists(destinationPath))
        {
            return false;
        }

        var sourceInfo = new FileInfo(sourcePath);
        var destinationInfo = new FileInfo(destinationPath);
        return sourceInfo.Length != destinationInfo.Length ||
            sourceInfo.CreationTimeUtc != destinationInfo.CreationTimeUtc;
    }
}
