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
/// Interaction logic for AscalonTradeChatView.xaml
/// </summary>
public partial class AscalonTradeChatView : UserControl
{
    private readonly INotificationService notificationService;

    [GenerateDependencyProperty]
    private AscalonTradeChatOptions options = default!;

    [GenerateDependencyProperty]
    private ITradeChatService<AscalonTradeChatOptions> tradeChatService = default!;

    public AscalonTradeChatView(
        IOptions<AscalonTradeChatOptions> options,
        INotificationService notificationService,
        ITradeChatService<AscalonTradeChatOptions> tradeChatService)
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
