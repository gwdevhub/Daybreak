using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsName(Name = "Minimap Window")]
[OptionsSynchronizationIgnore]
public sealed class MinimapWindowOptions
{
    public bool Pinned { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double DpiX { get; set; }
    public double DpiY { get; set; }
}
