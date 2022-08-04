using Daybreak.Configuration;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Extensions;
using System.IO;

namespace Daybreak.Services.Configuration
{
    public sealed class ConfigurationManager : IConfigurationManager
    {
        private const string ConfigName = "Daybreak.config.json";

        private ApplicationConfiguration applicationConfiguration;
        private readonly ILogger<ConfigurationManager> logger;

        public event EventHandler ConfigurationChanged;

        public ConfigurationManager(ILogger<ConfigurationManager> logger)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            try
            {
                var serializedConfig = File.ReadAllText(ConfigName);
                this.applicationConfiguration = serializedConfig.Deserialize<ApplicationConfiguration>();
            }
            catch(Exception e)
            {
                this.logger.LogWarning(e, $"Failed to load configuration. Falling back to default configuration");
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
