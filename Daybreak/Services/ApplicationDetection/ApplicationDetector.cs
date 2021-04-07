using Daybreak.Services.Credentials;
using Palletizer.WPF.Services.ConfigurationManager;
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
        private const string ProcessName = "gw";

        private readonly IConfigurationManager configurationManager;
        private readonly ICredentialManager credentialManager;

        public bool IsGuildwarsRunning => GuildwarsProcessDetected();

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
            if (auth is null)
            {
                throw new InvalidOperationException($"No credentials available");
            }

            if (Process.Start(executable, new List<string> { "-email", auth.Username, "-password", auth.Password, "-character", configuration.CharacterName }) is null)
            {
                throw new InvalidOperationException($"Unable to launch executable");
            }
        }

        private static bool GuildwarsProcessDetected()
        {
            var process = Process.GetProcessesByName(ProcessName).FirstOrDefault();
            return process is not null;
        }
    }
}
