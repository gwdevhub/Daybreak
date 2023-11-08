using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.Mods;
using Daybreak.Services.Mutex;
using Daybreak.Services.Notifications;
using Daybreak.Services.Privilege;
using Daybreak.Utils;
using Daybreak.Views;
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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Daybreak.Utils.NativeMethods;

namespace Daybreak.Services.ApplicationLauncher;

public class ApplicationLauncher : IApplicationLauncher
{
    private const int MaxRetries = 10;
    private const string ProcessName = "gw";
    private const string ArenaNetMutex = "AN-Mute";

    private readonly INotificationService notificationService;
    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly IMutexHandler mutexHandler;
    private readonly IModsManager modsManager;
    private readonly ILogger<ApplicationLauncher> logger;
    private readonly IPrivilegeManager privilegeManager;

    public ApplicationLauncher(
        INotificationService notificationService,
        ILiveOptions<LauncherOptions> launcherOptions,
        IMutexHandler mutexHandler,
        IModsManager modsManager,
        ILogger<ApplicationLauncher> logger,
        IPrivilegeManager privilegeManager)
    {
        this.logger = logger.ThrowIfNull();
        this.mutexHandler = mutexHandler.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.modsManager = modsManager.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
    }

    public async Task<GuildWarsApplicationLaunchContext?> LaunchGuildwars(LaunchConfigurationWithCredentials launchConfigurationWithCredentials)
    {
        launchConfigurationWithCredentials.ThrowIfNull();
        var credentials = launchConfigurationWithCredentials.Credentials!.ThrowIfNull();
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

        var gwProcess = await this.LaunchGuildwarsProcess(
            credentials.Username!.ThrowIfNull(),
            credentials.Password!.ThrowIfNull(),
            credentials.CharacterName!.ThrowIfNull(),
            launchConfigurationWithCredentials.ExecutablePath!.ThrowIfNull());
        if (gwProcess is null)
        {
            return default;
        }


        return new GuildWarsApplicationLaunchContext { LaunchConfiguration = launchConfigurationWithCredentials, GuildWarsProcess = gwProcess };
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
                FileName = processName
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
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = "cmd.exe",
                UseShellExecute = true,
                CreateNoWindow = true,
                Arguments = $"/c runas /trustlevel:0x20000 {processName}"
            }
        };
        if (process.Start() is false)
        {
            throw new InvalidOperationException($"Unable to start {processName} as normal user");
        }

        Application.Current.Shutdown();
    }

    private async Task<Process?> LaunchGuildwarsProcess(string email, Models.SecureString password, string character, string executable)
    {
        if (File.Exists(executable) is false)
        {
            throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
        }

        var args = new List<string>()
        {
            "-email",
            email,
            "-password",
            password!.ToString()
        };
        if (!string.IsNullOrWhiteSpace(character))
        {
            args.Add("-character");
            args.Add($"\"{character}\"");
        }

        var mods = this.modsManager.GetMods().Where(m => m.IsEnabled).ToList();
        var disabledmods = this.modsManager.GetMods().Where(m => !m.IsEnabled).ToList();
        foreach(var mod in mods)
        {
            args.AddRange(mod.GetCustomArguments());
        }

        var identity = this.launcherOptions.Value.LaunchGuildwarsAsCurrentUser ?
            System.Security.Principal.WindowsIdentity.GetCurrent().Name :
            System.Security.Principal.WindowsIdentity.GetAnonymous().Name;
        this.logger.LogInformation($"Launching guildwars as [{identity}] identity");
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                Arguments = string.Join(" ", args),
                FileName = executable,
            }
        };

        foreach(var mod in disabledmods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStartingDisabled(process, cts.Token);
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

        foreach(var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsStarting(process, cts.Token);
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
        process = Process.GetProcessById(pId);

        foreach (var mod in mods)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
            try
            {
                await mod.OnGuildWarsCreated(process, cts.Token);
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
                this.KillGuildWarsProcess(process);
                this.logger.LogError(e, $"{mod.Name} unhandled exception");
                this.notificationService.NotifyError(
                    title: $"{mod.Name} exception",
                    description: $"Mod encountered exception of type {e.GetType().Name} while processing {nameof(mod.OnGuildWarsCreated)}");
                return default;
            }
        }

        if (clientHandle != IntPtr.Zero)
        {
            McPatch(process.Handle);
            _ = ResumeThread(clientHandle);
            CloseHandle(clientHandle);
        }

        var retries = 0;
        while (retries < MaxRetries)
        {
            await Task.Delay(1000);
            retries++;
            var gwProcess = Process.GetProcessesByName("gw").FirstOrDefault();
            if (gwProcess is null && retries < MaxRetries)
            {
                continue;
            }
            else if (gwProcess is null && retries >= MaxRetries)
            {
                throw new InvalidOperationException("Newly launched gw process not detected");
            }

            if (gwProcess!.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            int titleLength = NativeMethods.GetWindowTextLength(gwProcess.MainWindowHandle);
            var titleBuffer = new StringBuilder(titleLength);
            var readCount = NativeMethods.GetWindowText(gwProcess.MainWindowHandle, titleBuffer, titleLength + 1);
            var title = titleBuffer.ToString();
            if (title != "Guild Wars")
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
             * Run the actions one by one, to avoid injection issues
             */
            foreach (var mod in mods)
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(this.launcherOptions.Value.ModStartupTimeout));
                try
                {
                    await mod.OnGuildWarsStarted(process, cts.Token);
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
                    this.KillGuildWarsProcess(process);
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

    public void KillGuildWarsProcess(Process process)
    {
        process.ThrowIfNull();
        try
        {
            if (process.StartInfo is not null &&
                process.StartInfo.FileName.Contains("Gw.exe", StringComparison.OrdinalIgnoreCase))
            {
                process.Kill(true);
                return;
            }

            if (process.MainModule?.FileName is not null &&
                process.MainModule.FileName.Contains("Gw.exe", StringComparison.OrdinalIgnoreCase))
            {
                process.Kill(true);
                return;
            }
        }
        catch(Exception e)
        {
            this.logger.LogError(e, $"Failed to kill GuildWars process with id {process?.Id}");
            this.notificationService.NotifyError(
                title: "Failed to kill GuildWars process",
                description: $"Encountered exception while trying to kill GuildWars process with id {process?.Id}. Check logs for details");
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
            .Where(p =>
            {
                return launchConfigurationWithCredentials.FirstOrDefault(l => l.ExecutablePath == p.MainModule?.FileName) is not null;
            })
            .Select(p =>
            {
                var config = launchConfigurationWithCredentials.FirstOrDefault(l => l.ExecutablePath == p.MainModule?.FileName);
                return config is not null ?
                    new GuildWarsApplicationLaunchContext { GuildWarsProcess = p, LaunchConfiguration = config } :
                    default;
            });
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
        StartupInfo startinfo = new()
        {
            cb = Marshal.SizeOf(typeof(StartupInfo))
        };
        var saProcess = new SecurityAttributes();
        saProcess.nLength = (uint)Marshal.SizeOf(saProcess);
        var saThread = new SecurityAttributes();
        saThread.nLength = (uint)Marshal.SizeOf(saThread);

        var lastDirectory = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(Path.GetDirectoryName(path)!);

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
                ResumeThread(procinfo.hThread);
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
        {
            0x56, 0x57, 0x68, 0x00, 0x01, 0x00, 0x00, 0x89, 0x85, 0xF4, 0xFE, 0xFF, 0xFF, 0xC7, 0x00, 0x00, 0x00, 0x00,
            0x00
        };
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

        byte[] payload = { 0x31, 0xC0, 0x90, 0xC3 };

        return NativeMethods.WriteProcessMemory(processHandle, mcpatch, payload, payload.Length, out _);
    }

    /// <summary>
    /// https://github.com/GregLando113/gwlauncher/blob/master/GW%20Launcher/MulticlientPatch.cs
    /// </summary>
    private static int SearchBytes(IReadOnlyList<byte> haystack, IReadOnlyList<byte> needle)
    {
        var len = needle.Count;
        var limit = haystack.Count - len;
        for (var i = 0; i <= limit; i++)
        {
            var k = 0;
            for (; k < len; k++)
            {
                if (needle[k] != haystack[i + k])
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
                Marshal.SizeOf(typeof(NativeMethods.ProcessBasicInformation)), out _) != 0)
        {
            return IntPtr.Zero;
        }

        var buffer = new byte[Marshal.SizeOf(typeof(PEB))];

        if (!NativeMethods.ReadProcessMemory(process, pbi.PebBaseAddress, buffer, Marshal.SizeOf(typeof(PEB)), out _))
        {
            return IntPtr.Zero;
        }

        PEB peb = new()
        {
            ImageBaseAddress = (IntPtr)BitConverter.ToInt32(buffer, 8)
        };

        return peb.ImageBaseAddress + 0x1000;
    }
}
