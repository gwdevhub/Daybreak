using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;

namespace Daybreak.Services.ApplicationDetection
{
    public class ApplicationDetector : IApplicationDetector
    {
        private const string ToolboxProcessName = "GWToolbox";
        private const string ProcessName = "gw";

        private readonly IConfigurationManager configurationManager;
        private readonly ICredentialManager credentialManager;

        public bool IsGuildwarsRunning => GuildwarsProcessDetected();
        public bool IsToolboxRunning => GuildwarsToolboxProcessDetected();

        public ApplicationDetector(
            IConfigurationManager configurationManager,
            ICredentialManager credentialManager)
        {
            this.credentialManager = credentialManager.ThrowIfNull(nameof(credentialManager));
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
        }

        public void LaunchGuildwars()
        {
            var configuration = this.configurationManager.GetConfiguration();
            var executable = configuration.GamePath;
            if (File.Exists(executable) is false)
            {
                throw new InvalidOperationException($"Guildwars executable doesn't exist at {executable}");
            }

            if (string.IsNullOrEmpty(configuration.CharacterName))
            {
                throw new InvalidOperationException($"No character name set");
            }

            var auth = this.credentialManager.GetCredentials();
            auth.Do(
                onSome: (credentials) =>
                {
                    if (Process.Start(executable, new List<string> { "-email", credentials.Username, "-password", credentials.Password, "-character", configuration.CharacterName }) is null)
                    {
                        throw new InvalidOperationException($"Unable to launch {executable}");
                    }
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

        private static bool GuildwarsProcessDetected()
        {
            return Process.GetProcessesByName(ProcessName).FirstOrDefault() is not null;
        }

        private static bool GuildwarsToolboxProcessDetected()
        {
            return Process.GetProcessesByName(ToolboxProcessName).FirstOrDefault() is not null;
        }
    }
}
