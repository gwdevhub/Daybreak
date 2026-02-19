using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using Daybreak.Configuration.Options;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Daybreak.Services.ApplicationLauncher;

internal sealed class ApplicationLauncher(
    IDaybreakInjector daybreakInjector,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    INotificationService notificationService,
    IOptionsMonitor<LauncherOptions> launcherOptions,
    IModsManager modsManager,
    IGuildWarsReadyChecker guildWarsReadyChecker,
    IGuildWarsProcessFinder guildWarsProcessFinder,
    IDaybreakRestartingService daybreakRestartingService,
    IPrivilegeManager privilegeManager,
    ILogger<ApplicationLauncher> logger
) : IApplicationLauncher
{
    private const string SteamAppIdFile = "steam_appid.txt";
    private const string SteamAppId = "29720";

    private static readonly TimeSpan LaunchTimeout = TimeSpan.FromMinutes(1);

    private readonly IDaybreakInjector daybreakInjector = daybreakInjector.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager =
        guildWarsExecutableManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsMonitor<LauncherOptions> launcherOptions =
        launcherOptions.ThrowIfNull();
    private readonly IModsManager modsManager = modsManager.ThrowIfNull();
    private readonly ILogger<ApplicationLauncher> logger = logger.ThrowIfNull();
    private readonly IGuildWarsReadyChecker guildWarsReadyChecker =
        guildWarsReadyChecker.ThrowIfNull();
    private readonly IGuildWarsProcessFinder guildWarsProcessFinder =
        guildWarsProcessFinder.ThrowIfNull();
    private readonly IDaybreakRestartingService daybreakRestartingService =
        daybreakRestartingService.ThrowIfNull();

    private readonly IPrivilegeManager privilegeManager =
        privilegeManager.ThrowIfNull();

    public async Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials,
        CancellationToken cancellationToken
    )
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        launchConfigurationWithCredentials.Credentials.ThrowIfNull();
        using var timeout = new CancellationTokenSource(LaunchTimeout);
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            timeout.Token
        );
        var (gwProcess, enabledMods) = await this.LaunchGuildwarsProcess(
            launchConfigurationWithCredentials,
            cancellation.Token
        );
        if (gwProcess is null)
        {
            return default;
        }

        return new GuildWarsApplicationLaunchContext
        {
            LaunchConfiguration = launchConfigurationWithCredentials,
            GuildWarsProcess = gwProcess,
            ProcessId = (uint)gwProcess.Id,
            EnabledMods = enabledMods,
        };
    }

    public void RestartDaybreak()
    {
        this.daybreakRestartingService.RestartDaybreak();
    }

    public void RestartDaybreakAsAdmin()
    {
        this.daybreakRestartingService.RestartDaybreakAsAdmin();
    }

    public void RestartDaybreakAsNormalUser()
    {
        this.daybreakRestartingService.RestartDaybreakAsNormalUser();
    }

    private async Task<(Process? Process, IReadOnlyList<IModService> EnabledMods)> LaunchGuildwarsProcess(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var email = launchConfigurationWithCredentials.Credentials?.Username;
        var password = launchConfigurationWithCredentials.Credentials?.Password;
        var executable = launchConfigurationWithCredentials.ExecutablePath;
        if (executable is not null && File.Exists(executable) is false)
        {
            throw new ExecutableNotFoundException(
                $"Guildwars executable doesn't exist at {executable}"
            );
        }
        else if (executable is null)
        {
            var availableExecutables = this.guildWarsExecutableManager.GetExecutableList();
            var runningInstances = this.GetGuildwarsProcesses();
            var firstAvailableExecutable = availableExecutables.FirstOrDefault(e =>
                FindFirstOrDefault(
                    runningInstances,
                    p =>
                        Path.GetFullPath(p.MainModule?.FileName ?? string.Empty)
                        == Path.GetFullPath(e)
                )
                    is null
            );
            if (firstAvailableExecutable is null)
            {
                this.notificationService.NotifyError(
                    title: "Can not launch Guild Wars",
                    description: "No available executables found. All executables are currently in use"
                );
                return (default, []);
            }

            executable = firstAvailableExecutable;
        }

        if (executable is null)
        {
            this.notificationService.NotifyError(
                title: "Can not launch Guild Wars",
                description: "No executable path provided and no available executables found"
            );
            throw new InvalidOperationException(
                "No executable path provided and no available executables found"
            );
        }

        if (Path.GetDirectoryName(executable) is not string workingDirectory)
        {
            this.notificationService.NotifyError(
                title: "Can not launch Guild Wars",
                description: "Unable to determine executable directory"
            );
            throw new InvalidOperationException(
                "Unable to determine executable directory for Steam support"
            );
        }

        var args = new List<string>();

        args.AddRange(PopulateCommandLineArgs("-email", email) ?? []);
        args.AddRange(PopulateCommandLineArgs("-password", password) ?? []);
        args.AddRange(PopulateCommandLineArgs("-character", "Daybreak") ?? []);

        foreach (var arg in launchConfigurationWithCredentials.Arguments?.Split(" ") ?? [])
        {
            args.Add(arg);
        }

        if (launchConfigurationWithCredentials.CustomModLoadoutEnabled)
        {
            scopedLogger.LogDebug("Custom mod loadout enabled. Overriding mod list with: {modList}", string.Join(',', launchConfigurationWithCredentials.EnabledMods ?? []));
        }

        var mods = this.modsManager.GetMods();
        var enabledMods = mods
            .Where(m =>
            {
                var willRun = launchConfigurationWithCredentials.CustomModLoadoutEnabled
                    ? !m.CanDisable || (launchConfigurationWithCredentials.EnabledMods?.Contains(m.Name) is true)
                    : m.IsEnabled;
                scopedLogger.LogDebug(
                        "Checking {ModName}.\nIsInstalled: {isInstalled}.\nCanDisable: {canDisable}.\nIsEnabled: {isEnabled}.\nWillRun: {willRun}",
                        m.Name, m.IsInstalled, m.CanDisable, m.IsEnabled, willRun);
                return willRun;
            })
            .ToList();

        var disabledMods = mods.Except(enabledMods);
        if (this.launcherOptions.CurrentValue.CancelLaunchOutOfDateMods)
        {
            foreach (var mod in enabledMods)
            {
                if (await mod.IsUpdateAvailable(cancellationToken))
                {
                    this.notificationService.NotifyError<NavigateToModsViewHandler>(
                        title: "Can not launch Guild Wars",
                        description: "Mods are out of date. Please update your mods to launch Guild Wars"
                    );

                    throw new InvalidOperationException("Mods are out of date");
                }
            }
        }

        foreach (var mod in enabledMods)
        {
            args.AddRange(mod.GetCustomArguments());
        }

        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                Arguments = string.Join(" ", args),
                FileName = executable,
            },
        };

        var steamAppIdFilePath = Path.Combine(workingDirectory, SteamAppIdFile);
        if (launchConfigurationWithCredentials.SteamSupport)
        {
            await File.WriteAllTextAsync(steamAppIdFilePath, SteamAppId, cancellationToken);
            scopedLogger.LogDebug(
                "Created {SteamAppIdFile} for Steam support at {steamAppIdFilePath}",
                SteamAppIdFile,
                steamAppIdFilePath
            );
        }
        else if (File.Exists(steamAppIdFilePath))
        {
            File.Delete(steamAppIdFilePath);
            scopedLogger.LogDebug(
                "Deleted existing {SteamAppIdFile} to disable Steam support at {steamAppIdFilePath}",
                SteamAppIdFile,
                steamAppIdFilePath
            );
        }

        var applicationLauncherContext = new ApplicationLauncherContext
        {
            Process = process,
            ExecutablePath = executable,
            ProcessId = 0,
        };
        var guildWarsStartingDisabledContext = new GuildWarsStartingDisabledContext
        {
            ApplicationLauncherContext = applicationLauncherContext,
        };
        foreach (var mod in disabledMods)
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout)
            );
            try
            {
                await mod.OnGuildWarsStartingDisabled(guildWarsStartingDisabledContext, cts.Token);
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStartingDisabled)}"
                );
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStartingDisabled)}"
                );
                return (default, []);
            }
        }

        var guildWarsStartingContext = new GuildWarsStartingContext
        {
            ApplicationLauncherContext = applicationLauncherContext,
            CancelStartup = false,
        };
        foreach (var mod in enabledMods)
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout)
            );
            try
            {
                await mod.OnGuildWarsStarting(guildWarsStartingContext, cts.Token);
                if (guildWarsStartingContext.CancelStartup)
                {
                    scopedLogger.LogError("{mod.Name} canceled startup", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}"
                    );
                    return (default, []);
                }
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarting)}"
                );
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarting)}"
                );
                return (default, []);
            }
        }

        var launchResult = await this.daybreakInjector.Launch(
            executable,
            this.privilegeManager.AdminPrivileges,
            [.. args],
            cancellationToken
        );
        if ((int)launchResult.ExitCode < 0)
        {
            scopedLogger.LogError(
                "Failed to launch Guild Wars via injector with launch result {launchResult}",
                launchResult
            );
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: $"Injector failed to launch Guild Wars with launch result {launchResult}. Check logs for details"
            );
            return (default, []);
        }

        var threadHandle = launchResult.ThreadHandle;
        var pId = launchResult.ProcessId;
        if (threadHandle <= 0 || pId <= 0)
        {
            scopedLogger.LogError(
                "Invalid process or thread handle returned from injector. ProcessId: {pId}, ThreadHandle: {threadHandle}",
                pId,
                threadHandle
            );
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: "Injector returned invalid process or thread handle. Check logs for details"
            );
            return (default, []);
        }

        // Reset launch context with the launched process
        process = Process.GetProcessById(pId);
        applicationLauncherContext = new ApplicationLauncherContext
        {
            ExecutablePath = executable,
            Process = process,
            ProcessId = (uint)pId,
        };
        var guildWarsCreatedContext = new GuildWarsCreatedContext
        {
            ApplicationLauncherContext = applicationLauncherContext,
            CancelStartup = false,
        };
        foreach (var mod in enabledMods)
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout)
            );
            try
            {
                await mod.OnGuildWarsCreated(guildWarsCreatedContext, cts.Token);
                if (guildWarsCreatedContext.CancelStartup)
                {
                    scopedLogger.LogError("{mod.Name} canceled startup", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}"
                    );
                    this.KillGuildWarsProcess(
                        new GuildWarsApplicationLaunchContext
                        {
                            GuildWarsProcess = process,
                            LaunchConfiguration = launchConfigurationWithCredentials,
                            ProcessId = (uint)pId,
                        }
                    );
                    return (default, []);
                }
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsCreated)}"
                );
            }
            catch (Exception e)
            {
                this.KillGuildWarsProcess(
                    new GuildWarsApplicationLaunchContext
                    {
                        GuildWarsProcess = process,
                        LaunchConfiguration = launchConfigurationWithCredentials,
                        ProcessId = (uint)pId,
                    }
                );
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsCreated)}"
                );
                return (default, []);
            }
        }

        var resumeResult = await this.daybreakInjector.Resume(threadHandle, cancellationToken);
        if (resumeResult < 0)
        {
            scopedLogger.LogError(
                "Failed to resume Guild Wars process via injector with resume result {resumeResult}",
                resumeResult
            );
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: $"Injector failed to resume Guild Wars with resume result {resumeResult}. Check logs for details"
            );
            return (default, []);
        }

        scopedLogger.LogDebug("Waiting for Guild Wars to be ready...");
        var sw = Stopwatch.StartNew();
        var isReady = await this.guildWarsReadyChecker.WaitForReady(
            process,
            LaunchTimeout - sw.Elapsed,
            cancellationToken
        );
        if (!isReady)
        {
            scopedLogger.LogError("Guild Wars process was not ready in time");
            this.notificationService.NotifyError(
                title: "Guild Wars process not ready",
                description: "Guild Wars process exited or timed out before becoming ready. Please check logs for details"
            );
            return (default, []);
        }

        /*
         * Run the actions one by one, to avoid injection issues
         */
        var guildWarsStartedContext = new GuildWarsStartedContext
        {
            ApplicationLauncherContext = applicationLauncherContext,
        };
        foreach (var mod in enabledMods)
        {
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout)
            );
            try
            {
                await mod.OnGuildWarsStarted(guildWarsStartedContext, cts.Token);
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarted)}"
                );
            }
            catch (Exception e)
            {
                this.KillGuildWarsProcess(
                    new GuildWarsApplicationLaunchContext
                    {
                        GuildWarsProcess = process,
                        LaunchConfiguration = launchConfigurationWithCredentials,
                        ProcessId = (uint)pId,
                    }
                );
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarted)}"
                );
                return (default, []);
            }
        }

        return (process, enabledMods);
    }

    public GuildWarsApplicationLaunchContext? GetGuildwarsProcess(
        LaunchConfigurationWithCredentials launchConfigurationWithCredentials
    )
    {
        launchConfigurationWithCredentials.ThrowIfNull();

        return this.guildWarsProcessFinder.FindProcess(launchConfigurationWithCredentials);
    }

    public IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcesses(
        params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials
    )
    {
        launchConfigurationWithCredentials.ThrowIfNull();

        return this.guildWarsProcessFinder.FindProcesses(launchConfigurationWithCredentials);
    }

    public ReadOnlyMemory<Process> GetGuildwarsProcesses()
    {
        return this.guildWarsProcessFinder.GetGuildWarsProcesses();
    }

    public void KillGuildWarsProcess(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var process = guildWarsApplicationLaunchContext.GuildWarsProcess;
            if (
                process.MainModule?.FileName is not null
                && process.MainModule.FileName.Contains(
                    "Gw.exe",
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                process.Kill(true);
                return;
            }
        }
        catch (Exception e)
            when (e.Message.Contains(
                    "Only part of a ReadProcessMemory or WriteProcessMemory request was completed"
                )
            )
        {
            guildWarsApplicationLaunchContext.GuildWarsProcess.Kill();
        }
        catch (Exception e) when (e.Message.Contains("Access is denied"))
        {
            scopedLogger.LogError(
                e,
                "Insuficient privileges to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}",
                guildWarsApplicationLaunchContext.ProcessId
            );
            Task.Run(() =>
                this.privilegeManager.RequestAdminPrivileges<LaunchView>(
                    "Insufficient privileges to kill Guild Wars process. Please restart as administrator and try again.",
                    default,
                    CancellationToken.None
                )
            );
        }
        catch (Exception e)
        {
            scopedLogger.LogError(
                e,
                "Failed to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}",
                guildWarsApplicationLaunchContext.ProcessId
            );
            this.notificationService.NotifyError(
                title: "Failed to kill GuildWars process",
                description: $"Encountered exception while trying to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}. Check logs for details"
            );
        }
    }

    public IEnumerable<string> GetLoadedModules(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext
    )
    {
        if (guildWarsApplicationLaunchContext.GuildWarsProcess?.HasExited is false)
        {
            foreach (
                ProcessModule module in guildWarsApplicationLaunchContext.GuildWarsProcess.Modules
            )
            {
                yield return module.ModuleName;
            }
        }
    }

    // TODO: Needs to be reworked to be cross-platform
    public async Task<IEnumerable<IModService>> CheckMods(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (guildWarsApplicationLaunchContext.LaunchConfiguration.ExecutablePath is null)
        {
            return [];
        }

        var guildWarsRunningContext = new GuildWarsRunningContext
        {
            ApplicationLauncherContext = new ApplicationLauncherContext
            {
                ExecutablePath = guildWarsApplicationLaunchContext
                    .LaunchConfiguration
                    .ExecutablePath,
                Process = guildWarsApplicationLaunchContext.GuildWarsProcess,
                ProcessId = guildWarsApplicationLaunchContext.ProcessId,
            },
            LoadedModules = [.. this.GetLoadedModules(guildWarsApplicationLaunchContext)],
        };

        var modsNeedReapply = await this
            .modsManager.GetMods()
            .ToAsyncEnumerable()
            .Where(
                async (m, ct) =>
                {
                    try
                    {
                        return await m.ShouldRunAgain(guildWarsRunningContext, ct);
                    }
                    catch (Exception e)
                    {
                        scopedLogger.LogError(
                            e,
                            "Mod {modName} encountered an error while checking if it should run again",
                            m.Name
                        );
                        return false;
                    }
                }
            )
            .ToListAsync(cancellationToken);
        return modsNeedReapply;
    }

    public async Task<bool> ReapplyMods(
        GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext,
        IEnumerable<IModService> mods,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (guildWarsApplicationLaunchContext.LaunchConfiguration.ExecutablePath is null)
        {
            return false;
        }

        var guildWarsRunningContext = new GuildWarsRunningContext
        {
            ApplicationLauncherContext = new ApplicationLauncherContext
            {
                ExecutablePath = guildWarsApplicationLaunchContext
                    .LaunchConfiguration
                    .ExecutablePath,
                Process = guildWarsApplicationLaunchContext.GuildWarsProcess,
                ProcessId = guildWarsApplicationLaunchContext.ProcessId,
            },
            LoadedModules = [.. this.GetLoadedModules(guildWarsApplicationLaunchContext)],
        };

        foreach (var mod in mods)
        {
            try
            {
                await mod.OnGuildWarsRunning(guildWarsRunningContext, cancellationToken);
            }
            catch (Exception e)
            {
                scopedLogger.LogError(
                    e,
                    "Mod {modName} encountered an error while reapplying",
                    mod.Name
                );
                this.notificationService.NotifyError(
                    title: $"{mod.Name} encountered an error",
                    description: $"Mod {mod.Name} encountered an error while reapplying. Check logs for details"
                );
            }
        }

        return true;
    }

    private static string[]? PopulateCommandLineArgs(string argName, string? argValue)
    {
        if (argValue is null || argValue.IsNullOrWhiteSpace())
        {
            return default;
        }

        return [argName, $"\"{argValue}\""];
    }

    private static Process? FindFirstOrDefault(
        ReadOnlyMemory<Process> processes,
        Func<Process, bool> selector
    )
    {
        var span = processes.Span;
        for (var i = 0; i < span.Length; i++)
        {
            if (selector(span[i]))
            {
                return span[i];
            }
        }

        return default;
    }
}
