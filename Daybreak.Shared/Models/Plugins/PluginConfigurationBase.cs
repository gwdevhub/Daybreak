using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Browser;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Startup;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Slim;
using System.Core.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Daybreak.Shared.Models.Plugins;

public abstract class PluginConfigurationBase
{
    private const string DaybreakUserAgent = "Daybreak";
    private const string ChromeImpersonationUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79";

    public virtual void RegisterResolvers(IServiceManager serviceManager) { }
    public virtual void RegisterServices(IServiceCollection services) { }
    public virtual void RegisterViews(IViewProducer viewProducer) { }
    public virtual void RegisterStartupActions(IStartupActionProducer startupActionProducer) { }
    public virtual void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer) { }
    public virtual void RegisterOptions(IOptionsProducer optionsProducer) { }
    public virtual void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer) { }
    public virtual void RegisterMods(IModsManager modsManager) { }
    public virtual void RegisterBrowserExtensions(IBrowserExtensionsProducer browserExtensionsProducer) { }
    public virtual void RegisterLaunchArgumentHandlers(IArgumentHandlerProducer argumentHandlerProducer) { }
    public virtual void RegisterMenuButtons(IMenuServiceProducer menuServiceProducer) { }
    public virtual void RegisterThemes(IThemeProducer themeProducer) { }

    public PluginConfigurationBase()
    {
    }
    
    public static HttpMessageHandler SetupLoggingAndMetrics<T>(System.IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();
        var metricsService = serviceProvider.GetRequiredService<IMetricsService>();


        return new MetricsHttpMessageHandler<T>(
            metricsService,
            new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() });
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
