using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.ApplicationLifetime;
using Daybreak.Services.Bloogum;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Logging;
using Daybreak.Services.Mutex;
using Daybreak.Services.Runtime;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Updater;
using Daybreak.Services.ViewManagement;
using Daybreak.Views;
using Slim;
using System.Extensions;

namespace Daybreak.Configuration
{
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
            serviceProducer.RegisterSingleton<IApplicationLauncher, ApplicationLauncher>();
            serviceProducer.RegisterSingleton<IScreenshotProvider, ScreenshotProvider>();
            serviceProducer.RegisterSingleton<IConfigurationManager, ConfigurationManager>();
            serviceProducer.RegisterSingleton<IBloogumClient, BloogumClient>();
            serviceProducer.RegisterSingleton<IApplicationUpdater, ApplicationUpdater>();
            serviceProducer.RegisterSingleton<IMutexHandler, MutexHandler>();
            serviceProducer.RegisterSingleton<IRuntimeStore, RuntimeStore>();
            serviceProducer.RegisterSingleton<IBuildTemplateManager, BuildTemplateManager>();
            serviceProducer.RegisterSingleton<IIconRetriever, IconRetriever>();
        }
        public static void RegisterLifetimeServices(IApplicationLifetimeProducer applicationLifetimeProducer)
        {
            applicationLifetimeProducer.ThrowIfNull(nameof(applicationLifetimeProducer));

            applicationLifetimeProducer.RegisterService<ILoggingDatabase>();
            applicationLifetimeProducer.RegisterService<IScreenshotProvider>();
            applicationLifetimeProducer.RegisterService<IApplicationUpdater>();
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
        }
    }
}
