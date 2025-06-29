using System.Diagnostics.Metrics;

namespace Daybreak.Shared.Models.Metrics;

public readonly struct Metric
{
    public DateTime Timestamp { get; init; }
    public Instrument Instrument { get; init; }
    public object Measurement { get; init; }
}
