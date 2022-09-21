using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Services.Credentials;
using Daybreak.Services.Mutex;
using Daybreak.Services.Privilege;
using Daybreak.Utils;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.ApplicationLauncher
{
    public class ApplicationLauncher : IApplicationLauncher
    {
        private const int MaxRetries = 10;
        private const string TexModProcessName = "TexMod";
        private const string UModProcessName = "uMod";
        private const string ToolboxProcessName = "GWToolbox";
        private const string ProcessName = "gw";
        private const string ArenaNetMutex = "AN-Mute";

        private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
        private readonly ICredentialManager credentialManager;
        private readonly IMutexHandler mutexHandler;
        private readonly ILogger<ApplicationLauncher> logger;
        private readonly IPrivilegeManager privilegeManager;

        public bool IsTexmodRunning => TexModProcessDetected();
        public bool IsGuildwarsRunning => this.GuildwarsProcessDetected();
        public bool IsToolboxRunning => GuildwarsToolboxProcessDetected();

        public ApplicationLauncher(
            ILiveOptions<ApplicationConfiguration> liveOptions,
            ICredentialManager credentialManager,
            IMutexHandler mutexHandler,
            ILogger<ApplicationLauncher> logger,
            IPrivilegeManager privilegeManager)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.mutexHandler = mutexHandler.ThrowIfNull(nameof(mutexHandler));
            this.credentialManager = credentialManager.ThrowIfNull(nameof(credentialManager));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
            this.privilegeManager = privilegeManager.ThrowIfNull(nameof(privilegeManager));
        }

        public async Task<bool> LaunchGuildwars()
        {
            var configuration = this.liveOptions.Value;
            var auth = await this.credentialManager.GetDefaultCredentials().ConfigureAwait(false);
            return await auth.Switch(
                onSome: async (credentials) =>
                {
                    if (configuration.ExperimentalFeatures.MultiLaunchSupport is true)
                    {
                        if (this.privilegeManager.AdminPrivileges is false)
                        {
                            this.privilegeManager.RequestAdminPrivileges<CompanionView>("You need administrator rights in order to start using multi-launch");
                            return false;
                        }

                        this.ClearGwLocks();
                    }

                    return await this.LaunchGuildwarsProcess(credentials.Username, credentials.Password, credentials.CharacterName);
                },
                onNone: () =>
                {
                    throw new CredentialsNotFoundException($"No credentials available");
                })
                .ExtractValue();
        }

        public Task LaunchGuildwarsToolbox()
        {
            return Task.Run(() =>
            {
                var configuration = this.liveOptions.Value;
                var executable = configuration.ToolboxPath;
                if (File.Exists(executable) is false)
                {
                    throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
                }

                if (Process.Start(executable) is null)
                {
                    throw new InvalidOperationException($"Unable to launch {executable}");
                }
            });
        }

        public Task LaunchTexmod()
        {
            return Task.Run(() =>
            {
                var configuration = this.liveOptions.Value;
                var executable = configuration.TexmodPath;
                if (File.Exists(executable) is false)
                {
                    throw new ExecutableNotFoundException($"Texmod executable doesn't exist at {executable}");
                }

                if (Process.Start(executable) is null)
                {
                    throw new InvalidOperationException($"Unable to launch {executable}");
                }
            });
        }

        public void RestartDaybreakAsAdmin()
        {
            this.logger.LogInformation("Restarting daybreak with admin rights");
            var processName = Process.GetCurrentProcess()?.MainModule?.FileName;
            if (processName.IsNullOrWhiteSpace() || File.Exists(processName) is false)
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

        private async Task<bool> LaunchGuildwarsProcess(string email, Models.SecureString password, string character)
        {
            var executable = this.liveOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
            if (executable is null)
            {
                throw new ExecutableNotFoundException($"No executable selected");
            }

            if (File.Exists(executable.Path) is false)
            {
                throw new ExecutableNotFoundException($"Guildwars executable doesn't exist at {executable}");
            }

            var args = new List<string>()
            {
                "-email",
                email,
                "-password",
                password
            };
            if (!string.IsNullOrWhiteSpace(character))
            {
                args.Add("-character");
                args.Add(character);
            }

            var identity = this.liveOptions.Value.ExperimentalFeatures.LaunchGuildwarsAsCurrentUser ?
                System.Security.Principal.WindowsIdentity.GetCurrent().Name :
                System.Security.Principal.WindowsIdentity.GetAnonymous().Name;
            this.logger.LogInformation($"Launching guildwars as [{identity}] identity");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = string.Join(" ", args),
                    FileName = executable.Path
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
                var gwProcess = Process.GetProcessesByName("gw").FirstOrDefault();
                if (gwProcess is null && retries < MaxRetries)
                {
                    continue;
                }
                else if (gwProcess is null && retries >= MaxRetries)
                {
                    throw new InvalidOperationException("Newly launched gw process not detected");
                }

                if (gwProcess.MainWindowHandle == IntPtr.Zero)
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

                return true;
            }
        }

        private bool GuildwarsProcessDetected()
        {
            if (this.liveOptions.Value.ExperimentalFeatures.MultiLaunchSupport is true)
            {
                try
                {
                    var path = this.liveOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
                    if (path is null)
                    {
                        return false;
                    }

                    return Process.GetProcessesByName(ProcessName).Where(process => string.Equals(path.Path, process.MainModule.FileName, StringComparison.Ordinal)).Any();
                }
                catch
                {
                    return true;
                }
            }

            return Process.GetProcessesByName(ProcessName).Any();
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
            var path = this.liveOptions.Value.GuildwarsPaths.Where(path => path.Default).FirstOrDefault();
            if (path is null)
            {
                throw new ExecutableNotFoundException("No executable currently selected");
            }

            var gamePath = path.Path;
            try
            {
                var registryKey = GetGuildwarsRegistryKey(true);
                registryKey.SetValue("Path", gamePath);
                registryKey.SetValue("Src", gamePath);
                registryKey.Close();
            }
            catch (SecurityException ex)
            {
                this.logger.LogCritical($"Multi-launch requires administrator rights. Details: {ex}");
            }
        }

        private static RegistryKey GetGuildwarsRegistryKey(bool write)
        {
            var gwKey = Registry.CurrentUser.OpenSubKey("SOFTWARE")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
            if (gwKey is not null)
            {
                return gwKey;
            }

            gwKey = Registry.CurrentUser.OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
            if (gwKey is not null)
            {
                return gwKey;
            }

            gwKey = Registry.LocalMachine.OpenSubKey("SOFTWARE")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
            if (gwKey is not null)
            {
                return gwKey;
            }

            gwKey = Registry.LocalMachine.OpenSubKey("SOFTWARE")?.OpenSubKey("WOW6432Node")?.OpenSubKey("ArenaNet")?.OpenSubKey("Guild Wars", write);
            if (gwKey is not null)
            {
                return gwKey;
            }

            throw new InvalidOperationException("Could not find registry key for guildwars.");
        }

        private static bool GuildwarsToolboxProcessDetected()
        {
            return Process.GetProcessesByName(ToolboxProcessName).Any();
        }

        private static bool TexModProcessDetected()
        {
            return Process.GetProcesses()
                .Where(process => string.Equals(process.ProcessName, UModProcessName, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(process.ProcessName, TexModProcessName, StringComparison.OrdinalIgnoreCase)).Any();
        }
    }
}
