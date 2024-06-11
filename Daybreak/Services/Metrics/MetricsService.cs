using Daybreak.Models.Metrics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Threading;

namespace Daybreak.Services.Metrics;

internal sealed class MetricsService : IMetricsService, IDisposable
{
    private const int MetricsStoreLimit = 10000;
    private const string MetricsNamespace = "Daybreak";
    private const string MetricsStoreName = "MetricsStore";

    private static readonly SemaphoreSlim MetricsSemaphore = new(1, 1);

    private readonly MeterListener meterListener;
    private readonly Meter daybreakMeter;
    private readonly ConcurrentDictionary<string, LinkedList<Metric>> metricStore = new();
    private readonly ConcurrentDictionary<Instrument, AggregationTypes> aggregationTypeMapping = new();

    public event EventHandler<RecordedMetric>? MetricRecorded;
    public event EventHandler<MetricSet>? SetRecorded;

    public MetricsService()
    {
        this.daybreakMeter = new Meter($"{MetricsNamespace}.{MetricsStoreName}");
        this.meterListener = new MeterListener
        {
            InstrumentPublished = (instrument, listener) =>
            {
                if (instrument.Meter.Name == this.daybreakMeter.Name)
                {
                    listener.EnableMeasurementEvents(instrument);
                }
            }
        };

        this.meterListener.Start();
    }

    public ObservableGauge<T> CreateObservableGauge<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory)
        where T : struct
    {
        this.meterListener.SetMeasurementEventCallback<T>(this.MeasurementRecorded);
        var instrument = this.daybreakMeter.CreateObservableGauge<T>(name, valueFactory, unitName, description);
        this.aggregationTypeMapping[instrument] = aggregationType;
        return instrument;
    }

    public ObservableCounter<T> CreateObservableCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType, Func<T> valueFactory)
        where T : struct
    {
        this.meterListener.SetMeasurementEventCallback<T>(this.MeasurementRecorded);
        var instrument = this.daybreakMeter.CreateObservableCounter<T>(name, valueFactory, unitName, description);
        this.aggregationTypeMapping[instrument] = aggregationType;
        return instrument;
    }

    public Counter<T> CreateCounter<T>(string name, string unitName, string description, AggregationTypes aggregationType)
        where T : struct
    {
        this.meterListener.SetMeasurementEventCallback<T>(this.MeasurementRecorded);
        var instrument = this.daybreakMeter.CreateCounter<T>(name, unitName, description);
        this.aggregationTypeMapping[instrument] = aggregationType;
        return instrument;
    }

    public Histogram<T> CreateHistogram<T>(string name, string unitName, string description, AggregationTypes aggregationType)
        where T : struct
    {
        this.meterListener.SetMeasurementEventCallback<T>(this.MeasurementRecorded);
        var instrument = this.daybreakMeter.CreateHistogram<T>(name, unitName, description);
        this.aggregationTypeMapping[instrument] = aggregationType;
        return instrument;
    }

    public MetricSet GetMetrics(string name)
    {
        if (this.metricStore.TryGetValue(name, out var metrics) is false)
        {
            throw new InvalidOperationException($"Could not find any metrics with name {name}");
        }

        var instrument = metrics.First?.Value.Instrument;
        if (instrument is null)
        {
            throw new InvalidOperationException($"Could not identify the instrument producing the desired metrics with name {name}");
        }

        this.aggregationTypeMapping.TryGetValue(instrument!, out var aggregationType);

        return new MetricSet { Instrument = instrument, Metrics = metrics.ToImmutableList(), AggregationType = aggregationType };
    }

    public IEnumerable<MetricSet> GetMetrics()
    {
        var retList = new List<MetricSet>();
        foreach(var value in this.metricStore.Values)
        {
            if (value.First is null)
            {
                continue;
            }

            var instrument = value.First.Value.Instrument;
            this.aggregationTypeMapping.TryGetValue(instrument, out var aggregationType);
            retList.Add(new MetricSet { Instrument = instrument, Metrics = value.ToImmutableList(), AggregationType = aggregationType });
        }

        return retList;
    }

    private void MeasurementRecorded<T>(Instrument instrument, T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        MetricSet? newMetricSet = default;
        RecordedMetric? newRecordedMetric = default;
        MetricsSemaphore.Wait();

        if (measurement is null)
        {
            return;
        }

        if (this.metricStore.TryGetValue(instrument.Name, out var metrics) is false)
        {
            metrics = new LinkedList<Metric>();
            this.metricStore[instrument.Name] = metrics;
            newMetricSet = new MetricSet { Metrics = metrics, AggregationType = this.aggregationTypeMapping[instrument], Instrument = instrument };
            
        }

        var metric = new Metric { Timestamp = DateTime.UtcNow, Instrument = instrument, Measurement = measurement };
        metrics.AddLast(metric);
        if (metrics.Count > MetricsStoreLimit)
        {
            metrics.RemoveFirst();
        }

        newRecordedMetric = new RecordedMetric { Instrument = instrument, Metric = metric };
        MetricsSemaphore.Release();

        if (newMetricSet is not null)
        {
            this.SetRecorded?.Invoke(this, newMetricSet);
        }

        if (newRecordedMetric is not null)
        {
            this.MetricRecorded?.Invoke(this, newRecordedMetric);
        }
    }

    public void Dispose()
    {
        this.meterListener?.Dispose();
    }
}
