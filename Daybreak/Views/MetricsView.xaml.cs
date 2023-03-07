using Daybreak.Models.Metrics;
using Daybreak.Services.Metrics;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Extensions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for MetricsView.xaml
/// </summary>
public partial class MetricsView : UserControl
{
    private readonly IMetricsService metricsService;

    [GenerateDependencyProperty]
    private IEnumerable<MetricSet> metrics = new List<MetricSet>();
    
    public MetricsView(
        IMetricsService metricsService)
    {
        this.metricsService = metricsService.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.Metrics = this.metricsService.GetMetrics();
    }

    private void WpfPlot_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        var wpfPlot = sender.As<WpfPlot>();
        if (wpfPlot.DataContext is not MetricSet metricSet)
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

        var orderedMetricSet = metricSet.Metrics.OrderByDescending(m => (DateTime.UtcNow - m.Timestamp).TotalMilliseconds);
        var startTime = orderedMetricSet.First().Timestamp;
        var dataSet = orderedMetricSet.Select(m => ((m.Timestamp - startTime).TotalSeconds, m.Measurement));
        
        wpfPlot.Plot.XLabel("Timestamp (seconds)");
        wpfPlot.Plot.YLabel(metricSet.Instrument.Unit);
        switch (metricSet.AggregationType)
        {
            case AggregationTypes.NoAggregate:
                this.PlotNoAggregation(wpfPlot, dataSet);
                break;
            case AggregationTypes.P95:
                this.PlotPercentageAggregation(wpfPlot, dataSet, 0.95);
                break;
            case AggregationTypes.P98:
                this.PlotPercentageAggregation(wpfPlot, dataSet, 0.98);
                break;
            case AggregationTypes.P99:
                this.PlotPercentageAggregation(wpfPlot, dataSet, 0.99);
                break;
        }
    }

    private void PlotNoAggregation(WpfPlot wpfPlot, IEnumerable<(double TotalSeconds, object Measurement)> dataSet)
    {
        var xValues = dataSet.Select(set => set.TotalSeconds).ToArray();
        var yValues = dataSet.Select(set => double.Parse(set.Measurement.ToString()!)).ToArray();
        wpfPlot.Plot.AddFill(xValues, yValues, color: Color.Red);
        wpfPlot.Refresh();
    }

    private void PlotPercentageAggregation(WpfPlot wpfPlot, IEnumerable<(double TotalSeconds, object Measurement)> dataSet, double percentage)
    {
        var dataSetArray = dataSet.ToArray();
        var valuesToTake = Math.Round(dataSetArray.Length * percentage).ToInt();
        var orderedByValueDataSet = dataSetArray.OrderBy(s => s.Measurement);
        var finalDataSet = orderedByValueDataSet.Take(valuesToTake).OrderBy(s => s.TotalSeconds);

        this.PlotNoAggregation(wpfPlot, finalDataSet);
    }
}
