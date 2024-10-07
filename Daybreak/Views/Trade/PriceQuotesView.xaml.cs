using Daybreak.Controls.Templates;
using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for PriceQuotesView.xaml
/// </summary>
public partial class PriceQuotesView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly ITraderQuoteService traderQuoteService;

    private List<TraderQuoteModel> traderQuotesCache = [];

    [GenerateDependencyProperty]
    private bool loading = false;

    public ObservableCollection<TraderQuoteModel> TraderQuotes { get; } = [];

    public PriceQuotesView(
        IViewManager viewManager,
        ITraderQuoteService traderQuoteService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.traderQuoteService = traderQuoteService.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.Loading = true;
        await new TaskFactory().StartNew(async () =>
        {
            var sellQuotes = (await this.traderQuoteService.GetSellQuotes(CancellationToken.None)).OrderBy(q => q.Item!.Name).OrderBy(q => q.Item is Rune);
            var buyQuotes = (await this.traderQuoteService.GetBuyQuotes(CancellationToken.None)).OrderBy(q => q.Item!.Name).OrderBy(q => q.Item is Rune);
            var quotes = sellQuotes.Select(q =>
            {
                if (buyQuotes.FirstOrDefault(b => b.Item?.Id == q.Item?.Id) is TraderQuote buyQuote)
                {
                    return new TraderQuoteModel
                    {
                        Item = q.Item,
                        BuyPrice = buyQuote.Price,
                        SellPrice = q.Price,
                        TimeStamp = q.Timestamp ?? buyQuote.Timestamp ?? DateTime.UtcNow
                    };
                }

                return new TraderQuoteModel
                {
                    Item = q.Item,
                    BuyPrice = q.Price,
                    SellPrice = q.Price,
                    TimeStamp = q.Timestamp ?? DateTime.UtcNow
                };
            });
            this.traderQuotesCache = [.. quotes];
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.TraderQuotes.ClearAnd().AddRange(this.traderQuotesCache);
            this.Loading = false;
        });
    }

    private void SearchTextBox_TextChanged(object _, string e)
    {
        var filteredItems = this.traderQuotesCache.Where(t => 
            e.IsNullOrWhiteSpace() || StringUtils.MatchesSearchString(t.Item!.Name!, e)).ToList().OrderBy(q => q.Item!.Name).OrderBy(q => q.Item is Rune);
        this.TraderQuotes.ClearAnd().AddRange(filteredItems);
    }

    private void HighlightButton_Clicked(object sender, EventArgs _)
    {
        if (sender is not FrameworkElement element ||
            element.DataContext is not TraderQuote traderQuote ||
            traderQuote.Item is not ItemBase item)
        {
            return;
        }

        this.viewManager.ShowView<PriceHistoryView>(item);
    }
}
