using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace Daybreak.Models.Metrics;

public sealed class MetricSetViewModel
{
    public Instrument? Instrument { get; set; }
    public AggregationTypes AggregationType { get; set; }
    public ObservableCollection<Metric>? Metrics { get; set; }
}
