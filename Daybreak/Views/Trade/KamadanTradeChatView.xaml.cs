using Daybreak.Configuration.Options;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.TradeChat;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for KamadanTradeChatView.xaml
/// </summary>
public partial class KamadanTradeChatView : UserControl
{
    private readonly INotificationService notificationService;

    [GenerateDependencyProperty]
    private KamadanTradeChatOptions options = default!;

    [GenerateDependencyProperty]
    private ITradeChatService<KamadanTradeChatOptions> tradeChatService = default!;

    public KamadanTradeChatView(
        IOptions<KamadanTradeChatOptions> options,
        INotificationService notificationService,
        ITradeChatService<KamadanTradeChatOptions> tradeChatService)
    {
        this.Options = options.ThrowIfNull().Value.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.TradeChatService = tradeChatService.ThrowIfNull();
        this.InitializeComponent();
    }

    private void TradeChatTemplate_NameCopyClicked(object _, TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Name copied to clipboard");
        Clipboard.SetText(e.Sender);
    }

    private void TradeChatTemplate_MessageCopyClicked(object _, TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Message copied to clipboard");
        Clipboard.SetText(e.Message);
    }

    private void TradeChatTemplate_TimestampCopyClicked(object _, TraderMessage e)
    {
        this.notificationService.NotifyInformation("Copied", "Timestamp copied to clipboard");
        Clipboard.SetText(e.Timestamp.ToString());
    }
}
