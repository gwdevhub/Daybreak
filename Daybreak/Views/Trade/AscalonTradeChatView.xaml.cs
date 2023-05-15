using Daybreak.Configuration.Options;
using Daybreak.Services.TradeChat;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for AscalonTradeChatView.xaml
/// </summary>
public partial class AscalonTradeChatView : UserControl
{
    [GenerateDependencyProperty]
    private AscalonTradeChatOptions options = default!;

    [GenerateDependencyProperty]
    private ITradeChatService<AscalonTradeChatOptions> tradeChatService = default!;

    public AscalonTradeChatView(
        IOptions<AscalonTradeChatOptions> options,
        ITradeChatService<AscalonTradeChatOptions> tradeChatService)
    {
        this.Options = options.ThrowIfNull().Value.ThrowIfNull();
        this.TradeChatService = tradeChatService.ThrowIfNull();
        this.InitializeComponent();
    }
}
