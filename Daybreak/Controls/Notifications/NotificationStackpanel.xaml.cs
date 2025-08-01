﻿using Daybreak.Shared;
using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Sounds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Daybreak.Controls.Notifications;
/// <summary>
/// Interaction logic for NotificationStackpanel.xaml
/// </summary>
public partial class NotificationStackpanel : UserControl
{
    private static readonly TimeSpan ProcInterval = TimeSpan.FromSeconds(1);

    private readonly ISoundService soundService;
    private readonly INotificationProducer notificationProducer;
    private readonly DispatcherTimer dispatcherTimer = new();
    private CancellationTokenSource? cancellationToken;

    public ObservableCollection<NotificationWrapper> Notifications { get; } = [];

    public NotificationStackpanel() :
        this(Global.GlobalServiceProvider.GetRequiredService<INotificationProducer>(),
             Global.GlobalServiceProvider.GetRequiredService<ISoundService>())
    {
    }

    internal NotificationStackpanel(
        INotificationProducer notificationProducer,
        ISoundService soundService)
    {
        this.soundService = soundService.ThrowIfNull();
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
        notificationWrapper.Notification!.Closed = true;
        this.soundService.PlayNotifyClose();
    }

    private void NotificationView_Closed(object sender, EventArgs e)
    {
        if (sender.As<UserControl>()?.DataContext is not NotificationWrapper notificationWrapper)
        {
            return;
        }

        this.Notifications.Remove(notificationWrapper);
        _ = this.notificationProducer.OpenNotification(notificationWrapper.Notification!);
        notificationWrapper.Notification!.Closed = true;
        this.soundService.PlayNotifyClose();
    }

    private async void ConsumeNotifications()
    {
        this.cancellationToken = new CancellationTokenSource();
        await foreach(var notification in this.notificationProducer.Consume(this.cancellationToken.Token))
        {
            this.Notifications.Insert(0, new NotificationWrapper { Notification = notification, DispatcherTimer = this.dispatcherTimer });
            switch (notification.Level)
            {
                case LogLevel.Critical:
                case LogLevel.Warning:
                case LogLevel.Error:
                    this.soundService.PlayNotifyError();
                    break;
                case LogLevel.Information:
                case LogLevel.Debug:
                case LogLevel.Trace:
                    this.soundService.PlayNotifyInformation();
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected notification level {notification.Level}");
            }
        }
    }
}
