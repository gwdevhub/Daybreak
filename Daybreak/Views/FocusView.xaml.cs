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
    private bool mainPlayerDataValid;

    [GenerateDependencyProperty]
    private MainPlayerResourceContext mainPlayerResourceContext = new();

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private bool pauseDataFetching;

    private bool browserMaximized = false;
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

        base.OnPropertyChanged(e);
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
                this.Browser.Visibility = Visibility.Visible;
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
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        await this.guildwarsMemoryCache.EnsureInitialized(context, cancellationToken);
        this.PeriodicallyReadMainPlayerContextData(cancellationToken);
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveBegin(this.OnMainWindowSizeOrMoveStart);
        this.mainWindowEventsHook.UnregisterHookOnSizeOrMoveEnd(this.OnMainWindowSizeOrMoveEnd);
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
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
