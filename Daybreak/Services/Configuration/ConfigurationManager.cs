using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Utils;
using System;
using System.IO;

namespace Palletizer.WPF.Services.ConfigurationManager
{
    public sealed class ConfigurationManager : IConfigurationManager
    {
        private const string ConfigName = "Daybreak.config.json";

        private ApplicationConfiguration applicationConfiguration;

        public ConfigurationManager()
        {
            try
            {
                var serializedConfig = File.ReadAllText(ConfigName);
                this.applicationConfiguration = serializedConfig.Deserialize<ApplicationConfiguration>();
            }
            catch(Exception e)
            {
                throw new FatalException("Failed to load application configuration. See inner exception for details", e);
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
        }
    }
}
