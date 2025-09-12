using System.Collections.Immutable;
using System.Drawing;

namespace Daybreak.Shared.Models.ColorPalette;

public sealed class AccentColor : ColorBase
{
    public static readonly AccentColor Yellow = new(ColorNames.Yellow, Colors.YellowColor, Hexes.YellowHex);
    public static readonly AccentColor LightYellow = new(ColorNames.LightYellow, Colors.LightYellowColor, Hexes.LightYellowHex);
    public static readonly AccentColor Orange = new(ColorNames.Orange, Colors.OrangeColor, Hexes.OrangeHex);
    public static readonly AccentColor OrangeLight = new(ColorNames.OrangeLight, Colors.OrangeLightColor, Hexes.OrangeLightHex);
    public static readonly AccentColor OrangeLighter = new(ColorNames.OrangeLighter, Colors.OrangeLighterColor, Hexes.OrangeLighterHex);
    public static readonly AccentColor DarkRed = new(ColorNames.DarkRed, Colors.DarkRedColor, Hexes.DarkRedHex);
    public static readonly AccentColor Red = new(ColorNames.Red, Colors.RedColor, Hexes.RedHex);
    public static readonly AccentColor MagentaLight = new(ColorNames.MagentaLight, Colors.MagentaLightColor, Hexes.MagentaLightHex);
    public static readonly AccentColor Magenta = new(ColorNames.Magenta, Colors.MagentaColor, Hexes.MagentaHex);
    public static readonly AccentColor DarkMagenta = new(ColorNames.DarkMagenta, Colors.DarkMagentaColor, Hexes.DarkMagentaHex);
    public static readonly AccentColor LightPurple = new(ColorNames.LightPurple, Colors.LightPurpleColor, Hexes.LightPurpleHex);
    public static readonly AccentColor Purple = new(ColorNames.Purple, Colors.PurpleColor, Hexes.PurpleHex);
    public static readonly AccentColor DarkPurple = new(ColorNames.DarkPurple, Colors.DarkPurpleColor, Hexes.DarkPurpleHex);
    public static readonly AccentColor LightBlue = new(ColorNames.LightBlue, Colors.LightBlueColor, Hexes.LightBlueHex);
    public static readonly AccentColor MidBlue = new(ColorNames.MidBlue, Colors.MidBlueColor, Hexes.MidBlueHex);
    public static readonly AccentColor Blue = new(ColorNames.Blue, Colors.BlueColor, Hexes.BlueHex);
    public static readonly AccentColor DarkBlue = new(ColorNames.DarkBlue, Colors.DarkBlueColor, Hexes.DarkBlueHex);
    public static readonly AccentColor LightTeal = new(ColorNames.LightTeal, Colors.LightTealColor, Hexes.LightTealHex);
    public static readonly AccentColor Teal = new(ColorNames.Teal, Colors.TealColor, Hexes.TealHex);
    public static readonly AccentColor DarkTeal = new(ColorNames.DarkTeal, Colors.DarkTealColor, Hexes.DarkTealHex);
    public static readonly AccentColor LightGreen = new(ColorNames.LightGreen, Colors.LightGreenColor, Hexes.LightGreenHex);
    public static readonly AccentColor Green = new(ColorNames.Green, Colors.GreenColor, Hexes.GreenHex);
    public static readonly AccentColor DarkGreen = new(ColorNames.DarkGreen, Colors.DarkGreenColor, Hexes.DarkGreenHex);
    public static readonly AccentColor LightGray = new(ColorNames.Gray30, Colors.Gray30Color, Hexes.Gray30Hex);
    public static readonly ImmutableArray<AccentColor> Accents = [
            Yellow,
            LightYellow,
            Orange,
            OrangeLight,
            OrangeLighter,
            DarkRed,
            Red,
            MagentaLight,
            Magenta,
            DarkMagenta,
            LightPurple,
            Purple,
            DarkPurple,
            LightBlue,
            MidBlue,
            Blue,
            DarkBlue,
            LightTeal,
            Teal,
            DarkTeal,
            LightGreen,
            Green,
            DarkGreen
        ];

    private AccentColor(ColorNames name, Color color, string hex) : base(name, color, hex)
    {
    }
}
