using Daybreak.Configuration;

namespace Daybreak.Services.Configuration
{
    public interface IConfigurationManager
    {
        ApplicationConfiguration GetConfiguration();
        void SaveConfiguration(ApplicationConfiguration applicationConfiguration);
    }
}
