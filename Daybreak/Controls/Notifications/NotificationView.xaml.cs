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

    private DispatcherTimer? dispatcherTimer;
    private DateTime notificationExpirationTime = DateTime.Now;

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

        this.notificationExpirationTime = DateTime.Now + ExpirationDuration;
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
        this.notificationExpirationTime = DateTime.Now + ExpirationDuration;
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not NotificationWrapper notificationWrapper ||
            notificationWrapper.Notification is not INotification notification)
        {
            return;
        }

        if (this.dispatcherTimer is not null)
        {
            this.dispatcherTimer.Tick -= this.DispatcherTimer_Tick;
        }

        this.SlideOutAnimation();
        notification?.OnClick?.Invoke();
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs __)
    {
        if (DateTime.Now < this.notificationExpirationTime)
        {
            return;
        }

        if (sender is not DispatcherTimer senderDispatcherTimer)
        {
            return;
        }

        senderDispatcherTimer.Tick -= this.DispatcherTimer_Tick;
        this.SlideOutAnimation();
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

    private void SlideOutAnimation()
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
            this.Expired?.Invoke(this, e);
        };
        this.BeginAnimation(WidthProperty, doubleAnimation);
    }
}
