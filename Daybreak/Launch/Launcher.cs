using Daybreak.Configuration;
using Daybreak.Services.Initialization;
using Daybreak.Services.Options;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using System.Extensions.Core;
using System.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Tests")]
namespace Daybreak.Launch;

public partial class Launcher
{
    [STAThread]
    public static int Main(string[] args)
    {
#if DEBUG
        AllocateAnsiConsole();
#endif
        CreateAndShowSplash(args, LaunchSequence);
        return 0;
    }

    private static void LaunchSequence(string[] args, PhotinoBlazorApp splashApp)
    {
        var scopedLogger = splashApp.Services.GetRequiredService<ILogger<Launcher>>().CreateScopedLogger();
        var builder = CreateMainBuilder(args);
        var pluginsService = splashApp.Services.GetRequiredService<IPluginsService>();
        var optionsManager = splashApp.Services.GetRequiredService<OptionsManager>();
        var optionsProvider = splashApp.Services.GetRequiredService<IOptionsProvider>();

        pluginsService.LoadPlugins();
        var loadedPlugins = pluginsService.GetCurrentlyLoadedPlugins();
        var configurations = loadedPlugins.Select<AvailablePlugin, (string, PluginConfigurationBase?, AvailablePlugin?)>(p => (p.Name, p.Configuration, (AvailablePlugin?)p))
            .Prepend(("Daybreak Core", new ProjectConfiguration(), default)).ToList();

        var menuEntryProducer = new MenuProducer(splashApp.Services.GetRequiredService<ILogger<MenuProducer>>());
        var minProgress = 0.1;
        var maxProgress = 0.9;
        var totalConfigurations = configurations.Count;
        var stepsPerConfiguration = 10; // Number of operations for each configuration
        var step = (maxProgress - minProgress) / (totalConfigurations * stepsPerConfiguration);
        var progress = minProgress;
        foreach (var (pluginName, configuration, plugin) in configurations)
        {
            if (configuration is null)
            {
                scopedLogger.LogWarning("No configuration found for plugin {PluginName}, skipping...", pluginName);
                continue;
            }

            LoadConfiguration(pluginName, splashApp, builder, configuration, menuEntryProducer, progress, step);
            progress += stepsPerConfiguration * step;
        }

        // Setup bootstrapped services
        builder.Services.AddSingleton<IReadOnlyDictionary<string, MenuCategory>>(menuEntryProducer.categories.AsReadOnly());
        builder.Services.AddSingleton(pluginsService);
        builder.Services.AddSingleton(optionsManager);
        builder.Services.AddSingleton(optionsProvider);

        var mainApp = CreateMainApp(builder);
        LaunchState.UpdateProgress(LaunchState.ExecutingArgumentHandlers);
        ExecuteArgumentHandlers(mainApp, args);

        LaunchState.UpdateProgress(LaunchState.Finalizing);
        CloseSplash(splashApp);
        RunMainApp(mainApp);
    }

    private static void ExecuteArgumentHandlers(PhotinoBlazorApp app, string[] args)
    {
        var applicationArgumentService = app.Services.GetRequiredService<IApplicationArgumentService>();
        applicationArgumentService.HandleArguments(args);
    }

    private static void LoadConfiguration(
        string name,
        PhotinoBlazorApp bootstrap,
        PhotinoBlazorAppBuilder builder,
        PluginConfigurationBase configuration,
        MenuProducer menuProducer,
        double progress,
        double step)
    {
        var scopedLogger = bootstrap.Services.GetRequiredService<ILogger<Launcher>>().CreateScopedLogger();
        scopedLogger.LogInformation("Loading configuration for {Configuration.Name}", name);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingServices(progress));
        RegisterServices(builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingOptions(progress));
        RegisterOptions(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingViews(progress));
        RegisterViews(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingMods(progress));
        RegisterMods(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingStartupActions(progress));
        RegisterStartupActions(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingPostUpdateActions(progress));
        RegisterPostUpdateActions(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingNotificationHandlers(progress));
        RegisterNotificationHandlers(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingArgumentHandlers(progress));
        RegisterArgumentHandlers(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingThemes(progress));
        RegisterThemes(bootstrap, builder, configuration, scopedLogger);

        progress += step;
        LaunchState.UpdateProgress(LaunchState.LoadingMenuEntries(progress));
        RegisterMenuEntries(configuration, menuProducer, scopedLogger);

        scopedLogger.LogInformation("Finished loading configuration for {Configuration.Name}", name);
    }

    private static void RegisterServices(PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            configuration.RegisterServices(builder.Services);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering services");
        }
    }

    private static void RegisterOptions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var optionsProducer = new OptionProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<OptionProducer>>());
            configuration.RegisterOptions(optionsProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering options");
        }
    }

    private static void RegisterViews(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var viewProducer = new TrailBlazrViewProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<TrailBlazrViewProducer>>());
            configuration.RegisterViews(viewProducer);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering views");
        }
    }

    private static void RegisterMods(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var modsProducer = new ModsProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ModsProducer>>());
            configuration.RegisterMods(modsProducer);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering mods");
        }
    }

    private static void RegisterStartupActions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var startupActionProducer = new StartupActionsProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<StartupActionsProducer>>());
            configuration.RegisterStartupActions(startupActionProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering startup actions");
        }
    }

    private static void RegisterPostUpdateActions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var postUpdateActionProducer = new PostUpdateActionProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<PostUpdateActionProducer>>());
            configuration.RegisterPostUpdateActions(postUpdateActionProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering post-update actions");
        }
    }

    private static void RegisterNotificationHandlers(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var notificationHandlerProducer = new NotificationHandlerProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<NotificationHandlerProducer>>());
            configuration.RegisterNotificationHandlers(notificationHandlerProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering notification handlers");
        }
    }

    private static void RegisterArgumentHandlers(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var argumentHandlerProducer = new ArgumentHandlerProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ArgumentHandlerProducer>>());
            configuration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering argument handlers");
        }
    }

    private static void RegisterThemes(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var themeProducer = new ThemeProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ThemeProducer>>());
            configuration.RegisterThemes(themeProducer);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering themes");
        }
    }

    private static void RegisterMenuEntries(PluginConfigurationBase configuration, MenuProducer menuProducer, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            configuration.RegisterMenuButtons(menuProducer);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering menu entries");
        }
    }
}
