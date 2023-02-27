using Daybreak.Configuration;
using System;

namespace Daybreak.Services.Configuration;

public interface IConfigurationManager
{
    event EventHandler ConfigurationChanged;
    ApplicationConfiguration GetConfiguration();
    void SaveConfiguration(ApplicationConfiguration applicationConfiguration);
}
