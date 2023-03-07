using System;
using System.Diagnostics.Metrics;

namespace Daybreak.Models.Metrics;

public readonly struct Metric
{
    public DateTime Timestamp { get; init; }
    public Instrument Instrument { get; init; }
    public object Measurement { get; init; }
}
