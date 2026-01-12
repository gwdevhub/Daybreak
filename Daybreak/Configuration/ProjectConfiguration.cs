using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using Daybreak.Configuration.Options;
using Daybreak.Extensions;
using Daybreak.Services.Api;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.ApplicationArguments.ArgumentHandling;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Credentials;
using Daybreak.Services.DirectSong;
using Daybreak.Services.Downloads;
using Daybreak.Services.Events;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.ExecutableManagement;
using Daybreak.Services.Experience;
using Daybreak.Services.Graph;
using Daybreak.Services.Graph.Models;
using Daybreak.Services.Guildwars;
using Daybreak.Services.GuildWars;
using Daybreak.Services.Injection;
using Daybreak.Services.InternetChecker;
using Daybreak.Services.Keyboard;
using Daybreak.Services.LaunchConfigurations;
using Daybreak.Services.Logging;
using Daybreak.Services.MDns;
using Daybreak.Services.Metrics;
using Daybreak.Services.Mods;
using Daybreak.Services.Monitoring;
using Daybreak.Services.Notifications;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Services.Onboarding;
using Daybreak.Services.Options;
using Daybreak.Services.Plugins;
using Daybreak.Services.Privilege;
using Daybreak.Services.Registry;
using Daybreak.Services.ReShade;
using Daybreak.Services.Screens;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Shortcuts;
using Daybreak.Services.Startup;
using Daybreak.Services.Startup.Actions;
using Daybreak.Services.Telemetry;
using Daybreak.Services.Themes;
using Daybreak.Services.Toolbox;
using Daybreak.Services.Toolbox.Notifications;
using Daybreak.Services.Toolbox.Utilities;
using Daybreak.Services.TradeChat;
using Daybreak.Services.TradeChat.Models;
using Daybreak.Services.TradeChat.Notifications;
using Daybreak.Services.UMod;
using Daybreak.Services.Updater;
using Daybreak.Services.Wiki;
using Daybreak.Shared.Models.Plugins;
using Daybreak.Shared.Services.Api;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.ApplicationLauncher;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.DirectSong;
using Daybreak.Shared.Services.Downloads;
using Daybreak.Shared.Services.Events;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Experience;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Initialization;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Services.InternetChecker;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.MDns;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Metrics;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Onboarding;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Registry;
using Daybreak.Shared.Services.ReShade;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Screenshots;
using Daybreak.Shared.Services.SevenZip;
using Daybreak.Shared.Services.Shortcuts;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Toolbox;
using Daybreak.Shared.Services.TradeChat;
using Daybreak.Shared.Services.UMod;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Services.Wiki;
using Daybreak.Themes;
using Daybreak.Views;
using Daybreak.Views.Copy;
using Daybreak.Views.Installation;
using Daybreak.Views.Mods;
using Daybreak.Views.Trade;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using OpenTelemetry.Resources;
using TrailBlazr.Extensions;
using TrailBlazr.Services;
using IMenuService = Daybreak.Shared.Services.Menu.IMenuService;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using MenuService = Daybreak.Services.Menu.MenuService;

namespace Daybreak.Configuration;

public class ProjectConfiguration : PluginConfigurationBase
{
    public static readonly Version CurrentVersion = Version.Parse(Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? throw new InvalidOperationException("Unable to get current version"));

    public override void RegisterServices(IServiceCollection services)
    {
        services.ThrowIfNull();

        // Add FluentUI components
        services.AddFluentUIComponents();

        services.AddTrailBlazr();
        RegisterHttpClients(services);
        services.RegisterClientWebSocket<TradeChatService<KamadanTradeChatOptions>>()
            .Build();
        services.RegisterClientWebSocket<TradeChatService<AscalonTradeChatOptions>>()
            .Build();
        services.AddSingleton<IPublicClientApplication>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<PublicClientApplication>>();
            return PublicClientApplicationBuilder.Create(SecretManager.GetSecret(SecretKeys.AadApplicationId))
                .WithLogging((logLevel, message, containsPii) =>
                {
                    if (containsPii && logLevel > Microsoft.Identity.Client.LogLevel.Info)
                    {
                        // Redact logs that contain PII which user can enable to send to the telemetry server
                        message = "[REDACTED]";
                    }

                    var equivalentLogLevel = logLevel switch
                    {
                        Microsoft.Identity.Client.LogLevel.Error => LogLevel.Error,
                        Microsoft.Identity.Client.LogLevel.Warning => LogLevel.Warning,
                        Microsoft.Identity.Client.LogLevel.Info => LogLevel.Information,
                        Microsoft.Identity.Client.LogLevel.Verbose => LogLevel.Debug,
                        _ => LogLevel.None
                    };

                    logger.Log(equivalentLogLevel, message);
                }, enablePiiLogging: true, enableDefaultPlatformLogging: true)
                .WithCacheOptions(new CacheOptions { UseSharedCache = true })
                .WithRedirectUri(BlazorGraphClient.RedirectUri)
                .WithWindowsEmbeddedBrowserSupport()
                .WithHttpClientFactory(
                    new DaybreakMsalHttpClientProvider(new HttpClient(SetupLoggingAndMetrics<PublicClientApplication>(sp))))
                .Build();
        });

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
        services.AddSingleton<IMenuService, MenuService>();
        services.AddSingleton<IMenuServiceInitializer, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMenuServiceButtonHandler, MenuService>(sp => sp.GetRequiredService<IMenuService>().Cast<MenuService>());
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<INotificationProducer, NotificationService>(sp => sp.GetRequiredService<INotificationService>().Cast<NotificationService>());
        services.AddSingleton<IInternetCheckingService, InternetCheckingService>();
        services.AddSingleton<INotificationStorage, InMemoryNotificationStorage>();
        services.AddSingleton<IModsManager, ModsManager>();
        services.AddSingleton<IPluginsService, PluginsService>();
        services.AddSingleton<ISevenZipExtractor, Daybreak.Services.SevenZip.SevenZipExtractor>();
        services.AddSingleton<IMDomainNameService, MDomainNameService>();
        services.AddSingleton<IAttachedApiAccessor, AttachedApiAccessor>();
        services.AddSingleton<PrivilegeContext>();
        services.AddSingleton<ViewRedirectContext>();
        services.AddSingleton<JSConsoleInterop>();
        services.AddSingleton<OptionsManager>();
        services.AddSingleton<IOptionsProvider, OptionsManager>(sp => sp.GetRequiredService<OptionsManager>());
        
        services.AddScoped<ICredentialManager, CredentialManager>();
        services.AddScoped<IApplicationLauncher, ApplicationLauncher>();
        services.AddScoped<IBuildTemplateManager, BuildTemplateManager>();
        services.AddScoped<IOnboardingService, OnboardingService>();
        services.AddScoped<IExperienceCalculator, ExperienceCalculator>();
        services.AddScoped<IAttributePointCalculator, AttributePointCalculator>();
        services.AddScoped<IDownloadService, DownloadService>();
        services.AddScoped<IGuildWarsInstaller, IntegratedGuildwarsInstaller>();
        services.AddScoped<IExceptionHandler, ExceptionHandler>();
        services.AddScoped<ITradeChatService<KamadanTradeChatOptions>, TradeChatService<KamadanTradeChatOptions>>();
        services.AddScoped<ITradeChatService<AscalonTradeChatOptions>, TradeChatService<AscalonTradeChatOptions>>();
        services.AddScoped<ITraderQuoteService, TraderQuoteService>();
        services.AddScoped<ITradeHistoryDatabase, TradeHistoryDatabase>();
        services.AddScoped<IGuildWarsCopyService, GuildWarsCopyService>();
        services.AddScoped<IItemHashService, ItemHashService>();
        services.AddScoped<IRegistryService, RegistryService>();
        services.AddScoped<IToolboxClient, ToolboxClient>();
        services.AddScoped<IDaybreakInjector, DaybreakInjector>();
        services.AddScoped<IProcessInjector, ProcessInjector>();
        services.AddScoped<IStubInjector, StubInjector>();
        services.AddScoped<ILaunchConfigurationService, LaunchConfigurationService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IApplicationArgumentService, ApplicationArgumentService>();
        services.AddScoped<IWikiService, WikiService>();
        services.AddScoped<IPrivilegeManager, PrivilegeManager>();
        services.AddScoped<IGraphClient, BlazorGraphClient>();
        services.AddScoped<IScreenshotService, ScreenshotService>();

        services.AddHostedSingleton<IApplicationUpdater, ApplicationUpdater>();
        services.AddHostedSingleton<IThemeManager, BlazorThemeInteropService>();
        services.AddHostedSingleton<IKeyboardHookService, KeyboardHookService>();
        services.AddHostedSingleton<IScreenManager, ScreenManager>();
        services.AddHostedSingleton<IConnectivityStatus, ConnectivityStatus>();
        services.AddHostedSingleton<ITradeAlertingService, TradeAlertingService>();
        services.AddHostedSingleton<IGuildWarsExecutableManager, GuildWarsExecutableManager>();
        services.AddHostedSingleton<IEventNotifierService, EventNotifierService>();
        services.AddHostedSingleton<IShortcutManager, ShortcutManager>();
        services.AddHostedSingleton<IMDomainRegistrar, MDomainRegistrar>();
        services.AddHostedSingleton<IOptionsSynchronizationService, OptionsSynchronizationService>();
        services.AddHostedSingleton<GameScreenshotsTheme>();
        services.AddHostedService<StartupActionManager>();
        services.AddHostedService<ProcessorUsageMonitor>();
        services.AddHostedService<MemoryUsageMonitor>();
        services.AddHostedService<TelemetryHost>();
        services.AddHostedService<DiskUsageMonitor>();
    }

    public override void RegisterViews(IViewProducer viewProducer)
    {
        viewProducer.ThrowIfNull();

        viewProducer.RegisterView<LaunchView, LaunchViewModel>(isSingleton: true);
        viewProducer.RegisterView<OptionView, OptionViewModel>();
        viewProducer.RegisterView<FocusView, FocusViewModel>();
        viewProducer.RegisterView<BuildListView, BuildListViewModel>();
        viewProducer.RegisterView<BuildRoutingView, BuildRoutingViewModel>();
        viewProducer.RegisterView<SingleBuildTemplateView, SingleBuildTemplateViewModel>();
        viewProducer.RegisterView<TeamBuildTemplateView, TeamBuildTemplateViewModel>();
        viewProducer.RegisterView<AccountsView, AccountsViewModel>();
        viewProducer.RegisterView<ExecutablesView, ExecutablesViewModel>();
        viewProducer.RegisterView<LaunchConfigurationsView, LaunchConfigurationsViewModel>();
        viewProducer.RegisterView<RequestElevationView, RequestElevationViewModel>();
        viewProducer.RegisterView<RequestDelevationView, RequestDelevationViewModel>();
        viewProducer.RegisterView<SettingsSynchronizationView, SettingsSynchronizationViewModel>();
        viewProducer.RegisterView<TelemetryView, TelemetryViewModel>();
        viewProducer.RegisterView<VersionManagementView, VersionManagementViewModel>();
        viewProducer.RegisterView<UpdateConfirmationView, UpdateConfirmationViewModel>();
        viewProducer.RegisterView<UpdateView, UpdateViewModel>();
        viewProducer.RegisterView<PluginsView, PluginsViewModel>();
        viewProducer.RegisterView<LogsView, LogsViewModel>();
        viewProducer.RegisterView<MetricsView, MetricsViewModel>();
        viewProducer.RegisterView<GuildWarsPartySearchView, GuildWarsPartySearchViewModel>();
        viewProducer.RegisterView<EventCalendarView, EventCalendarViewModel>();
        viewProducer.RegisterView<TradeChatView, TradeChatViewModel>();
        viewProducer.RegisterView<GuildWarsDownloadView, GuildWarsDownloadViewModel>();
        viewProducer.RegisterView<GuildWarsCopySelectionView, GuildWarsCopySelectionViewModel>();
        viewProducer.RegisterView<GuildWarsCopyView, GuildWarsCopyViewModel>();
        viewProducer.RegisterView<TradeAlertsView, TradeAlertsViewModel>();
        viewProducer.RegisterView<TradeMessageView, TradeMessageViewModel>();
        viewProducer.RegisterView<TradeQuoteView, TradeQuoteViewModel>();
        viewProducer.RegisterView<LauncherOnboardingView, LauncherOnboardingViewModel>();
        viewProducer.RegisterView<ModsView, ModsViewModel>();
        viewProducer.RegisterView<ModInstallationView, ModInstallationViewModel>();
        viewProducer.RegisterView<ModInstallationConfirmationView, ModInstallationConfirmationViewModel>();
        viewProducer.RegisterView<ScreenSelectorView, ScreenSelectorViewModel>();
        viewProducer.RegisterView<UModManagementView, UModManagementViewModel>();
        viewProducer.RegisterView<TradeNotificationView, TradeNotificationViewModel>();
        viewProducer.RegisterView<WikiView, WikiViewModel>();
        viewProducer.RegisterView<ReShadeManagementView, ReShadeManagementViewModel>();
        viewProducer.RegisterView<GuildWarsMarketView, GuildWarsMarketViewModel>();
    }

    public override void RegisterStartupActions(IStartupActionProducer startupActionProducer)
    {
        startupActionProducer.ThrowIfNull();

        startupActionProducer.RegisterAction<RenameInstallerAction>();
        startupActionProducer.RegisterAction<UpdateUModAction>();
        startupActionProducer.RegisterAction<CredentialsOptionsMigrator>();
        startupActionProducer.RegisterAction<UpdateToolboxAction>();
        startupActionProducer.RegisterAction<UpdateGuildWarsExecutable>();
        startupActionProducer.RegisterAction<UpdateReShadeAction>();
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
        optionsProducer.RegisterOptions<ThemeOptions>();

        optionsProducer.RegisterOptions<SynchronizationOptions>();
        optionsProducer.RegisterOptions<FocusViewOptions>();

        optionsProducer.RegisterOptions<ToolboxOptions>();
        optionsProducer.RegisterOptions<UModOptions>();
        optionsProducer.RegisterOptions<DSOALOptions>();
        optionsProducer.RegisterOptions<ReShadeOptions>();
        optionsProducer.RegisterOptions<DXVKOptions>();

        optionsProducer.RegisterOptions<ScreenManagerOptions>();
        optionsProducer.RegisterOptions<KamadanTradeChatOptions>();
        optionsProducer.RegisterOptions<AscalonTradeChatOptions>();
        optionsProducer.RegisterOptions<TraderQuotesOptions>();
        optionsProducer.RegisterOptions<TradeAlertingOptions>();
        optionsProducer.RegisterOptions<EventNotifierOptions>();
        optionsProducer.RegisterOptions<PluginsServiceOptions>();
        optionsProducer.RegisterOptions<GuildwarsExecutableOptions>();
        optionsProducer.RegisterOptions<CredentialManagerOptions>();
        optionsProducer.RegisterOptions<LaunchConfigurationServiceOptions>();
        optionsProducer.RegisterOptions<DirectSongOptions>();
        optionsProducer.RegisterOptions<GuildWarsVersionCheckerOptions>();

        optionsProducer.RegisterOptions<GuildWarsScreenPlacerOptions>();
    }

    public override void RegisterNotificationHandlers(INotificationHandlerProducer notificationHandlerProducer)
    {
        notificationHandlerProducer.RegisterNotificationHandler<NoActionHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<MessageBoxHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<TradeMessageNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<UpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<NavigateToCalendarViewHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<GuildWarsUpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<GuildWarsBatchUpdateNotificationHandler>();
        notificationHandlerProducer.RegisterNotificationHandler<ToolboxUpdateHandler>();
    }

    public override void RegisterMods(IModsProducer modsManager)
    {
        modsManager.RegisterMod<IReShadeService, ReShadeService>();
        modsManager.RegisterMod<IGuildWarsVersionChecker, GuildWarsVersionChecker>();
        modsManager.RegisterMod<IDaybreakApiService, DaybreakApiService>();
        modsManager.RegisterMod<IToolboxService, ToolboxService>();
        modsManager.RegisterMod<IUModService, UModService>();
        modsManager.RegisterMod<IGuildwarsScreenPlacer, GuildwarsScreenPlacer>();
        modsManager.RegisterMod<IDirectSongService, DirectSongService>(singleton: true);
    }

    public override void RegisterLaunchArgumentHandlers(IArgumentHandlerProducer argumentHandlerProducer)
    {
        argumentHandlerProducer.ThrowIfNull();
        argumentHandlerProducer.RegisterArgumentHandler<AutoLaunchArgumentHandler>();
    }

    public override void RegisterMenuButtons(IMenuServiceProducer menuServiceProducer)
    {
        menuServiceProducer.ThrowIfNull();
        menuServiceProducer.CreateIfNotExistCategory("Guild Wars")
            .RegisterButton("Game Companion", "Open game companion", sp => sp.GetRequiredService<IViewManager>().ShowView<LaunchView>())
            .RegisterButton("Manage Builds", "Open builds manager", sp => sp.GetRequiredService<IViewManager>().ShowView<BuildListView>())
            .RegisterButton("Manage Mods", "Open Guild Wars mods manager", sp => sp.GetRequiredService<IViewManager>().ShowView<ModsView>())
            .RegisterButton("Download Guild Wars", "Download Guild Wars installer", sp => sp.GetRequiredService<IViewManager>().ShowView<GuildWarsDownloadView>())
            .RegisterButton("Copy Guild Wars", "Copy Guild Wars from an existing installation", sp => sp.GetRequiredService<IViewManager>().ShowView<GuildWarsCopySelectionView>())
            .RegisterButton("Event Calendar", "Show current and upcoming events", sp => sp.GetRequiredService<IViewManager>().ShowView<EventCalendarView>())
            .RegisterButton("Guild Wars Party Search", "Show party search broadcasts", sp => sp.GetRequiredService<IViewManager>().ShowView<GuildWarsPartySearchView>())
            .RegisterButton("Guild Wars Marketplace", "Show guild wars marketplace", sp => sp.GetRequiredService<IViewManager>().ShowView<GuildWarsMarketView>());
        menuServiceProducer.CreateIfNotExistCategory("Daybreak")
            //TODO: Implement Notifications view
            //.RegisterButton("Notifications", "Open notifications view", sp => { })
            .RegisterButton("Manage Plugins", "Open plugins view", sp => sp.GetRequiredService<IViewManager>().ShowView<PluginsView>())
            .RegisterButton("Manage version", "Open version manager", sp => sp.GetRequiredService<IViewManager>().ShowView<VersionManagementView>())
            .RegisterButton("Help", "Open Daybreak wiki", sp => sp.GetRequiredService<IViewManager>().ShowView<WikiView>((nameof(WikiView.Page), "Home")));
        menuServiceProducer.CreateIfNotExistCategory("Trade")
            .RegisterButton("Alerts", "Open trade alerts manager", sp => sp.GetRequiredService<IViewManager>().ShowView<TradeAlertsView>())
            .RegisterButton("Kamadan", "Open kamadan trade chat", sp => sp.GetRequiredService<IViewManager>().ShowView<TradeChatView>((nameof(TradeChatView.Source), nameof(TraderSource.Kamadan))))
            .RegisterButton("Ascalon", "Open ascalon trade chat", sp => sp.GetRequiredService<IViewManager>().ShowView<TradeChatView>((nameof(TradeChatView.Source), nameof(TraderSource.Ascalon))));
        menuServiceProducer.CreateIfNotExistCategory("Settings")
            .RegisterButton("Accounts", "Accounts Settings", sp => sp.GetRequiredService<IViewManager>().ShowView<AccountsView>())
            .RegisterButton("Executables", "Executables Settings", sp => sp.GetRequiredService<IViewManager>().ShowView<ExecutablesView>())
            .RegisterButton("Launch configurations", "Launch configurations settings", sp => sp.GetRequiredService<IViewManager>().ShowView<LaunchConfigurationsView>());
        menuServiceProducer.CreateIfNotExistCategory("Diagnostics")
            .RegisterButton("Telemetry", "Open telemetry view", sp => sp.GetRequiredService<IViewManager>().ShowView<TelemetryView>())
            .RegisterButton("Logs", "Open logs view", sp => sp.GetRequiredService<IViewManager>().ShowView<LogsView>())
            .RegisterButton("Metrics", "Open metrics view", sp => sp.GetRequiredService<IViewManager>().ShowView<MetricsView>());
    }

    public override void RegisterThemes(IThemeProducer themeProducer)
    {
        themeProducer.ThrowIfNull();
        var themes = CoreThemes.Themes
            .Concat(HeroThemes.Themes)
            .Concat(LiveThemes.Themes);
        foreach (var theme in themes)
        {
            themeProducer.RegisterTheme(theme);
        }
    }

    private static IServiceCollection RegisterHttpClients(IServiceCollection services)
    {
        return services
            .ThrowIfNull()
            .RegisterHttpClient<ApplicationUpdater>()
                .WithMessageHandler(SetupLoggingAndMetrics<ApplicationUpdater>)
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
            .RegisterHttpClient<TraderQuoteService>()
                .WithMessageHandler(SetupLoggingAndMetrics<TraderQuoteService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<InternetCheckingService>()
                .WithMessageHandler(SetupLoggingAndMetrics<InternetCheckingService>)
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
            .RegisterHttpClient<ScopedApiContext>()
                .WithMessageHandler(SetupLoggingAndMetrics<ScopedApiContext>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<WikiService>()
                .WithMessageHandler(SetupLoggingAndMetrics<WikiService>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build()
            .RegisterHttpClient<BlazorGraphClient>()
                .WithMessageHandler(SetupLoggingAndMetrics<BlazorGraphClient>)
                .WithDefaultRequestHeadersSetup(SetupDaybreakUserAgent)
                .Build();
    }
}
