using Daybreak.Attributes;
using Newtonsoft.Json;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Memory Reader")]
public sealed class MemoryReaderOptions
{
    [JsonProperty(nameof(MemoryReaderFrequency))]
    [OptionRange<double>(MinValue = 0, MaxValue = 1000)]
    [OptionName(Name = "Memory Reader Frequency", Description = "Measured in ms. Sets how often should the launcher polls information from the game. Actual frequency is capped by the memory reading speed")]
    public double MemoryReaderFrequency { get; set; } = 0;
}
