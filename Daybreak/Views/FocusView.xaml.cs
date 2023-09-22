using Daybreak.Configuration.Options;
using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Experience;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
using Daybreak.Views.Trade;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for FocusView.xaml
/// </summary>
public partial class FocusView : UserControl
{
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IApplicationLauncher applicationLauncher;
    private readonly IGuildwarsMemoryCache guildwarsMemoryCache;
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private InventoryData inventoryData = new();

    [GenerateDependencyProperty]
    private GameData gameData = new();

    [GenerateDependencyProperty]
    private bool mainPlayerDataValid;

    [GenerateDependencyProperty]
    private PathingData pathingData = new();

    [GenerateDependencyProperty]
    private MainPlayerResourceContext mainPlayerResourceContext = new();

    [GenerateDependencyProperty]
    private bool loadingPathingData;

    [GenerateDependencyProperty]
    private bool faultyPathingData;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private bool inventoryVisible;

    [GenerateDependencyProperty]
    private bool minimapVisible;

    private bool browserMaximized = false;
    private bool minimapMaximized = false;
    private bool inventoryMaximized = false;
    private CancellationTokenSource? cancellationTokenSource;

    public FocusView(
        IBuildTemplateManager buildTemplateManager,
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryCache guildwarsMemoryCache,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions,
        ILogger<FocusView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryCache = guildwarsMemoryCache.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == BrowserAddressProperty &&
            this.Browser.BrowserEnabled)
        {
            this.liveUpdateableOptions.Value.BrowserUrl = this.BrowserAddress;
            this.liveUpdateableOptions.UpdateOption();
        }

        base.OnPropertyChanged(e);
    }

    private async Task UpdatePathingData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
            this.cancellationTokenSource?.Cancel();
            return;
        }

        var pathingMeta = await this.guildwarsMemoryCache.ReadPathingMetaData(this.cancellationTokenSource?.Token ?? CancellationToken.None);
        if (pathingMeta?.TrapezoidCount == this.PathingData?.Trapezoids?.Count ||
            this.cancellationTokenSource?.IsCancellationRequested is not false)
        {
            await this.Dispatcher.InvokeAsync(() => this.LoadingPathingData = false);
            return;
        }

        await this.Dispatcher.InvokeAsync(() => this.LoadingPathingData = true);
        var maybePathingData = await this.guildwarsMemoryCache.ReadPathingData(this.cancellationTokenSource?.Token ?? CancellationToken.None);
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
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.WhenAll(
                    this.UpdatePathingData(),
                    Task.Delay(1000, cancellationToken)).ConfigureAwait(true);

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadGameData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var readGameDataTask = this.guildwarsMemoryCache.ReadGameData(cancellationToken);
                await Task.WhenAll(
                    readGameDataTask,
                    Task.Delay(16, cancellationToken)).ConfigureAwait(true);

                var maybeGameData = await readGameDataTask;
                if (maybeGameData is null)
                {
                    continue;
                }

                this.GameData = maybeGameData;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadInventoryData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var readInventoryTask = this.guildwarsMemoryCache.ReadInventoryData(cancellationToken);
                await Task.WhenAll(
                    readInventoryTask,
                    Task.Delay(1000, cancellationToken)).ConfigureAwait(true);

                var maybeInventoryData = await readInventoryTask;
                if (maybeInventoryData is null)
                {
                    continue;
                }

                this.InventoryData = maybeInventoryData;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private async void PeriodicallyReadMainPlayerContextData(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (this.applicationLauncher.IsGuildwarsRunning is false)
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
                    Task.Delay(16, cancellationToken)).ConfigureAwait(true);

                var userData = await readUserDataTask;
                var sessionData = await readSessionDataTask;
                var mainPlayerData = await readMainPlayerDataTask;
                if (userData?.User is null ||
                    sessionData?.Session is null ||
                    mainPlayerData?.PlayerInformation is null ||
                    sessionData.Session.InstanceType is InstanceType.Loading or InstanceType.Undefined)
                {
                    this.MainPlayerDataValid = false;
                    this.Browser.Visibility = Visibility.Collapsed;
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
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Encountered non-terminating exception. Silently continuing");
            }
        }
    }

    private void FocusView_Loaded(object _, RoutedEventArgs e)
    {
        this.BrowserAddress = this.liveUpdateableOptions.Value.BrowserUrl;
        this.InventoryVisible = this.liveUpdateableOptions.Value.InventoryComponentVisible;
        this.MinimapVisible = this.liveUpdateableOptions.Value.MinimapComponentVisible;
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        this.PeriodicallyReadMainPlayerContextData(cancellationToken);
        if (this.InventoryVisible)
        {
            this.PeriodicallyReadInventoryData(cancellationToken);
        }

        if (this.MinimapVisible)
        {
            this.PeriodicallyReadGameData(cancellationToken);
            this.PeriodicallyReadPathingData(cancellationToken);
        }
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
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
        if (e is null)
        {
            return;
        }

        var buildEntry = this.buildTemplateManager.CreateBuild();
        buildEntry.Build = e.Build;
        buildEntry.Name = e.PreferredName ?? buildEntry.Name;
        this.viewManager.ShowView<BuildTemplateView>(buildEntry);
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
        if (e.NpcDefinition?.WikiUrl.IsNullOrWhiteSpace() is true)
        {
            return;
        }

        this.BrowserAddress = e.NpcDefinition!.WikiUrl;
    }

    private void GuildwarsMinimap_PlayerInformationClicked(object _, PlayerInformation e)
    {
        if (e.NpcDefinition?.WikiUrl.IsNullOrWhiteSpace() is true)
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

    private void InventoryComponent_PriceHistoryClicked(object _, ItemBase e)
    {
        this.viewManager.ShowView<PriceHistoryView>(e);
    }
}
