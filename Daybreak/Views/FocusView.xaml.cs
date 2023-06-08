using Daybreak.Configuration.Options;
using Daybreak.Models;
using Daybreak.Models.Guildwars;
using Daybreak.Services.ApplicationLauncher;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Experience;
using Daybreak.Services.Navigation;
using Daybreak.Services.Scanner;
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
    private readonly IGuildwarsMemoryReader guildwarsMemoryReader;
    private readonly IExperienceCalculator experienceCalculator;
    private readonly IViewManager viewManager;
    private readonly ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions;
    private readonly ILogger<FocusView> logger;

    [GenerateDependencyProperty]
    private InventoryData inventoryData;

    [GenerateDependencyProperty]
    private GameData gameData;

    [GenerateDependencyProperty]
    private bool mainPlayerDataValid;

    [GenerateDependencyProperty]
    private PathingData pathingData;

    [GenerateDependencyProperty]
    private bool loadingPathingData;

    [GenerateDependencyProperty]
    private bool faultyPathingData;

    [GenerateDependencyProperty]
    private string browserAddress = string.Empty;

    [GenerateDependencyProperty]
    private bool inventoryVisible;

    private bool browserMaximized = false;
    private bool minimapMaximized = false;
    private bool inventoryMaximized = false;
    private CancellationTokenSource? cancellationTokenSource;
    private CancellationTokenSource? loadingPathingDataCancellationTokenSource;

    public FocusView(
        IBuildTemplateManager buildTemplateManager,
        IApplicationLauncher applicationLauncher,
        IGuildwarsMemoryReader guildwarsMemoryReader,
        IExperienceCalculator experienceCalculator,
        IViewManager viewManager,
        ILiveUpdateableOptions<FocusViewOptions> liveUpdateableOptions,
        ILogger<FocusView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.applicationLauncher = applicationLauncher.ThrowIfNull();
        this.guildwarsMemoryReader = guildwarsMemoryReader.ThrowIfNull();
        this.experienceCalculator = experienceCalculator.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.gameData = new GameData();
        this.pathingData = new PathingData();

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

    private TimeSpan GetMemoryReaderLatency()
    {
        return TimeSpan.FromMilliseconds(this.liveUpdateableOptions.Value.MemoryReaderFrequency);
    }

    private async Task UpdatePathingData(CancellationToken cancellationToken)
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        this.Dispatcher.Invoke(() =>
        {
            this.LoadingPathingData = true;
        });

        await this.guildwarsMemoryReader.EnsureInitialized(cancellationToken);
        
        var maybePathingData = await this.guildwarsMemoryReader.ReadPathingData(cancellationToken);
        if (maybePathingData is not PathingData pathingData ||
            pathingData.Trapezoids is null ||
            pathingData.Trapezoids.Count == 0)
        {
            await Task.Delay(1000, cancellationToken);
            await this.UpdatePathingData(cancellationToken);
            return;
        }

        this.Dispatcher.Invoke(() =>
        {
            this.PathingData = pathingData;
            this.LoadingPathingData = false;
        });
    }

    private async Task UpdateGameData()
    {
        if (this.applicationLauncher.IsGuildwarsRunning is false)
        {
            this.logger.LogInformation($"Executable is not running. Returning to {nameof(LauncherView)}");
            this.viewManager.ShowView<LauncherView>();
        }

        await this.guildwarsMemoryReader.EnsureInitialized(this.cancellationTokenSource?.Token ?? CancellationToken.None).ConfigureAwait(true);

        var maybeGameData = await this.guildwarsMemoryReader.ReadGameData(this.cancellationTokenSource?.Token ?? CancellationToken.None).ConfigureAwait(true);
        if (maybeGameData is not GameData gameData)
        {
            this.MainPlayerDataValid = false;
            this.Browser.Visibility = this.MainPlayerDataValid is true ? Visibility.Visible : Visibility.Collapsed;
            return;
        }

        if (gameData.MainPlayer is null ||
            gameData.User is null ||
            gameData.Session is null ||
            gameData.Valid is false)
        {
            this.MainPlayerDataValid = false;
            this.Browser.Visibility = this.MainPlayerDataValid is true ? Visibility.Visible : Visibility.Collapsed;
            return;
        }

        this.GameData = gameData;
        this.MainPlayerDataValid = true;
        var pathingMeta = await this.guildwarsMemoryReader.ReadPathingMetaData(this.cancellationTokenSource?.Token ?? CancellationToken.None);
        if (pathingMeta?.TrapezoidCount != this.PathingData.Trapezoids?.Count &&
            this.loadingPathingDataCancellationTokenSource is null)
        {
            this.loadingPathingDataCancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(() => this.UpdatePathingData(this.loadingPathingDataCancellationTokenSource.Token), this.loadingPathingDataCancellationTokenSource.Token)
                .ContinueWith(_ =>
                {
                    this.loadingPathingDataCancellationTokenSource?.Dispose();
                    this.loadingPathingDataCancellationTokenSource = null;
                });
        }

        this.Browser.Visibility = this.MainPlayerDataValid is true ?
            this.minimapMaximized ?
                Visibility.Hidden :
                Visibility.Visible :
            Visibility.Collapsed;
    }

    private async void PeriodicallyReadGameData(CancellationToken cancellationToken)
    {
        var memoryReaderLatency = this.GetMemoryReaderLatency();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.WhenAll(
                    this.UpdateGameData(),
                    Task.Delay(memoryReaderLatency, cancellationToken)).ConfigureAwait(true);

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

                var readInventoryTask = this.guildwarsMemoryReader.ReadInventory(cancellationToken);
                await Task.WhenAll(
                    readInventoryTask,
                    Task.Delay(1000, cancellationToken)).ConfigureAwait(true);

                var maybeInventoryData = await readInventoryTask;
                if (!maybeInventoryData.HasValue)
                {
                    continue;
                }

                this.InventoryData = maybeInventoryData.Value;
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
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = this.cancellationTokenSource.Token;
        this.PeriodicallyReadGameData(cancellationToken);
        if (this.InventoryVisible)
        {
            this.PeriodicallyReadInventoryData(cancellationToken);
        }
    }

    private void FocusView_Unloaded(object _, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
        this.loadingPathingDataCancellationTokenSource?.Cancel();
        this.loadingPathingDataCancellationTokenSource = null;
        this.guildwarsMemoryReader?.Stop();
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

    private void Browser_BuildDecoded(object _, Build e)
    {
        if (e is null)
        {
            return;
        }

        var buildEntry = this.buildTemplateManager.CreateBuild();
        buildEntry.Build = e;
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
        if (e.Icon?.WikiUrl.IsNullOrWhiteSpace() is true)
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
}
