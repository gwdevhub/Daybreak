using System.Diagnostics.Metrics;

namespace Daybreak.Shared.Models.Metrics;

public sealed class MetricSet
{
    public AggregationTypes AggregationType { get; init; }

    public Instrument? Instrument { get; init; }

    public IEnumerable<Metric>? Metrics { get; init; }
}
