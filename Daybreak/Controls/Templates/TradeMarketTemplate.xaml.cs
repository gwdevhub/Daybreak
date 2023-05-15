using Daybreak.Models.Trade;
using Daybreak.Services.TradeChat;
using Daybreak.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.Templates;
/// <summary>
/// Interaction logic for TradeMarketTemplate.xaml
/// </summary>
public partial class TradeMarketTemplate : UserControl
{
    private readonly List<TraderQuote> traderQuotes = new();

    [GenerateDependencyProperty]
    private ITradeChatService tradeChatService = default!;
    [GenerateDependencyProperty]
    private string title = default!;
    [GenerateDependencyProperty]
    private string marketUrl = default!;

    public ObservableCollection<TraderQuote> TraderQuotes { get; } = new ObservableCollection<TraderQuote>();

    public TradeMarketTemplate()
    {
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object _, RoutedEventArgs __)
    {
        var quotes = await this.TradeChatService.GetBuyQuotes(CancellationToken.None);
        this.traderQuotes.ClearAnd().AddRange(quotes);
        await this.SetVisibleTraderQuotes(quotes);
    }
    private void MarketUrl_MouseLeftButtonDown(object _, MouseButtonEventArgs __)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = this.MarketUrl
            }
        };

        process.Start();
    }


    private async void SearchTextBox_TextChanged(object _, string e)
    {
        var filteredQuotes = this.traderQuotes.Where(t => StringUtils.MatchesSearchString(t.Item.Name!, e));
        await this.SetVisibleTraderQuotes(filteredQuotes);
    }

    private async Task SetVisibleTraderQuotes(IEnumerable<TraderQuote> quotes)
    {
        var quotesList = quotes.ToList();
        await this.Dispatcher.InvokeAsync(() =>
        {
            var quotesToAdd = quotesList.Except(this.TraderQuotes).ToList();
            var quotesToRemove = this.TraderQuotes.Except(quotesList).ToList();
            foreach(var quote in quotesToRemove)
            {
                this.TraderQuotes.Remove(quote);
            }

            foreach (var quote in quotesToAdd)
            {
                this.TraderQuotes.Add(quote);
            }
        });
    }
}
