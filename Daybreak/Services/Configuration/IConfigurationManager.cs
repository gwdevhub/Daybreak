using Daybreak.Configuration;

namespace Palletizer.WPF.Services.ConfigurationManager
{
    public interface IConfigurationManager
    {
        ApplicationConfiguration GetConfiguration();
        void SaveConfiguration(ApplicationConfiguration applicationConfiguration);
    }
}
