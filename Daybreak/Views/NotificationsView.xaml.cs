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

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for NotificationsView.xaml
/// </summary>
public partial class NotificationsView : UserControl
{
    private readonly INotificationProducer notificationProducer;
    private readonly ILogger<NotificationsView> logger;

    private CancellationTokenSource? cancellationTokenSource;

    public ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();

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
            var notifications = this.notificationProducer.GetAllNotifications().OrderBy(n => n.CreationTime);
            var notificationsToAdd = notifications.Where(n => this.Notifications.None(n2 => n2.Id == n.Id)).ToList();
            var notificationsToRemove = this.Notifications.Where(n => notifications.None(n2 => n2.Id == n.Id)).ToList();
            this.Notifications.AddRange(notificationsToAdd);
            foreach(var notification in notificationsToRemove)
            {
                this.Notifications.Remove(notification);
            }

            await Task.Delay(1000, cancellationToken);
        }
    }

    private void NotificationTemplate_OpenClicked(object _, Notification e)
    {
        this.notificationProducer.OpenNotification(e, e.Dismissible);
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
}
