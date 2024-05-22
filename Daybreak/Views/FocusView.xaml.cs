using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Models;
using Daybreak.Models.Builds;
using Daybreak.Models.FocusView;
using Daybreak.Models.Guildwars;
using Daybreak.Models.LaunchConfigurations;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Experience;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Scanner;
using Daybreak.Services.Screens;
using Daybreak.Services.Window;
using Daybreak.Views.Trade;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;
using Position = Daybreak.Models.Guildwars.Position;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for FocusView.xaml
/// </summary>
public partial class FocusView : UserControl
{
    private const string NamePlaceholder = "[NamePlaceholder]";
    private const string WikiUrl = "https://wiki.guildwars.com/wiki/[NamePlaceholder]";
    private const int MaxRetries = 5;

    private static readonly TimeSpan UninitializedBackoff = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan GameDataFrequency = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan PathingDataFrequency = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MainPlayerDataFrequency = TimeSpan.FromMilliseconds(16);
    private static readonly TimeSpan GameStateFrequency = TimeSpan.FromMilliseconds(16);
    private static readonly TimeSpan InventoryDataFrequency = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan CartoDataFrequency = TimeSpan.FromSeconds(1);

    private readonly IWindowEventsHook<MainWindow> mainWindowEventsHook;
    private readonly INotificationService notificationService;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly IScreenManager screenManager;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions;
    private readonly ILiveUpdateableOptions<MinimapWindowOptions> minimapWindowOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private InventoryData inventoryData = new();

    [GenerateDependencyProperty]
    private GameState gameState = new();

    [GenerateDependencyProperty]
    private GameData gameData = new();

    [GenerateDependencyProperty]
    private bool mainPlayerDataValid;

    [GenerateDependencyProperty]
    private PathingData pathingData = new();

    [GenerateDependencyProperty]
    private MainPlayerResourceContext mainPlayerResourceContext = new();

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private bool loadingPathingData;

    [GenerateDependencyProperty]
    private bool faultyPathingData;

    [GenerateDependencyProperty]
    private bool inventoryVisible;

    [GenerateDependencyProperty]
    private bool minimapVisible;

    [GenerateDependencyProperty]
    private bool canRotateMinimap;

    [GenerateDependencyProperty]
    private bool minimapExtracted;

    [GenerateDependencyProperty]
    private bool showCartoProgress;

    [GenerateDependencyProperty]
    private CartoProgressContext cartoTitle = default!;

    [GenerateDependencyProperty]
    private bool pauseDataFetching;

    private IWindowEventsHook<MinimapWindow>? minimapWindowEventsHook;
    private MinimapWindow? minimapWindow;
    private bool browserMaximized = false;
    private bool minimapMaximized = false;
    private bool inventoryMaximized = false;
    private CancellationTokenSource? cancellationTokenSource;

    public FocusView(
        IWindowEventsHook<MainWindow> mainWindowEventsHook,
        INotificationService notificationService,
        IBuildTemplateManager buildTemplateManager,
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        IScreenManager screenManager,
        ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions,
        ILiveUpdateableOptions<MinimapWindowOptions> minimapWindowOptions,
        ILogger<FocusView> logger)
    {
        this.mainWindowEventsHook = mainWindowEventsHook.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.screenManager = screenManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.minimapWindowOptions = minimapWindowOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == BrowserAddressProperty &&
            this.Browser.BrowserEnabled)
        {
            this.liveUpdateableOptions.Value.BrowserHistory = this.Browser.BrowserHistoryManager.BrowserHistory;
            this.liveUpdateableOptions.UpdateOption();
        }
        else if (e.Property == DataContextProperty &&
                this.DataContext is GuildWarsApplicationLaunchContext context)
        {
            this.Minimap.LaunchContext = context;
        }

        base.OnPropertyChanged(e);
    }

    private async Task UpdatePathingData()
    {
        if (this.DataContext is not GuildWarsApplicationLaunchContext context)
        {
            this.logger.LogInformation($"Data context is not set to {nameof(GuildWarsApplicationLaunchContext)}. Can not retrieve executable");
            return;
        }

        if (context.GuildWarsProcess?.HasExited is not false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
            this.cancellationTokenSource?.Cancel();
            return;
        }

        var pathingMeta = await this.guildwarsMemoryCache.ReadPathingMetaData(this.cancellationTokenSource?.Token ?? CancellationToken.None) ?? throw new HttpRequestException();
        if (pathingMeta?.TrapezoidCount == this.PathingData?.Trapezoids?.Count ||
            this.cancellationTokenSource?.IsCancellationRequested is not false)
        {
            await this.Dispatcher.InvokeAsync(() => this.LoadingPathingData = false);
            return;
        }

        await this.Dispatcher.InvokeAsync(() => this.LoadingPathingData = true);
        var maybePathingData = await this.guildwarsMemoryCache.ReadPathingData(this.cancellationTokenSource?.Token ?? CancellationToken.None) ?? throw new HttpRequestException();
        if (maybePathingData is not PathingData pathingData ||
            pathingData.Trapezoids is null ||
            pathingData.Trapezoids.Count == 0)
        {
            return;
        }

        await this.Dispatcher.InvokeAsync(() =>
        {
            this.PathingData = pathingData;
            this.LoadingPathingData = false;
        });
    }

    private async void PeriodicallyReadPathingData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadPathingData), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(PathingDataFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(PathingDataFrequency, cancellationToken);
                    continue;
                }

                if (!this.MinimapVisible &&
                    this.minimapWindow is null)
                {
                    await Task.Delay(PathingDataFrequency, cancellationToken);
                    continue;
                }

                await Task.WhenAll(
                    this.UpdatePathingData(),
                    Task.Delay(PathingDataFrequency, cancellationToken)).ConfigureAwait(true);
                retries = 0;

            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic reading");
                return;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadGameState(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadGameState), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(GameStateFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(GameStateFrequency, cancellationToken);
                    continue;
                }

                if (!this.MinimapVisible &&
                    this.minimapWindow is null)
                {
                    await Task.Delay(GameStateFrequency, cancellationToken);
                    continue;
                }

                var readGameStateTask = this.guildwarsMemoryCache.ReadGameState(cancellationToken);
                await Task.WhenAll(
                    readGameStateTask,
                    Task.Delay(GameStateFrequency, cancellationToken)).ConfigureAwait(true);

                var maybeGameState = await readGameStateTask ?? throw new HttpRequestException();
                this.GameState = maybeGameState;
                this.CanRotateMinimap = this.liveUpdateableOptions.Value.MinimapRotationEnabled;
                retries = 0;
            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic reading");
                return;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadCartoData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadCartoData), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(CartoDataFrequency, cancellationToken);
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(CartoDataFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(CartoDataFrequency, cancellationToken);
                    continue;
                }

                if (!this.MinimapVisible &&
                    this.minimapWindow is null)
                {
                    await Task.Delay(CartoDataFrequency, cancellationToken);
                    continue;
                }

                var maybeMap = this.MainPlayerResourceContext?.Session?.Session?.CurrentMap;
                if (this.liveUpdateableOptions.Value.ShowCartoProgress &&
                    maybeMap is not null)
                {
                    var maybeCurrentContinent = Continent.Continents.FirstOrDefault(c => c.Regions?.Any(r => r.Maps?.Contains(maybeMap) is true) is true);
                    if (maybeCurrentContinent == Continent.Tyria)
                    {
                        var titleInfo = (await this.guildwarsMemoryReader.GetTitleInformation(Title.TyrianCartographer.Id, cancellationToken))!;
                        this.CartoTitle = titleInfo is null ?
                            default :
                            new CartoProgressContext
                            {
                                Continent = Continent.Tyria,
                                ResolvedName = titleInfo.TitleName,
                                Percentage = titleInfo.CurrentPoints / 10d,
                                TitleInformationExtended = titleInfo,
                                TitleName = Title.TyrianCartographer.Name
                            };
                    }
                    else if (maybeCurrentContinent == Continent.Cantha)
                    {
                        var titleInfo = (await this.guildwarsMemoryReader.GetTitleInformation(Title.CanthanCartographer.Id, cancellationToken))!;
                        this.CartoTitle = titleInfo is null ?
                            default :
                            new CartoProgressContext
                            {
                                Continent = Continent.Cantha,
                                ResolvedName = titleInfo.TitleName,
                                Percentage = titleInfo.CurrentPoints / 10d,
                                TitleInformationExtended = titleInfo,
                                TitleName = Title.CanthanCartographer.Name
                            };
                    }
                    else if (maybeCurrentContinent == Continent.Elona)
                    {
                        var titleInfo = (await this.guildwarsMemoryReader.GetTitleInformation(Title.ElonianCartographer.Id, cancellationToken))!;
                        this.CartoTitle = titleInfo is null ?
                            default :
                            new CartoProgressContext
                            {
                                Continent = Continent.Elona,
                                ResolvedName = titleInfo.TitleName,
                                Percentage = titleInfo.CurrentPoints / 10d,
                                TitleInformationExtended = titleInfo,
                                TitleName = Title.ElonianCartographer.Name
                            };
                    }

                    this.ShowCartoProgress = this.CartoTitle is not null;
                }
                else
                {
                    this.ShowCartoProgress = false;
                }

                retries = 0;
            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic reading");
                return;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadGameData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadGameData), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(GameDataFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(GameDataFrequency, cancellationToken);
                    continue;
                }

                if (!this.MinimapVisible &&
                    this.minimapWindow is null)
                {
                    await Task.Delay(GameDataFrequency, cancellationToken);
                    continue;
                }

                var readGameDataTask = this.guildwarsMemoryCache.ReadGameData(cancellationToken);
                await Task.WhenAll(
                    readGameDataTask,
                    Task.Delay(GameDataFrequency, cancellationToken)).ConfigureAwait(true);

                var maybeGameData = await readGameDataTask ?? throw new HttpRequestException();
                if (maybeGameData is null)
                {
                    continue;
                }

                if (maybeGameData.MainPlayer is not null)
                {
                    maybeGameData.MainPlayer.Position = this.GameData?.MainPlayer?.Position ?? new Position();
                }
                
                foreach(var worldPlayer in maybeGameData.WorldPlayers ?? [])
                {
                    worldPlayer.Position = this.GameData?.WorldPlayers?.FirstOrDefault(w => w.Id == worldPlayer.Id)?.Position ?? new Position();
                }

                foreach (var partyPlayer in maybeGameData.Party ?? [])
                {
                    partyPlayer.Position = this.GameData?.Party?.FirstOrDefault(w => w.Id == partyPlayer.Id)?.Position ?? new Position();
                }

                foreach (var entity in maybeGameData.LivingEntities ?? [])
                {
                    var oldEntity = this.GameData?.LivingEntities?.FirstOrDefault(w => w.Id == entity.Id);
                    entity.Position = oldEntity?.Position ?? new Position();
                    entity.State = oldEntity?.State ?? LivingEntityState.Unknown;
                }

                this.GameData = maybeGameData;
                retries = 0;
            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic reading");
                return;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadInventoryData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadInventoryData), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(InventoryDataFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(InventoryDataFrequency, cancellationToken);
                    continue;
                }

                if (!this.InventoryVisible)
                {
                    await Task.Delay(InventoryDataFrequency, cancellationToken);
                    continue;
                }

                var readInventoryTask = this.guildwarsMemoryCache.ReadInventoryData(cancellationToken);
                await Task.WhenAll(
                    readInventoryTask,
                    Task.Delay(InventoryDataFrequency, cancellationToken)).ConfigureAwait(true);

                var maybeInventoryData = await readInventoryTask ?? throw new HttpRequestException();
                if (maybeInventoryData is null)
                {
                    continue;
                }

                this.InventoryData = maybeInventoryData;
                retries = 0;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadMainPlayerContextData(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.PeriodicallyReadMainPlayerContextData), string.Empty);
        var retries = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.PauseDataFetching)
                {
                    await Task.Delay(MainPlayerDataFrequency, cancellationToken);
                    continue;
                }

                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    await Task.Delay(MainPlayerDataFrequency, cancellationToken);
                    continue;
                }

                if (context.GuildWarsProcess?.HasExited is not false)
                {
                    this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
                    this.viewManager.ShowView<LauncherView>();
                    this.cancellationTokenSource?.Cancel();
                    return;
                }

                var readUserDataTask = this.guildwarsMemoryCache.ReadUserData(cancellationToken);
                var readSessionDataTask = this.guildwarsMemoryCache.ReadSessionData(cancellationToken);
                var readMainPlayerDataTask = this.guildwarsMemoryCache.ReadMainPlayerData(cancellationToken);
                await Task.WhenAll(
                    readUserDataTask,
                    readSessionDataTask,
                    readMainPlayerDataTask,
                    Task.Delay(MainPlayerDataFrequency, cancellationToken)).ConfigureAwait(true);

                var userData = await readUserDataTask ?? throw new HttpRequestException();
                var sessionData = await readSessionDataTask ?? throw new HttpRequestException();
                var mainPlayerData = await readMainPlayerDataTask ?? throw new HttpRequestException();
                if (userData?.User is null ||
                    sessionData?.Session is null ||
                    mainPlayerData?.PlayerInformation is null ||
                    sessionData.Session.InstanceType is InstanceType.Loading or InstanceType.Undefined ||
                    sessionData.Session.InstanceTimer == 0)
                {
                    this.MainPlayerDataValid = false;
                    continue;
                }

                this.MainPlayerResourceContext = new MainPlayerResourceContext
                {
                    Player = mainPlayerData,
                    User = userData,
                    Session = sessionData
                };

                this.MainPlayerDataValid = true;
                this.Browser.Visibility = this.minimapMaximized ? Visibility.Hidden : Visibility.Visible;
                retries = 0;
            }
            catch (InvalidOperationException ex)
            {
                scopedLogger.LogError(ex, "Encountered invalid operation exception. Cancelling periodic main player reading");
                return;
            }
            catch (Exception ex) when (ex is TimeoutException or OperationCanceledException or HttpRequestException)
            {
                if (this.DataContext is not GuildWarsApplicationLaunchContext context)
                {
                    continue;
                }

                scopedLogger.LogError(ex, "Encountered timeout. Verifying connection");
                try
                {
                    await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
                }
                catch (InvalidOperationException innerEx)
                {
                    retries++;
                    if (retries >= MaxRetries)
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Returning to launcher view");
                        this.notificationService.NotifyError(
                            title: "GuildWars unresponsive",
                            description: "Could not connect to Guild Wars instance. Returning to Launcher view");
                        this.viewManager.ShowView<LauncherView>();
                    }
                    else
                    {
                        scopedLogger.LogError(innerEx, "Could not ensure connection is initialized. Backing off before retrying");
                        await Task.Delay(UninitializedBackoff, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                scopedLogger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void FocusView_Loaded(object _, RoutedEventArgs e)
    {
        if (this.DataContext is not GuildWarsApplicationLaunchContext context)
        {
            return;
        }

        this.mainWindowEventsHook.RegisterHookOnSizeOrMoveBegin(this.OnMainWindowSizeOrMoveStart);
        this.mainWindowEventsHook.RegisterHookOnSizeOrMoveEnd(this.OnMainWindowSizeOrMoveEnd);
        this.Browser.BrowserHistoryManager.SetBrowserHistory(this.liveUpdateableOptions.Value.BrowserHistory);
        this.InventoryVisible = this.liveUpdateableOptions.Value.InventoryComponentVisible;
        this.MinimapVisible = this.liveUpdateableOptions.Value.MinimapComponentVisible;
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
        this.PeriodicallyReadMainPlayerContextData(cancellationToken);
        if (this.InventoryVisible)
        {
            this.PeriodicallyReadInventoryData(cancellationToken);
        }

        if (this.MinimapVisible)
        {
            this.PeriodicallyReadGameState(cancellationToken);
            this.PeriodicallyReadGameData(cancellationToken);
            this.PeriodicallyReadPathingData(cancellationToken);
            this.PeriodicallyReadCartoData(cancellationToken);
        }
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveBegin(this.OnMainWindowSizeOrMoveStart);
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveEnd(this.OnMainWindowSizeOrMoveEnd);
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
        this.GameData = default;
        this.GameState = default;
        this.PathingData = default;
        this.InventoryData = default;
        this.MinimapWindow_Closed(this, e);
    }

    private void Browser_MaximizeClicked(object _, EventArgs e)
    {
        this.browserMaximized = !this.browserMaximized;
        if (this.browserMaximized)
        {
            this.BrowserHolder.Children.Remove(this.Browser);
            this.FullScreenHolder.Children.Add(this.Browser);
            this.FullScreenHolder.Visibility = Visibility.Visible;
        }
        else
        {
            this.FullScreenHolder.Children.Remove(this.Browser);
            this.BrowserHolder.Children.Add(this.Browser);
            this.FullScreenHolder.Visibility = Visibility.Hidden;
        }
    }

    private void Browser_BuildDecoded(object _, DownloadedBuild e)
    {
        if (e is null ||
            e.Build is null)
        {
            return;
        }

        e.Build.Name = e.PreferredName ?? e.Build.Name;
        if (e.Build is SingleBuildEntry)
        {
            this.viewManager.ShowView<SingleBuildTemplateView>(e.Build);
        }
        else if (e.Build is TeamBuildEntry)
        {
            this.viewManager.ShowView<TeamBuildTemplateView>(e.Build);
        }
        
    }

    private void Component_NavigateToClicked(object _, string e)
    {
        this.BrowserAddress = e;
    }

    private void InventoryComponent_MaximizeClicked(object sender, EventArgs e)
    {
        this.inventoryMaximized = !this.inventoryMaximized;
        if (this.inventoryMaximized)
        {
            this.InventoryHolder.Children.Remove(this.InventoryComponent);
            this.FullScreenHolder.Children.Add(this.InventoryComponent);
            this.BrowserHolder.Visibility = Visibility.Hidden;
            this.FullScreenHolder.Visibility = Visibility.Visible;
        }
        else
        {
            this.FullScreenHolder.Children.Remove(this.InventoryComponent);
            this.InventoryHolder.Children.Add(this.InventoryComponent);
            this.BrowserHolder.Visibility = Visibility.Visible;
            this.FullScreenHolder.Visibility = Visibility.Hidden;
        }
    }

    private void GuildwarsMinimap_MaximizeClicked(object sender, EventArgs e)
    {
        this.minimapMaximized = !this.minimapMaximized;
        if (this.minimapMaximized)
        {
            this.MinimapHolder.Children.Remove(this.MinimapComponent);
            this.FullScreenHolder.Children.Add(this.MinimapComponent);
            this.Browser.Visibility = Visibility.Hidden;
            this.FullScreenHolder.Visibility = Visibility.Visible;
        }
        else
        {
            this.FullScreenHolder.Children.Remove(this.MinimapComponent);
            this.MinimapHolder.Children.Add(this.MinimapComponent);
            this.Browser.Visibility = Visibility.Visible;
            this.FullScreenHolder.Visibility = Visibility.Hidden;
        }
    }

    private void GuildwarsMinimap_QuestMetadataClicked(object _, QuestMetadata e)
    {
        this.BrowserAddress = e.Quest!.WikiUrl;
    }

    private void GuildwarsMinimap_LivingEntityClicked(object _, LivingEntity e)
    {
        if (e.NpcDefinition?.WikiUrl.IsNullOrWhiteSpace() is not false)
        {
            return;
        }

        this.BrowserAddress = e.NpcDefinition!.WikiUrl;
    }

    private void GuildwarsMinimap_PlayerInformationClicked(object _, PlayerInformation e)
    {
        if (e.NpcDefinition?.WikiUrl.IsNullOrWhiteSpace() is not false)
        {
            return;
        }

        this.BrowserAddress = e.NpcDefinition!.WikiUrl;
    }

    private void GuildwarsMinimap_MapIconClicked(object _, MapIcon e)
    {
        if (e.Icon?.WikiUrl!.IsNullOrWhiteSpace() is true)
        {
            return;
        }

        this.BrowserAddress = e.Icon!.WikiUrl;
    }

    private void GuildwarsMinimap_ProfessionClicked(object _, Profession e)
    {
        if (e.WikiUrl?.IsNullOrWhiteSpace() is true)
        {
            return;
        }

        this.BrowserAddress = e.WikiUrl;
    }

    private void InventoryComponent_ItemWikiClicked(object _, ItemBase e)
    {
        if (e is not IWikiEntity entity)
        {
            return;
        }

        this.BrowserAddress = entity.WikiUrl;
    }

    private void GuildwarsMinimap_NpcNameClicked(object _, string e)
    {
        if (e.IsNullOrEmpty() is not false)
        {
            return;
        }

        var indexOfSeparator = e.IndexOf("[");
        indexOfSeparator = indexOfSeparator >= 0 ? indexOfSeparator : e.Length;
        var curedNpcName = e[..indexOfSeparator];
        var npcUrl = WikiUrl.Replace(NamePlaceholder, curedNpcName);
        this.BrowserAddress = npcUrl;
    }

    private void InventoryComponent_PriceHistoryClicked(object _, ItemBase e)
    {
        this.viewManager.ShowView<PriceHistoryView>(e);
    }

    private void MinimapExtractButton_Clicked(object sender, EventArgs e)
    {
        if (this.minimapWindow is not null)
        {
            return;
        }

        if (this.minimapMaximized)
        {
            this.GuildwarsMinimap_MaximizeClicked(sender, e);
        }

        this.minimapWindow = new()
        {
            Resources = this.Resources,
            Opaque = !this.liveUpdateableOptions.Value.MinimapTransparency
        };

        var minimapWindowOptions = this.minimapWindowOptions.Value.ThrowIfNull();
        this.minimapWindow.Pinned = minimapWindowOptions.Pinned;
        var dpiScale = VisualTreeHelper.GetDpi(this.minimapWindow);
        var minimapSize = new Rect
        {
            X = minimapWindowOptions.X * dpiScale.DpiScaleX / minimapWindowOptions.DpiX,
            Y = minimapWindowOptions.Y * dpiScale.DpiScaleY / minimapWindowOptions.DpiY,
            Width = minimapWindowOptions.Width * dpiScale.DpiScaleX / minimapWindowOptions.DpiX,
            Height = minimapWindowOptions.Height * dpiScale.DpiScaleY / minimapWindowOptions.DpiY
        };

        if (minimapSize.Width > 0 && minimapSize.Height > 0 &&
            this.screenManager.Screens.Any(s => s.Size.Contains(minimapSize) || s.Size.IntersectsWith(minimapSize)))
        {
            this.minimapWindow.Left = minimapSize.Left;
            this.minimapWindow.Top = minimapSize.Top;
            this.minimapWindow.Width = minimapSize.Width;
            this.minimapWindow.Height = minimapSize.Height;
        }

        this.minimapWindow.Closed += this.MinimapWindow_Closed;
        this.MinimapExtracted = true;
        this.MinimapVisible = false;
        this.MinimapHolder.Children.Remove(this.MinimapComponent);
        this.minimapWindow.Content = this.MinimapComponent;
        this.minimapWindow.Show();
        this.minimapWindowEventsHook = new WindowEventsHook<MinimapWindow>(this.minimapWindow);
        this.minimapWindowEventsHook.RegisterHookOnSizeOrMoveBegin(this.OnMinimapWindowSizeOrMoveStart);
        this.minimapWindowEventsHook.RegisterHookOnSizeOrMoveEnd(this.OnMinimapWindowSizeOrMoveEnd);
        this.RowAutoMargin.RecalculateRows();
    }

    private void MinimapWindow_Closed(object? sender, EventArgs e)
    {
        if (!this.MinimapExtracted)
        {
            return;
        }

        if (this.minimapWindow is null)
        {
            return;
        }

        var options = this.minimapWindowOptions.Value.ThrowIfNull();
        var dpiScale = VisualTreeHelper.GetDpi(this.minimapWindow);
        options.X = this.minimapWindow.Left;
        options.Y = this.minimapWindow.Top;
        options.Width = this.minimapWindow.Width;
        options.Height = this.minimapWindow.Height;
        options.DpiX = dpiScale.DpiScaleX;
        options.DpiY = dpiScale.DpiScaleY;
        options.Pinned = this.minimapWindow.Pinned;

        this.minimapWindowOptions.UpdateOption();

        this.MinimapExtracted = false;
        this.MinimapVisible = this.liveUpdateableOptions.Value.MinimapComponentVisible;
        this.minimapWindow.Close();
        this.minimapWindow.Closed -= this.MinimapWindow_Closed;
        this.minimapWindow.Content = default!;
        this.minimapWindow = default;
        this.minimapWindowEventsHook?.Dispose();
        this.minimapWindowEventsHook = default;
        this.MinimapHolder.Children.Insert(0, this.MinimapComponent);
        this.RowAutoMargin.RecalculateRows();
    }

    private void OnMainWindowSizeOrMoveStart()
    {
        this.IsEnabled = false;
        this.PauseDataFetching = true;
    }

    private void OnMainWindowSizeOrMoveEnd()
    {
        this.IsEnabled = true;
        this.PauseDataFetching = false;
    }

    private void OnMinimapWindowSizeOrMoveStart()
    {
        this.PauseDataFetching = true;
    }

    private void OnMinimapWindowSizeOrMoveEnd()
    {
        this.PauseDataFetching = false;
    }
}
