using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;

/// <summary>
/// Interaction logic for TradeNotificationView.xaml
/// </summary>
public partial class TradeNotificationView : UserControl
{
    private readonly INotificationService notificationService;
    private readonly IViewManager viewManager;

    public TradeNotificationView(
        INotificationService notificationService,
        IViewManager viewManager)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void TradeChatMessageTemplate_NameCopyClicked(object _, Models.Trade.TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Name copied to clipboard");
        Clipboard.SetText(e.Sender);
    }

    private void TradeChatMessageTemplate_MessageCopyClicked(object _, Models.Trade.TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Message copied to clipboard");
        Clipboard.SetText(e.Message);
    }

    private void TradeChatMessageTemplate_TimestampCopyClicked(object _, Models.Trade.TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Timestamp copied to clipboard");
        Clipboard.SetText(e.Timestamp.ToString());
    }

    private void TradeChatMessageTemplate_CloseButtonClicked(object _, EventArgs e)
    {
        this.viewManager.ShowView<NotificationsView>();
    }
}
