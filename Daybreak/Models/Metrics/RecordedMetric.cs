using System.Diagnostics.Metrics;

namespace Daybreak.Models.Metrics;

public sealed class RecordedMetric
{
    public Metric Metric { get; init; }
    public Instrument? Instrument { get; init; }
}
