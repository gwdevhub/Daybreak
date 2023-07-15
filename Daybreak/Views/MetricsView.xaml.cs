using Daybreak.Models.Metrics;
using Daybreak.Services.Metrics;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WPF;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Data;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for MetricsView.xaml
/// </summary>
public partial class MetricsView : UserControl
{
    private readonly SolidColorPaint backgroundPaint;
    private readonly SolidColorPaint foregroundPaint;
    private readonly SolidColorPaint accentPaint;
    private readonly IMetricsService metricsService;

    public ObservableCollection<MetricSetViewModel> Metrics { get; } = new ObservableCollection<MetricSetViewModel>();
    
    public MetricsView(
        IMetricsService metricsService)
    {
        this.metricsService = metricsService.ThrowIfNull();

        this.InitializeComponent();

        var skBackgroundBrush = this.FindResource("MahApps.Brushes.ThemeBackground").Cast<SolidColorBrush>();
        var skForegroundBrush = this.FindResource("MahApps.Brushes.ThemeForeground").Cast<SolidColorBrush>();
        var skAccentBrush = this.FindResource("MahApps.Brushes.Accent").Cast<SolidColorBrush>();
        this.foregroundPaint = new SolidColorPaint(new SKColor(skForegroundBrush.Color.R, skForegroundBrush.Color.G, skForegroundBrush.Color.B, skForegroundBrush.Color.A));
        this.accentPaint = new SolidColorPaint(new SKColor(skAccentBrush.Color.R, skAccentBrush.Color.G, skAccentBrush.Color.B, skAccentBrush.Color.A), 2);
        this.backgroundPaint = new SolidColorPaint(new SKColor(skBackgroundBrush.Color.R, skBackgroundBrush.Color.G, skBackgroundBrush.Color.B, skBackgroundBrush.Color.A));
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.Metrics.AddRange(this.metricsService.GetMetrics().Select(s => new MetricSetViewModel { Instrument = s.Instrument, AggregationType = s.AggregationType, Metrics = new ObservableCollection<Metric>(s.Metrics ?? new List<Metric>()) }).ToList());
        this.metricsService.SetRecorded += this.MetricsService_SetRecorded;
        this.metricsService.MetricRecorded += this.MetricsService_MetricRecorded;
    }

    private void MetricsService_MetricRecorded(object? sender, RecordedMetric e)
    {
        this.Dispatcher.Invoke(() =>
        {
            if (this.Metrics.FirstOrDefault(m => m.Instrument == e.Instrument) is not MetricSetViewModel existingSet)
            {
                return;
            }

            existingSet.Metrics?.Add(e.Metric);
        });
    }

    private void MetricsService_SetRecorded(object? sender, MetricSet e)
    {
        this.Dispatcher.Invoke(() =>
        {
            this.Metrics.Add(new MetricSetViewModel { AggregationType = e.AggregationType, Instrument = e.Instrument, Metrics = new ObservableCollection<Metric>(e.Metrics ?? new List<Metric>()) });
        });
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        this.metricsService.MetricRecorded -= this.MetricsService_MetricRecorded;
        this.metricsService.SetRecorded -= this.MetricsService_SetRecorded;
    }

    private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
    {
        var cartesianChart = sender.Cast<CartesianChart>();
        if (cartesianChart.DataContext is not MetricSetViewModel metricSet)
        {
            return;
        }

        if (metricSet.Instrument is null ||
            metricSet.Metrics is null ||
            metricSet.Metrics.None())
        {
            return;
        }

        // If the value cannot be parsed to double, ignore the metrics for now.
        if (double.TryParse(metricSet.Metrics.First().Measurement.ToString(), out _) is false)
        {
            return;
        }

        cartesianChart.XAxes = new Axis[]
        {
            new Axis
            {
                Name = "Time",
                LabelsPaint = this.foregroundPaint,
                Labeler = (ticks) => new DateTime((long)ticks).ToString("HH:mm:ss")
            }
        };

        cartesianChart.YAxes = new Axis[]
        {
            new Axis
            {
                Name = metricSet.Instrument.Unit,
                LabelsPaint = this.foregroundPaint,
            }
        };

        cartesianChart.Series = new ISeries[]
        {
            new LineSeries<Metric>
            {
                Values = metricSet.AggregationType switch
                {
                    AggregationTypes.NoAggregate => this.PlotNoAggregation(metricSet.Metrics),
                    AggregationTypes.P95 => this.PlotPercentageAggregation(metricSet.Metrics, 0.95),
                    AggregationTypes.P98 => this.PlotPercentageAggregation(metricSet.Metrics, 0.98),
                    AggregationTypes.P99 => this.PlotPercentageAggregation(metricSet.Metrics, 0.99),
                    _ => throw new InvalidOperationException("Unable to plot metrics. Unknown aggregation")
                },
                Fill = default,
                IsHoverable = false,
                Stroke = this.accentPaint,
                LineSmoothness = 0,
                GeometryStroke = default,
                GeometryFill = default,
                GeometrySize = default,
                Name = string.Empty
            }
        };

        cartesianChart.Title = new LabelVisual
        {
            Text = metricSet.Instrument.Name,
            TextSize = 22,
            Paint = this.foregroundPaint
        };
    }

    private IEnumerable<Metric> PlotNoAggregation(IEnumerable<Metric> dataSet)
    {
        return dataSet;
    }

    private IEnumerable<Metric> PlotPercentageAggregation(IEnumerable<Metric> dataSet, double percentage)
    {
        var dataSetArray = dataSet.ToArray();
        var valuesToTake = Math.Round(dataSetArray.Length * percentage).ToInt();
        var orderedByValueDataSet = dataSetArray.OrderBy(s => s.Measurement);
        var finalDataSet = orderedByValueDataSet.Take(valuesToTake).OrderBy(s => s.Timestamp);

        return finalDataSet;
    }

    private void InterceptScroll(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        e.Handled = true;
    }
}
