using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.FileProviders;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;
using SimplePlugin.Options;
using SimplePlugin.Services;
using SimplePlugin.Themes;

namespace SimplePlugin.Configuration;

/// <summary>
/// This class acts as the manifest of the plugin.
/// This is where you can declare everything that the plugin provides.
/// Provides configuration and registration logic for plugins.
/// </summary>
public sealed class PluginConfiguration : PluginConfigurationBase
{
    public override void RegisterViews(IViewProducer viewProducer)
    {
    }

    public override void RegisterThemes(IThemeProducer themeProducer)
    {
        themeProducer.RegisterTheme(SimpleTheme.Instance);
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.RegisterAction<SimpleNotificationStartupAction>();
    }

    public override void RegisterServices(IServiceCollection services)
    {
    }

    public override void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer)
    {
    }

    public override void RegisterOptions(IOptionsProducer optionsProducer)
    {
        optionsProducer.RegisterOptions<SimpleOptions>();
    }

    public override void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer)
    {
    }

    public override void RegisterMenuButtons(IMenuServiceProducer menuServiceProducer)
    {
        menuServiceProducer.CreateIfNotExistCategory("Simple Plugin Category")
            .RegisterButton("Click me", "I dare you!", sp => sp.GetRequiredService<INotificationService>().NotifyError(title: "Gotcha!", description: "Not so simple, is it?"));
    }

    public override void RegisterLaunchArgumentHandlers(IArgumentHandlerProducer argumentHandlerProducer)
    {
    }

    public override void RegisterMods(IModsProducer modsProducer)
    {
        modsProducer.RegisterMod<SimpleNotificationMod, SimpleNotificationMod>(true);
    }

    public override void RegisterProviderAssemblies(IFileProviderProducer fileProviderProducer)
    {
        fileProviderProducer.RegisterAssembly(typeof(PluginConfiguration).Assembly);
    }
}
