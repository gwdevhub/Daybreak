namespace Daybreak.Shared.Models.Themes;

public sealed class StaticBackground(string backdropImage)
    : IAppBackground
{
    public string BackdropImage { get; } = backdropImage;
}
