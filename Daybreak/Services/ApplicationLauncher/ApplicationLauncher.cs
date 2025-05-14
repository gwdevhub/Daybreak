using Daybreak.Configuration.Options;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Mutex;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Daybreak.Shared.Utils.NativeMethods;

namespace Daybreak.Services.ApplicationLauncher;

internal sealed class ApplicationLauncher(
    INotificationService notificationService,
    ILiveOptions<LauncherOptions> launcherOptions,
    IMutexHandler mutexHandler,
    IModsManager modsManager,
    ILogger<ApplicationLauncher> logger,
    IPrivilegeManager privilegeManager) : IApplicationLauncher
{
    private const string ProcessName = "gw";
    private const string ArenaNetMutex = "AN-Mute";
    private const double LaunchMemoryThreshold = 200000000;

    private static readonly TimeSpan LaunchTimeout = TimeSpan.FromMinutes(1);

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILiveOptions<LauncherOptions> launcherOptions = launcherOptions.ThrowIfNull();
    private readonly IMutexHandler mutexHandler = mutexHandler.ThrowIfNull();
    private readonly IModsManager modsManager = modsManager.ThrowIfNull();
    private readonly ILogger<ApplicationLauncher> logger = logger.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();

    public async Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        launchConfigurationWithCredentials.Credentials!.ThrowIfNull();
        var configuration = this.launcherOptions.Value;
        if (configuration.MultiLaunchSupport is true)
        {
            if (this.privilegeManager.AdminPrivileges is false)
            {
                this.privilegeManager.RequestAdminPrivileges<LauncherView>("You need administrator rights in order to start using multi-launch");
                return null;
            }

            if (!this.ClearGwLocks(launchConfigurationWithCredentials.ExecutablePath!.ThrowIfNull()))
            {
                this.logger.LogError("Failed to clear GW locks. Canceling GuildWars launch");
                return null;
            }
        }
        else if (this.GetGuildwarsProcesses().Any())
        {
            this.notificationService.NotifyError(
                title: "Can not launch Guild Wars",
                description: "Multi-launch is disabled. Can not launch another instance of Guild Wars while the current one is running");
            return null;
        }

        using var timeout = new CancellationTokenSource(LaunchTimeout);
        var gwProcess = await this.LaunchGuildwarsProcess(launchConfigurationWithCredentials, timeout.Token);
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

        Application.Current.Shutdown();
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
                FileName = "cmd.exe",
                UseShellExecute = true,
                CreateNoWindow = true,
                Arguments = $"/c cd /d \"{Environment.CurrentDirectory}\" && timeout /t 1 /nobreak && runas /machine:x86 /trustlevel:0x20000 \"{processName}\" && exit"
            }
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as normal user");
        }

        Application.Current.Shutdown();
    }

    private async Task<Process?> LaunchGuildwarsProcess(LaunchConfigurationWithCredentials launchConfigurationWithCredentials, CancellationToken cancellationToken)
    {
        var email = launchConfigurationWithCredentials.Credentials!.Username;
        var password = launchConfigurationWithCredentials.Credentials!.Password;
        var executable = launchConfigurationWithCredentials.ExecutablePath!;
        if (File.Exists(executable) is false)
        {
            throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
        }

        var args = new List<string>()
        {
            "-email",
            $"\"{email}\"",
            "-password",
            $"\"{password}\"",
            "-character",
            "\"Daybreak\""
        };

        foreach(var arg in launchConfigurationWithCredentials.Arguments?.Split(" ") ?? [])
        {
            args.Add(arg);
        }

        var mods = this.modsManager.GetMods().Where(m => m.IsEnabled && m.IsInstalled).ToList();
        var disabledmods = this.modsManager.GetMods().Where(m => !m.IsEnabled && m.IsInstalled).ToList();
        foreach(var mod in mods)
        {
            args.AddRange(mod.GetCustomArguments());
        }

        var identity = this.launcherOptions.Value.LaunchGuildwarsAsCurrentUser ?
            WindowsIdentity.GetCurrent().Name :
            WindowsIdentity.GetAnonymous().Name;
        this.logger.LogInformation($"Launching guildwars as [{identity}] identity");
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                Arguments = string.Join(" ", args),
                FileName = executable,
            },
        };

        var applicationLauncherContext = new ApplicationLauncherContext { Process = process, ExecutablePath = executable, ProcessId = 0 };
        var guildWarsStartingDisabledContext = new GuildWarsStartingDisabledContext { ApplicationLauncherContext = applicationLauncherContext };
        foreach(var mod in disabledmods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStartingDisabled(guildWarsStartingDisabledContext, cts.Token);
            }
            catch (TaskCanceledException)
            {
                this.logger.LogError($"{mod.Name} timeout");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStartingDisabled)}");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"{mod.Name} unhandled exception");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStartingDisabled)}");
                return default;
            }
        }

        var guildWarsStartingContext = new GuildWarsStartingContext { ApplicationLauncherContext = applicationLauncherContext, CancelStartup = false };
        foreach (var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStarting(guildWarsStartingContext, cts.Token);
                if (guildWarsStartingContext.CancelStartup)
                {
                    this.logger.LogError($"{mod.Name} canceled startup");
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}");
                    return default;
                }
            }
            catch (TaskCanceledException)
            {
                this.logger.LogError($"{mod.Name} timeout");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarting)}");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"{mod.Name} unhandled exception");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarting)}");
                return default;
            }
        }

        var pId = LaunchClient(executable, string.Join(" ", args), this.privilegeManager.AdminPrivileges, out var clientHandle);
        do
        {
            process = Process.GetProcessById(pId);
            await Task.Delay(100, cancellationToken);
        } while (process is null);

        if (!McPatch(process.Handle))
        {
            var lastErr = Marshal.GetLastWin32Error();
            this.logger.LogError($"Failed to patch GuildWars process. Error code: {lastErr}");
            return default;
        }

        // Reset launch context with the launched process
        applicationLauncherContext = new ApplicationLauncherContext { ExecutablePath =  executable, Process = process, ProcessId = (uint)pId };
        var guildWarsCreatedContext = new GuildWarsCreatedContext { ApplicationLauncherContext = applicationLauncherContext, CancelStartup = false };
        foreach (var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsCreated(guildWarsCreatedContext, cts.Token);
                if (guildWarsCreatedContext.CancelStartup)
                {
                    this.logger.LogError($"{mod.Name} canceled startup");
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} canceled startup",
                        description: $"Mod canceled the startup during {nameof(mod.OnGuildWarsStarting)}");
                    this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                    return default;
                }
            }
            catch (TaskCanceledException)
            {
                this.logger.LogError($"{mod.Name} timeout");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} timeout",
                    description: $"Mod timed out while processing {nameof(mod.OnGuildWarsCreated)}");
            }
            catch (Exception e)
            {
                this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                this.logger.LogError(e, $"{mod.Name} unhandled exception");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsCreated)}");
                return default;
            }
        }

        if (clientHandle != IntPtr.Zero)
        {
            _ = ResumeThread(clientHandle);
            CloseHandle(clientHandle);
        }

        var sw = Stopwatch.StartNew();
        while (sw.Elapsed.TotalSeconds < LaunchTimeout.TotalSeconds)
        {
            await Task.Delay(500, cancellationToken);
            var gwProcess = Process.GetProcessesByName("gw").FirstOrDefault();
            if (gwProcess is null)
            {
                continue;
            }

            if (gwProcess!.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            var windows = GetRootWindowsOfProcess(gwProcess.Id)
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
            if (!windows.Contains("Guild Wars"))
            {
                continue;
            }

            var virtualMemory = gwProcess.VirtualMemorySize64;
            if (virtualMemory < LaunchMemoryThreshold)
            {
                continue;
            }

            var windowInfo = new NativeMethods.WindowInfo(true);
            NativeMethods.GetWindowInfo(gwProcess.MainWindowHandle, ref windowInfo);
            if (windowInfo.rcWindow.Width == 0 || windowInfo.rcWindow.Height == 0)
            {
                continue;
            }

            /*
             * GW loads more than 90 modules when it starts properly. If there are less than
             * 90 modules, GW probably has failed to start or has not started yet
             */
            if (gwProcess.Modules.Count < 90)
            {
                continue;
            }

            /*
             * Run the actions one by one, to avoid injection issues
             */
            var guildWarsStartedContext = new GuildWarsStartedContext { ApplicationLauncherContext = applicationLauncherContext };
            foreach (var mod in mods)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
                try
                {
                    await mod.OnGuildWarsStarted(guildWarsStartedContext, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    this.logger.LogError($"{mod.Name} timeout");
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} timeout",
                        description: $"Mod timed out while processing {nameof(mod.OnGuildWarsStarted)}");
                }
                catch (Exception e)
                {
                    this.KillGuildWarsProcess(new GuildWarsApplicationLaunchContext { GuildWarsProcess = process, LaunchConfiguration = launchConfigurationWithCredentials, ProcessId = (uint)pId });
                    this.logger.LogError(e, $"{mod.Name} unhandled exception");
                    this.notificationService.NotifyError(
                        title: $"{mod.Name} exception",
                        description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsStarted)}");
                    return default;
                }
            }

            return gwProcess;
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

    public IEnumerable<Process> GetGuildwarsProcesses()
    {
        return Process.GetProcessesByName(ProcessName);
    }

    public void KillGuildWarsProcess(GuildWarsApplicationLaunchContext guildWarsApplicationLaunchContext)
    {
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
            this.logger.LogError(e, $"Insuficient privileges to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}");
            this.privilegeManager.RequestAdminPrivileges<LauncherView>("Insufficient privileges to kill Guild Wars process. Please restart as administrator and try again.");
        }
        catch(Exception e)
        {
            this.logger.LogError(e, $"Failed to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}");
            this.notificationService.NotifyError(
                title: "Failed to kill GuildWars process",
                description: $"Encountered exception while trying to kill GuildWars process with id {guildWarsApplicationLaunchContext.ProcessId}. Check logs for details");
        }
    }

    private bool ClearGwLocks(string path)
    {
        if (!this.SetRegistryGuildwarsPath(path))
        {
            this.logger.LogError("Failed to set registry entries. Failing to start GuildWars");
            return false;
        }

        foreach (var process in Process.GetProcessesByName(ProcessName))
        {
            this.mutexHandler.CloseMutex(process, ArenaNetMutex);
        }

        return true;
    }

    private bool SetRegistryGuildwarsPath(string path)
    {
        try
        {
            var regSrc = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ArenaNet\\Guild Wars", "Src", null);
            if (regSrc != null && (string)regSrc != Path.GetFullPath(path))
            {
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ArenaNet\\Guild Wars", "Src", Path.GetFullPath(path));
                Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ArenaNet\\Guild Wars", "Path", Path.GetFullPath(path));
            }

            regSrc = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\ArenaNet\\Guild Wars", "Src", null);
            if (regSrc == null || (string)regSrc == Path.GetFullPath(path))
            {
                return true;
            }

            Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\ArenaNet\\Guild Wars", "Src",
                Path.GetFullPath(path));
            Microsoft.Win32.Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\ArenaNet\\Guild Wars", "Path",
                Path.GetFullPath(path));

            return true;
        }
        catch (UnauthorizedAccessException e)
        {
            this.logger.LogError(e, "Failed to patch registry");
            return false;
        }
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

    /// <summary>
    /// Launches the Guild Wars in suspended state. This fixes a lot of the injection issues
    /// Once everything is injected, the process is resumed.
    /// Source: https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static int LaunchClient(string path, string args, bool elevated, out IntPtr hThread)
    {
        var commandLine = $"\"{path}\" {args}";
        hThread = IntPtr.Zero;

        ProcessInformation procinfo;
        var startinfo = new StartupInfo
        {
            cb = Marshal.SizeOf<StartupInfo>()
        };
        var saProcess = new SecurityAttributes();
        saProcess.nLength = (uint)Marshal.SizeOf(saProcess);
        var saThread = new SecurityAttributes();
        saThread.nLength = (uint)Marshal.SizeOf(saThread);

        var lastDirectory = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(Path.GetDirectoryName(path)!);

        SetRegistryValue(@"Software\ArenaNet\Guild Wars", "Path", path);
        SetRegistryValue(@"Software\ArenaNet\Guild Wars", "Src", path);

        if (!elevated)
        {
            if (!SaferCreateLevel(SaferLevelScope.User, SaferLevel.NormalUser, SaferOpen.Open, out var hLevel,
                    IntPtr.Zero))
            {
                Debug.WriteLine("SaferCreateLevel");
                return 0;
            }

            if (!SaferComputeTokenFromLevel(hLevel, IntPtr.Zero, out var hRestrictedToken, 0, IntPtr.Zero))
            {
                Debug.WriteLine("SaferComputeTokenFromLevel");
                return 0;
            }

            SaferCloseLevel(hLevel);

            // Set the token to medium integrity.

            TokenMandatoryLabel tml;
            tml.Label.Attributes = 0x20; // SE_GROUP_INTEGRITY
            if (!ConvertStringSidToSid("S-1-16-8192", out tml.Label.Sid))
            {
                CloseHandle(hRestrictedToken);
                Debug.WriteLine("ConvertStringSidToSid");
            }

            if (!SetTokenInformation(hRestrictedToken, TokenInformationClass.TokenIntegrityLevel, ref tml,
                    (uint)Marshal.SizeOf(tml) + GetLengthSid(tml.Label.Sid)))
            {
                LocalFree(tml.Label.Sid);
                CloseHandle(hRestrictedToken);
                return 0;
            }

            LocalFree(tml.Label.Sid);
            if (!CreateProcessAsUser(hRestrictedToken, null!, commandLine, ref saProcess,
                    ref saProcess, false, (uint)CreationFlags.CreateSuspended, IntPtr.Zero,
                    null!, ref startinfo, out procinfo))
            {
                var error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"CreateProcessAsUser {error}");
                CloseHandle(procinfo.hThread);
                return 0;
            }

            CloseHandle(hRestrictedToken);
        }
        else
        {
            if (!CreateProcess(null!, commandLine, ref saProcess,
                    ref saThread, false, (uint)CreationFlags.CreateSuspended, IntPtr.Zero,
                    null!, ref startinfo, out procinfo))
            {
                var error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"CreateProcess {error}");
                _ = ResumeThread(procinfo.hThread);
                CloseHandle(procinfo.hThread);
                return 0;
            }
        }

        Directory.SetCurrentDirectory(lastDirectory);

        CloseHandle(procinfo.hProcess);
        hThread = procinfo.hThread;
        return procinfo.dwProcessId;
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static bool McPatch(IntPtr processHandle)
    {
        byte[] sigPatch =
        [
            0x56, 0x57, 0x68, 0x00, 0x01, 0x00, 0x00, 0x89, 0x85, 0xF4, 0xFE, 0xFF, 0xFF, 0xC7, 0x00, 0x00, 0x00, 0x00,
            0x00
        ];
        var moduleBase = GetProcessModuleBase(processHandle);
        var gwdata = new byte[0x48D000];

        if (!NativeMethods.ReadProcessMemory(processHandle, moduleBase, gwdata, gwdata.Length, out _))
        {
            return false;
        }

        var idx = SearchBytes(gwdata, sigPatch);

        if (idx == -1)
        {
            return false;
        }

        var mcpatch = moduleBase + idx - 0x1A;

        byte[] payload = [0x31, 0xC0, 0x90, 0xC3];

        return NativeMethods.WriteProcessMemory(processHandle, mcpatch, payload, payload.Length, out _);
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static int SearchBytes(Memory<byte> haystack, Memory<byte> needle)
    {
        var len = needle.Length;
        var limit = haystack.Length - len;
        for (var i = 0; i <= limit; i++)
        {
            var k = 0;
            for (; k < len; k++)
            {
                if (needle.Span[k] != haystack.Span[i + k])
                {
                    break;
                }
            }

            if (k == len)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static IntPtr GetProcessModuleBase(IntPtr process)
    {
        if (NativeMethods.NtQueryInformationProcess(process, NativeMethods.ProcessInfoClass.ProcessBasicInformation, out var pbi,
                Marshal.SizeOf<ProcessBasicInformation>(), out _) != 0)
        {
            return IntPtr.Zero;
        }

        var buffer = new byte[Marshal.SizeOf<PEB>()];

        if (!NativeMethods.ReadProcessMemory(process, pbi.PebBaseAddress, buffer, Marshal.SizeOf<PEB>(), out _))
        {
            return IntPtr.Zero;
        }

        PEB peb = new()
        {
            ImageBaseAddress = (IntPtr)BitConverter.ToInt32(buffer, 8)
        };

        return peb.ImageBaseAddress + 0x1000;
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

    private static void SetRegistryValue(string registryPath, string valueName, object newValue)
    {
        using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(registryPath);
        key.SetValue(valueName, newValue, RegistryValueKind.String);
    }
}
