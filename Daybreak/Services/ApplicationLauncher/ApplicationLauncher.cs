using Daybreak.Configuration.Options;
using Daybreak.Exceptions;
using Daybreak.Services.Credentials;
using Daybreak.Services.Mods;
using Daybreak.Services.Mutex;
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
using System.Threading.Tasks;
using System.Windows;
using static Daybreak.Utils.NativeMethods;

namespace Daybreak.Services.ApplicationLauncher;

public class ApplicationLauncher : IApplicationLauncher
{
    private const int MaxRetries = 10;
    private const string ProcessName = "gw";
    private const string ArenaNetMutex = "AN-Mute";

    private readonly ILiveOptions<LauncherOptions> launcherOptions;
    private readonly ICredentialManager credentialManager;
    private readonly IMutexHandler mutexHandler;
    private readonly IModsManager modsManager;
    private readonly ILogger<ApplicationLauncher> logger;
    private readonly IPrivilegeManager privilegeManager;

    public bool IsGuildwarsRunning => this.GetGuildwarsProcess() is not null;
    public Process? RunningGuildwarsProcess => this.GetGuildwarsProcess();

    public ApplicationLauncher(
        ILiveOptions<LauncherOptions> launcherOptions,
        ICredentialManager credentialManager,
        IMutexHandler mutexHandler,
        IModsManager modsManager,
        ILogger<ApplicationLauncher> logger,
        IPrivilegeManager privilegeManager)
    {
        this.logger = logger.ThrowIfNull();
        this.mutexHandler = mutexHandler.ThrowIfNull();
        this.credentialManager = credentialManager.ThrowIfNull();
        this.launcherOptions = launcherOptions.ThrowIfNull();
        this.modsManager = modsManager.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
    }

    public async Task<Process?> LaunchGuildwars()
    {
        var configuration = this.launcherOptions.Value;
        var auth = await this.credentialManager.GetDefaultCredentials().ConfigureAwait(false);
        return await auth.Switch<Task<Process?>>(
            onSome: async (credentials) =>
            {
                credentials.ThrowIfNull();
                if (configuration.MultiLaunchSupport is true)
                {
                    if (this.privilegeManager.AdminPrivileges is false)
                    {
                        this.privilegeManager.RequestAdminPrivileges<LauncherView>("You need administrator rights in order to start using multi-launch");
                        return null;
                    }

                    this.ClearGwLocks();
                }

                var gwProcess = await this.LaunchGuildwarsProcess(credentials.Username!, credentials.Password!, credentials.CharacterName!);
                if (gwProcess is null)
                {
                    return default;
                }

                
                return gwProcess;
            },
            onNone: () =>
            {
                throw new CredentialsNotFoundException($"No credentials available");
            })
            .ExtractValue()!;
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

    private async Task<Process?> LaunchGuildwarsProcess(string email, Models.SecureString password, string character)
    {
        var executable = this.launcherOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault() ?? throw new ExecutableNotFoundException($"No executable selected");
        if (File.Exists(executable.Path) is false)
        {
            throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
        }

        var args = new List<string>()
        {
            "-email",
            email,
            "-password",
            password!
        };
        if (!string.IsNullOrWhiteSpace(character))
        {
            args.Add("-character");
            args.Add($"\"{character}\"");
        }

        var mods = this.modsManager.GetMods().Where(m => m.IsEnabled).ToList();
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
                FileName = executable.Path,
            }
        };

        var preLaunchActions = this.modsManager.GetMods().Where(m => m.IsEnabled).Select(m => m.OnGuildwarsStarting(process));
        await Task.WhenAll(preLaunchActions);
        var pId = LaunchClient(executable.Path, string.Join(" ", args), this.privilegeManager.AdminPrivileges, out var clientHandle);
        process = Process.GetProcessById(pId);

        foreach(var mod in mods)
        {
            await mod.OnGuildWarsCreated(process);
        }

        if (clientHandle != IntPtr.Zero)
        {
            ResumeThread(clientHandle);
            CloseHandle(clientHandle);
        }

        /*
         * Run the actions one by one, to avoid injection issues
         */
        foreach (var mod in mods)
        {
            await mod.OnGuildwarsStarted(process);
        }

        var retries = 0;
        while (true)
        {
            await Task.Delay(100);
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

            return gwProcess;
        }
    }

    private Process? GetGuildwarsProcess()
    {
        if (this.launcherOptions.Value.MultiLaunchSupport is true)
        {
            try
            {
                var path = this.launcherOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
                if (path is null)
                {
                    return null;
                }

                return Process.GetProcessesByName(ProcessName).Where(process => string.Equals(path.Path, process.MainModule!.FileName, StringComparison.Ordinal)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        return Process.GetProcessesByName(ProcessName).Where(p => !p.HasExited).FirstOrDefault();
    }

    private void ClearGwLocks()
    {
        this.SetRegistryGuildwarsPath();
        foreach (var process in Process.GetProcessesByName(ProcessName))
        {
            this.mutexHandler.CloseMutex(process, ArenaNetMutex);
        }
    }

    private void SetRegistryGuildwarsPath()
    {
        var path = this.launcherOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault() ?? throw new ExecutableNotFoundException("No executable currently selected");
        var gamePath = path.Path;
        try
        {
            var registryKey = GetGuildwarsRegistryKey(true);
            registryKey.SetValue("Path", gamePath!);
            registryKey.SetValue("Src", gamePath!);
            registryKey.Close();
        }
        catch (SecurityException ex)
        {
            this.logger.LogCritical($"Multi-launch requires administrator rights. Details: {ex}");
        }
    }

    private static RegistryKey GetGuildwarsRegistryKey(bool write)
    {
        var gwKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
        if (gwKey is not null)
        {
            return gwKey;
        }

        gwKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
        if (gwKey is not null)
        {
            return gwKey;
        }

        gwKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
        if (gwKey is not null)
        {
            return gwKey;
        }

        gwKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
        if (gwKey is not null)
        {
            return gwKey;
        }

        throw new InvalidOperationException("Could not find registry key for guildwars.");
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
}
