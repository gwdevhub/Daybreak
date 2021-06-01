using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.ApplicationLifetime;
using Daybreak.Services.Bloogum;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Logging;
using Daybreak.Services.Mutex;
using Daybreak.Services.Privilege;
using Daybreak.Services.Runtime;
using Daybreak.Services.Screens;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Shortcuts;
using Daybreak.Services.Updater;
using Daybreak.Services.ViewManagement;
using Daybreak.Views;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Slim;
using System.Extensions;
using System.Net.Http;
using System.Windows.Extensions.Http;
using LiteDB;
using Daybreak.Controls;

namespace Daybreak.Configuration
{
    public static class ProjectConfiguration
    {
        public static void RegisterResolvers(IServiceManager serviceManager)
        {
            serviceManager.ThrowIfNull(nameof(serviceManager));

            serviceManager.RegisterScoped<ILoggerFactory, CVLoggerFactory>((sp) => new CVLoggerFactory(sp.GetService<ILogsManager>()));
            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory((serviceProvider, categoryType) =>
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(categoryType);
                    var logger = serviceProvider.GetService(loggerType).As<ILogger>();
                    var handler = new LoggingHttpMessageHandler(logger) { InnerHandler = new HttpClientHandler() };
                    return handler;
                }));
        }

        public static void RegisterServices(IServiceProducer serviceProducer)
        {
            serviceProducer.ThrowIfNull(nameof(serviceProducer));

            serviceProducer.RegisterSingleton<ApplicationLifetimeManager>();
            serviceProducer.RegisterSingleton<ViewManager>();
            serviceProducer.RegisterSingleton<IConfigurationManager, ConfigurationManager>();
            serviceProducer.RegisterSingleton<ILiteDatabase, LiteDatabase>(sp => new LiteDatabase("Daybreak.db"));
            serviceProducer.RegisterSingleton<IRuntimeStore, RuntimeStore>();
            serviceProducer.RegisterSingleton<IMutexHandler, MutexHandler>();
            serviceProducer.RegisterSingleton<IShortcutManager, ShortcutManager>();
            serviceProducer.RegisterScoped<ICredentialManager, CredentialManager>();
            serviceProducer.RegisterScoped<IApplicationLauncher, ApplicationLauncher>();
            serviceProducer.RegisterScoped<IScreenshotProvider, ScreenshotProvider>();
            serviceProducer.RegisterScoped<IBloogumClient, BloogumClient>();
            serviceProducer.RegisterScoped<IApplicationUpdater, ApplicationUpdater>();
            serviceProducer.RegisterScoped<IBuildTemplateManager, BuildTemplateManager>();
            serviceProducer.RegisterScoped<IIconRetriever, IconRetriever>();
            serviceProducer.RegisterScoped<IPrivilegeManager, PrivilegeManager>();
            serviceProducer.RegisterScoped<IScreenManager, ScreenManager>();
            serviceProducer.RegisterScoped<ILogsManager, JsonLogsManager>();
        }
        public static void RegisterLifetimeServices(IApplicationLifetimeProducer applicationLifetimeProducer)
        {
            applicationLifetimeProducer.ThrowIfNull(nameof(applicationLifetimeProducer));

            applicationLifetimeProducer.RegisterService<IScreenshotProvider>();
            applicationLifetimeProducer.RegisterService<IApplicationUpdater>();
            applicationLifetimeProducer.RegisterService<IShortcutManager>();
        }
        public static void RegisterViews(IViewProducer viewProducer)
        {
            viewProducer.ThrowIfNull(nameof(viewProducer));

            viewProducer.RegisterView<MainView>();
            viewProducer.RegisterView<SettingsView>();
            viewProducer.RegisterView<AskUpdateView>();
            viewProducer.RegisterView<UpdateView>();
            viewProducer.RegisterView<SettingsCategoryView>();
            viewProducer.RegisterView<AccountsView>();
            viewProducer.RegisterView<ExperimentalSettingsView>();
            viewProducer.RegisterView<ExecutablesView>();
            viewProducer.RegisterView<BuildTemplateView>();
            viewProducer.RegisterView<BuildsListView>();
            viewProducer.RegisterView<RequestElevationView>();
            viewProducer.RegisterView<ScreenChoiceView>();
            viewProducer.RegisterView<VersionManagementView>();
            viewProducer.RegisterView<LogsView>();
        }
    }
}
