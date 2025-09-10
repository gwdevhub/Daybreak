using Daybreak.Shared.Models.Metrics;
using Daybreak.Shared.Services.Metrics;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class MetricsViewModel(IMetricsService metricsService)
    : ViewModelBase<MetricsViewModel, MetricsView>
{
    private readonly IMetricsService metricsService = metricsService;

    public List<MetricSetViewModel> Metrics { get; } = [];

    public override async ValueTask Initialize(CancellationToken cancellationToken)
    {
        this.Metrics.ClearAnd().AddRange(this.metricsService.GetMetrics().Select(ConvertToViewModel));
        this.metricsService.SetRecorded += this.MetricsService_SetRecorded;
        this.metricsService.MetricRecorded += this.MetricsService_MetricRecorded;
        
        await base.Initialize(cancellationToken);
        _ = Task.Factory.StartNew(async () =>
        {
            await Task.Delay(3000);
            await this.InitializeCharts();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.metricsService.SetRecorded -= this.MetricsService_SetRecorded;
            this.metricsService.MetricRecorded -= this.MetricsService_MetricRecorded;
            
            // Cleanup charts
            foreach (var metricSet in this.Metrics)
            {
                try
                {
                    _ = this.View?.DestroyChart(metricSet);
                }
                catch
                {
                }
            }
        }

        base.Dispose(disposing);
    }

    private async ValueTask InitializeCharts()
    {
        if (this.View is null)
        {
            return;
        }

        foreach (var metricSet in this.Metrics)
        {
            await this.View.CreateChart(metricSet, ApplyAggregation);
        }
    }

    private async void MetricsService_MetricRecorded(object? sender, RecordedMetric e)
    {
        var metricSet = this.Metrics.FirstOrDefault(x => x.Instrument == e.Instrument);
        if (metricSet is null)
        {
            return;
        }

        if (this.View is null)
        {
            return;
        }

        metricSet.Metrics ??= [];
        metricSet.Metrics.Add(e.Metric);
        await this.View.AddDataPoint(metricSet, e.Metric);
        await this.RefreshViewAsync();
    }

    private async void MetricsService_SetRecorded(object? sender, MetricSet e)
    {
        var viewModel = ConvertToViewModel(e);
        this.Metrics.Add(viewModel);

        if (this.View is null)
        {
            return;
        }

        await this.View.CreateChart(viewModel, ApplyAggregation);
        await this.RefreshViewAsync();
    }

    private static IEnumerable<Metric> ApplyAggregation(MetricSetViewModel metricSet)
    {
        return metricSet.AggregationType switch
        {
            AggregationTypes.NoAggregate => PlotNoAggregation(metricSet.Metrics),
            AggregationTypes.P95 => PlotPercentageAggregation(metricSet.Metrics, 0.95),
            AggregationTypes.P98 => PlotPercentageAggregation(metricSet.Metrics, 0.98),
            AggregationTypes.P99 => PlotPercentageAggregation(metricSet.Metrics, 0.99),
            _ => PlotNoAggregation(metricSet.Metrics)
        };
    }

    private static IEnumerable<Metric> PlotNoAggregation(IEnumerable<Metric>? dataSet)
    {
        return dataSet ?? [];
    }

    private static IEnumerable<Metric> PlotPercentageAggregation(IEnumerable<Metric>? dataSet, double percentage)
    {
        var dataSetArray = dataSet?.ToArray() ?? [];
        var valuesToTake = Math.Round(dataSetArray.Length * percentage).ToInt();
        var orderedByValueDataSet = dataSetArray.OrderBy(s => s.Measurement);
        var finalDataSet = orderedByValueDataSet.Take(valuesToTake).OrderBy(s => s.Timestamp);

        return [.. finalDataSet];
    }

    private static MetricSetViewModel ConvertToViewModel(MetricSet set) => new() { Instrument = set.Instrument, AggregationType = set.AggregationType, Metrics = [.. set.Metrics ?? []] };
}
