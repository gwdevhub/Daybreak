using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Services.Options;
using Daybreak.Shared.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Photino.Blazor;
using Serilog;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Daybreak.Launch;

public static partial class Launcher
{
    public static void SetupLogging(PhotinoBlazorAppBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()

            .Enrich.FromLogContext()

            .Enrich.WithMachineName()

            .Enrich.WithProcessId()
            .Enrich.WithProcessName()

            .Enrich.WithThreadId()
            .Enrich.WithThreadName()

            .Enrich.WithEnvironmentName()

            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyInformationalVersion()

            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Sink(InMemorySink.Instance)
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

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

    internal static void StartHostedServices(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        var hostedServices = app.Services.GetServices<IHostedService>();
        foreach (var hostedService in hostedServices)
        {
            Task.Factory.StartNew(() => hostedService.StartAsync(cts.Token), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

    private static bool StopHostedServicesAsync(PhotinoBlazorApp app, CancellationTokenSource cts)
    {
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        var hostedServices = app.Services.GetServices<IHostedService>();
        var tasks = hostedServices.Select(s => s.StopAsync(cts.Token));
        Task.WaitAll(tasks, cts.Token);

        cts.Dispose();
        return false;
    }
}
