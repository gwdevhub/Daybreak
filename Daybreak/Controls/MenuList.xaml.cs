using Daybreak.Configuration.Options;
using Daybreak.Launch;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Views;
using Daybreak.Views.Copy;
using Daybreak.Views.Onboarding.Toolbox;
using Daybreak.Views.Onboarding.UMod;
using Daybreak.Views.Trade;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for MenuList.xaml
/// </summary>
public partial class MenuList : UserControl
{
    private readonly IViewManager viewManager;
    private readonly INotificationStorage notificationStorage;
    private readonly ILiveOptions<LauncherOptions> liveOptions;

    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty]
    private bool showingNotificationCount;
    [GenerateDependencyProperty]
    private int notificationCount;

    public MenuList()
        : this(
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IViewManager>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<INotificationStorage>(),
              Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<LauncherOptions>>())
    {
        this.InitializeComponent();
    }

    private MenuList(
        IViewManager viewManager,
        INotificationStorage notificationStorage,
        ILiveOptions<LauncherOptions> liveOptions)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.notificationStorage = notificationStorage.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
    }

    private void GameCompanionButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void AccountSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<AccountsView>();
    }

    private void GuildwarsSettingsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ExecutablesView>();
    }

    private void ManageBuildsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<BuildsListView>();
    }

    private void VersionManagementButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<VersionManagementView>();
    }
    
    private void LogsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<LogsView>();
    }

    private void MetricsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<MetricsView>();
    }

    private void DownloadGuildwarsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<GuildwarsDownloadView>();
    }

    private void CopyGuildwarsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<GuildwarsCopySelectionView>();
    }

    private void UModButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<UModOnboardingEntryView>();
    }

    private void ToolboxButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<ToolboxOnboardingEntryView>();
    }

    private void KamadanButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<KamadanTradeChatView>();
    }

    private void AscalonButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<AscalonTradeChatView>();
    }

    private void TraderQuotesButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<PriceQuotesView>();
    }

    private void NotificationsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<NotificationsView>();
    }

    private void TradeAlertsButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<TradeAlertsView>();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        this.PeriodicallyCheckUnopenedNotifications(this.cancellationTokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
        this.cancellationTokenSource = default;
    }

    private async void PeriodicallyCheckUnopenedNotifications(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var unopenedNotifications = this.notificationStorage.GetPendingNotifications().ToList();
            if (unopenedNotifications.Any())
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.ShowingNotificationCount = true;
                    this.NotificationCount = unopenedNotifications.Count;
                });
            }
            else
            {
                await this.Dispatcher.InvokeAsync(() =>
                {
                    this.ShowingNotificationCount = false;
                });
            }

            await Task.Delay(5000, cancellationToken);
        }
    }
}
