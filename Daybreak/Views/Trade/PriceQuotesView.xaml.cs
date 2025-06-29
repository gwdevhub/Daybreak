using Daybreak.Controls.Templates;
using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.TradeChat;
using Daybreak.Shared.Utils;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
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
            var sellQuotes = await this.traderQuoteService.GetSellQuotes(CancellationToken.None);
            var buyQuotes = await this.traderQuoteService.GetBuyQuotes(CancellationToken.None);
            var concatQuotes = new Dictionary<string, TraderQuoteModel>();
            InsertQuotes(concatQuotes, buyQuotes, false);
            InsertQuotes(concatQuotes, sellQuotes, true);
            var orderedQuotes = concatQuotes.Values.OrderBy(q => q.Item!.Name).OrderBy(q => q.Item is not Material).ToList();
            this.traderQuotesCache = [.. orderedQuotes];
            await this.Dispatcher.InvokeAsync(() =>
            {
                this.TraderQuotes.ClearAnd().AddRange(this.traderQuotesCache);
                this.Loading = false;
            });
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
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
            element.DataContext is not TraderQuoteModel traderQuote ||
            traderQuote.Item is not ItemBase item)
        {
            return;
        }

        this.viewManager.ShowView<PriceHistoryView>(item);
    }

    private static void InsertQuotes(Dictionary<string, TraderQuoteModel> concat, IEnumerable<TraderQuote> items, bool isSellPrice)
    {
        foreach(var quote in items)
        {
            if(quote.Item is null)
            {
                continue;
            }

            var index = $"{quote.Item.Id}-{quote.Item.As<IItemModHash>()?.ModHash}";
            if (!concat.TryGetValue(index, out var quoteModel))
            {
                quoteModel = new TraderQuoteModel { Item = quote.Item, TimeStamp = quote.Timestamp ?? DateTime.UtcNow, SellPrice = quote.Price, BuyPrice = quote.Price };
                concat[index] = quoteModel;
                
            }

            if (isSellPrice)
            {
                quoteModel.SellPrice = quote.Price;
            }
            else
            {
                quoteModel.BuyPrice = quote.Price;
            }
        }
    }
}
