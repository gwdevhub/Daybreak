using Daybreak.Models;
using Daybreak.Services.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Extensions;

namespace Daybreak.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddDaybreakOptions<TOptions>(this IServiceCollection services)
        where TOptions : class, new()
    {
        services.AddSingleton(new OptionEntry(typeof(TOptions)));
        services.AddSingleton<IOptionsFactory<TOptions>>(sp =>
            new DaybreakOptionsFactory<TOptions>(sp.GetRequiredService<OptionsManager>()));

        services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(sp =>
            new DaybreakOptionsChangeTokenSource<TOptions>(sp.GetRequiredService<OptionsManager>()));

        services.AddSingleton<IOptionsMonitorCache<TOptions>, OptionsCache<TOptions>>();

        services.AddSingleton<IOptionsMonitor<TOptions>>(sp =>
        {
            var factory = sp.GetRequiredService<IOptionsFactory<TOptions>>();
            var sources = sp.GetServices<IOptionsChangeTokenSource<TOptions>>();
            var cache = sp.GetRequiredService<IOptionsMonitorCache<TOptions>>();
            var monitor = new OptionsMonitor<TOptions>(factory, sources, cache);
            monitor.OnChange((_, name) => cache.TryRemove(name ?? Options.DefaultName));
            return new OptionsMonitor<TOptions>(factory, sources, cache);
        });

        services.AddSingleton<IOptions<TOptions>>(sp =>
            new OptionsWrapper<TOptions>(sp.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue));

        services.AddScoped<IOptionsSnapshot<TOptions>>(sp =>
            new OptionsManager<TOptions>(
                sp.GetRequiredService<IOptionsFactory<TOptions>>()));

        return services;
    }

    internal static IServiceCollection AddHostedSingleton<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface, IHostedService
        where TInterface : class
    {
        services.AddSingleton<TInterface, TImplementation>();
        services.AddHostedService(sp => sp.GetRequiredService<TInterface>().Cast<TImplementation>());
        return services;
    }

    internal static IServiceCollection AddHostedSingleton<TImplementation>(this IServiceCollection services)
        where TImplementation : class, IHostedService
    {
        services.AddSingleton<TImplementation>();
        services.AddHostedService(sp => sp.GetRequiredService<TImplementation>());
        return services;
    }
}
