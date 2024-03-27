using Daybreak.Models.Guildwars;
using Daybreak.Models.Trade;
using Daybreak.Services.Navigation;
using Daybreak.Services.TradeChat;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Threading;
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
    }

    private void UserControl_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        this.FetchPriceHistory();
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<PriceQuotesView>();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        var xAxis = this.XAxes.FirstOrDefault();
        if (xAxis is null)
        {
            return;
        }

        xAxis.MinLimit = this.StartDateTime.Ticks;
        xAxis.MaxLimit = this.EndDateTime.Ticks;
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
        this.EndDateTime = this.traderQuotes.OrderByDescending(t => t.Timestamp).FirstOrDefault()?.Timestamp ?? DateTime.Now;
        this.StartDateTime = this.EndDateTime - TimeSpan.FromDays(1);

        this.XAxes =
        [
            new Axis
            {
                Name = "Date",
                LabelsPaint = this.foregroundPaint,
                Labeler = (ticks) => new DateTime((long)ticks).ToString("d"),
                MinLimit = this.StartDateTime.Ticks,
                MaxLimit = this.EndDateTime.Ticks,
                MinStep = TimeSpan.FromHours(1).Ticks,
            }
        ];

        this.YAxes =
        [
            new Axis
            {
                Name = "Price",
                LabelsPaint = this.foregroundPaint,
                Labeler = (price) => $"{price*20d:0}g",
            }
        ];

        this.Series =
        [
            new LineSeries<TraderQuote>
            {
                Values = this.traderQuotes,
                Fill = default,
                IsHoverable = false,
                Stroke = this.accentPaint,
                LineSmoothness = 0,
                GeometryStroke = default,
                GeometryFill = default,
                GeometrySize = default,
                Name = string.Empty,
            }
        ];

        this.Title = new LabelVisual
        {
            Text = $"{this.traderQuotes.FirstOrDefault()?.Item?.Name} Price Chart",
            TextSize = 22,
            Paint = this.foregroundPaint,
        };
    }
}
