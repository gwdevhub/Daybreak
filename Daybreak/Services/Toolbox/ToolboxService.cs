using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.Builds;
using Daybreak.Models.Mods;
using Daybreak.Models.Progress;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Injection;
using Daybreak.Services.Notifications;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Services.Toolbox.Notifications;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;

internal sealed class ToolboxService(
    IBuildTemplateManager buildTemplateManager,
    INotificationService notificationService,
    IProcessInjector processInjector,
    IToolboxClient toolboxClient,
    ILiveUpdateableOptions<ToolboxOptions> toolboxOptions,
    ILogger<ToolboxService> logger) : IToolboxService
{
    private const string ToolboxDestinationDirectorySubPath = "GWToolbox";
    private const string ToolboxBuildsFileName = "builds.ini";

    private static readonly string ToolboxDestinationDirectoryPath = PathUtils.GetAbsolutePathFromRoot(ToolboxDestinationDirectorySubPath);
    private static readonly string UsualToolboxFolderLocation = Path.GetFullPath(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GWToolboxpp"));
    private static readonly string UsualToolboxLocation = Path.GetFullPath(
        Path.Combine(UsualToolboxFolderLocation, "GWToolboxdll.dll"));

    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IProcessInjector processInjector = processInjector.ThrowIfNull();
    private readonly IToolboxClient toolboxClient = toolboxClient.ThrowIfNull();
    private readonly ILiveUpdateableOptions<ToolboxOptions> toolboxOptions = toolboxOptions.ThrowIfNull();
    private readonly ILogger<ToolboxService> logger = logger.ThrowIfNull();

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

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => this.LaunchToolbox(guildWarsStartedContext.ApplicationLauncherContext.Process, cancellationToken);

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

    public async IAsyncEnumerable<TeamBuildEntry> GetToolboxBuilds([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var toolboxBuildsFile = Path.Combine(UsualToolboxFolderLocation, Environment.MachineName, ToolboxBuildsFileName);
        if (!File.Exists(toolboxBuildsFile))
        {
            yield break;
        }

        using var textReader = new StreamReader(toolboxBuildsFile);
        await foreach (var build in this.ParseToolboxBuilds(textReader, cancellationToken))
        {
            yield return build;
        }
    }

    public async Task<bool> SaveToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken)
    {
        if (!teamBuildEntry.IsToolboxBuild)
        {
            return false;
        }

        if (!teamBuildEntry.ToolboxBuildId.HasValue)
        {
            return false;
        }

        var toolboxBuildsFile = Path.Combine(UsualToolboxFolderLocation, Environment.MachineName, ToolboxBuildsFileName);
        if (!File.Exists(toolboxBuildsFile))
        {
            return false;
        }

        var buildsToSave = new List<TeamBuildEntry>();
        using var textReader = new StreamReader(new FileStream(toolboxBuildsFile, FileMode.Open, FileAccess.Read));
        await foreach(var build in this.ParseToolboxBuilds(textReader, cancellationToken))
        {
            if (!build.ToolboxBuildId.HasValue)
            {
                continue;
            }

            if (build.ToolboxBuildId.Value == teamBuildEntry.ToolboxBuildId.Value)
            {
                buildsToSave.Add(teamBuildEntry);
            }
            else
            {
                buildsToSave.Add(build);
            }
        }

        textReader.Close();
        textReader.Dispose();
        using var textWriter = new StreamWriter(new FileStream(toolboxBuildsFile, FileMode.Create, FileAccess.Write));
        return await this.WriteBuildsToStream(buildsToSave, textWriter, cancellationToken);
    }

    public async Task<bool> DeleteToolboxBuild(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken)
    {
        if (!teamBuildEntry.IsToolboxBuild)
        {
            return false;
        }

        if (!teamBuildEntry.ToolboxBuildId.HasValue)
        {
            return false;
        }

        var toolboxBuildsFile = Path.Combine(UsualToolboxFolderLocation, Environment.MachineName, ToolboxBuildsFileName);
        if (!File.Exists(toolboxBuildsFile))
        {
            return false;
        }

        var buildsToSave = new List<TeamBuildEntry>();
        using var textReader = new StreamReader(new FileStream(toolboxBuildsFile, FileMode.Open, FileAccess.Read));
        await foreach (var build in this.ParseToolboxBuilds(textReader, cancellationToken))
        {
            if (!build.ToolboxBuildId.HasValue)
            {
                continue;
            }

            if (build.ToolboxBuildId.Value == teamBuildEntry.ToolboxBuildId.Value)
            {
                continue;
            }

            buildsToSave.Add(build);
        }

        textReader.Close();
        textReader.Dispose();
        using var textWriter = new StreamWriter(new FileStream(toolboxBuildsFile, FileMode.Create, FileAccess.Write));
        return await this.WriteBuildsToStream(buildsToSave, textWriter, cancellationToken);
    }

    public async Task<bool> ExportBuildToToolbox(TeamBuildEntry teamBuildEntry, CancellationToken cancellationToken)
    {
        if (teamBuildEntry.IsToolboxBuild)
        {
            return await this.SaveToolboxBuild(teamBuildEntry, cancellationToken);
        }

        var toolboxBuildsFile = Path.Combine(UsualToolboxFolderLocation, Environment.MachineName, ToolboxBuildsFileName);
        if (!File.Exists(toolboxBuildsFile))
        {
            return false;
        }

        var buildsToSave = new List<TeamBuildEntry>();
        using var textReader = new StreamReader(new FileStream(toolboxBuildsFile, FileMode.Open, FileAccess.Read));
        await foreach (var build in this.ParseToolboxBuilds(textReader, cancellationToken))
        {
            if (!build.ToolboxBuildId.HasValue)
            {
                continue;
            }

            buildsToSave.Add(build);
        }

        buildsToSave.Add(teamBuildEntry);
        textReader.Close();
        textReader.Dispose();
        using var textWriter = new StreamWriter(new FileStream(toolboxBuildsFile, FileMode.Create, FileAccess.Write));
        return await this.WriteBuildsToStream(buildsToSave, textWriter, cancellationToken);
    }

    private async Task<bool> SetupToolboxDll(ToolboxInstallationStatus toolboxInstallationStatus)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (File.Exists(this.toolboxOptions.Value.DllPath) && !await this.IsUpdateAvailable(CancellationToken.None))
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

    private async IAsyncEnumerable<TeamBuildEntry> ParseToolboxBuilds(TextReader textReader, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        TeamBuildEntry? teamBuild = default;
        while (await textReader.ReadLineAsync(cancellationToken) is string headerLine)
        {
            if (!headerLine.StartsWith('[') ||
                !headerLine.EndsWith(']') ||
                !headerLine.Contains("builds"))
            {
                continue;
            }

            if (!int.TryParse(headerLine.Replace("[builds", "").Replace("]", ""), out var buildId))
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
                var propertyName = teamBuildLine[..equalSignIndex].Trim(' ');
                var propertyValue = teamBuildLine[(equalSignIndex + 1)..].Trim(' ');
                if (propertyName is "buildname")
                {
                    teamBuild = this.buildTemplateManager.CreateTeamBuild(propertyValue);
                    teamBuild.Builds.Clear();
                    teamBuild.IsToolboxBuild = true;
                    teamBuild.ToolboxBuildId = buildId;
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
                        propertyName = singleBuildLine[..equalSignIndex].Trim(' ');
                        propertyValue = singleBuildLine[(equalSignIndex + 1)..];
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
            }
        }
    }

    private async Task<bool> WriteBuildsToStream(List<TeamBuildEntry> builds, TextWriter textWriter, CancellationToken cancellationToken)
    {
        for (var i = 0; i < builds.Count; i++)
        {
            var build = builds[i];
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[builds{i:D3}]");
            stringBuilder.AppendLine($"buildname = {build.Name}");
            stringBuilder.AppendLine($"showNumbers = true");
            stringBuilder.AppendLine($"count = {build.Builds.Count}");
            for (var j = 0; j < build.Builds.Count; j++)
            {
                var singleBuild = build.Builds[j];
                stringBuilder.AppendLine($"name{j} = {singleBuild.Name}");
                stringBuilder.AppendLine($"template{j} = {this.buildTemplateManager.EncodeTemplate(singleBuild)}");
            }

            stringBuilder.AppendLine();
            await textWriter.WriteLineAsync(stringBuilder, cancellationToken);
        }

        return true;
    }
}
