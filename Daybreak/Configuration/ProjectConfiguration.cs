using Daybreak.Services.ApplicationDetection;
using Daybreak.Services.ApplicationLifetime;
using Daybreak.Services.Bloogum;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.Logging;
using Daybreak.Services.Screenshots;
using Daybreak.Services.ViewManagement;
using Daybreak.Views;
using Microsoft.Web.WebView2.Core;
using Slim;
using System.Extensions;

namespace Daybreak.Configuration
{
    // TODO: Credit http://bloogum.net/guildwars/
    public static class ProjectConfiguration
    {
        public static void RegisterServices(IServiceProducer serviceProducer)
        {
            serviceProducer.ThrowIfNull(nameof(serviceProducer));

            serviceProducer.RegisterSingleton<ICredentialManager, CredentialManager>();
            serviceProducer.RegisterSingleton<ILoggingDatabase, FlatLoggingDatabase>();
            serviceProducer.RegisterSingleton<ILogger, Logger>();
            serviceProducer.RegisterSingleton<ApplicationLifetimeManager>();
            serviceProducer.RegisterSingleton<ViewManager>();
            serviceProducer.RegisterSingleton<IApplicationDetector, ApplicationDetector>();
            serviceProducer.RegisterSingleton<IScreenshotProvider, ScreenshotProvider>();
            serviceProducer.RegisterSingleton<IConfigurationManager, ConfigurationManager>();
            serviceProducer.RegisterSingleton<IBloogumClient, BloogumClient>();
            serviceProducer.RegisterSingleton<CoreWebView2Environment, CoreWebView2Environment>((sp) => TaskExtensions.RunSync(() => CoreWebView2Environment.CreateAsync(null, "BrowserData", null)));
        }
        public static void RegisterLifetimeServices(IApplicationLifetimeProducer applicationLifetimeProducer)
        {
            applicationLifetimeProducer.ThrowIfNull(nameof(applicationLifetimeProducer));

            applicationLifetimeProducer.RegisterService<ILoggingDatabase>();
            applicationLifetimeProducer.RegisterService<IScreenshotProvider>();
        }
        public static void RegisterViews(IViewProducer viewProducer)
        {
            viewProducer.ThrowIfNull(nameof(viewProducer));

            viewProducer.RegisterView<MainView>();
            viewProducer.RegisterView<SettingsView>();
        }
    }
}
