using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Progress;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.Scanner;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Services.Toolbox.Utilities;
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
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;

internal sealed class ToolboxService : IToolboxService
{
    private const string ToolboxDestinationDirectory = "GWToolbox";

    private static readonly string UsualToolboxLocation = Path.GetFullPath(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GWToolboxpp", "GWToolboxdll.dll"));

    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly INotificationService notificationService;
    private readonly IProcessInjector processInjector;
    private readonly IToolboxClient toolboxClient;
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
    public bool IsInstalled => File.Exists(this.toolboxOptions.Value.DllPath);

    public ToolboxService(
        IGuildwarsMemoryCache guildwarsMemoryCache,
        INotificationService notificationService,
        IProcessInjector processInjector,
        IToolboxClient toolboxClient,
        ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
        ILogger<ToolboxService> logger)
    {
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.processInjector = processInjector.ThrowIfNull();
        this.toolboxClient = toolboxClient.ThrowIfNull();
        this.toolboxOptions = toolboxOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task OnGuildwarsStarting(Process process) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(Process process)
    {
        await this.LaunchToolbox(process);
    }

    public Task OnGuildwarsStarted(Process process) => Task.CompletedTask;

    public IEnumerable<string> GetCustomArguments()
    {
        return Enumerable.Empty<string>();
    }

    public bool LoadToolboxFromUsualLocation()
    {
        if (!File.Exists(UsualToolboxLocation))
        {
            return false;
        }

        this.toolboxOptions.Value.DllPath = UsualToolboxLocation;
        this.toolboxOptions.UpdateOption();
        return true;
    }

    public bool LoadToolboxFromDisk()
    {
        var filePicker = new OpenFileDialog
        {
            Filter = "GWToolboxdll (GWToolboxdll.dll)|GWToolboxdll.dll",
            Multiselect = false,
            RestoreDirectory = true,
            Title = "Please select the GWToolboxdll dll"
        };
        if (filePicker.ShowDialog() is false)
        {
            return false;
        }

        var fileName = filePicker.FileName;
        this.toolboxOptions.Value.DllPath = Path.GetFullPath(fileName);
        this.toolboxOptions.UpdateOption();
        return true;
    }

    public async Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus)
    {
        if ((await this.SetupToolboxDll(toolboxInstallationStatus)) is false)
        {
            this.logger.LogError("Failed to setup the uMod executable");
            return false;
        }

        toolboxInstallationStatus.CurrentStep = ToolboxInstallationStatus.Finished;
        return true;
    }

    private async Task<bool> SetupToolboxDll(ToolboxInstallationStatus toolboxInstallationStatus)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.SetupToolboxDll), string.Empty);
        if (File.Exists(this.toolboxOptions.Value.DllPath))
        {
            return true;
        }

        var result = await this.toolboxClient.DownloadLatestDll(toolboxInstallationStatus, CancellationToken.None);
        _ = result switch
        {
            DownloadLatestOperation.Success => scopedLogger.LogInformation(result.Message),
            DownloadLatestOperation.NonSuccessStatusCode => scopedLogger.LogError(result.Message),
            DownloadLatestOperation.NoVersionFound => scopedLogger.LogError(result.Message),
            DownloadLatestOperation.ExceptionEncountered exceptionResult => scopedLogger.LogError(exceptionResult.Exception, exceptionResult.Message),
            _ => throw new InvalidOperationException("Unexpected result")
        };

        if (result is not DownloadLatestOperation.Success success)
        {
            return false;
        }

        var toolboxOptions = this.toolboxOptions.Value;
        toolboxOptions.DllPath = Path.GetFullPath(success.PathToDll);
        this.toolboxOptions.UpdateOption();
        return true;
    }

    private async Task LaunchToolbox(Process process)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.LaunchToolbox), string.Empty);
        if (this.toolboxOptions.Value.Enabled is false)
        {
            scopedLogger.LogInformation("Toolbox disabled");
            return;
        }

        var dll = this.toolboxOptions.Value.DllPath;
        if (File.Exists(dll) is false)
        {
            scopedLogger.LogError("Dll file does not exist");
            throw new ExecutableNotFoundException($"GWToolbox dll doesn't exist at {dll}");
        }

        scopedLogger.LogInformation("Injecting toolbox dll");
        if (this.processInjector.Inject(process, dll))
        {
            scopedLogger.LogInformation("Injected toolbox dll");
            this.notificationService.NotifyInformation(
                title: "GWToolbox started",
                description: "GWToolbox has been injected");
        }
        else
        {
            scopedLogger.LogError("Failed to inject toolbox dll");
            this.notificationService.NotifyError(
                title: "GWToolbox failed to start",
                description: "Failed to inject GWToolbox");
        }

        return;
    }
}
