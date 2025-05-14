using System.Diagnostics.Metrics;

namespace Daybreak.Shared.Models.Metrics;

public sealed class RecordedMetric
{
    public Metric Metric { get; init; }
    public Instrument? Instrument { get; init; }
}
