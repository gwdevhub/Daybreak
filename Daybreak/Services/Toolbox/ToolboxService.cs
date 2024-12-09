using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.Mods;
using Daybreak.Models.Progress;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.Scanner;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Services.Toolbox.Notifications;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using PeNet.Header.Resource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;

internal sealed class ToolboxService : IToolboxService
{
    private const string ToolboxDestinationDirectorySubPath = "GWToolbox";
    private const string ToolboxBuildsFileName = "builds.ini";

    private static readonly string ToolboxDestinationDirectoryPath = PathUtils.GetAbsolutePathFromRoot(ToolboxDestinationDirectorySubPath);
    private static readonly string UsualToolboxFolderLocation = Path.GetFullPath(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GWToolboxpp"));
    private static readonly string UsualToolboxLocation = Path.GetFullPath(
        Path.Combine(UsualToolboxFolderLocation, "GWToolboxdll.dll"));

    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly INotificationService notificationService;
    private readonly IProcessInjector processInjector;
    private readonly IToolboxClient toolboxClient;
    private readonly ILiveUpdateableOptions<ToolboxOptions> toolboxOptions;
    private readonly ILogger<ToolboxService> logger;

    public string Name => "GWToolbox";
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
        IBuildTemplateManager buildTemplateManager,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        INotificationService notificationService,
        IProcessInjector processInjector,
        IToolboxClient toolboxClient,
        ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
        ILogger<ToolboxService> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.processInjector = processInjector.ThrowIfNull();
        this.toolboxClient = toolboxClient.ThrowIfNull();
        this.toolboxOptions = toolboxOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        await this.LaunchToolbox(guildWarsCreatedContext.ApplicationLauncherContext.Process, cancellationToken);

        /*
         * Toolbox startup conflicts with Daybreak GWCA integration. Wait some time
         * so that toolbox has the chance to set up its hooks.
         */
        await Task.Delay(TimeSpan.FromSeconds(this.toolboxOptions.Value.StartupDelay), cancellationToken);
    }

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public IEnumerable<string> GetCustomArguments() => [];

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

    public async IAsyncEnumerable<TeamBuildEntry> GetToolboxBuilds([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var toolboxBuildsFile = Path.Combine(UsualToolboxFolderLocation, Environment.MachineName, ToolboxBuildsFileName);
        if (!File.Exists(toolboxBuildsFile))
        {
            yield break;
        }

        using var textReader = new StreamReader(toolboxBuildsFile);
        TeamBuildEntry? teamBuild = default;
        while (await textReader.ReadLineAsync(cancellationToken) is string headerLine)
        {
            if (!headerLine.StartsWith('[') ||
                !headerLine.EndsWith(']') ||
                !headerLine.Contains("builds"))
            {
                continue;
            }

            var loadedBuild = false;
            while (await textReader.ReadLineAsync(cancellationToken) is string teamBuildLine)
            {
                if (string.IsNullOrWhiteSpace(teamBuildLine))
                {
                    break;
                }

                var equalSignIndex = teamBuildLine.IndexOf('=');
                var propertyName = teamBuildLine.Substring(0, equalSignIndex).Trim(' ');
                var propertyValue = teamBuildLine.Substring(equalSignIndex + 1, teamBuildLine.Length - equalSignIndex - 1).Trim(' ');
                if (propertyName is "buildname")
                {
                    teamBuild = this.buildTemplateManager.CreateTeamBuild(propertyValue);
                    teamBuild.Builds.Clear();
                }

                if (teamBuild is null)
                {
                    continue;
                }

                if (propertyName is "count" &&
                    int.TryParse(propertyValue, out var buildsCount) &&
                    buildsCount > 0)
                {
                    var buildsBuffer = new (string Template, string Name)[buildsCount];
                    for (var i = 0; i < buildsCount; i++)
                    {
                        buildsBuffer[i] = (string.Empty, string.Empty);
                    }

                    while (await textReader.ReadLineAsync(cancellationToken) is string singleBuildLine)
                    {
                        if (string.IsNullOrWhiteSpace(singleBuildLine))
                        {
                            break;
                        }

                        equalSignIndex = singleBuildLine.IndexOf('=');
                        propertyName = singleBuildLine.Substring(0, equalSignIndex).Trim(' ');
                        propertyValue = singleBuildLine.Substring(equalSignIndex + 1, singleBuildLine.Length - equalSignIndex - 1);
                        if (propertyName.StartsWith("name") &&
                            int.TryParse(propertyName.Replace("name", ""), out var singleBuildNameIndex))
                        {
                            buildsBuffer[singleBuildNameIndex].Name = propertyValue;
                        }
                        else if (propertyName.StartsWith("template") &&
                            int.TryParse(propertyName.Replace("template", ""), out var singleBuildTemplateIndex))
                        {
                            buildsBuffer[singleBuildTemplateIndex].Template = propertyValue;
                        }
                    }

                    if (buildsBuffer.Length == 0)
                    {
                        continue;
                    }

                    foreach ((var buildTemplate, var buildName) in buildsBuffer)
                    {
                        if (!this.buildTemplateManager.TryDecodeTemplate(buildTemplate, out var build) ||
                            build is not SingleBuildEntry singleBuild)
                        {
                            continue;
                        }

                        loadedBuild = true;
                        singleBuild.Name = buildName;
                        teamBuild?.Builds.Add(singleBuild);
                    }
                }

                if (loadedBuild)
                {
                    break;
                }
            }

            if (teamBuild is not null)
            {
                yield return teamBuild;
                teamBuild = default;
                loadedBuild = false;
            }
        }
    }

    public async Task NotifyUserIfUpdateAvailable(CancellationToken cancellationToken)
    {
        if (await this.IsUpdateAvailable(cancellationToken))
        {
            this.notificationService.NotifyInformation<ToolboxUpdateHandler>("GWToolboxpp update available", "Click on this notification to update GWToolboxpp");
        }
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
        var scopedLogger = this.logger.CreateScopedLogger();
        if (File.Exists(this.toolboxOptions.Value.DllPath) && !(await this.IsUpdateAvailable(CancellationToken.None)))
        {
            return true;
        }

        var result = await this.toolboxClient.DownloadLatestDll(toolboxInstallationStatus, UsualToolboxFolderLocation, CancellationToken.None);
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

    private async Task LaunchToolbox(Process process, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
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
        if (await this.processInjector.Inject(process, dll, cancellationToken))
        {
            scopedLogger.LogInformation("Injected toolbox dll");
            this.notificationService.NotifyInformation(
                title: "GWToolbox started",
                description: "GWToolbox has been injected. Delaying startup so that GWToolbox has time to initialize");
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

    private async Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        if (!File.Exists(this.toolboxOptions.Value.DllPath))
        {
            return true;
        }

        var latestVersion = await this.toolboxClient.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            return false;
        }

        var fileInfo = FileVersionInfo.GetVersionInfo(this.toolboxOptions.Value.DllPath);
        var current = fileInfo.ProductVersion?.Replace('_', '-');
        if (latestVersion.ToString().EndsWith("-release") &&
            current?.EndsWith("-release") is not true)
        {
            current += "-release";
        }

        if (!Daybreak.Models.Versioning.Version.TryParse(current, out var currentVersion))
        {
            return true;
        }

        if (latestVersion.CompareTo(currentVersion) > 0)
        {   
            return true;
        }

        return false;
    }
}
