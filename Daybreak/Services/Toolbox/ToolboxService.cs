using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Progress;
using Daybreak.Services.Downloads;
using Daybreak.Services.Scanner;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;

public sealed class ToolboxService : IToolboxService
{
    private const int MaxRetries = 10;
    private const string ToolboxLatestUri = "https://github.com/HasKha/GWToolboxpp/releases/download/6.0_Release/gwtoolbox.exe";
    private const string ExecutableName = "GWToolboxpp.exe";
    private const string ToolboxDestinationDirectory = "GWToolbox";

    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IDownloadService downloadService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ILiveUpdateableOptions<ToolboxOptions> toolboxOptions;
    private readonly ILogger<ToolboxService> logger;

    public bool IsEnabled
    {
        get => this.toolboxOptions.Value.Enabled;
        set
        {
            this.toolboxOptions.Value.Enabled = value;
            this.toolboxOptions.UpdateOption();
        }
    }
    public bool IsInstalled => File.Exists(this.toolboxOptions.Value.Path);

    public ToolboxService(
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IDownloadService downloadService,
        ILiveOptions<LauncherOptions> launcherOptions,
        ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
        ILogger<ToolboxService> logger)
    {
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.downloadService = downloadService.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.toolboxOptions = toolboxOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task OnGuildwarsStarting(Process process)
    {
        return Task.CompletedTask;
    }

    public async Task OnGuildwarsStarted(Process process)
    {
        await this.LaunchToolbox();
    }

    public IEnumerable<string> GetCustomArguments()
    {
        return Enumerable.Empty<string>();
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

    private async Task LaunchToolbox()
    {
        if (this.toolboxOptions.Value.Enabled is false)
        {
            return;
        }

        var executable = this.toolboxOptions.Value.Path;
        if (File.Exists(executable) is false)
        {
            throw new ExecutableNotFoundException($"GWToolbox executable doesn't exist at {executable}");
        }

        if (Process.GetProcessesByName("GWToolboxpp").FirstOrDefault() is Process)
        {
            this.logger.LogInformation("GWToolboxpp is already running");
            return;
        }

        // Try to detect when Guildwars has successfully launched and is on character selection screen
        for (var i = 0; i < 10; i++)
        {
            await this.guildwarsMemoryReader.EnsureInitialized(CancellationToken.None);
            var preGameData = await this.guildwarsMemoryReader.ReadPreGameData(CancellationToken.None);
            if (preGameData is null)
            {
                await Task.Delay(1000);
                continue;
            }

            break;
        }

        this.logger.LogInformation($"Launching GWToolbox");
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = executable
            }
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to launch {executable}");
        }

        var retries = 0;
        while (true)
        {
            await Task.Delay(100);
            retries++;
            var toolboxProcess = Process.GetProcessesByName("GWToolboxpp").FirstOrDefault();
            if (toolboxProcess is null && retries < MaxRetries)
            {
                continue;
            }
            else if (toolboxProcess is null && retries >= MaxRetries)
            {
                throw new InvalidOperationException("Newly launched GWToolbox process not detected");
            }

            if (toolboxProcess!.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            var titleLength = NativeMethods.GetWindowTextLength(toolboxProcess.MainWindowHandle);
            var titleBuffer = new StringBuilder(titleLength);
            _ = NativeMethods.GetWindowText(toolboxProcess.MainWindowHandle, titleBuffer, titleLength + 1);
            var title = titleBuffer.ToString();
            if (title != "GWToolbox - Launch")
            {
                continue;
            }

            return;
        }
    }
}
