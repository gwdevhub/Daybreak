using Daybreak.Shared.Services.FileProviders;
using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using System.Core.Extensions;
using System.Net.Http.Headers;

namespace Daybreak.Shared.Models.Plugins;

public abstract class PluginConfigurationBase
{
    private const string DaybreakUserAgent = "Daybreak";
    private const string ChromeImpersonationUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79";

    public virtual void RegisterServices(IServiceCollection services) { }
    public virtual void RegisterViews(IViewProducer viewProducer) { }
    public virtual void RegisterStartupActions(IStartupActionProducer startupActionProducer) { }
    public virtual void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer) { }
    public virtual void RegisterOptions(IOptionsProducer optionsProducer) { }
    public virtual void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer) { }
    public virtual void RegisterMods(IModsProducer modsProducer) { }
    public virtual void RegisterLaunchArgumentHandlers(IArgumentHandlerProducer argumentHandlerProducer) { }
    public virtual void RegisterMenuButtons(IMenuServiceProducer menuServiceProducer) { }
    public virtual void RegisterThemes(IThemeProducer themeProducer) { }
    public virtual void RegisterProviderAssemblies(IFileProviderProducer fileProviderProducer) { }
    public virtual void ConfigureWindow(PhotinoBlazorApp app) { }

    public PluginConfigurationBase()
    {
    }
    
    public static HttpMessageHandler SetupLoggingAndMetrics<T>(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();
        return new LoggingHttpMessageHandler(logger) { InnerHandler = new HttpClientHandler() };
    }

    public static void SetupDaybreakUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", DaybreakUserAgent);
    }

    public static void SetupChromeImpersonationUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", ChromeImpersonationUserAgent);
    }
}
