using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
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
using Daybreak.Services.Navigation;
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
using Daybreak.Views.Onboarding.UMod;
using Daybreak.Views.Onboarding.Toolbox;
using Daybreak.Services.Themes;
using Daybreak.Services.TradeChat;
using Daybreak.Views.Trade;
using System.Net.WebSockets;
using Daybreak.Services.Notifications;
using Daybreak.Services.Charts;
using Daybreak.Services.Images;
using Daybreak.Services.InternetChecker;
using System;
using Daybreak.Services.Sounds;
using Daybreak.Models.Notifications.Handling;
using Daybreak.Services.TradeChat.Notifications;
using Daybreak.Views.Copy;
using Daybreak.Services.DSOAL;
using Daybreak.Services.Mods;
using Daybreak.Views.Onboarding.DSOAL;
using Daybreak.Services.Registry;
using Daybreak.Services.DSOAL.Actions;
using Daybreak.Services.Events;
using Daybreak.Controls;
using Daybreak.Services.Plugins;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Services.Injection;
using Daybreak.Services.ReShade;
using Daybreak.Views.Onboarding.ReShade;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Views.Launch;
using Daybreak.Services.DirectSong;
using Daybreak.Views.Onboarding.DirectSong;
using Daybreak.Services.SevenZip;
using Daybreak.Services.ReShade.Notifications;
using Daybreak.Services.UBlockOrigin;
using Daybreak.Services.Browser;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Services.Window;
using Daybreak.Launch;
using Daybreak.Views.Installation;
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
using Daybreak.Shared.Services.Mutex;
using Daybreak.Shared.Services.IconRetrieve;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Screenshots;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Experience;
using Daybreak.Shared.Services.Window;
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
using Daybreak.Shared.Shared.Models.Plugins;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.DXVK;
using Daybreak.Services.DXVK;
using Daybreak.Views.Onboarding.DXVK;
using Daybreak.Shared.Services.MDns;
using Daybreak.Services.MDns;

namespace Daybreak.Configuration;

public class ProjectConfiguration : PluginConfigurationBase
{
    private const string DbConnectionString = "Data Source=Daybreak.sqlite.db";

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
        services.AddSingleton<ILogsManager, JsonLogsManager>();
        services.AddSingleton<IConsoleLogsWriter, ConsoleLogsWriter>();
        services.AddSingleton<IEventViewerLogsWriter, EventViewerLogsWriter>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>(sp =>
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new CVLoggerProvider(sp.GetService<ILogsWriter>()));
            return factory;
        });
        services.AddSingleton<ILogsWriter, CompositeLogsWriter>(sp => new CompositeLogsWriter(
            sp.GetService<ILogsManager>()!,
            sp.GetService<IConsoleLogsWriter>()!,
            sp.GetService<IEventViewerLogsWriter>()!));

        services.AddScoped((sp) => new ScopeMetadata(new CorrelationVector()));
        services.AddSingleton<ViewManager>();
        services.AddSingleton<ProcessorUsageMonitor>();
        services.AddSingleton<MemoryUsageMonitor>();
        services.AddSingleton<DiskUsageMonitor>();
        services.AddSingleton<IViewManager, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<IViewProducer, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<PostUpdateActionManager>();
        services.AddSingleton<IPostUpdateActionManager>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProducer>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProvider>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IMenuServiceInitializer, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMenuServiceProducer, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMenuServiceButtonHandler, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMutexHandler, MutexHandler>();
        services.AddSingleton<IShortcutManager, ShortcutManager>();
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<IStartupActionProducer, StartupActionManager>();
        services.AddSingleton<IOptionsProducer, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IOptionsUpdateHook, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IOptionsProvider, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().Cast<OptionsManager>());
        services.AddSingleton<IThemeManager, ThemeManager>();
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
        services.AddSingleton<IWindowEventsHook<MainWindow>, WindowEventsHook<MainWindow>>();
        services.AddSingleton<IMDomainNameService, MDomainNameService>();
        services.AddSingleton<IMDomainRegistrar, MDomainRegistrar>();
        services.AddSingleton<IAttachedApiAccessor, AttachedApiAccessor>();
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

        viewProducer.RegisterPermanentView<Views.FocusView>();
        viewProducer.RegisterView<LauncherView>();
        viewProducer.RegisterView<UpdateView>();
        viewProducer.RegisterView<AccountsView>();
        viewProducer.RegisterView<ExecutablesView>();
        viewProducer.RegisterView<SingleBuildTemplateView>();
        viewProducer.RegisterView<BuildsListView>();
        viewProducer.RegisterView<RequestElevationView>();
        viewProducer.RegisterView<RequestDelevationView>();
        viewProducer.RegisterView<ScreenChoiceView>();
        viewProducer.RegisterView<VersionManagementView>();
        viewProducer.RegisterView<LogsView>();
        viewProducer.RegisterView<GraphAuthorizationView>();
        viewProducer.RegisterView<BuildsSynchronizationView>();
        viewProducer.RegisterView<LauncherOnboardingView>();
        viewProducer.RegisterView<MetricsView>();
        viewProducer.RegisterView<GuildWarsDownloadView>();
        viewProducer.RegisterView<GuildWarsDownloadSelectionView>();
        viewProducer.RegisterView<UModInstallingView>();
        viewProducer.RegisterView<UModInstallationChoiceView>();
        viewProducer.RegisterView<UModOnboardingEntryView>();
        viewProducer.RegisterView<UModMainView>();
        viewProducer.RegisterView<UModBrowserView>();
        viewProducer.RegisterView<ToolboxInstallationView>();
        viewProducer.RegisterView<ToolboxInstallationChoiceView>();
        viewProducer.RegisterView<ToolboxOnboardingEntryView>();
        viewProducer.RegisterView<ToolboxSwitchView>();
        viewProducer.RegisterView<ToolboxHomepageView>();
        viewProducer.RegisterView<OptionSectionView>();
        viewProducer.RegisterView<KamadanTradeChatView>();
        viewProducer.RegisterView<AscalonTradeChatView>();
        viewProducer.RegisterView<PriceQuotesView>();
        viewProducer.RegisterView<PriceHistoryView>();
        viewProducer.RegisterView<NotificationsView>();
        viewProducer.RegisterView<TradeAlertsView>();
        viewProducer.RegisterView<TradeAlertSetupView>();
        viewProducer.RegisterView<TradeNotificationView>();
        viewProducer.RegisterView<TradeAlertsChoiceView>();
        viewProducer.RegisterView<QuoteAlertSetupView>();
        viewProducer.RegisterView<GuildwarsCopySelectionView>();
        viewProducer.RegisterView<GuildwarsCopyView>();
        viewProducer.RegisterView<DSOALInstallingView>();
        viewProducer.RegisterView<DSOALOnboardingEntryView>();
        viewProducer.RegisterView<DSOALSwitchView>();
        viewProducer.RegisterView<DSOALHomepageView>();
        viewProducer.RegisterView<DSOALBrowserView>();
        viewProducer.RegisterView<PluginsView>();
        viewProducer.RegisterView<PluginsConfirmationView>();
        viewProducer.RegisterView<ReShadeInstallingView>();
        viewProducer.RegisterView<ReShadeInstallationChoiceView>();
        viewProducer.RegisterView<ReShadeOnboardingEntryView>();
        viewProducer.RegisterView<ReShadeMainView>();
        viewProducer.RegisterView<ReShadeBrowserView>();
        viewProducer.RegisterView<ReShadeStockEffectsSelectorView>();
        viewProducer.RegisterView<ReShadeConfigView>();
        viewProducer.RegisterView<ReShadePresetView>();
        viewProducer.RegisterView<LaunchConfigurationView>();
        viewProducer.RegisterView<LaunchConfigurationsView>();
        viewProducer.RegisterView<DirectSongInstallationView>();
        viewProducer.RegisterView<DirectSongInstallationChoiceView>();
        viewProducer.RegisterView<DirectSongOnboardingEntryView>();
        viewProducer.RegisterView<DirectSongSwitchView>();
        viewProducer.RegisterView<SettingsSynchronizationView>();
        viewProducer.RegisterView<TeamBuildTemplateView>();
        viewProducer.RegisterView<EventCalendarView>();
        viewProducer.RegisterView<UpdateConfirmationView>();
        viewProducer.RegisterView<GuildWarsPartySearchView>();
        viewProducer.RegisterView<DXVKInstallationChoiceView>();
        viewProducer.RegisterView<DXVKInstallingView>();
        viewProducer.RegisterView<DXVKOnboardingEntryView>();
        viewProducer.RegisterView<DXVKSwitchView>();
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
            .RegisterButton("Notifications", "Open notifications view", sp => sp.GetRequiredService<ViewManager>().ShowView<NotificationsView>())
            .RegisterButton("Plugins", "Open plugins view", sp => sp.GetRequiredService<ViewManager>().ShowView<PluginsView>())
            .RegisterButton("Manage client version", "Open version manager", sp => sp.GetRequiredService<ViewManager>().ShowView<VersionManagementView>());
        menuServiceProducer.CreateIfNotExistCategory("Guild Wars")
            .RegisterButton("Game companion", "Open game companion", sp => sp.GetRequiredService<ViewManager>().ShowView<LauncherView>())
            .RegisterButton("Manage builds", "Open builds manager", sp => sp.GetRequiredService<ViewManager>().ShowView<BuildsListView>())
            .RegisterButton("Download Guild Wars", "Download Guild Wars installer", sp => sp.GetRequiredService<ViewManager>().ShowView<GuildWarsDownloadSelectionView>())
            .RegisterButton("Copy Guild Wars", "Copy Guild Wars from an existing installation", sp => sp.GetRequiredService<ViewManager>().ShowView<GuildwarsCopySelectionView>())
            .RegisterButton("Event Calendar", "Show current and upcoming events", sp => sp.GetRequiredService<ViewManager>().ShowView<EventCalendarView>())
            .RegisterButton("Guild Wars Party Search", "Show party search broadcasts", sp => sp.GetRequiredService<ViewManager>().ShowView<GuildWarsPartySearchView>());
        menuServiceProducer.CreateIfNotExistCategory("Trade")
            .RegisterButton("Alerts", "Open trade alerts manager", sp => sp.GetRequiredService<ViewManager>().ShowView<TradeAlertsView>())
            .RegisterButton("Kamadan", "Open kamadan trade chat", sp => sp.GetRequiredService<ViewManager>().ShowView<KamadanTradeChatView>())
            .RegisterButton("Ascalon", "Open ascalon trade chat", sp => sp.GetRequiredService<ViewManager>().ShowView<AscalonTradeChatView>())
            .RegisterButton("Trader Quotes", "Open trader quotes view", sp => sp.GetRequiredService<ViewManager>().ShowView<PriceQuotesView>());
        menuServiceProducer.CreateIfNotExistCategory("Mods")
            .RegisterButton("uMod", "Open uMod manager", sp => sp.GetRequiredService<ViewManager>().ShowView<UModOnboardingEntryView>())
            .RegisterButton("GWToolboxpp", "Open GWToolbox manager", sp => sp.GetRequiredService<ViewManager>().ShowView<ToolboxOnboardingEntryView>())
            .RegisterButton("DSOAL", "Open DSOAL manager", sp => sp.GetRequiredService<ViewManager>().ShowView<DSOALOnboardingEntryView>())
            .RegisterButton("ReShade", "Open ReShade manager", sp => sp.GetRequiredService<ViewManager>().ShowView<ReShadeOnboardingEntryView>())
            .RegisterButton("DirectSong", "Open DirectSong manager", sp => sp.GetRequiredService<ViewManager>().ShowView<DirectSongOnboardingEntryView>())
            .RegisterButton("DXVK", "Open DXVK manager", sp => sp.GetRequiredService<ViewManager>().ShowView<DXVKOnboardingEntryView>());
        menuServiceProducer.CreateIfNotExistCategory("Settings")
            .RegisterButton("Accounts", "Accounts Settings", sp => sp.GetRequiredService<ViewManager>().ShowView<AccountsView>())
            .RegisterButton("Executables", "Executables Settings", sp => sp.GetRequiredService<ViewManager>().ShowView<ExecutablesView>())
            .RegisterButton("Launch configurations", "Launch configurations settings", sp => sp.GetRequiredService<ViewManager>().ShowView<LaunchConfigurationsView>());
        menuServiceProducer.CreateIfNotExistCategory("Diagnostics")
            .RegisterButton("Logs", "Open logs view", sp => sp.GetRequiredService<ViewManager>().ShowView<LogsView>())
            .RegisterButton("Metrics", "Open metrics view", sp => sp.GetRequiredService<ViewManager>().ShowView<MetricsView>());
    }

    private IServiceCollection RegisterHttpClients(IServiceCollection services)
    {
        return services
            .ThrowIfNull()
            .RegisterHttpClient<ApplicationUpdater>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<ApplicationUpdater>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<OnlinePictureClient>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<OnlinePictureClient>)
                .WithDefaultRequestHeadersSetup(this.SetupChromeImpersonationUserAgent)
                .Build()
            .RegisterHttpClient<GraphClient>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<GraphClient>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<DownloadService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<DownloadService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TradeChatService<KamadanTradeChatOptions>>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<TradeChatService<KamadanTradeChatOptions>>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TradeChatService<AscalonTradeChatOptions>>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<TradeChatService<AscalonTradeChatOptions>>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<IconCache>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<IconCache>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<TraderQuoteService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<TraderQuoteService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<PriceHistoryService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<PriceHistoryService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<InternetCheckingService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<InternetCheckingService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ChromiumBrowserWrapper>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<ChromiumBrowserWrapper>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ToolboxClient>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<ToolboxClient>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ReShadeService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<ReShadeService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<UModService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<UModService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<UBlockOriginService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<UBlockOriginService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<DXVKService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<DXVKService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<ScopedApiContext>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<ScopedApiContext>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build();
    }
}
