using System.Drawing;

namespace Daybreak.Shared.Models.ColorPalette;
public static class Colors
{
    public static readonly Color Gray220Color = ConvertHexToColor(Hexes.Gray220Hex);
    public static readonly Color Gray210Color = ConvertHexToColor(Hexes.Gray210Hex);
    public static readonly Color Gray200Color = ConvertHexToColor(Hexes.Gray200Hex);
    public static readonly Color Gray190Color = ConvertHexToColor(Hexes.Gray190Hex);
    public static readonly Color Gray180Color = ConvertHexToColor(Hexes.Gray180Hex);
    public static readonly Color Gray170Color = ConvertHexToColor(Hexes.Gray170Hex);
    public static readonly Color Gray160Color = ConvertHexToColor(Hexes.Gray160Hex);
    public static readonly Color Gray150Color = ConvertHexToColor(Hexes.Gray150Hex);
    public static readonly Color Gray140Color = ConvertHexToColor(Hexes.Gray140Hex);
    public static readonly Color Gray130Color = ConvertHexToColor(Hexes.Gray130Hex);
    public static readonly Color Gray120Color = ConvertHexToColor(Hexes.Gray120Hex);
    public static readonly Color Gray110Color = ConvertHexToColor(Hexes.Gray110Hex);
    public static readonly Color Gray100Color = ConvertHexToColor(Hexes.Gray100Hex);
    public static readonly Color Gray90Color = ConvertHexToColor(Hexes.Gray90Hex);
    public static readonly Color Gray80Color = ConvertHexToColor(Hexes.Gray80Hex);
    public static readonly Color Gray70Color = ConvertHexToColor(Hexes.Gray70Hex);
    public static readonly Color Gray60Color = ConvertHexToColor(Hexes.Gray60Hex);
    public static readonly Color Gray50Color = ConvertHexToColor(Hexes.Gray50Hex);
    public static readonly Color Gray40Color = ConvertHexToColor(Hexes.Gray40Hex);
    public static readonly Color Gray30Color = ConvertHexToColor(Hexes.Gray30Hex);
    public static readonly Color Gray20Color = ConvertHexToColor(Hexes.Gray20Hex);
    public static readonly Color Gray10Color = ConvertHexToColor(Hexes.Gray10Hex);

    public static readonly Color YellowColor = ConvertHexToColor(Hexes.YellowHex);
    public static readonly Color LightYellowColor = ConvertHexToColor(Hexes.LightYellowHex);
    public static readonly Color OrangeColor = ConvertHexToColor(Hexes.OrangeHex);
    public static readonly Color OrangeLightColor = ConvertHexToColor(Hexes.OrangeLightHex);
    public static readonly Color OrangeLighterColor = ConvertHexToColor(Hexes.OrangeLighterHex);
    public static readonly Color DarkRedColor = ConvertHexToColor(Hexes.DarkRedHex);
    public static readonly Color RedColor = ConvertHexToColor(Hexes.RedHex);
    public static readonly Color MagentaLightColor = ConvertHexToColor(Hexes.MagentaLightHex);
    public static readonly Color MagentaColor = ConvertHexToColor(Hexes.MagentaHex);
    public static readonly Color DarkMagentaColor = ConvertHexToColor(Hexes.DarkMagentaHex);
    public static readonly Color LightPurpleColor = ConvertHexToColor(Hexes.LightPurpleHex);
    public static readonly Color PurpleColor = ConvertHexToColor(Hexes.PurpleHex);
    public static readonly Color DarkPurpleColor = ConvertHexToColor(Hexes.DarkPurpleHex);
    public static readonly Color LightBlueColor = ConvertHexToColor(Hexes.LightBlueHex);
    public static readonly Color MidBlueColor = ConvertHexToColor(Hexes.MidBlueHex);
    public static readonly Color BlueColor = ConvertHexToColor(Hexes.BlueHex);
    public static readonly Color DarkBlueColor = ConvertHexToColor(Hexes.DarkBlueHex);
    public static readonly Color LightTealColor = ConvertHexToColor(Hexes.LightTealHex);
    public static readonly Color TealColor = ConvertHexToColor(Hexes.TealHex);
    public static readonly Color DarkTealColor = ConvertHexToColor(Hexes.DarkTealHex);
    public static readonly Color LightGreenColor = ConvertHexToColor(Hexes.LightGreenHex);
    public static readonly Color GreenColor = ConvertHexToColor(Hexes.GreenHex);
    public static readonly Color DarkGreenColor = ConvertHexToColor(Hexes.DarkGreenHex);

    private static Color ConvertHexToColor(string hex)
    {
        hex = hex.TrimStart('#');

        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);

        return Color.FromArgb(1, r, g, b);
    }
}
