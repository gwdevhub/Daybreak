using Daybreak.Configuration.Options;
using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat;
using System;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for PriceQuotesView.xaml
/// </summary>
public partial class PriceQuotesView : UserControl
{
    private readonly ITradeChatService<KamadanTradeChatOptions> tradeChatService;

    public ObservableCollection<TraderQuote> TraderQuotes { get; } = new ObservableCollection<TraderQuote>();

    public PriceQuotesView(
        ITradeChatService<KamadanTradeChatOptions> tradeChatService)
    {
        this.tradeChatService = tradeChatService.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var trades = await this.tradeChatService.GetBuyQuotes(CancellationToken.None);
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.TraderQuotes.ClearAnd().AddRange(trades);
        });
    }
}
