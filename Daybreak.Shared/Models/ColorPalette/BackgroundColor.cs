using System.Collections.Immutable;
using System.Drawing;

namespace Daybreak.Shared.Models.ColorPalette;
public sealed class BackgroundColor : ColorBase
{
    public static readonly BackgroundColor Gray220 = new(ColorNames.Gray220, Colors.Gray220Color, Hexes.Gray220Hex);
    public static readonly BackgroundColor Gray210 = new(ColorNames.Gray210, Colors.Gray210Color, Hexes.Gray210Hex);
    public static readonly BackgroundColor Gray200 = new(ColorNames.Gray200, Colors.Gray200Color, Hexes.Gray200Hex);
    public static readonly BackgroundColor Gray190 = new(ColorNames.Gray190, Colors.Gray190Color, Hexes.Gray190Hex);
    public static readonly BackgroundColor Gray180 = new(ColorNames.Gray180, Colors.Gray180Color, Hexes.Gray180Hex);
    public static readonly BackgroundColor Gray170 = new(ColorNames.Gray170, Colors.Gray170Color, Hexes.Gray170Hex);
    public static readonly BackgroundColor Gray160 = new(ColorNames.Gray160, Colors.Gray160Color, Hexes.Gray160Hex);
    public static readonly BackgroundColor Gray150 = new(ColorNames.Gray150, Colors.Gray150Color, Hexes.Gray150Hex);
    public static readonly BackgroundColor Gray140 = new(ColorNames.Gray140, Colors.Gray140Color, Hexes.Gray140Hex);
    public static readonly BackgroundColor Gray130 = new(ColorNames.Gray130, Colors.Gray130Color, Hexes.Gray130Hex);
    public static readonly BackgroundColor Gray120 = new(ColorNames.Gray120, Colors.Gray120Color, Hexes.Gray120Hex);
    public static readonly BackgroundColor Gray110 = new(ColorNames.Gray110, Colors.Gray110Color, Hexes.Gray110Hex);
    public static readonly BackgroundColor Gray100 = new(ColorNames.Gray100, Colors.Gray100Color, Hexes.Gray100Hex);
    public static readonly BackgroundColor Gray90 = new(ColorNames.Gray90, Colors.Gray90Color, Hexes.Gray90Hex);
    public static readonly BackgroundColor Gray80 = new(ColorNames.Gray80, Colors.Gray80Color, Hexes.Gray80Hex);
    public static readonly BackgroundColor Gray70 = new(ColorNames.Gray70, Colors.Gray70Color, Hexes.Gray70Hex);
    public static readonly BackgroundColor Gray60 = new(ColorNames.Gray60, Colors.Gray60Color, Hexes.Gray60Hex);
    public static readonly BackgroundColor Gray50 = new(ColorNames.Gray50, Colors.Gray50Color, Hexes.Gray50Hex);
    public static readonly BackgroundColor Gray40 = new(ColorNames.Gray40, Colors.Gray40Color, Hexes.Gray40Hex);
    public static readonly BackgroundColor Gray30 = new(ColorNames.Gray30, Colors.Gray30Color, Hexes.Gray30Hex);
    public static readonly BackgroundColor Gray20 = new(ColorNames.Gray20, Colors.Gray20Color, Hexes.Gray20Hex);
    public static readonly BackgroundColor Gray10 = new(ColorNames.Gray10, Colors.Gray10Color, Hexes.Gray10Hex);
    public static readonly ImmutableArray<BackgroundColor> Background = [
            Gray220,
            Gray210,
            Gray200,
            Gray190,
            Gray180,
            Gray170,
            Gray160,
            Gray150,
            Gray140,
            Gray130,
            Gray120,
            Gray110,
            Gray100,
            Gray90,
            Gray80,
            Gray70,
            Gray60,
            Gray50,
            Gray40,
            Gray30,
            Gray20,
            Gray10
        ];

    private BackgroundColor(ColorNames name, Color color, string hex) : base(name, color, hex)
    {
    }
}
