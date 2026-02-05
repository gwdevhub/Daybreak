using Daybreak.Configuration;
using Daybreak.Services.FileProviders;
using Daybreak.Services.Initialization;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Models.Themes;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using System.Extensions.Core;
using System.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Daybreak.Launch;

public partial class Launcher
{
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        AllocateAnsiConsole();
#endif

        var bootstrap = SetupBootstrap();
        LaunchSequence(args, bootstrap);
    }

    private static void LaunchSequence(string[] args, IServiceProvider bootstrap)
    {
        var scopedLogger = bootstrap.GetRequiredService<ILogger<Launcher>>().CreateScopedLogger();
        scopedLogger.LogDebug("Starting Daybreak Launcher...");
        var builder = CreateMainBuilder(args);
        var pluginsService = bootstrap.GetRequiredService<IPluginsService>();
        var loadedPlugins = pluginsService.LoadPlugins();
        var configurations = loadedPlugins.Select<AvailablePlugin, (string, PluginConfigurationBase?, AvailablePlugin?)>(p => (p.Name, p.Configuration, (AvailablePlugin?)p))
            .Prepend(("Daybreak Core", new ProjectConfiguration(), default)).ToList();

        var menuEntryProducer = new MenuProducer(bootstrap.GetRequiredService<ILogger<MenuProducer>>());
        foreach (var (pluginName, configuration, plugin) in configurations)
        {
            if (configuration is null)
            {
                scopedLogger.LogWarning("No configuration found for plugin {PluginName}, skipping...", pluginName);
                continue;
            }

            LoadConfiguration(pluginName, bootstrap, builder, configuration, menuEntryProducer);
        }

        builder.Services.AddSingleton<IReadOnlyDictionary<string, MenuCategory>>(menuEntryProducer.categories.AsReadOnly());
        var mainApp = CreateMainApp(builder, args);
        foreach (var (pluginName, configuration, plugin) in configurations)
        {
            if (configuration is null)
            {
                scopedLogger.LogWarning("No configuration found for plugin {PluginName}, skipping theme registration...", pluginName);
                continue;
            }

            RegisterThemes(bootstrap, configuration, scopedLogger);
        }

        scopedLogger.LogInformation("Registering loaded plugins");
        var finalPluginService = mainApp.Services.GetRequiredService<IPluginsService>();
        finalPluginService.UpdateLoadedPlugins(loadedPlugins);

        var theme = mainApp.Services.GetRequiredService<GameScreenshotsTheme>();
        Theme.Themes.Add(theme);
        var keyboardHook = mainApp.Services.GetRequiredService<IKeyboardHookService>();
        keyboardHook.Start();
        ExecuteArgumentHandlers(mainApp, args);
        RunMainApp(mainApp);
    }

    private static void ExecuteArgumentHandlers(PhotinoBlazorApp app, string[] args)
    {
        var applicationArgumentService = app.Services.GetRequiredService<IApplicationArgumentService>();
        applicationArgumentService.HandleArguments(args);
    }

    private static void LoadConfiguration(
        string name,
        IServiceProvider bootstrap,
        PhotinoBlazorAppBuilder builder,
        PluginConfigurationBase configuration,
        MenuProducer menuProducer)
    {
        var scopedLogger = bootstrap.GetRequiredService<ILogger<Launcher>>().CreateScopedLogger();
        scopedLogger.LogInformation("Loading configuration for {Configuration.Name}", name);

        RegisterServices(builder, configuration, scopedLogger);
        RegisterOptions(bootstrap, builder, configuration, scopedLogger);
        RegisterViews(bootstrap, builder, configuration, scopedLogger);
        RegisterMods(bootstrap, builder, configuration, scopedLogger);
        RegisterStartupActions(bootstrap, builder, configuration, scopedLogger);
        RegisterPostUpdateActions(bootstrap, builder, configuration, scopedLogger);
        RegisterNotificationHandlers(bootstrap, builder, configuration, scopedLogger);
        RegisterArgumentHandlers(bootstrap, builder, configuration, scopedLogger);
        RegisterMenuEntries(configuration, menuProducer, scopedLogger);
        RegisterFileProviders(bootstrap, builder, configuration, scopedLogger);

        scopedLogger.LogInformation("Finished loading configuration for {Configuration.Name}", name);
    }

    private static void RegisterServices(PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            configuration.RegisterServices(builder.Services);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering services");
        }
    }

    private static void RegisterOptions(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var optionsProducer = new OptionProducer(builder.Services, bootstrap.GetRequiredService<ILogger<OptionProducer>>());
            configuration.RegisterOptions(optionsProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering options");
        }
    }

    private static void RegisterViews(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var viewProducer = new TrailBlazrViewProducer(builder.Services, bootstrap.GetRequiredService<ILogger<TrailBlazrViewProducer>>());
            configuration.RegisterViews(viewProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering views");
        }
    }

    private static void RegisterMods(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var modsProducer = new ModsProducer(builder.Services, bootstrap.GetRequiredService<ILogger<ModsProducer>>());
            configuration.RegisterMods(modsProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering mods");
        }
    }

    private static void RegisterStartupActions(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var startupActionProducer = new StartupActionsProducer(builder.Services, bootstrap.GetRequiredService<ILogger<StartupActionsProducer>>());
            configuration.RegisterStartupActions(startupActionProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering startup actions");
        }
    }

    private static void RegisterPostUpdateActions(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var postUpdateActionProducer = new PostUpdateActionProducer(builder.Services, bootstrap.GetRequiredService<ILogger<PostUpdateActionProducer>>());
            configuration.RegisterPostUpdateActions(postUpdateActionProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering post-update actions");
        }
    }

    private static void RegisterNotificationHandlers(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var notificationHandlerProducer = new NotificationHandlerProducer(builder.Services, bootstrap.GetRequiredService<ILogger<NotificationHandlerProducer>>());
            configuration.RegisterNotificationHandlers(notificationHandlerProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering notification handlers");
        }
    }

    private static void RegisterArgumentHandlers(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var argumentHandlerProducer = new ArgumentHandlerProducer(builder.Services, bootstrap.GetRequiredService<ILogger<ArgumentHandlerProducer>>());
            configuration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering argument handlers");
        }
    }

    private static void RegisterThemes(IServiceProvider bootstrap, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var themeProducer = new ThemeProducer(bootstrap.GetRequiredService<ILogger<ThemeProducer>>());
            configuration.RegisterThemes(themeProducer);
        }
        catch (Exception ex)
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
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering menu entries");
        }
    }

    private static void RegisterFileProviders(IServiceProvider bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration, ScopedLogger<Launcher> scopedLogger)
    {
        try
        {
            var fileProviderProducer = new AssemblyFileProviderProducer(builder.Services, bootstrap.GetRequiredService<ILogger<AssemblyFileProviderProducer>>());
            configuration.RegisterProviderAssemblies(fileProviderProducer);
        }
        catch (Exception ex)
        {
            scopedLogger.LogError(ex, "An error occurred while registering file providers");
        }
    }
}
