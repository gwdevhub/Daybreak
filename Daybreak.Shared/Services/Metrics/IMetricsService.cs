using System.Diagnostics.Metrics;
using System;
using System.Collections.Generic;
using Daybreak.Shared.Models.Metrics;

namespace Daybreak.Shared.Services.Metrics;

public interface IMetricsService
{
    event EventHandler<RecordedMetric>? MetricRecorded;

    event EventHandler<MetricSet>? SetRecorded;

    ObservableGauge<T> CreateObservableGauge<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory) where T : struct;

    ObservableCounter<T> CreateObservableCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory) where T : struct;

    Counter<T> CreateCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType) where T : struct;
    
    Histogram<T> CreateHistogram<T>(string name, string unitName, string description, AggregationTypes aggregationType) where T : struct;

    MetricSet GetMetrics(string name);

    IEnumerable<MetricSet> GetMetrics();
}
