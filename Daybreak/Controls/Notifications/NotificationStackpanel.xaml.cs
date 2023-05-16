using Daybreak.Models.Notifications;
using Daybreak.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Daybreak.Controls.Notifications;
/// <summary>
/// Interaction logic for NotificationStackpanel.xaml
/// </summary>
public partial class NotificationStackpanel : UserControl
{
    private static readonly TimeSpan ProcInterval = TimeSpan.FromSeconds(1);

    private readonly INotificationProducer notificationProducer;
    private readonly DispatcherTimer dispatcherTimer = new();
    private CancellationTokenSource? cancellationToken;

    public ObservableCollection<NotificationWrapper> Notifications { get; } = new();

    public NotificationStackpanel() :
        this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<INotificationProducer>())
    {
    }

    public NotificationStackpanel(
        INotificationProducer notificationProducer)
    {
        this.notificationProducer = notificationProducer.ThrowIfNull();
        this.InitializeComponent();

        this.dispatcherTimer.Interval = ProcInterval;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationToken?.Cancel();
        this.dispatcherTimer.Start();
        this.ConsumeNotifications();
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.cancellationToken?.Cancel();
        this.dispatcherTimer.Stop();
    }

    private void NotificationView_Expired(object sender, EventArgs e)
    {
        if (sender.As<UserControl>()?.DataContext is not NotificationWrapper notificationWrapper)
        {
            return;
        }

        this.Notifications.Remove(notificationWrapper);
    }

    private async void ConsumeNotifications()
    {
        this.cancellationToken = new CancellationTokenSource();
        await foreach(var notification in this.notificationProducer.Consume(this.cancellationToken.Token))
        {
            this.Notifications.Insert(0, new NotificationWrapper { Notification = notification, DispatcherTimer = this.dispatcherTimer });
        }
    }
}
