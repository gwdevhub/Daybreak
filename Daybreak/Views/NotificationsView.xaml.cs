using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for NotificationsView.xaml
/// </summary>
public partial class NotificationsView : UserControl
{
    private readonly INotificationProducer notificationProducer;
    private readonly ILogger<NotificationsView> logger;

    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool descending;

    [GenerateDependencyProperty(InitialValue = false)]
    private bool showAll;

    public ObservableCollection<Notification> Notifications { get; } = [];

    public NotificationsView(
        INotificationProducer notificationProducer,
        ILogger<NotificationsView> logger)
    {
        this.notificationProducer = notificationProducer.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        this.PeriodicallyLoadNotifications(this.cancellationTokenSource.Token);
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private async void PeriodicallyLoadNotifications(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var unsortedNotifications = this.ShowAll ?
                (await this.notificationProducer.GetAllNotifications(cancellationToken)).ToList() :
                (await this.notificationProducer.GetPendingNotifications(cancellationToken)).ToList();
            var notifications = this.Descending ?
                unsortedNotifications.OrderByDescending(n => n.CreationTime).Take(100).ToList() :
                unsortedNotifications.OrderBy(n => n.CreationTime).Take(100).ToList();
            using var context = await this.semaphoreSlim.Acquire();
            var notificationsToAdd = notifications.Where(n => this.Notifications.None(n2 => n2.Id == n.Id)).ToList();
            var notificationsToRemove = this.Notifications.Where(n => notifications.None(n2 => n2.Id == n.Id)).ToList();
            foreach (var notification in notificationsToRemove)
            {
                this.Notifications.Remove(notification);
            }

            foreach (var notification in notificationsToAdd)
            {
                var index = this.Descending ?
                    this.Notifications.IndexOfWhere(n2 => n2.CreationTime <= notification.CreationTime) :
                    this.Notifications.IndexOfWhere(n2 => n2.CreationTime >= notification.CreationTime);
                if (index == -1)
                {
                    this.Notifications.Add(notification);
                }
                else
                {
                    this.Notifications.Insert(index, notification);
                }
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    private void NotificationTemplate_OpenClicked(object _, Notification e)
    {
        this.notificationProducer.OpenNotification(e, e.Dismissible);
        e.Closed = true;
    }

    private async void NotificationTemplate_RemoveClicked(object _, Notification e)
    {
        await this.notificationProducer.RemoveNotification(e, CancellationToken.None);
        this.Notifications.Remove(e);
    }

    private async void HighlightButton_Clicked(object sender, EventArgs e)
    {
        await this.notificationProducer.RemoveAllNotifications(CancellationToken.None);
        this.Notifications.Clear();
    }

    private void SortButton_Clicked(object sender, EventArgs e)
    {
        lock (this.Notifications)
        {
            this.Descending = !this.Descending;
            var sortedNotification = this.Descending ?
                this.Notifications.OrderByDescending(n => n.CreationTime).ToList() :
                this.Notifications.OrderBy(n => n.CreationTime).ToList();
            this.Notifications.ClearAnd().AddRange(sortedNotification);
        }
    }

    private async void ToggleSwitch_Toggled(object sender, System.Windows.RoutedEventArgs e)
    {
        using var context = await this.semaphoreSlim.Acquire();
        var unsortedNotifications = this.ShowAll ?
                    (await this.notificationProducer.GetAllNotifications(CancellationToken.None)) :
                    (await this.notificationProducer.GetPendingNotifications(CancellationToken.None));
        var sortedNotification = this.Descending ?
                unsortedNotifications.OrderByDescending(n => n.CreationTime).ToList() :
                unsortedNotifications.OrderBy(n => n.CreationTime).ToList();
        this.Notifications.ClearAnd().AddRange(sortedNotification);
    }
}
