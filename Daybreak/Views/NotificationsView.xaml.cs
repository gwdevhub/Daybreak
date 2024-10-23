using Daybreak.Models.Notifications;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
                this.notificationProducer.GetAllNotifications().ToList() :
                this.notificationProducer.GetPendingNotifications().ToList();
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

    private void NotificationTemplate_RemoveClicked(object _, Notification e)
    {
        this.notificationProducer.RemoveNotification(e);
        this.Notifications.Remove(e);
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.notificationProducer.RemoveAllNotifications();
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
                    this.notificationProducer.GetAllNotifications() :
                    this.notificationProducer.GetPendingNotifications();
        var sortedNotification = this.Descending ?
                unsortedNotifications.OrderByDescending(n => n.CreationTime).ToList() :
                unsortedNotifications.OrderBy(n => n.CreationTime).ToList();
        this.Notifications.ClearAnd().AddRange(sortedNotification);
    }
}
