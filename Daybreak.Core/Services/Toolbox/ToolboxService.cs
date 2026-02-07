using Daybreak.Configuration.Options;
using Daybreak.Services.Toolbox.Models;
using Daybreak.Utils;
using Daybreak.Services.Toolbox.Notifications;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Toolbox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using System.Text;

namespace Daybreak.Services.Toolbox;

internal sealed class ToolboxService(
    IOptionsProvider optionsProvider,
    IBuildTemplateManager buildTemplateManager,
    INotificationService notificationService,
    IProcessInjector processInjector,
    IToolboxClient toolboxClient,
    IOptionsMonitor<ToolboxOptions> toolboxOptions,
    ILogger<ToolboxService> logger) : IToolboxService
{
    private const string ToolboxDllName = "GWToolboxdll.dll";
    private const string ToolboxBuildsFileName = "builds.ini";

    private static readonly ProgressUpdate ProgressStarting = new(0, "Starting GWToolboxpp setup");
    private static readonly ProgressUpdate ProgressFinished = new(1, "GWToolboxpp setup complete");
    private static readonly ProgressUpdate ProgressFailed = new(1, "Failed to setup GWToolboxpp");
    private static readonly string UsualToolboxFolderLocation = Path.GetFullPath(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GWToolboxpp"));
    private static readonly string UsualToolboxLocation = Path.GetFullPath(
        Path.Combine(UsualToolboxFolderLocation, ToolboxDllName));

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IBuildTemplateManager buildTemplateManager = buildTemplateManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IProcessInjector processInjector = processInjector.ThrowIfNull();
    private readonly IToolboxClient toolboxClient = toolboxClient.ThrowIfNull();
    private readonly IOptionsMonitor<ToolboxOptions> toolboxOptions = toolboxOptions.ThrowIfNull();
    private readonly ILogger<ToolboxService> logger = logger.ThrowIfNull();

    public string Name => "GWToolbox";
    public string Description => "GWToolboxpp is a popular third-party mod for Guild Wars that enhances the game's user interface and provides various quality-of-life improvements.";
    public bool IsVisible => true;
    public bool CanCustomManage => false;
    public bool CanUninstall => true;
    public bool IsEnabled
    {
        get => this.toolboxOptions.CurrentValue.Enabled;
        set
        {
            var options = this.toolboxOptions.CurrentValue;
            options.Enabled = value;
            this.optionsProvider.SaveOption(options);
        }
    }
    public bool IsInstalled => File.Exists(UsualToolboxLocation);

    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
    {
        return this.IsUpdateAvailableInternal(cancellationToken);
    }

    public async Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        return await this.PerformInstallation(cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(progress =>
        {
            progress.Report(new ProgressUpdate(0, "Uninstalling Toolbox"));
            if (!this.IsInstalled)
            {
                progress.Report(new ProgressUpdate(1, "Toolbox is not installed"));
                return Task.FromResult(true);
            }

            if (!File.Exists(UsualToolboxLocation))
            {
                progress.Report(new ProgressUpdate(1, "Toolbox is not installed"));
                return Task.FromResult(true);
            }

            File.Delete(UsualToolboxLocation);
            progress.Report(new ProgressUpdate(1, "Toolbox uninstalled successfully"));
            return Task.FromResult(true);
        }, cancellationToken);
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(async progress => await Task.Factory.StartNew(() => this.SetupToolbox(progress, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap(), cancellationToken);
    }

    public Task OnCustomManagement(CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Toolbox mod does not support custom management");
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
    {
        if (!guildWarsRunningContext.LoadedModules.Contains(ToolboxDllName) &&
            this.IsEnabled)
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
    {
        return this.LaunchToolbox(guildWarsRunningContext.ApplicationLauncherContext.Process, cancellationToken);
    }

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken) => this.LaunchToolbox(guildWarsStartedContext.ApplicationLauncherContext.Process, cancellationToken);

    public IEnumerable<string> GetCustomArguments() => [];

    public async Task NotifyUserIfUpdateAvailable(CancellationToken cancellationToken)
    {
        if (await this.IsUpdateAvailableInternal(cancellationToken))
        {
            this.notificationService.NotifyInformation<ToolboxUpdateHandler>("GWToolboxpp update available", "Click on this notification to update GWToolboxpp", expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(15));
        }
    }

    public async Task<bool> SetupToolbox(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        progress.Report(ProgressStarting);
        if (!await this.SetupToolboxDll(progress, cancellationToken))
        {
            this.logger.LogError("Failed to setup the uMod executable");
            progress.Report(ProgressFailed);
            return false;
        }

        progress.Report(ProgressFinished);
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
        await foreach (var build in this.ParseToolboxBuilds(textReader, cancellationToken))
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
        await using var textWriter = new StreamWriter(new FileStream(toolboxBuildsFile, FileMode.Create, FileAccess.Write));
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
        await using var textWriter = new StreamWriter(new FileStream(toolboxBuildsFile, FileMode.Create, FileAccess.Write));
        return await this.WriteBuildsToStream(buildsToSave, textWriter, cancellationToken);
    }

    private async Task<bool> SetupToolboxDll(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (File.Exists(UsualToolboxLocation) && !await this.IsUpdateAvailableInternal(cancellationToken))
        {
            return true;
        }

        var result = await this.toolboxClient.DownloadLatestDll(progress, UsualToolboxFolderLocation, cancellationToken);
        _ = result switch
        {
            DownloadLatestOperation.Success => scopedLogger.LogDebug(result.Message),
            DownloadLatestOperation.NonSuccessStatusCode => scopedLogger.LogError(result.Message),
            DownloadLatestOperation.NoVersionFound => scopedLogger.LogError(result.Message),
            DownloadLatestOperation.ExceptionEncountered exceptionResult => scopedLogger.LogError(exceptionResult.Exception, exceptionResult.Message),
            _ => throw new InvalidOperationException("Unexpected result")
        };

        return result is DownloadLatestOperation.Success;
    }

    private async Task<bool> IsUpdateAvailableInternal(CancellationToken cancellationToken)
    {
        if (!File.Exists(UsualToolboxLocation))
        {
            return true;
        }

        var latestVersion = await this.toolboxClient.GetLatestVersion(cancellationToken);
        if (latestVersion is null)
        {
            return false;
        }

        var current = PeVersionReader.GetProductVersion(UsualToolboxLocation)?.Replace('_', '-');
        if (latestVersion.ToString().EndsWith("-release") &&
            current?.EndsWith("-release") is not true)
        {
            current += "-release";
        }

        if (!Version.TryParse(current, out var currentVersion))
        {
            return true;
        }

        if (latestVersion.CompareTo(currentVersion) > 0)
        {
            return true;
        }

        return false;
    }

    private async Task LaunchToolbox(Process process, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.IsEnabled)
        {
            scopedLogger.LogDebug("Toolbox disabled");
            return;
        }

        var dll = UsualToolboxLocation;
        if (!File.Exists(dll))
        {
            scopedLogger.LogError("Dll file does not exist");
            throw new ExecutableNotFoundException($"GWToolbox dll doesn't exist at {dll}");
        }

        scopedLogger.LogDebug("Injecting toolbox dll");
        if (await this.processInjector.Inject(process, dll, cancellationToken))
        {
            scopedLogger.LogDebug("Injected toolbox dll");
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
            stringBuilder.AppendLine("showNumbers = true");
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
