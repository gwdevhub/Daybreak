using Daybreak.Configuration;
using Daybreak.Services.Initialization;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Models.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Tests")]
namespace Daybreak.Launch;

public static partial class Launcher
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
        var builder = CreateMainBuilder(args);
        var coreConfiguration = new ProjectConfiguration();

        LaunchState.UpdateProgress(LaunchState.LoadingServices);
        RegisterServices(builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingOptions);
        RegisterOptions(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingViews);
        RegisterViews(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingMods);
        RegisterMods(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingStartupActions);
        RegisterStartupActions(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingPostUpdateActions);
        RegisterPostUpdateActions(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingNotificationHandlers);
        RegisterNotificationHandlers(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingArgumentHandlers);
        RegisterArgumentHandlers(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingThemes);
        RegisterThemes(splashApp, builder, coreConfiguration);

        LaunchState.UpdateProgress(LaunchState.LoadingMenuEntries);
        RegisterMenuEntries(splashApp, builder, coreConfiguration);
    }

    private static void RegisterServices(PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        configuration.RegisterServices(builder.Services);
    }

    private static void RegisterOptions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var optionsProducer = new OptionProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<OptionProducer>>());
        configuration.RegisterOptions(optionsProducer);
    }

    private static void RegisterViews(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var viewProducer = new TrailBlazrViewProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<TrailBlazrViewProducer>>());
        configuration.RegisterViews(viewProducer);
    }

    private static void RegisterMods(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var modsProducer = new ModsProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ModsProducer>>());
        configuration.RegisterMods(modsProducer);
    }

    private static void RegisterStartupActions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var startupActionProducer = new StartupActionsProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<StartupActionsProducer>>());
        configuration.RegisterStartupActions(startupActionProducer);
    }

    private static void RegisterPostUpdateActions(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var postUpdateActionProducer = new PostUpdateActionProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<PostUpdateActionProducer>>());
        configuration.RegisterPostUpdateActions(postUpdateActionProducer);
    }

    private static void RegisterNotificationHandlers(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var notificationHandlerProducer = new NotificationHandlerProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<NotificationHandlerProducer>>());
        configuration.RegisterNotificationHandlers(notificationHandlerProducer);
    }

    private static void RegisterArgumentHandlers(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var argumentHandlerProducer = new ArgumentHandlerProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ArgumentHandlerProducer>>());
        configuration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
    }

    private static void RegisterThemes(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var themeProducer = new ThemeProducer(builder.Services, bootstrap.Services.GetRequiredService<ILogger<ThemeProducer>>());
        configuration.RegisterThemes(themeProducer);
    }

    private static void RegisterMenuEntries(PhotinoBlazorApp bootstrap, PhotinoBlazorAppBuilder builder, PluginConfigurationBase configuration)
    {
        var menuEntryProducer = new MenuProducer(bootstrap.Services.GetRequiredService<ILogger<MenuProducer>>());
        configuration.RegisterMenuButtons(menuEntryProducer);

        builder.Services.AddSingleton<IReadOnlyDictionary<string, MenuCategory>>(menuEntryProducer.categories.AsReadOnly());
    }
}
