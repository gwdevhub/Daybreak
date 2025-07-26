using Daybreak.Shared.Models.Guildwars;
using Daybreak.Shared.Models.Trade;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.TradeChat;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Views.Trade;
/// <summary>
/// Interaction logic for PriceHistoryView.xaml
/// </summary>
public partial class PriceHistoryView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IPriceHistoryService priceHistoryService;
    private readonly List<TraderQuote> traderQuotes = [];
    private readonly SolidColorPaint backgroundPaint;
    private readonly SolidColorPaint foregroundPaint;
    private readonly SolidColorPaint accentPaint;

    [GenerateDependencyProperty]
    private ISeries[] series = default!;

    [GenerateDependencyProperty]
    private LabelVisual title = default!;

    [GenerateDependencyProperty]
    private Axis[] xAxes = default!;

    [GenerateDependencyProperty]
    private Axis[] yAxes = default!;
    
    [GenerateDependencyProperty]
    private DateTime startDateTime = DateTime.MinValue;
    
    [GenerateDependencyProperty]
    private DateTime endDateTime = DateTime.MaxValue;

    [GenerateDependencyProperty]
    private Margin drawMargin = new(float.NaN, float.NaN, float.NaN, 300);

    [GenerateDependencyProperty]
    private bool loading = false;

    public PriceHistoryView(
        IViewManager viewManager,
        IPriceHistoryService priceHistoryService)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.priceHistoryService = priceHistoryService.ThrowIfNull();
        this.InitializeComponent();

        var skBackgroundBrush = this.FindResource("MahApps.Brushes.ThemeBackground").Cast<SolidColorBrush>();
        var skForegroundBrush = this.FindResource("MahApps.Brushes.ThemeForeground").Cast<SolidColorBrush>();
        var skAccentBrush = this.FindResource("MahApps.Brushes.Accent").Cast<SolidColorBrush>();
        this.foregroundPaint = new SolidColorPaint(new SKColor(skForegroundBrush.Color.R, skForegroundBrush.Color.G, skForegroundBrush.Color.B, skForegroundBrush.Color.A));
        this.accentPaint = new SolidColorPaint(new SKColor(skAccentBrush.Color.R, skAccentBrush.Color.G, skAccentBrush.Color.B, skAccentBrush.Color.A), 2);
        this.backgroundPaint = new SolidColorPaint(new SKColor(skBackgroundBrush.Color.R, skBackgroundBrush.Color.G, skBackgroundBrush.Color.B, skBackgroundBrush.Color.A));
        this.PopulateChart();
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        this.FetchPriceHistory();
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<PriceQuotesView>();
    }

    private async void FetchPriceHistory()
    {
        if (this.DataContext is not ItemBase itemBase)
        {
            return;
        }

        this.Loading = true;
        var priceHistory = await this.priceHistoryService.GetPriceHistory(itemBase, CancellationToken.None).ConfigureAwait(false);
        this.Dispatcher.Invoke(() => {
            this.traderQuotes.ClearAnd().AddRange(priceHistory);
            if (this.traderQuotes.Count >= 1)
            {
                var lastQuote = this.traderQuotes.LastOrDefault() ?? throw new InvalidOperationException();
                this.traderQuotes.Add(new TraderQuote
                {
                    Item = lastQuote.Item,
                    Price = lastQuote.Price,
                    Timestamp = DateTime.Now,
                });
            }

            this.PopulateChart();
            this.Loading = false;
        });
    }

    private void PopulateChart()
    {
        this.XAxes =
        [
            new DateTimeAxis(TimeSpan.FromHours(1), dateTime => dateTime.ToString("d"))
            {
                Name = "Date",
                LabelsPaint = this.foregroundPaint,
                NameTextSize = 16,
                NamePaint = this.foregroundPaint,
                InLineNamePlacement = true
            }
        ];

        this.YAxes =
        [
            new Axis
            {
                Name = "Price",
                NameTextSize = 16,
                NamePadding = new Padding(0, 15),
                NamePaint = this.foregroundPaint,
                LabelsPaint = this.foregroundPaint,
                Labeler = Labelers.Currency,
                InLineNamePlacement = true
            }
        ];

        this.Series =
        [
            new StepLineSeries<DateTimePoint>
            {
                Values = [.. ProcessQuotes(this.traderQuotes).Select(t => new DateTimePoint(t.Timestamp ?? DateTime.MinValue, t.Price))],
                Stroke = this.accentPaint,
                Name = "Historical Price",
                GeometryStroke = default,
                GeometryFill = default,
                GeometrySize = default,
            }
        ];

        this.Title = new LabelVisual
        {
            Text = $"{this.traderQuotes.FirstOrDefault()?.Item?.Name} Price Chart",
            TextSize = 22,
            Paint = this.foregroundPaint
        };
    }

    private static IEnumerable<TraderQuote> ProcessQuotes(IEnumerable<TraderQuote> quotes)
    {
        var lastPricePoint = double.MinValue;
        foreach(var quote in quotes.OrderBy(p => p.Timestamp))
        {
            if (quote.Price != lastPricePoint)
            {
                lastPricePoint = quote.Price;
                yield return quote;
            }
        }
    }
}
