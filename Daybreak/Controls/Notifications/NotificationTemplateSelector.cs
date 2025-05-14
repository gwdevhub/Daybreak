using Daybreak.Shared.Models.Notifications;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Controls.Notifications;

public sealed class NotificationTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (container is not FrameworkElement frameworkElement)
        {
            return base.SelectTemplate(item, container);
        }

        if (item is not NotificationWrapper notificationWrapper)
        {
            return base.SelectTemplate(item, container);
        }

        if (notificationWrapper.Notification?.Level is LogLevel.Information)
        {
            return frameworkElement.FindResource("InformationNotificationDataTemplate").Cast<DataTemplate>();
        }

        if (notificationWrapper.Notification?.Level is LogLevel.Error)
        {
            return frameworkElement.FindResource("ErrorNotificationDataTemplate").Cast<DataTemplate>();
        }

        return base.SelectTemplate(item, container);
    }
}
