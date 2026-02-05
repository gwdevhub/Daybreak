using Daybreak.Configuration.Options;
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
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.NET;
using System.ComponentModel;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using static Daybreak.Shared.Utils.NativeMethods;

namespace Daybreak.Services.ApplicationLauncher;

// NOTE: ApplicationLauncher stays as-is. It orchestrates launching via IDaybreakInjector.
// For Linux support, IDaybreakInjector needs a platform-specific implementation that:
// 1. Sets up Wine prefix (WINEPREFIX environment variable)
// 2. Calls "wine Daybreak.Injector.exe <args>" instead of direct execution
// See DaybreakInjector.cs for the Windows implementation.
internal sealed class ApplicationLauncher(
    PhotinoWindow photinoWindow,
    IDaybreakInjector daybreakInjector,
    IGuildWarsExecutableManager guildWarsExecutableManager,
    INotificationService notificationService,
    IOptionsMonitor<LauncherOptions> launcherOptions,
    IModsManager modsManager,
    IPrivilegeManager privilegeManager,
    ILogger<ApplicationLauncher> logger) : IApplicationLauncher
{
    private const string SteamAppIdFile = "steam_appid.txt";
    private const string SteamAppId = "29720";
    private const string ProcessName = "gw";
    private const double LaunchMemoryThreshold = 200000000;

    private static readonly TimeSpan LaunchTimeout = TimeSpan.FromMinutes(1);

    private readonly PhotinoWindow photinoWindow = photinoWindow.ThrowIfNull();
    private readonly IDaybreakInjector daybreakInjector = daybreakInjector.ThrowIfNull();
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsMonitor<LauncherOptions> launcherOptions = launcherOptions.ThrowIfNull();
    private readonly IModsManager modsManager = modsManager.ThrowIfNull();
    private readonly ILogger<ApplicationLauncher> logger = logger.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();

    public async Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(LaunchConfigurationWithCredentials launchConfigurationWithCredentials, CancellationToken cancellationToken)
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        launchConfigurationWithCredentials.Credentials.ThrowIfNull();
        using var timeout = new CancellationTokenSource(LaunchTimeout);
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        var gwProcess = await this.LaunchGuildwarsProcess(launchConfigurationWithCredentials, cancellation.Token);
        if (gwProcess is null)
        {
            return default;
        }


        return new GuildWarsApplicationLaunchContext { LaunchConfiguration = launchConfigurationWithCredentials, GuildWarsProcess = gwProcess, ProcessId = (uint)gwProcess.Id };
    }

    public void RestartDaybreak()
    {
        if (this.privilegeManager.AdminPrivileges)
        {
            this.RestartDaybreakAsAdmin();
        }
        else
        {
            this.RestartDaybreakAsNormalUser();
        }
    }

    public void RestartDaybreakAsAdmin()
    {
        this.logger.LogInformation("Restarting daybreak with admin rights");
        var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
        if (processName!.IsNullOrWhiteSpace() || File.Exists(processName) is false)
        {
            throw new InvalidOperationException("Unable to find executable. Aborting restart");
        }

        var process = new Process()
        {
            StartInfo = new()
            {
                Verb = "runas",
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = true,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = $"/c cd /d \"{Environment.CurrentDirectory}\" && timeout /t 1 /nobreak && start \"\" \"{processName}\" && exit"
            }
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as admin");
        }

        this.photinoWindow.Close();
    }

    public void RestartDaybreakAsNormalUser()
    {
        this.logger.LogInformation("Restarting daybreak as normal user");
        var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
        if (processName!.IsNullOrWhiteSpace() || File.Exists(processName) is false)
        {
            throw new InvalidOperationException("Unable to find executable. Aborting restart");
        }

        var process = new Process()
        {
            StartInfo = new()
            {
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = processName,
                UseShellExecute = true,
                CreateNoWindow = true
            }
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as normal user");
        }

        this.photinoWindow.Close();
    }

    private async Task<Process?> LaunchGuildwarsProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var email = launchConfigurationWithCredentials.Credentials?.Username;
        var password = launchConfigurationWithCredentials.Credentials?.Password;
        var executable = launchConfigurationWithCredentials.ExecutablePath;
        if (executable is not null &&
            File.Exists(executable) is false)
        {
            throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
        }
        else if (executable is null)
        {
            var availableExecutables = this.guildWarsExecutableManager.GetExecutableList();
            var runningInstances = this.GetGuildwarsProcesses();
            var firstAvailableExecutable = availableExecutables.FirstOrDefault(e => !runningInstances.Any(p => Path.GetFullPath(p.MainModule?.FileName ?? string.Empty) == Path.GetFullPath(e)));
            if (firstAvailableExecutable is null)
            {
                this.notificationService.NotifyError(
                    title: "Can not launch Guild Wars",
                    description: "No available executables found. All executables are currently in use");
                return default;
            }

            executable = firstAvailableExecutable;
        }

        if (executable is null)
        {
            this.notificationService.NotifyError(
                    title: "Can not launch Guild Wars",
                    description: "No executable path provided and no available executables found");
            throw new InvalidOperationException("No executable path provided and no available executables found");
        }

        if (Path.GetDirectoryName(executable) is not string workingDirectory)
        {
            this.notificationService.NotifyError(
                title: "Can not launch Guild Wars",
                description: "Unable to determine executable directory");
            throw new InvalidOperationException("Unable to determine executable directory for Steam support");
        }

        var args = new List<string>();

        args.AddRange(PopulateCommandLineArgs("-email", email) ?? []);
        args.AddRange(PopulateCommandLineArgs("-password", password) ?? []);
        args.AddRange(PopulateCommandLineArgs("-character", "Daybreak") ?? []);

        foreach (var arg in launchConfigurationWithCredentials.Arguments?.Split(" ") ?? [])
        {
            args.Add(arg);
        }

        var mods = this.modsManager.GetMods().Where(m => m.IsEnabled && m.IsInstalled).ToList();
        var disabledmods = this.modsManager.GetMods().Where(m => !m.IsEnabled && m.IsInstalled).ToList();
        foreach (var mod in mods)
        {
            args.AddRange(mod.GetCustomArguments());
        }

        var identity = this.launcherOptions.CurrentValue.LaunchGuildwarsAsCurrentUser ?
            WindowsIdentity.GetCurrent().Name :
            WindowsIdentity.GetAnonymous().Name;
        scopedLogger.LogDebug("Launching guildwars as [{identity}] identity", identity);
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
            scopedLogger.LogDebug("Created {SteamAppIdFile} for Steam support at {steamAppIdFilePath}", SteamAppIdFile, steamAppIdFilePath);
        }
        else if (File.Exists(steamAppIdFilePath))
        {
            File.Delete(steamAppIdFilePath);
            scopedLogger.LogDebug("Deleted existing {SteamAppIdFile} to disable Steam support at {steamAppIdFilePath}", SteamAppIdFile, steamAppIdFilePath);
        }

        var applicationLauncherContext = new ApplicationLauncherContext { Process = process, ExecutablePath = executable, ProcessId = 0 };
        var guildWarsStartingDisabledContext = new GuildWarsStartingDisabledContext { ApplicationLauncherContext = applicationLauncherContext };
        foreach (var mod in disabledmods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStartingDisabled(guildWarsStartingDisabledContext, cts.Token);
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStartingDisabled)}");
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStartingDisabled)}");
                return default;
            }
        }

        var guildWarsStartingContext = new GuildWarsStartingContext { ApplicationLauncherContext = applicationLauncherContext, CancelStartup = false };
        foreach (var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStarting(guildWarsStartingContext, cts.Token);
                if (guildWarsStartingContext.CancelStartup)
                {
                    scopedLogger.LogError("{mod.Name} canceled startup", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}");
                    return default;
                }
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarting)}");
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarting)}");
                return default;
            }
        }

        var launchResult = await this.daybreakInjector.Launch(executable, this.privilegeManager.AdminPrivileges, [.. args], cancellationToken);
        if ((int)launchResult.ExitCode < 0)
        {
            scopedLogger.LogError("Failed to launch Guild Wars via injector with launch result {launchResult}", launchResult);
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: $"Injector failed to launch Guild Wars with launch result {launchResult}. Check logs for details");
            return default;
        }

        var threadHandle = launchResult.ThreadHandle;
        var pId = launchResult.ProcessId;
        if (threadHandle <= 0 || pId <= 0)
        {
            scopedLogger.LogError("Invalid process or thread handle returned from injector. ProcessId: {pId}, ThreadHandle: {threadHandle}", pId, threadHandle);
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: "Injector returned invalid process or thread handle. Check logs for details");
            return default;
        }

        // Reset launch context with the launched process
        process = Process.GetProcessById(pId);
        applicationLauncherContext = new ApplicationLauncherContext { ExecutablePath = executable, Process = process, ProcessId = (uint)pId };
        var guildWarsCreatedContext = new GuildWarsCreatedContext { ApplicationLauncherContext = applicationLauncherContext, CancelStartup = false };
        foreach (var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsCreated(guildWarsCreatedContext, cts.Token);
                if (guildWarsCreatedContext.CancelStartup)
                {
                    scopedLogger.LogError("{mod.Name} canceled startup", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}");
                    this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                    return default;
                }
            }
            catch (TaskCanceledException)
            {
                scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsCreated)}");
            }
            catch (Exception e)
            {
                this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsCreated)}");
                return default;
            }
        }

        var resumeResult = await this.daybreakInjector.Resume(threadHandle, cancellationToken);
        if (resumeResult < 0)
        {
            scopedLogger.LogError("Failed to resume Guild Wars process via injector with resume result {resumeResult}", resumeResult);
            this.notificationService.NotifyError(
                title: "Failed to launch Guild Wars",
                description: $"Injector failed to resume Guild Wars with resume result {resumeResult}. Check logs for details");
            return default;
        }

        var sw = Stopwatch.StartNew();
        while (sw.Elapsed.TotalSeconds < LaunchTimeout.TotalSeconds)
        {
            await Task.Delay(500, cancellationToken);
            if (process.HasExited)
            {
                scopedLogger.LogError("Guild Wars process exited before the main window was shown. Process ID: {process.Id}", process.Id);
                this.notificationService.NotifyError(
                    title: "Guild Wars process exited",
                    description: "Guild Wars process exited before the main window was shown. Please check logs for details");
                return default;
            }

            if (process.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            var windows = GetRootWindowsOfProcess(process.Id)
                .Select(root => (root, GetChildWindows(root)))
                .SelectMany(tuple =>
                {
                    tuple.Item2.Add(tuple.root);
                    return tuple.Item2;
                })
                .Select(GetWindowTitle).ToList();

            /*
             * Detect when the game window has shown. Because both the updater and the game window are called Guild Wars,
             * we need to look at the other windows created by the process. Especially, we need to detect the input windows
             * to check when the game is ready to accept input
             */
            if (!windows.Contains("Guild Wars Reforged"))
            {
                continue;
            }

            var virtualMemory = process.VirtualMemorySize64;
            if (virtualMemory < LaunchMemoryThreshold)
            {
                continue;
            }

            /*
             * Run the actions one by one, to avoid injection issues
             */
            var guildWarsStartedContext = new GuildWarsStartedContext { ApplicationLauncherContext = applicationLauncherContext };
            foreach (var mod in mods)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.CurrentValue.ModStartupTimeout));
                try
                {
                    await mod.OnGuildWarsStarted(guildWarsStartedContext, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    scopedLogger.LogError("{mod.Name} timeout", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} timeout",
                        description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarted)}");
                }
                catch (Exception e)
                {
                    this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                    scopedLogger.LogError(e, "{mod.Name} unhandled exception", mod.Name);
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} exception",
                        description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarted)}");
                    return default;
                }
            }

            return process;
        }

        throw new InvalidOperationException("Unable to launch Guild Wars process. Timed out waiting for the main window to launch");
    }

    public GuildWarsApplicationLaunchContext? GetGuildwarsProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();

        return GetGuildwarsProcessesInternal(launchConfigurationWithCredentials)
            .FirstOrDefault();
    }

    public IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcesses(params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();

        return GetGuildwarsProcessesInternal(launchConfigurationWithCredentials);
    }

    public IReadOnlyList<Process> GetGuildwarsProcesses()
    {
        return Process.GetProcessesByName(ProcessName);
    }

    public void KillGuildWarsProcess(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var process = guildWarsApplicationLaunchContext.GuildWarsProcess;
            if (process.MainModule?.FileName is not null &&
                process.MainModule.FileName.Contains("Gw.exe", StringComparison.OrdinalIgnoreCase))
            {
                process.Kill(true);
                return;
            }
        }
        catch (Win32Exception e) when (e.Message.Contains("Only part of a ReadProcessMemory or WriteProcessMemory request was completed"))
        {
            guildWarsApplicationLaunchContext.GuildWarsProcess.Kill();
        }
        catch (Win32Exception e) when (e.Message.Contains("Access is denied"))
        {
            scopedLogger.LogError(e, "Insuficient privileges to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}", guildWarsApplicationLaunchContext.ProcessId);
            Task.Run(() => this.privilegeManager.RequestAdminPrivileges<LaunchView>("Insufficient privileges to kill Guild Wars process. Please restart as administrator and try again.", default, CancellationToken.None));
        }
        catch (Exception e)
        {
            scopedLogger.LogError(e, "Failed to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}", guildWarsApplicationLaunchContext.ProcessId);
            this.notificationService.NotifyError(
                title: "Failed to kill GuildWars process",
                description: $"Encountered exception while trying to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}. Check logs for details");
        }
    }

    public IEnumerable<string> GetLoadedModules(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext)
    {
        if (guildWarsApplicationLaunchContext.GuildWarsProcess?.HasExited is false)
        {
            foreach (ProcessModule module in guildWarsApplicationLaunchContext.GuildWarsProcess.Modules)
            {
                yield return module.ModuleName;
            }
        }
    }

    public async Task<IEnumerable<IModService>> CheckMods(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext, CancellationToken cancellationToken)
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
                ExecutablePath = guildWarsApplicationLaunchContext.LaunchConfiguration.ExecutablePath,
                Process = guildWarsApplicationLaunchContext.GuildWarsProcess,
                ProcessId = guildWarsApplicationLaunchContext.ProcessId
            },
            LoadedModules = [.. this.GetLoadedModules(guildWarsApplicationLaunchContext)]
        };

        var modsNeedReapply = await this.modsManager.GetMods().ToAsyncEnumerable()
            .Where(async (m, ct) =>
            {
                try
                {
                    return await m.ShouldRunAgain(guildWarsRunningContext, ct);
                }
                catch (Exception e)
                {
                    scopedLogger.LogError(e, "Mod {modName} encountered an error while checking if it should run again", m.Name);
                    return false;
                }
            }).ToListAsync(cancellationToken);
        return modsNeedReapply;
    }

    public async Task<bool> ReapplyMods(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext, IEnumerable<IModService> mods, CancellationToken cancellationToken)
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
                ExecutablePath = guildWarsApplicationLaunchContext.LaunchConfiguration.ExecutablePath,
                Process = guildWarsApplicationLaunchContext.GuildWarsProcess,
                ProcessId = guildWarsApplicationLaunchContext.ProcessId
            },
            LoadedModules = [.. this.GetLoadedModules(guildWarsApplicationLaunchContext)]
        };

        foreach (var mod in mods)
        {
            try
            {
                await mod.OnGuildWarsRunning(guildWarsRunningContext, cancellationToken);
            }
            catch (Exception e)
            {
                scopedLogger.LogError(e, "Mod {modName} encountered an error while reapplying", mod.Name);
                this.notificationService.NotifyError(
                    title: $"{mod.Name} encountered an error",
                    description: $"Mod {mod.Name} encountered an error while reapplying. Check logs for details");
            }
        }

        return true;
    }

    private static IEnumerable<GuildWarsApplicationLaunchContext?> GetGuildwarsProcessesInternal(params LaunchConfigurationWithCredentials[] launchConfigurationWithCredentials)
    {
        return Process.GetProcessesByName(ProcessName)
            .Select(Process =>
            {
                (var AssociatedConfiguration, _, var ProcessId) = launchConfigurationWithCredentials
                    .Select(c => (c, ConfigurationMatchesProcess(c, Process, out var processId), processId))
                    .FirstOrDefault(c => c.Item2);
                return (Process, AssociatedConfiguration, ProcessId);
            })
            .Where(tuple => tuple.AssociatedConfiguration is not null)
            .Select(tuple => new GuildWarsApplicationLaunchContext
            {
                GuildWarsProcess = tuple.Process,
                LaunchConfiguration = tuple.AssociatedConfiguration!,
                ProcessId = tuple.ProcessId
            });
    }

    private static bool ConfigurationMatchesProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials, Process process, out uint processId)
    {
        try
        {
            processId = (uint)process.Id;
            return launchConfigurationWithCredentials.ExecutablePath == process.MainModule?.FileName;
        }
        catch (Win32Exception ex) when (ex.Message.Contains("Access is denied") || ex.Message.Contains("Only part of a ReadProcessMemory or WriteProcessMemory request was completed."))
        {
            processId = 0;
            /*
             * The process is running elevated. There is no way to use the standard C# libraries
             * to figure out what is the path of the running process.
             * We have to resort to low-leve Windows API to figure out the path of the elevated process.
             * We create a process snapshot and compare the paths
             */
            var hSnapshot = CreateToolhelp32Snapshot(0x00000002, 0); // 0x00000002 is the TH32CS_SNAPPROCESS flag
            var pe32 = new ProcessEntry32
            {
                dwSize = (uint)Marshal.SizeOf<ProcessEntry32>()
            };

            var nameBuffer = new StringBuilder(1024);
            if (Process32First(hSnapshot, ref pe32))
            {
                do
                {
                    var size = 1024U;
                    if (pe32.szExeFile == "Gw.exe")
                    {
                        var maybeDesiredProcessHandle = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, pe32.th32ProcessID);
                        if (QueryFullProcessImageName(maybeDesiredProcessHandle, 0, nameBuffer, ref size) &&
                            nameBuffer.ToString() == launchConfigurationWithCredentials.ExecutablePath)
                        {
                            processId = pe32.th32ProcessID;
                            CloseHandle(maybeDesiredProcessHandle);
                            return true;
                        }

                        CloseHandle(maybeDesiredProcessHandle);
                    }
                } while (Process32Next(hSnapshot, ref pe32));
            }

            CloseHandle(hSnapshot);
            return false;
        }
    }

    private static List<IntPtr> GetRootWindowsOfProcess(int pid)
    {
        var rootWindows = GetChildWindows(IntPtr.Zero);
        var dsProcRootWindows = new List<IntPtr>();
        foreach (IntPtr hWnd in rootWindows)
        {
            _ = GetWindowThreadProcessId(hWnd, out var lpdwProcessId);
            if (lpdwProcessId == pid)
                dsProcRootWindows.Add(hWnd);
        }

        return dsProcRootWindows;
    }

    private static List<IntPtr> GetChildWindows(IntPtr parent)
    {
        var result = new List<IntPtr>();
        var listHandle = GCHandle.Alloc(result);
        try
        {
            var childProc = new Win32Callback(EnumWindow);
            EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
                listHandle.Free();
        }

        return result;
    }

    private static string GetWindowTitle(IntPtr hwnd)
    {
        var titleLength = NativeMethods.GetWindowTextLength(hwnd);
        var titleBuffer = new StringBuilder(titleLength);
        _ = NativeMethods.GetWindowText(hwnd, titleBuffer, titleLength + 1);
        var title = titleBuffer.ToString();

        return title;
    }

    private static bool EnumWindow(IntPtr handle, IntPtr pointer)
    {
        var gch = GCHandle.FromIntPtr(pointer);
        if (gch.Target is not List<IntPtr> list)
        {
            return false;
        }

        list.Add(handle);
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
}
