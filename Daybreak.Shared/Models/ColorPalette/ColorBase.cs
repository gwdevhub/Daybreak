using System.Drawing;

namespace Daybreak.Shared.Models.ColorPalette;
public abstract class ColorBase(ColorNames name, Color color, string hex)
{
    public ColorNames Name { get; init; } = name;
    public Color Color { get; init; } = color;
    public string Hex { get; init; } = hex;
}
