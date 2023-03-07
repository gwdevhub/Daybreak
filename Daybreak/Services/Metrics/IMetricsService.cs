using System.Diagnostics.Metrics;
using System;
using Daybreak.Models.Metrics;
using System.Collections.Generic;

namespace Daybreak.Services.Metrics;

public interface IMetricsService
{
    public ObservableGauge<T> CreateObservableGauge<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory) where T : struct;

    public ObservableCounter<T> CreateObservableCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory) where T : struct;

    public Counter<T> CreateCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType) where T : struct;
    
    public Histogram<T> CreateHistogram<T>(string name, string unitName, string description, AggregationTypes aggregationType) where T : struct;

    public MetricSet GetMetrics(string name);

    public IEnumerable<MetricSet> GetMetrics();
}
