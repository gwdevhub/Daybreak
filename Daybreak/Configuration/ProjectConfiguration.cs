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
using LiteDB;
using Daybreak.Services.Options;
using Daybreak.Models;
using Microsoft.CorrelationVector;
using System.Logging;
using Daybreak.Services.Updater.PostUpdate;
using System.Core.Extensions;
using Daybreak.Services.Graph;
using Microsoft.Extensions.DependencyInjection;
using Daybreak.Services.Navigation;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Menu;
using Daybreak.Services.Scanner;
using Daybreak.Services.Experience;
using Daybreak.Services.Metrics;
using Daybreak.Services.Monitoring;
using Daybreak.Services.Downloads;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Pathfinding;
using Daybreak.Services.Startup;
using Daybreak.Services.Startup.Actions;
using Daybreak.Services.Drawing;
using Daybreak.Services.Drawing.Modules.Entities;
using Daybreak.Services.Drawing.Modules.MapIcons;
using Daybreak.Services.Drawing.Modules;
using Daybreak.Services.Guildwars;
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
using Daybreak.Services.TradeChat.Models;
using Daybreak.Services.Charts;
using Daybreak.Services.Images;
using Daybreak.Services.Drawing.Modules.Bosses;
using Daybreak.Services.InternetChecker;
using System;
using Daybreak.Services.Sounds;
using Daybreak.Services.Notifications.Models;
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
using Daybreak.Models.Plugins;
using Daybreak.Services.Plugins;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Services.Injection;
using Daybreak.Services.ReShade;
using Daybreak.Views.Onboarding.ReShade;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Views.Launch;
using Daybreak.Services.GWCA;
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

namespace Daybreak.Configuration;

public class ProjectConfiguration : PluginConfigurationBase
{
    public override void RegisterResolvers(IServiceManager serviceManager)
    {
        serviceManager.ThrowIfNull();

        serviceManager.RegisterOptionsManager<OptionsManager>();
        serviceManager.RegisterResolver(new LoggerResolver());
        serviceManager.RegisterResolver(new ClientWebSocketResolver());
        serviceManager
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
            .RegisterHttpClient<GWCAClient>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<GWCAClient>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Build()
            .RegisterHttpClient<UModService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<UModService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<UBlockOriginService>()
                .WithMessageHandler(this.SetupLoggingAndMetrics<UBlockOriginService>)
                .WithDefaultRequestHeadersSetup(this.SetupDaybreakUserAgent)
                .Build();
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services.ThrowIfNull();

        this.RegisterLiteCollections(services);

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
        services.AddSingleton<ILiteDatabase, LiteDatabase>(sp => new LiteDatabase("Daybreak.db"));
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
        services.AddSingleton<IGuildwarsMemoryCache, GuildwarsMemoryCache>();
        services.AddSingleton<IPluginsService, PluginsService>();
        services.AddSingleton<ISplashScreenService, SplashScreenService>();
        services.AddSingleton<IGuildWarsExecutableManager, GuildWarsExecutableManager>();
        services.AddSingleton<IGWCAClient, GWCAClient>();
        services.AddSingleton<IGuildwarsMemoryReader, GWCAMemoryReader>();
        services.AddSingleton<ISevenZipExtractor, SevenZipExtractor>();
        services.AddSingleton<IGraphClient, GraphClient>();
        services.AddSingleton<IOptionsSynchronizationService, OptionsSynchronizationService>();
        services.AddSingleton<IWindowEventsHook<MainWindow>, WindowEventsHook<MainWindow>>();
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
        services.AddScoped<IPathfinder, SharpNavPathfinder>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IExperienceCalculator, ExperienceCalculator>();
        services.AddScoped<IAttributePointCalculator, AttributePointCalculator>();
        services.AddScoped<IDownloadService, DownloadService>();
        services.AddScoped<IGuildwarsInstaller, GuildwarsInstaller>();
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<IDrawingService, DrawingService>();
        services.AddScoped<IDrawingModuleProducer, DrawingService>(sp => sp.GetRequiredService<IDrawingService>().As<DrawingService>()!);
        services.AddScoped<ITradeChatService<KamadanTradeChatOptions>, TradeChatService<KamadanTradeChatOptions>>();
        services.AddScoped<ITradeChatService<AscalonTradeChatOptions>, TradeChatService<AscalonTradeChatOptions>>();
        services.AddScoped<ITraderQuoteService, TraderQuoteService>();
        services.AddScoped<IPriceHistoryDatabase, PriceHistoryDatabase>();
        services.AddScoped<IPriceHistoryService, PriceHistoryService>();
        services.AddScoped<IWordHighlightingService, WordHighlightingService>();
        services.AddScoped<ITradeHistoryDatabase, TradeHistoryDatabase>();
        services.AddScoped<IGuildwarsCopyService, GuildwarsCopyService>();
        services.AddScoped<IItemHashService, ItemHashService>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<IEventNotifierService, EventNotifierService>();
        services.AddScoped<IBackgroundProvider, BackgroundProvider>();
        services.AddScoped<IToolboxClient, ToolboxClient>();
        services.AddScoped<IProcessInjector, ProcessInjector>();
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
        viewProducer.RegisterView<GuildwarsDownloadView>();
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
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.ThrowIfNull();

        startupActionProducer.RegisterAction<RenameInstallerAction>();
        startupActionProducer.RegisterAction<FixSymbolicLinkStartupAction>();
        startupActionProducer.RegisterAction<UpdateUModAction>();
        startupActionProducer.RegisterAction<FixPriceHistoryEntries>();
        startupActionProducer.RegisterAction<CredentialsOptionsMigrator>();
        startupActionProducer.RegisterAction<BrowserHistorySizeEnforcer>();
    }

    public override void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer)
    {
        postUpdateActionProducer.ThrowIfNull();
    }

    public override void RegisterDrawingModules(IDrawingModuleProducer drawingModuleProducer)
    {
        drawingModuleProducer.ThrowIfNull();

        drawingModuleProducer.RegisterDrawingModule<DeadEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<NeutralEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<AlliedCreatureEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<AllyEntityDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<NeutralBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<WarriorBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<RangerBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<MesmerBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<NecromancerBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<MonkBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<ElementalistBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<AssassinBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<RitualistBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<DervishBossEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<ParagonBossEntityDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<EnemyEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<NpcEntityDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<UnknownEntityDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<ResurrectionShrineDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<CollectorDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<GateDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<StairsDownDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<StairsUpDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<CircledStarDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<AreaMapDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<DungeonBossDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<DungeonKeyDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<FlagDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<PersonDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<PathfindingDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<PlayerPositionHistoryDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<QuestObjectiveDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<MainPlayerDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<PartyMemberDrawingModule>();
        drawingModuleProducer.RegisterDrawingModule<WorldPlayerDrawingModule>();

        drawingModuleProducer.RegisterDrawingModule<EngagementAreaDrawingModule>();
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

        optionsProducer.RegisterOptions<MemoryReaderOptions>();
        optionsProducer.RegisterOptions<ImageCacheOptions>();

        optionsProducer.RegisterOptions<BackgroundProviderOptions>();

        optionsProducer.RegisterOptions<ToolboxOptions>();
        optionsProducer.RegisterOptions<UModOptions>();
        optionsProducer.RegisterOptions<DSOALOptions>();
        optionsProducer.RegisterOptions<ReShadeOptions>();

        optionsProducer.RegisterOptions<ScreenManagerOptions>();
        optionsProducer.RegisterOptions<KamadanTradeChatOptions>();
        optionsProducer.RegisterOptions<AscalonTradeChatOptions>();
        optionsProducer.RegisterOptions<LoggingOptions>();
        optionsProducer.RegisterOptions<PriceHistoryOptions>();
        optionsProducer.RegisterOptions<TraderQuotesOptions>();
        optionsProducer.RegisterOptions<PathfindingOptions>();
        optionsProducer.RegisterOptions<NotificationStorageOptions>();
        optionsProducer.RegisterOptions<TradeAlertingOptions>();
        optionsProducer.RegisterOptions<TraderMessagesOptions>();
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
    }

    public override void RegisterMods(IModsManager modsManager)
    {
        modsManager.RegisterMod<IReShadeService, ReShadeService>();
        modsManager.RegisterMod<IUModService, UModService>();
        modsManager.RegisterMod<IToolboxService, ToolboxService>();
        modsManager.RegisterMod<IDSOALService, DSOALService>();
        modsManager.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
        modsManager.RegisterMod<IGWCAInjector, GWCAInjector>();
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

    private void RegisterLiteCollections(IServiceCollection services)
    {
        this.RegisterLiteCollection<Models.Log, LoggingOptions>(services);
        this.RegisterLiteCollection<TraderQuoteDTO, PriceHistoryOptions>(services);
        this.RegisterLiteCollection<NotificationDTO, NotificationStorageOptions>(services);
        this.RegisterLiteCollection<TraderMessageDTO, TraderMessagesOptions>(services);
    }
}
