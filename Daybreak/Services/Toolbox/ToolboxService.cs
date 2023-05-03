using Daybreak.Configuration.Options;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;

public sealed class ToolboxService : IToolboxService
{
    private const string ToolboxLatestUri = "https://github.com/HasKha/GWToolboxpp/releases/download/6.0_Release/gwtoolbox.exe";
    private const string ExecutableName = "GWToolboxpp.exe";
    private const string ToolboxDestinationDirectory = "GWToolbox";

    private readonly IDownloadService downloadService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<ToolboxOptions> toolboxOptions;
    private readonly ILogger<ToolboxService> logger;

    public bool ToolboxExists => File.Exists(this.toolboxOptions.Value.Path);

    public bool Enabled
    {
        get => this.toolboxOptions.Value.Enabled;
        set
        {
            this.toolboxOptions.Value.Enabled = value;
            this.toolboxOptions.UpdateOption();
        }
    }

    public ToolboxService(
        IDownloadService downloadService,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
        ILogger<ToolboxService> logger)
    {
        this.downloadService = downloadService.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.toolboxOptions = toolboxOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public bool LoadToolboxFromDisk()
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "Executable Files (*.exe)|*.exe",
            Multiselect = false,
            RestoreDirectory = true,
            Title = "Please select the GWToolboxpp executable"
        };
        if (filePicker.ShowDialog() is false)
        {
            return false;
        }

        var fileName = filePicker.FileName;
        this.toolboxOptions.Value.Path = Path.GetFullPath(fileName);
        this.toolboxOptions.UpdateOption();
        return true;
    }

    public async Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus)
    {
        if ((await this.SetupToolboxExecutable(toolboxInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        toolboxInstallationStatus.CurrentStep = ToolboxInstallationStatus.Finished;
        return true;
    }

    private async Task<bool> SetupToolboxExecutable(ToolboxInstallationStatus toolboxInstallationStatus)
    {
        if (File.Exists(this.toolboxOptions.Value.Path))
        {
            return true;
        }

        var destinationPath = Path.Combine(ToolboxDestinationDirectory, ExecutableName);
        if ((await this.downloadService.DownloadFile(ToolboxLatestUri, destinationPath, toolboxInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to install uMod");
            return false;
        }

        var toolboxOptions = this.toolboxOptions.Value;
        toolboxOptions.Path = Path.GetFullPath(destinationPath);
        this.toolboxOptions.UpdateOption();
        return true;
    }
}
