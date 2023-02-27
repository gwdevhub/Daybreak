using Daybreak.Configuration;
using Daybreak.Services.Configuration;
using System;
using System.Configuration;
using System.Extensions;

namespace Daybreak.Services.Options;

public sealed class ApplicationConfigurationOptionsManager : IOptionsManager
{
    private readonly IConfigurationManager configurationManager;

    public ApplicationConfigurationOptionsManager(IConfigurationManager configurationManager)
    {
        this.configurationManager = configurationManager;
    }

    public T GetOptions<T>() where T : class
    {
        if (typeof(T) == typeof(ApplicationConfiguration))
        {
            return this.configurationManager.GetConfiguration().Cast<T>();
        }

        throw new InvalidOperationException($"{nameof(ApplicationConfigurationOptionsManager)} cannot return options of type {typeof(T).Name}");
    }

    public void UpdateOptions<T>(T value) where T : class
    {
        if (typeof(T) == typeof(ApplicationConfiguration))
        {
            this.configurationManager.SaveConfiguration(value.Cast<ApplicationConfiguration>());
            return;
        }

        throw new InvalidOperationException($"{nameof(ApplicationConfigurationOptionsManager)} cannot save options of type {typeof(T).Name}");
    }
}
