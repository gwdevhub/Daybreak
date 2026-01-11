using Daybreak.Shared.Attributes;

namespace SimplePlugin.Options;

[OptionsName(Name = "Simple Plugin Options")]
public sealed class SimpleOptions
{
    [OptionName(Name = "Enable Feature X", Description = "Enables or disables Feature X in the Simple Plugin (does nothing but looks cool)")]
    public bool EnableFeatureX { get; set; } = true;
    
    [OptionName(Name = "Things Count", Description = "Specifies the number of things to process (just a number for demonstration)")]
    [OptionRange<int>(MinValue = 0, MaxValue = 100)]
    public int ThingsCount { get; set; }
}
