using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Screen Manager")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
public sealed class ScreenManagerOptions
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}
