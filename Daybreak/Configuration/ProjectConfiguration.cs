using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Bloogum;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Configuration;
using Daybreak.Services.Credentials;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Logging;
using Daybreak.Services.Mutex;
using Daybreak.Services.Privilege;
using Daybreak.Services.Screens;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Shortcuts;
using Daybreak.Services.Updater;
using Daybreak.Views;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Slim;
using System.Extensions;
using System.Net.Http;
using LiteDB;
using Daybreak.Services.Options;
using Daybreak.Models;
using Microsoft.CorrelationVector;
using System.Logging;
using Daybreak.Services.Updater.PostUpdate;
using System.Core.Extensions;
using Daybreak.Services.Updater.PostUpdate.Actions;
using Daybreak.Services.Graph;
using Microsoft.Extensions.DependencyInjection;
using Daybreak.Services.Navigation;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Menu;
using Daybreak.Services.Scanner;

namespace Daybreak.Configuration;

public static class ProjectConfiguration
{
    public static void RegisterResolvers(IServiceManager serviceManager)
    {
        serviceManager.ThrowIfNull();

        serviceManager.RegisterOptionsManager<ApplicationConfigurationOptionsManager>();
        serviceManager.RegisterResolver(new LoggerResolver());
        serviceManager
            .RegisterHttpClient<ApplicationUpdater>()
                .WithMessageHandler(sp =>
                {
                    var logger = sp.GetService<ILogger<ApplicationUpdater>>();
                    return new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() };
                })
                .Build()
            .RegisterHttpClient<BloogumClient>()
                .WithMessageHandler(sp =>
                {
                    var logger = sp.GetService<ILogger<BloogumClient>>();
                    return new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() };
                })
                .Build()
            .RegisterHttpClient<GraphClient>()
                .WithMessageHandler(sp =>
                {
                    var logger = sp.GetService<ILogger<GraphClient>>();
                    return new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() };
                })
                .Build();
    }

    public static void RegisterServices(IServiceCollection services)
    {
        services.ThrowIfNull();

        services.AddSingleton<ILogsManager, JsonLogsManager>();
        services.AddSingleton<IDebugLogsWriter, Services.Logging.DebugLogsWriter>();
        services.AddSingleton<IEventViewerLogsWriter, EventViewerLogsWriter>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>(sp =>
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new CVLoggerProvider(sp.GetService<ILogsWriter>()));
            return factory;
        });
        services.AddSingleton<ILogsWriter, CompositeLogsWriter>(sp => new CompositeLogsWriter(
            sp.GetService<ILogsManager>()!,
            sp.GetService<IDebugLogsWriter>()!,
            sp.GetService<IEventViewerLogsWriter>()!));

        services.AddScoped((sp) => new ScopeMetadata(new CorrelationVector()));
        services.AddSingleton<ViewManager>();
        services.AddSingleton<IViewManager, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<IViewProducer, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<PostUpdateActionManager>();
        services.AddSingleton<IPostUpdateActionManager>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProducer>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProvider>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IMenuServiceInitializer, MenuService>(sp => sp.GetRequiredService<IMenuService>().As<MenuService>());
        services.AddSingleton<IConfigurationManager, ConfigurationManager>();
        services.AddSingleton<ILiteDatabase, LiteDatabase>(sp => new LiteDatabase("Daybreak.db"));
        services.AddSingleton<IMutexHandler, MutexHandler>();
        services.AddSingleton<IShortcutManager, ShortcutManager>();
        services.AddSingleton<IIconBrowser, IconBrowser>();
        services.AddSingleton<IIconDownloader, IconDownloader>();
        services.AddScoped<ICredentialManager, CredentialManager>();
        services.AddScoped<IApplicationLauncher, ApplicationLauncher>();
        services.AddScoped<IScreenshotProvider, ScreenshotProvider>();
        services.AddScoped<IBloogumClient, BloogumClient>();
        services.AddScoped<IApplicationUpdater, ApplicationUpdater>();
        services.AddScoped<IBuildTemplateManager, BuildTemplateManager>();
        services.AddScoped<IIconCache, IconCache>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddScoped<IScreenManager, ScreenManager>();
        services.AddScoped<IGraphClient, GraphClient>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IGuildwarsMemoryReader, GuildwarsMemoryReader>();
        services.AddScoped<IMemoryScanner, MemoryScanner>();
    }

    public static void RegisterViews(IViewProducer viewProducer)
    {
        viewProducer.ThrowIfNull();

        viewProducer.RegisterPermanentView<LauncherView>();
        viewProducer.RegisterView<SettingsView>();
        viewProducer.RegisterView<AskUpdateView>();
        viewProducer.RegisterView<UpdateView>();
        viewProducer.RegisterView<AccountsView>();
        viewProducer.RegisterView<ExperimentalSettingsView>();
        viewProducer.RegisterView<ExecutablesView>();
        viewProducer.RegisterView<BuildTemplateView>();
        viewProducer.RegisterView<BuildsListView>();
        viewProducer.RegisterView<RequestElevationView>();
        viewProducer.RegisterView<ScreenChoiceView>();
        viewProducer.RegisterView<VersionManagementView>();
        viewProducer.RegisterView<LogsView>();
        viewProducer.RegisterView<IconDownloadView>();
        viewProducer.RegisterView<GraphAuthorizationView>();
        viewProducer.RegisterView<BuildsSynchronizationView>();
        viewProducer.RegisterView<OnboardingView>();
        viewProducer.RegisterView<FocusView>();
    }

    public static void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer)
    {
        postUpdateActionProducer.ThrowIfNull();

        postUpdateActionProducer.AddPostUpdateAction<RenameInstallerAction>();
    }
}
