using Daybreak.Models;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.Logging;
using Daybreak.Services.Mutex;
using Daybreak.Utils;
using Microsoft.Win32;
using Pepa.Wpf.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Daybreak.Services.ApplicationDetection
{
    public class ApplicationDetector : IApplicationDetector
    {
        private const string ToolboxProcessName = "GWToolbox";
        private const string ProcessName = "gw";
        private const string ArenaNetMutex = "AN-Mute";

        private readonly IConfigurationManager configurationManager;
        private readonly ICredentialManager credentialManager;
        private readonly IMutexHandler mutexHandler;
        private readonly ILogger logger;

        public bool IsGuildwarsRunning => this.GuildwarsProcessDetected();
        public bool IsToolboxRunning => GuildwarsToolboxProcessDetected();

        public ApplicationDetector(
            IConfigurationManager configurationManager,
            ICredentialManager credentialManager,
            IMutexHandler mutexHandler,
            ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.mutexHandler = mutexHandler.ThrowIfNull(nameof(mutexHandler));
            this.credentialManager = credentialManager.ThrowIfNull(nameof(credentialManager));
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
        }

        public void LaunchGuildwars()
        {
            var configuration = this.configurationManager.GetConfiguration();
            if (string.IsNullOrEmpty(configuration.CharacterName))
            {
                throw new InvalidOperationException($"No character name set");
            }

            var auth = this.credentialManager.GetCredentials();
            auth.Do(
                onSome: (credentials) =>
                {
                    if (configuration.ExperimentalFeatures.MultiLaunchSupport is true)
                    {
                        ClearGwLocks();
                    }

                    LaunchGuildwarsProcess(credentials.Username, credentials.Password, configuration.CharacterName);
                },
                onNone: () =>
                {
                    throw new InvalidOperationException($"No credentials available");
                });
        }

        public void LaunchGuildwarsToolbox()
        {
            var configuration = this.configurationManager.GetConfiguration();
            var executable = configuration.ToolboxPath;
            if (File.Exists(executable) is false)
            {
                throw new InvalidOperationException($"Guildwars executable doesn't exist at {executable}");
            }

            if (Process.Start(executable) is null)
            {
                throw new InvalidOperationException($"Unable to launch {executable}");
            }
        }

        private void LaunchGuildwarsProcess(string email, Models.SecureString password, string character)
        {
            var executable = this.configurationManager.GetConfiguration().GamePath;
            if (File.Exists(executable) is false)
            {
                throw new InvalidOperationException($"Guildwars executable doesn't exist at {executable}");
            }

            if (Process.Start(executable, new List<string> { "-email", email, "-password", password, "-character", character }) is null)
            {
                throw new InvalidOperationException($"Unable to launch {executable}");
            }
        }

        private bool GuildwarsProcessDetected()
        {
            if (this.configurationManager.GetConfiguration().ExperimentalFeatures.MultiLaunchSupport is true)
            {
                try
                {
                    using var stream = File.OpenWrite(this.configurationManager.GetConfiguration().GamePath);
                    return false;
                }
                catch
                {
                    return true;
                }
            }

            return Process.GetProcessesByName(ProcessName).FirstOrDefault() is not null;
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
            var gamePath = this.configurationManager.GetConfiguration().GamePath;
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

        private RegistryKey GetGuildwarsRegistryKey(bool write)
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
            return Process.GetProcessesByName(ToolboxProcessName).FirstOrDefault() is not null;
        }
    }
}
