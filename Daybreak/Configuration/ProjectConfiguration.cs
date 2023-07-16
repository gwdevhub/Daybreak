﻿using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.Bloogum;
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
using Daybreak.Services.Graph;
using Microsoft.Extensions.DependencyInjection;
using Daybreak.Services.Navigation;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Menu;
using Daybreak.Services.Scanner;
using Daybreak.Services.Experience;
using Daybreak.Services.Metrics;
using Daybreak.Services.Monitoring;
using System.Net.Http.Headers;
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
using Daybreak.Utils;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Options;
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

namespace Daybreak.Configuration;

public static class ProjectConfiguration
{
    private const string DaybreakUserAgent = "Daybreak";
    private const string ChromeImpersonationUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.79";

    public static void RegisterResolvers(IServiceManager serviceManager)
    {
        serviceManager.ThrowIfNull();

        serviceManager.RegisterOptionsManager<OptionsManager>();
        serviceManager.RegisterResolver(new LoggerResolver());
        serviceManager.RegisterResolver(new ClientWebSocketResolver());
        serviceManager
            .RegisterHttpClient<ApplicationUpdater>()
                .WithMessageHandler(SetupLoggingAndMetrics<ApplicationUpdater>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<BloogumClient>()
                .WithMessageHandler(SetupLoggingAndMetrics<BloogumClient>)
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
        services.AddSingleton<ProcessorUsageMonitor>();
        services.AddSingleton<MemoryUsageMonitor>();
        services.AddSingleton<IViewManager, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<IViewProducer, ViewManager>(sp => sp.GetRequiredService<ViewManager>());
        services.AddSingleton<PostUpdateActionManager>();
        services.AddSingleton<IPostUpdateActionManager>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProducer>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IPostUpdateActionProvider>(sp => sp.GetRequiredService<PostUpdateActionManager>());
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IMenuServiceInitializer, MenuService>(sp => sp.GetRequiredService<IMenuService>().As<MenuService>()!);
        services.AddSingleton<ILiteDatabase, LiteDatabase>(sp => new LiteDatabase("Daybreak.db"));
        services.AddSingleton<IMutexHandler, MutexHandler>();
        services.AddSingleton<IShortcutManager, ShortcutManager>();
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<IStartupActionProducer, StartupActionManager>();
        services.AddSingleton<IOptionsProducer, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().As<OptionsManager>()!);
        services.AddSingleton<IOptionsUpdateHook, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().As<OptionsManager>()!);
        services.AddSingleton<IOptionsProvider, OptionsManager>(sp => sp.GetRequiredService<IOptionsManager>().As<OptionsManager>()!);
        services.AddSingleton<IThemeManager, ThemeManager>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<INotificationProducer, NotificationService>(sp => sp.GetRequiredService<INotificationService>().As<NotificationService>()!);
        services.AddSingleton<INotificationHandlerProducer, NotificationService>(sp => sp.GetRequiredService<INotificationService>().As<NotificationService>()!);
        services.AddSingleton<ILiveChartInitializer, LiveChartInitializer>();
        services.AddSingleton<IImageCache, ImageCache>();
        services.AddSingleton<ISoundService, SoundService>();
        services.AddSingleton<IInternetCheckingService, InternetCheckingService>();
        services.AddSingleton<IConnectivityStatus, ConnectivityStatus>();
        services.AddSingleton<INotificationStorage, NotificationStorage>();
        services.AddSingleton<ITradeAlertingService, TradeAlertingService>();
        services.AddSingleton<IModsManager, ModsManager>();
        services.AddSingleton<IGuildwarsMemoryCache, GuildwarsMemoryCache>();
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
        services.AddScoped<IExperienceCalculator, ExperienceCalculator>();
        services.AddScoped<IAttributePointCalculator, AttributePointCalculator>();
        services.AddScoped<IDownloadService, DownloadService>();
        services.AddScoped<IGuildwarsInstaller, GuildwarsInstaller>();
        services.AddScoped<IGuildwarsEntityDebouncer, GuildwarsEntityDebouncer>();
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<IPathfinder, StupidPathfinder>();
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
    }

    public static void RegisterViews(IViewProducer viewProducer)
    {
        viewProducer.ThrowIfNull();

        viewProducer.RegisterPermanentView<Views.FocusView>();
        viewProducer.RegisterView<LauncherView>();
        viewProducer.RegisterView<AskUpdateView>();
        viewProducer.RegisterView<UpdateView>();
        viewProducer.RegisterView<AccountsView>();
        viewProducer.RegisterView<ExecutablesView>();
        viewProducer.RegisterView<BuildTemplateView>();
        viewProducer.RegisterView<BuildsListView>();
        viewProducer.RegisterView<RequestElevationView>();
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
        viewProducer.RegisterView<UModSwitchView>();
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
        viewProducer.RegisterView<GuildwarsCopySelectionView>();
        viewProducer.RegisterView<GuildwarsCopyView>();
        viewProducer.RegisterView<DSOALInstallingView>();
        viewProducer.RegisterView<DSOALOnboardingEntryView>();
        viewProducer.RegisterView<DSOALSwitchView>();
        viewProducer.RegisterView<DSOALHomepageView>();
        viewProducer.RegisterView<DSOALBrowserView>();
    }

    public static void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.ThrowIfNull();

        startupActionProducer.RegisterAction<RenameInstallerAction>();
        startupActionProducer.RegisterAction<FixSymbolicLinkStartupAction>();
    }

    public static void RegisterPostUpdateActions(IPostUpdateActionProducer postUpdateActionProducer)
    {
        postUpdateActionProducer.ThrowIfNull();
    }

    public static void RegisterDrawingModules(IDrawingModuleProducer drawingModuleProducer)
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

    public static void RegisterOptions(IOptionsProducer optionsProducer)
    {
        optionsProducer.ThrowIfNull();

        optionsProducer.RegisterOptions<LauncherOptions>();
        optionsProducer.RegisterOptions<SoundOptions>();
        optionsProducer.RegisterOptions<ThemeOptions>();

        optionsProducer.RegisterOptions<BrowserOptions>();
        optionsProducer.RegisterOptions<BuildSynchronizationOptions>();
        optionsProducer.RegisterOptions<FocusViewOptions>();

        optionsProducer.RegisterOptions<MemoryReaderOptions>();
        optionsProducer.RegisterOptions<ImageCacheOptions>();

        optionsProducer.RegisterOptions<ToolboxOptions>();
        optionsProducer.RegisterOptions<UModOptions>();
        optionsProducer.RegisterOptions<DSOALOptions>();

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
    }

    public static void RegisterLiteCollections(IServiceCollection services)
    {
        RegisterLiteCollection<Models.Log, LoggingOptions>(services);
        RegisterLiteCollection<TraderQuoteDTO, PriceHistoryOptions>(services);
        RegisterLiteCollection<NotificationDTO, NotificationStorageOptions>(services);
        RegisterLiteCollection<TraderMessageDTO, TraderMessagesOptions>(services);
    }

    public static void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer)
    {
        notificationHandlerProducer.RegisterNotificationHandler<NoActionHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<MessageBoxHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<TradeMessageNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<FixSymbolicLinkNotificationHandler>();
    }

    public static void RegisterMods(IModsManager modsManager)
    {
        modsManager.RegisterMod<IToolboxService, ToolboxService>();
        modsManager.RegisterMod<IUModService, UModService>();
        modsManager.RegisterMod<IDSOALService, DSOALService>();
    }

    private static void RegisterLiteCollection<TCollectionType, TOptionsType>(IServiceCollection services)
        where TOptionsType : class, ILiteCollectionOptions<TCollectionType>
    {
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TOptionsType>>();
            var liteDatabase = sp.GetRequiredService<ILiteDatabase>();
            return liteDatabase.GetCollection<TCollectionType>(options.Value.CollectionName, BsonAutoId.Int64);
        });
    }

    private static void SetupDaybreakUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", DaybreakUserAgent);
    }

    private static void SetupChromeImpersonationUserAgent(HttpRequestHeaders httpRequestHeaders)
    {
        httpRequestHeaders.ThrowIfNull().TryAddWithoutValidation("User-Agent", ChromeImpersonationUserAgent);
    }

    private static HttpMessageHandler SetupLoggingAndMetrics<T>(System.IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();
        var metricsService = serviceProvider.GetRequiredService<IMetricsService>();
        

        return new MetricsHttpMessageHandler<T>(
            metricsService,
            new LoggingHttpMessageHandler(logger!) { InnerHandler = new HttpClientHandler() });
    }
}
