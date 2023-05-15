using Daybreak.Configuration.Options;
using Daybreak.Services.TradeChat;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for KamadanTradeChatView.xaml
/// </summary>
public partial class KamadanTradeChatView : UserControl
{
    [GenerateDependencyProperty]
    private KamadanTradeChatOptions options = default!;

    [GenerateDependencyProperty]
    private ITradeChatService<KamadanTradeChatOptions> tradeChatService = default!;

    public KamadanTradeChatView(
        IOptions<KamadanTradeChatOptions> options,
        ITradeChatService<KamadanTradeChatOptions> tradeChatService)
    {
        this.Options = options.ThrowIfNull().Value.ThrowIfNull();
        this.TradeChatService = tradeChatService.ThrowIfNull();
        this.InitializeComponent();
    }
}
