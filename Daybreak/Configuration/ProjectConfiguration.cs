using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Credentials;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Logging;
using Daybreak.Services.Privilege;
using Daybreak.Services.Screens;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Shortcuts;
using Daybreak.Services.Updater;
using Daybreak.Views;
using Microsoft.Extensions.Logging;
using Slim;
using System.Extensions;
using Daybreak.Services.Options;
using Microsoft.CorrelationVector;
using System.Logging;
using Daybreak.Services.Updater.PostUpdate;
using System.Core.Extensions;
using Daybreak.Services.Graph;
using Microsoft.Extensions.DependencyInjection;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Menu;
using Daybreak.Services.Experience;
using Daybreak.Services.Metrics;
using Daybreak.Services.Monitoring;
using Daybreak.Services.Downloads;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Startup;
using Daybreak.Services.Startup.Actions;
using Daybreak.Services.GuildWars;
using Daybreak.Configuration.Options;
using System.Configuration;
using Daybreak.Services.UMod;
using Daybreak.Services.Toolbox;
using Daybreak.Services.Themes;
using Daybreak.Services.TradeChat;
using System.Net.WebSockets;
using Daybreak.Services.Notifications;
using Daybreak.Services.Charts;
using Daybreak.Services.Images;
using Daybreak.Services.InternetChecker;
using Daybreak.Services.Sounds;
using Daybreak.Services.TradeChat.Notifications;
using Daybreak.Services.DSOAL;
using Daybreak.Services.Mods;
using Daybreak.Services.Registry;
using Daybreak.Services.DSOAL.Actions;
using Daybreak.Services.Events;
using Daybreak.Controls;
using Daybreak.Services.Plugins;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Services.Injection;
using Daybreak.Services.ReShade;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.DirectSong;
using Daybreak.Services.SevenZip;
using Daybreak.Services.ReShade.Notifications;
using Daybreak.Services.UBlockOrigin;
using Daybreak.Services.Browser;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Services.Toolbox.Notifications;
using Daybreak.Services.Guildwars;
using Daybreak.Services.Api;
using Daybreak.Services.Notifications.Handlers;
using Microsoft.Data.Sqlite;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Logging;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.TradeChat;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Shortcuts;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.Images;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Onboarding;
using Daybreak.Shared.Services.Sounds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Browser;
using Daybreak.Shared.Services.DSOAL;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Services.IconRetrieve;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Screenshots;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Experience;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Shared.Services.Events;
using Daybreak.Shared.Services.Startup;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.ReShade;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.DXVK;
using Daybreak.Services.DXVK;
using Daybreak.Shared.Services.MDns;
using Daybreak.Services.MDns;
using OpenTelemetry.Resources;
using Daybreak.Services.Telemetry;
using System.Reflection;
using Version = Daybreak.Shared.Models.Versioning.Version;
using Daybreak.Shared.Models.Plugins;
using TrailBlazr.Extensions;
using TrailBlazr.Services;
using Microsoft.Fast.Components.FluentUI;
using Daybreak.Themes;

namespace Daybreak.Configuration;

public class ProjectConfiguration : PluginConfigurationBase
{
    private const string DbConnectionString = "Data Source=Daybreak.sqlite.db";

    public static readonly Version CurrentVersion = Version.Parse(Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? throw new InvalidOperationException("Unable to get current version"));

    public override void RegisterResolvers(IServiceManager serviceManager)
    {
        serviceManager.ThrowIfNull();

        serviceManager.RegisterOptionsManager<OptionsManager>();
        serviceManager.RegisterResolver(new LoggerResolver());
        serviceManager.RegisterResolver(new ClientWebSocketResolver());
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services.ThrowIfNull();

        // Add FluentUI components
        services.AddFluentUIComponents();
        
        services.AddTrailBlazr();
        this.RegisterHttpClients(services);
        services.AddScoped(sp =>
        {
            var connection = new SqliteConnection(DbConnectionString);
            connection.Open();
            return connection;
        });
        services.AddScoped(sp => new TradeQuoteDbContext(sp.GetRequiredService<SqliteConnection>()));
        services.AddScoped(sp => new NotificationsDbContext(sp.GetRequiredService<SqliteConnection>()));
        services.AddScoped(sp => new TradeMessagesDbContext(sp.GetRequiredService<SqliteConnection>()));
        services.AddSingleton<SwappableLoggerProvider>();
        services.AddSingleton<ILogsManager, JsonLogsManager>();
        services.AddSingleton<IConsoleLogsWriter, ConsoleLogsWriter>();
        services.AddSingleton<IEventViewerLogsWriter, EventViewerLogsWriter>();
        services.AddSingleton<ILoggerFactory, ILoggerFactory>(sp =>
        {
            var swappableLoggerProvider = sp.GetRequiredService<SwappableLoggerProvider>();
            var factory = LoggerFactory.Create(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddProvider(new CVLoggerProvider(sp.GetRequiredService<ILogsWriter>()));
                logging.AddProvider(swappableLoggerProvider);
                logging.AddFilter<SwappableLoggerProvider>(static (_, level) => level >= LogLevel.Warning);
            });

            return factory;
        });
        services.AddSingleton<ILogsWriter, CompositeLogsWriter>(sp => new CompositeLogsWriter(
            sp.GetRequiredService<ILogsManager>(),
            sp.GetRequiredService<IConsoleLogsWriter>(),
            sp.GetRequiredService<IEventViewerLogsWriter>()));

        services.AddScoped((sp) => new ScopeMetadata(new CorrelationVector()));
        services.AddSingleton(sp =>
        {
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService("Daybreak", serviceVersion: CurrentVersion.ToString());
            var attributes = new List<KeyValuePair<string, object>>();
#if DEBUG
            attributes.Add(new KeyValuePair<string, object>("deployment.environment", "debug"));
#else
            attributes.Add(new KeyValuePair<string, object>("deployment.environment", "production"));
#endif
            resourceBuilder.AddAttributes(attributes);
            return resourceBuilder;
        });

        services.AddSingleton<AppViewModel>();

        services.AddSingleton<ViewManager>();
        services.AddSingleton<ProcessorUsageMonitor>();
        services.AddSingleton<MemoryUsageMonitor>();
        services.AddSingleton<DiskUsageMonitor>();
        services.AddSingleton<IViewManager, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<PostUpdateActionManager>();
        services.AddSingleton<IPostUpdateActionManager>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProducer>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProvider>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IMenuServiceInitializer, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMenuServiceProducer, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMenuServiceButtonHandler, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IShortcutManager, ShortcutManager>();
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<IStartupActionProducer, StartupActionManager>();
        services.AddSingleton<IOptionsProducer, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IOptionsUpdateHook, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IOptionsProvider, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IThemeManager, BlazorThemeInteropService>();
        services.AddSingleton<IThemeProducer, BlazorThemeInteropService>(sp => sp.GetRequiredService<IThemeManager>().Cast<BlazorThemeInteropService>());
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<INotificationProducer, NotificationService>(sp => sp.GetRequiredService<INotificationService>().Cast<NotificationService>());
        services.AddSingleton<INotificationHandlerProducer, NotificationService>(sp => sp.GetRequiredService<INotificationService>().Cast<NotificationService>());
        services.AddSingleton<ILiveChartInitializer, LiveChartInitializer>();
        services.AddSingleton<IImageCache, ImageCache>();
        services.AddSingleton<ISoundService, SoundService>();
        services.AddSingleton<IInternetCheckingService, InternetCheckingService>();
        services.AddSingleton<IConnectivityStatus, ConnectivityStatus>();
        services.AddSingleton<INotificationStorage, NotificationStorage>();
        services.AddSingleton<ITradeAlertingService, TradeAlertingService>();
        services.AddSingleton<IModsManager, ModsManager>();
        services.AddSingleton<IPluginsService, PluginsService>();
        services.AddSingleton<ISplashScreenService, SplashScreenService>();
        services.AddSingleton<IGuildWarsExecutableManager, GuildWarsExecutableManager>();
        services.AddSingleton<ISevenZipExtractor, SevenZipExtractor>();
        services.AddSingleton<IGraphClient, GraphClient>();
        services.AddSingleton<IOptionsSynchronizationService, OptionsSynchronizationService>();
        services.AddSingleton<IMDomainNameService, MDomainNameService>();
        services.AddSingleton<IMDomainRegistrar, MDomainRegistrar>();
        services.AddSingleton<IAttachedApiAccessor, AttachedApiAccessor>();
        services.AddSingleton<TelemetryHost>();
        services.AddScoped<IBrowserExtensionsManager, BrowserExtensionsManager>();
        services.AddScoped<IBrowserExtensionsProducer, BrowserExtensionsManager>(sp => sp.GetRequiredService<IBrowserExtensionsManager>().Cast<BrowserExtensionsManager>());
        services.AddScoped<ICredentialManager, CredentialManager>();
        services.AddScoped<IApplicationLauncher, ApplicationLauncher>();
        services.AddScoped<IScreenshotProvider, ScreenshotProvider>();
        services.AddScoped<IOnlinePictureClient, OnlinePictureClient>();
        services.AddScoped<IApplicationUpdater, ApplicationUpdater>();
        services.AddScoped<IBuildTemplateManager, BuildTemplateManager>();
        services.AddScoped<IIconCache, IconCache>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddScoped<IScreenManager, ScreenManager>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IExperienceCalculator, ExperienceCalculator>();
        services.AddScoped<IAttributePointCalculator, AttributePointCalculator>();
        services.AddScoped<IDownloadService, DownloadService>();
        services.AddScoped<IGuildWarsInstaller, IntegratedGuildwarsInstaller>();
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<ITradeChatService<KamadanTradeChatOptions>, TradeChatService<KamadanTradeChatOptions>>();
        services.AddScoped<ITradeChatService<AscalonTradeChatOptions>, TradeChatService<AscalonTradeChatOptions>>();
        services.AddScoped<ITraderQuoteService, TraderQuoteService>();
        services.AddScoped<IPriceHistoryDatabase, PriceHistoryDatabase>();
        services.AddScoped<IPriceHistoryService, PriceHistoryService>();
        services.AddScoped<IWordHighlightingService, WordHighlightingService>();
        services.AddScoped<ITradeHistoryDatabase, TradeHistoryDatabase>();
        services.AddScoped<IGuildWarsCopyService, GuildWarsCopyService>();
        services.AddScoped<IItemHashService, ItemHashService>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<IEventNotifierService, EventNotifierService>();
        services.AddScoped<IBackgroundProvider, BackgroundProvider>();
        services.AddScoped<IToolboxClient, ToolboxClient>();
        services.AddScoped<IProcessInjector, ProcessInjector>();
        services.AddScoped<IStubInjector, StubInjector>();
        services.AddScoped<ILaunchConfigurationService, LaunchConfigurationService>();
        services.AddScoped<IBrowserHistoryManager, BrowserHistoryManager>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IApplicationArgumentService, ApplicationArgumentService>();
        services.AddScoped<IArgumentHandlerProducer, IApplicationArgumentService>(sp => sp.GetRequiredService<IApplicationArgumentService>());
    }

    public override void RegisterViews(IViewProducer viewProducer)
    {
        viewProducer.ThrowIfNull();

        viewProducer.RegisterView<LaunchView, LaunchViewModel>(isSingleton: true);
        viewProducer.RegisterView<OptionView, OptionViewModel>();
        viewProducer.RegisterView<Views.FocusView, FocusViewModel>();
        viewProducer.RegisterView<BuildListView, BuildListViewModel>();
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.ThrowIfNull();

        startupActionProducer.RegisterAction<RestoreWindowPositionStartupAction>();
        startupActionProducer.RegisterAction<EnsureDatabaseTablesExist>();
        startupActionProducer.RegisterAction<RenameInstallerAction>();
        startupActionProducer.RegisterAction<FixSymbolicLinkStartupAction>();
        startupActionProducer.RegisterAction<UpdateUModAction>();
        startupActionProducer.RegisterAction<CredentialsOptionsMigrator>();
        startupActionProducer.RegisterAction<BrowserHistorySizeEnforcer>();
        startupActionProducer.RegisterAction<CleanupDatabases>();
        startupActionProducer.RegisterAction<DeleteOldDatabase>();
        startupActionProducer.RegisterAction<UpdateToolboxAction>();
    }

    public override void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer)
    {
        postUpdateActionProducer.ThrowIfNull();
    }

    public override void RegisterOptions(IOptionsProducer optionsProducer)
    {
        optionsProducer.ThrowIfNull();

        optionsProducer.RegisterOptions<TelemetryOptions>();
        optionsProducer.RegisterOptions<LauncherOptions>();
        optionsProducer.RegisterOptions<SoundOptions>();
        optionsProducer.RegisterOptions<ThemeOptions>();

        optionsProducer.RegisterOptions<BrowserOptions>();
        optionsProducer.RegisterOptions<SynchronizationOptions>();
        optionsProducer.RegisterOptions<FocusViewOptions>();

        optionsProducer.RegisterOptions<ImageCacheOptions>();

        optionsProducer.RegisterOptions<BackgroundProviderOptions>();

        optionsProducer.RegisterOptions<ToolboxOptions>();
        optionsProducer.RegisterOptions<UModOptions>();
        optionsProducer.RegisterOptions<DSOALOptions>();
        optionsProducer.RegisterOptions<ReShadeOptions>();
        optionsProducer.RegisterOptions<DXVKOptions>();

        optionsProducer.RegisterOptions<ScreenManagerOptions>();
        optionsProducer.RegisterOptions<KamadanTradeChatOptions>();
        optionsProducer.RegisterOptions<AscalonTradeChatOptions>();
        optionsProducer.RegisterOptions<PriceHistoryOptions>();
        optionsProducer.RegisterOptions<TraderQuotesOptions>();
        optionsProducer.RegisterOptions<TradeAlertingOptions>();
        optionsProducer.RegisterOptions<EventNotifierOptions>();
        optionsProducer.RegisterOptions<PluginsServiceOptions>();
        optionsProducer.RegisterOptions<GuildwarsExecutableOptions>();
        optionsProducer.RegisterOptions<CredentialManagerOptions>();
        optionsProducer.RegisterOptions<LaunchConfigurationServiceOptions>();
        optionsProducer.RegisterOptions<DirectSongOptions>();
        optionsProducer.RegisterOptions<MinimapWindowOptions>();
    }

    public override void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer)
    {
        notificationHandlerProducer.RegisterNotificationHandler<NoActionHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<MessageBoxHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<TradeMessageNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<FixSymbolicLinkNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<UpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<ReShadeConfigChangedHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<NavigateToCalendarViewHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<GuildWarsUpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<GuildWarsBatchUpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<ToolboxUpdateHandler>();
    }

    public override void RegisterMods(IModsManager modsManager)
    {
        modsManager.RegisterMod<IGuildWarsVersionChecker, GuildWarsVersionChecker>();
        modsManager.RegisterMod<IDaybreakApiService, DaybreakApiService>();
        modsManager.RegisterMod<IToolboxService, ToolboxService>();
        modsManager.RegisterMod<IReShadeService, ReShadeService>();
        modsManager.RegisterMod<IUModService, UModService>();
        modsManager.RegisterMod<IDSOALService, DSOALService>();
        modsManager.RegisterMod<IDXVKService, DXVKService>();
        modsManager.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
        modsManager.RegisterMod<IDirectSongService, DirectSongService>(singleton: true);
    }

    public override void RegisterBrowserExtensions(IBrowserExtensionsProducer browserExtensionsProducer)
    {
        browserExtensionsProducer.ThrowIfNull();
        browserExtensionsProducer.RegisterExtension<UBlockOriginService>();
    }

    public override void RegisterLaunchArgumentHandlers(IArgumentHandlerProducer argumentHandlerProducer)
    {
        argumentHandlerProducer.ThrowIfNull();
        argumentHandlerProducer.RegisterArgumentHandler<AutoLaunchArgumentHandler>();
    }

    public override void RegisterMenuButtons(IMenuServiceProducer menuServiceProducer)
    {
        menuServiceProducer.ThrowIfNull();
        menuServiceProducer.CreateIfNotExistCategory("Launcher")
            .RegisterButton("Notifications", "Open notifications view", sp => { })
            .RegisterButton("Plugins", "Open plugins view", sp => { })
            .RegisterButton("Manage client version", "Open version manager", sp => { });
        menuServiceProducer.CreateIfNotExistCategory("Guild Wars")
            .RegisterButton("Game companion", "Open game companion", sp => sp.GetRequiredService<IViewManager>().ShowView<LaunchView>())
            .RegisterButton("Manage builds", "Open builds manager", sp => sp.GetRequiredService<IViewManager>().ShowView<BuildListView>())
            .RegisterButton("Download Guild Wars", "Download Guild Wars installer", sp => { })
            .RegisterButton("Copy Guild Wars", "Copy Guild Wars from an existing installation", sp => { })
            .RegisterButton("Event Calendar", "Show current and upcoming events", sp => { })
            .RegisterButton("Guild Wars Party Search", "Show party search broadcasts", sp => { });
        menuServiceProducer.CreateIfNotExistCategory("Trade")
            .RegisterButton("Alerts", "Open trade alerts manager", sp => { })
            .RegisterButton("Kamadan", "Open kamadan trade chat", sp => { })
            .RegisterButton("Ascalon", "Open ascalon trade chat", sp => { })
            .RegisterButton("Trader Quotes", "Open trader quotes view", sp => { });
        menuServiceProducer.CreateIfNotExistCategory("Mods")
            .RegisterButton("uMod", "Open uMod manager", sp => { })
            .RegisterButton("GWToolboxpp", "Open GWToolbox manager", sp => { })
            .RegisterButton("DSOAL", "Open DSOAL manager", sp => { })
            .RegisterButton("ReShade", "Open ReShade manager", sp => { })
            .RegisterButton("DirectSong", "Open DirectSong manager", sp => { })
            .RegisterButton("DXVK", "Open DXVK manager", sp => { });
        menuServiceProducer.CreateIfNotExistCategory("Settings")
            .RegisterButton("Accounts", "Accounts Settings", sp => { })
            .RegisterButton("Executables", "Executables Settings", sp => { })
            .RegisterButton("Launch configurations", "Launch configurations settings", sp => { });
        menuServiceProducer.CreateIfNotExistCategory("Diagnostics")
            .RegisterButton("Telemetry", "Open telemetry view", sp => { })
            .RegisterButton("Logs", "Open logs view", sp => { })
            .RegisterButton("Metrics", "Open metrics view", sp => { });
    }

    public override void RegisterThemes(IThemeProducer themeProducer)
    {
        themeProducer.ThrowIfNull();
        foreach(var theme in CoreThemes.Themes)
        {
            themeProducer.RegisterTheme(theme);
        }

        foreach (var theme in HeroThemes.Themes)
        {
            themeProducer.RegisterTheme(theme);
        }
    }

    private IServiceCollection RegisterHttpClients(IServiceCollection services)
    {
        return services
            .ThrowIfNull()
            .RegisterHttpClient<ApplicationUpdater>()
                .WithMessageHandler(SetupLoggingAndMetrics<ApplicationUpdater>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<OnlinePictureClient>()
                .WithMessageHandler(SetupLoggingAndMetrics<OnlinePictureClient>)
                .WithDefaultRequestHeadersSetup(SetupChromeImpersonationUserAgent)
                .Build()
            .RegisterHttpClient<GraphClient>()
                .WithMessageHandler(SetupLoggingAndMetrics<GraphClient>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<DownloadService>()
                .WithMessageHandler(SetupLoggingAndMetrics<DownloadService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TradeChatService<KamadanTradeChatOptions>>()
                .WithMessageHandler(SetupLoggingAndMetrics<TradeChatService<KamadanTradeChatOptions>>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TradeChatService<AscalonTradeChatOptions>>()
                .WithMessageHandler(SetupLoggingAndMetrics<TradeChatService<AscalonTradeChatOptions>>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<IconCache>()
                .WithMessageHandler(SetupLoggingAndMetrics<IconCache>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TraderQuoteService>()
                .WithMessageHandler(SetupLoggingAndMetrics<TraderQuoteService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<PriceHistoryService>()
                .WithMessageHandler(SetupLoggingAndMetrics<PriceHistoryService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<InternetCheckingService>()
                .WithMessageHandler(SetupLoggingAndMetrics<InternetCheckingService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ChromiumBrowserWrapper>()
                .WithMessageHandler(SetupLoggingAndMetrics<ChromiumBrowserWrapper>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ToolboxClient>()
                .WithMessageHandler(SetupLoggingAndMetrics<ToolboxClient>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ReShadeService>()
                .WithMessageHandler(SetupLoggingAndMetrics<ReShadeService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<UModService>()
                .WithMessageHandler(SetupLoggingAndMetrics<UModService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<UBlockOriginService>()
                .WithMessageHandler(SetupLoggingAndMetrics<UBlockOriginService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<DXVKService>()
                .WithMessageHandler(SetupLoggingAndMetrics<DXVKService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ScopedApiContext>()
                .WithMessageHandler(SetupLoggingAndMetrics<ScopedApiContext>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build();
    }
}
