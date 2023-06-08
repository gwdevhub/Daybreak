using Daybreak.Models.Notifications;
using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Notifications;
/// <summary>
/// Interaction logic for NotificationTemplate.xaml
/// </summary>
public partial class NotificationTemplate : UserControl
{
    public event EventHandler<Notification>? OpenClicked;
    public event EventHandler<Notification>? RemoveClicked;

    public NotificationTemplate()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not Notification notification)
        {
            return;
        }

        this.OpenClicked?.Invoke(this, notification);
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not Notification notification)
        {
            return;
        }

        this.RemoveClicked?.Invoke(this, notification);
    }
}
