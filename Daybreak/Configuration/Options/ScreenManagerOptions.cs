using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Screen Manager")]
public sealed class ScreenManagerOptions
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double DpiX { get; set; }
    public double DpiY { get; set; }
}
