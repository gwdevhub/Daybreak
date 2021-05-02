using Daybreak.Configuration;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using System;
using System.Extensions;
using System.IO;

namespace Daybreak.Services.Configuration
{
    public sealed class ConfigurationManager : IConfigurationManager
    {
        private const string ConfigName = "Daybreak.config.json";

        private ApplicationConfiguration applicationConfiguration;
        private readonly ILogger logger;

        public event EventHandler ConfigurationChanged;

        public ConfigurationManager(ILogger logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            try
            {
                var serializedConfig = File.ReadAllText(ConfigName);
                this.applicationConfiguration = serializedConfig.Deserialize<ApplicationConfiguration>();
            }
            catch(Exception e)
            {
                this.logger.LogWarning($"No configuration detected. Loading default configuration. Details: {e}");
                this.applicationConfiguration = new ApplicationConfiguration();
            }
        }

        public ApplicationConfiguration GetConfiguration()
        {
            return this.applicationConfiguration;
        }

        public void SaveConfiguration(ApplicationConfiguration applicationConfiguration)
        {
            File.WriteAllText(ConfigName, applicationConfiguration.Serialize());
            this.applicationConfiguration = applicationConfiguration;
            this.ConfigurationChanged?.Invoke(this, new EventArgs());
        }
    }
}
