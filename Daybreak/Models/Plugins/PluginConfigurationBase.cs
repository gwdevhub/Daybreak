using Daybreak.Configuration.Options;
using Daybreak.Services.Browser;
using Daybreak.Services.Drawing;
using Daybreak.Services.Metrics;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Utils;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slim;
using System.Core.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Daybreak.Models.Plugins;

public abstract class PluginConfigurationBase
{
    private const string DaybreakUserAgent = "Daybreak";
    private const string ChromeImpersonationUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79";

    public virtual void RegisterResolvers(IServiceManager serviceManager) { }
    public virtual void RegisterServices(IServiceCollection services) { }
    public virtual void RegisterViews(IViewProducer viewProducer) { }
    public virtual void RegisterStartupActions(IStartupActionProducer startupActionProducer) { }
    public virtual void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer) { }
    public virtual void RegisterDrawingModules(IDrawingModuleProducer drawingModuleProducer) { }
    public virtual void RegisterOptions(IOptionsProducer optionsProducer) { }
    public virtual void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer) { }
    public virtual void RegisterMods(IModsManager modsManager) { }
    public virtual void RegisterBrowserExtensions(IBrowserExtensionsProducer browserExtensionsProducer) { }

    public PluginConfigurationBase()
    {
    }

    public void RegisterLiteCollection<TCollectionType, TOptionsType>(IServiceCollection services)
        where TOptionsType : class, ILiteCollectionOptions<TCollectionType>
    {
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TOptionsType>>();
            var liteDatabase = sp.GetRequiredService<ILiteDatabase>();
            return liteDatabase.GetCollection<TCollectionType>(options.Value.CollectionName, BsonAutoId.Int64);
        });
    }
    
    public HttpMessageHandler SetupLoggingAndMetrics<T>(System.IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();
        var metricsService = serviceProvider.GetRequiredService<IMetricsService>();


        return new MetricsHttpMessageHandler<T>(
            metricsService,
            new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() });
    }

    public void SetupDaybreakUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", DaybreakUserAgent);
    }

    public void SetupChromeImpersonationUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", ChromeImpersonationUserAgent);
    }
}
