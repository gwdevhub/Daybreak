using Daybreak.Models.Notifications;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Daybreak.Controls.Notifications;
/// <summary>
/// Interaction logic for NotificationView.xaml
/// </summary>
public partial class NotificationView : UserControl
{
    private static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan ExpirationDuration = TimeSpan.FromSeconds(5);

    public event EventHandler? Expired;
    public event EventHandler? Closed;

    private DispatcherTimer? dispatcherTimer;
    private DateTime showNotificationExpirationTime = DateTime.Now; // The default notification timeout. Can be extended by moving the mouse over the notification
    private DateTime requestedNotificationExpirationTime = DateTime.Now; // Some notifications can have their desired timeouts. Don't make the notification disappear before this time

    public NotificationView()
    {
        this.InitializeComponent();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not NotificationWrapper notificationWrapper ||
            notificationWrapper.DispatcherTimer is null)
        {
            return;
        }

        this.showNotificationExpirationTime = DateTime.Now + ExpirationDuration;
        this.requestedNotificationExpirationTime = notificationWrapper.Notification?.ExpirationTime ?? DateTime.Now;
        if (this.dispatcherTimer is not null)
        {
            this.dispatcherTimer.Tick -= this.DispatcherTimer_Tick;
        }

        this.dispatcherTimer = notificationWrapper.DispatcherTimer;
        notificationWrapper.DispatcherTimer.Tick += this.DispatcherTimer_Tick;
        this.SlideInAnimation();
    }

    private void UserControl_Unloaded(object _, RoutedEventArgs __)
    {
        if (this.dispatcherTimer is null)
        {
            return;
        }

        this.dispatcherTimer.Tick -= this.DispatcherTimer_Tick;
    }

    private void UserControl_PreviewMouseMove(object _, MouseEventArgs __)
    {
        this.showNotificationExpirationTime = DateTime.Now + ExpirationDuration;
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not NotificationWrapper notificationWrapper ||
            notificationWrapper.Notification is not ICancellableNotification notification)
        {
            return;
        }

        if (!notification.Dismissible)
        {
            return;
        }

        if (this.dispatcherTimer is not null)
        {
            this.dispatcherTimer.Tick -= this.DispatcherTimer_Tick;
        }

        this.SlideOutAnimation(this.Closed);
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs __)
    {
        if (sender is not DispatcherTimer senderDispatcherTimer)
        {
            return;
        }

        if (this.DataContext is not NotificationWrapper notificationWrapper ||
            notificationWrapper.Notification is not ICancellableNotification notification)
        {
            senderDispatcherTimer.Tick -= this.DispatcherTimer_Tick;
            this.SlideOutAnimation(this.Closed);
            return;
        }

        if (notification.CancellationRequested)
        {
            senderDispatcherTimer.Tick -= this.DispatcherTimer_Tick;
            this.SlideOutAnimation(this.Closed);
            return;
        }

        if (DateTime.Now < this.showNotificationExpirationTime ||
            DateTime.Now < this.requestedNotificationExpirationTime)
        {
            return;
        }


        senderDispatcherTimer.Tick -= this.DispatcherTimer_Tick;
        this.SlideOutAnimation(this.Expired);
    }

    private void SlideInAnimation()
    {
        var doubleAnimation = new DoubleAnimation
        {
            From = 0,
            To = this.Width,
            Duration = AnimationDuration,
            AccelerationRatio = 0.3,
            DecelerationRatio = 0.3
        };

        this.BeginAnimation(WidthProperty, doubleAnimation);
    }

    private void SlideOutAnimation(EventHandler? onCompleted)
    {
        var doubleAnimation = new DoubleAnimation
        {
            From = this.Width,
            To = 0,
            Duration = AnimationDuration,
            AccelerationRatio = 0.3,
            DecelerationRatio = 0.3
        };

        doubleAnimation.Completed += (_, e) =>
        {
            onCompleted?.Invoke(this, e);
        };
        this.BeginAnimation(WidthProperty, doubleAnimation);
    }
}
