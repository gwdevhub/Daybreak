using System.Windows.Media;

namespace Daybreak.Shared.Models;
/// <summary>
/// https://blog.lsonline.fr/fluent-ui-core-color/
/// </summary>
public class ColorPalette
{
    public const string Black = "#000000";
    public const string Gray220Hex = "#11100f";
    public const string Gray210Hex = "#161514";
    public const string Gray200Hex = "#1b1a19";
    public const string Gray190Hex = "#201f1e";
    public const string Gray180Hex = "#252423";
    public const string Gray170Hex = "#292827";
    public const string Gray160Hex = "#323130";
    public const string Gray150Hex = "#3b3a39";
    public const string Gray140Hex = "#484644";
    public const string Gray130Hex = "#605e5c";
    public const string Gray120Hex = "#797775";
    public const string Gray110Hex = "#8a8886";
    public const string Gray100Hex = "#979593";
    public const string Gray90Hex = "#a19f9d";
    public const string Gray80Hex = "#b3b0ad";
    public const string Gray70Hex = "#bebbb8";
    public const string Gray60Hex = "#c8c6c4";
    public const string Gray50Hex = "#d2d0ce";
    public const string Gray40Hex = "#e1dfdd";
    public const string Gray30Hex = "#edebe9";
    public const string Gray20Hex = "#f3f2f1";
    public const string Gray10Hex = "#faf9f8";

    public const string YellowHex = "#FFB900";
    public const string LightYellowHex = "#FFF100";
    public const string OrangeHex = "#D83B01";
    public const string OrangeLightHex = "#EA4300";
    public const string OrangeLighterHex = "#FF8C00";
    public const string DarkRedHex = "#A80000";
    public const string RedHex = "#E81123";
    public const string MagentaLightHex = "#E3008C";
    public const string MagentaHex = "#B4009E";
    public const string DarkMagentaHex = "#5C005C";
    public const string LightPurpleHex = "#B4A0FF";
    public const string PurpleHex = "#5C2D91";
    public const string DarkPurpleHex = "#32145A";
    public const string LightBlueHex = "#00BCF2";
    public const string MidBlueHex = "#00188F";
    public const string BlueHex = "#0078d7";
    public const string DarkBlueHex = "#002050";
    public const string LightTealHex = "#00B294";
    public const string TealHex = "#008272";
    public const string DarkTealHex = "#004B50";
    public const string LightGreenHex = "#BAD80A";
    public const string GreenHex = "#107C10";
    public const string DarkGreenHex = "#004B1C";

    public static readonly Color YellowColor = ConvertHexToColor(YellowHex);
    public static readonly Color LightYellowColor = ConvertHexToColor(LightYellowHex);
    public static readonly Color OrangeColor = ConvertHexToColor(OrangeHex);
    public static readonly Color OrangeLightColor = ConvertHexToColor(OrangeLightHex);
    public static readonly Color OrangeLighterColor = ConvertHexToColor(OrangeLighterHex);
    public static readonly Color DarkRedColor = ConvertHexToColor(DarkRedHex);
    public static readonly Color RedColor = ConvertHexToColor(RedHex);
    public static readonly Color MagentaLightColor = ConvertHexToColor(MagentaLightHex);
    public static readonly Color MagentaColor = ConvertHexToColor(MagentaHex);
    public static readonly Color DarkMagentaColor = ConvertHexToColor(DarkMagentaHex);
    public static readonly Color LightPurpleColor = ConvertHexToColor(LightPurpleHex);
    public static readonly Color PurpleColor = ConvertHexToColor(PurpleHex);
    public static readonly Color DarkPurpleColor = ConvertHexToColor(DarkPurpleHex);
    public static readonly Color LightBlueColor = ConvertHexToColor(LightBlueHex);
    public static readonly Color MidBlueColor = ConvertHexToColor(MidBlueHex);
    public static readonly Color BlueColor = ConvertHexToColor(BlueHex);
    public static readonly Color DarkBlueColor = ConvertHexToColor(DarkBlueHex);
    public static readonly Color LightTealColor = ConvertHexToColor(LightTealHex);
    public static readonly Color TealColor = ConvertHexToColor(TealHex);
    public static readonly Color DarkTealColor = ConvertHexToColor(DarkTealHex);
    public static readonly Color LightGreenColor = ConvertHexToColor(LightGreenHex);
    public static readonly Color GreenColor = ConvertHexToColor(GreenHex);
    public static readonly Color DarkGreenColor = ConvertHexToColor(DarkGreenHex);

    public static readonly Color Gray220Color = ConvertHexToColor(Gray220Hex);
    public static readonly Color Gray210Color = ConvertHexToColor(Gray210Hex);
    public static readonly Color Gray200Color = ConvertHexToColor(Gray200Hex);
    public static readonly Color Gray190Color = ConvertHexToColor(Gray190Hex);
    public static readonly Color Gray180Color = ConvertHexToColor(Gray180Hex);
    public static readonly Color Gray170Color = ConvertHexToColor(Gray170Hex);
    public static readonly Color Gray160Color = ConvertHexToColor(Gray160Hex);
    public static readonly Color Gray150Color = ConvertHexToColor(Gray150Hex);
    public static readonly Color Gray140Color = ConvertHexToColor(Gray140Hex);
    public static readonly Color Gray130Color = ConvertHexToColor(Gray130Hex);
    public static readonly Color Gray120Color = ConvertHexToColor(Gray120Hex);
    public static readonly Color Gray110Color = ConvertHexToColor(Gray110Hex);
    public static readonly Color Gray100Color = ConvertHexToColor(Gray100Hex);
    public static readonly Color Gray90Color = ConvertHexToColor(Gray90Hex);
    public static readonly Color Gray80Color = ConvertHexToColor(Gray80Hex);
    public static readonly Color Gray70Color = ConvertHexToColor(Gray70Hex);
    public static readonly Color Gray60Color = ConvertHexToColor(Gray60Hex);
    public static readonly Color Gray50Color = ConvertHexToColor(Gray50Hex);
    public static readonly Color Gray40Color = ConvertHexToColor(Gray40Hex);
    public static readonly Color Gray30Color = ConvertHexToColor(Gray30Hex);
    public static readonly Color Gray20Color = ConvertHexToColor(Gray20Hex);
    public static readonly Color Gray10Color = ConvertHexToColor(Gray10Hex);

    public enum Colors
    {
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
        DarkGreen,

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
    }

    public static readonly IReadOnlyList<AccentColor> AccentColors =
    [
        AccentColor.Yellow,
        AccentColor.LightYellow,
        AccentColor.Orange,
        AccentColor.OrangeLight,
        AccentColor.OrangeLighter,
        AccentColor.DarkRed,
        AccentColor.Red,
        AccentColor.MagentaLight,
        AccentColor.Magenta,
        AccentColor.DarkMagenta,
        AccentColor.LightPurple,
        AccentColor.Purple,
        AccentColor.DarkPurple,
        AccentColor.LightBlue,
        AccentColor.MidBlue,
        AccentColor.Blue,
        AccentColor.DarkBlue,
        AccentColor.LightTeal,
        AccentColor.Teal,
        AccentColor.DarkTeal,
        AccentColor.LightGreen,
        AccentColor.Green,
        AccentColor.DarkGreen
    ];

    public static readonly IReadOnlyList<BackgroundColor> BackgroundColors =
    [
        BackgroundColor.Gray220,
        BackgroundColor.Gray210,
        BackgroundColor.Gray200,
        BackgroundColor.Gray190,
        BackgroundColor.Gray180,
        BackgroundColor.Gray170,
        BackgroundColor.Gray160,
        BackgroundColor.Gray150,
        BackgroundColor.Gray140,
        BackgroundColor.Gray130,
        BackgroundColor.Gray120,
        BackgroundColor.Gray110,
        BackgroundColor.Gray100,
        BackgroundColor.Gray90,
        BackgroundColor.Gray80,
        BackgroundColor.Gray70,
        BackgroundColor.Gray60,
        BackgroundColor.Gray50,
        BackgroundColor.Gray40,
        BackgroundColor.Gray30,
        BackgroundColor.Gray20,
        BackgroundColor.Gray10
    ];

    public abstract class ColorBase(Colors name, Color color, string hex)
    {
        public Colors Name { get; init; } = name;
        public Color Color { get; init; } = color;
        public string Hex { get; init; } = hex;
    }

    public sealed class AccentColor : ColorBase
    {
        public static AccentColor Yellow { get; } = new AccentColor(Colors.Yellow, YellowColor, YellowHex);
        public static AccentColor LightYellow { get; } = new AccentColor(Colors.LightYellow, LightYellowColor, LightYellowHex);
        public static AccentColor Orange { get; } = new AccentColor(Colors.Orange, OrangeColor, OrangeHex);
        public static AccentColor OrangeLight { get; } = new AccentColor(Colors.OrangeLight, OrangeLightColor, OrangeLightHex);
        public static AccentColor OrangeLighter { get; } = new AccentColor(Colors.OrangeLighter, OrangeLighterColor, OrangeLighterHex);
        public static AccentColor DarkRed { get; } = new AccentColor(Colors.DarkRed, DarkRedColor, DarkRedHex);
        public static AccentColor Red { get; } = new AccentColor(Colors.Red, RedColor, RedHex);
        public static AccentColor MagentaLight { get; } = new AccentColor(Colors.MagentaLight, MagentaLightColor, MagentaLightHex);
        public static AccentColor Magenta { get; } = new AccentColor(Colors.Magenta, MagentaColor, MagentaHex);
        public static AccentColor DarkMagenta { get; } = new AccentColor(Colors.DarkMagenta, DarkMagentaColor, DarkMagentaHex);
        public static AccentColor LightPurple { get; } = new AccentColor(Colors.LightPurple, LightPurpleColor, LightPurpleHex);
        public static AccentColor Purple { get; } = new AccentColor(Colors.Purple, PurpleColor, PurpleHex);
        public static AccentColor DarkPurple { get; } = new AccentColor(Colors.DarkPurple, DarkPurpleColor, DarkPurpleHex);
        public static AccentColor LightBlue { get; } = new AccentColor(Colors.LightBlue, LightBlueColor, LightBlueHex);
        public static AccentColor MidBlue { get; } = new AccentColor(Colors.MidBlue, MidBlueColor, MidBlueHex);
        public static AccentColor Blue { get; } = new AccentColor(Colors.Blue, BlueColor, BlueHex);
        public static AccentColor DarkBlue { get; } = new AccentColor(Colors.DarkBlue, DarkBlueColor, DarkBlueHex);
        public static AccentColor LightTeal { get; } = new AccentColor(Colors.LightTeal, LightTealColor, LightTealHex);
        public static AccentColor Teal { get; } = new AccentColor(Colors.Teal, TealColor, TealHex);
        public static AccentColor DarkTeal { get; } = new AccentColor(Colors.DarkTeal, DarkTealColor, DarkTealHex);
        public static AccentColor LightGreen { get; } = new AccentColor(Colors.LightGreen, LightGreenColor, LightGreenHex);
        public static AccentColor Green { get; } = new AccentColor(Colors.Green, GreenColor, GreenHex);
        public static AccentColor DarkGreen { get; } = new AccentColor(Colors.DarkGreen, DarkGreenColor, DarkGreenHex);
        public static AccentColor LightGray { get; } = new AccentColor(Colors.Gray30, Gray30Color, Gray30Hex);

        private AccentColor(Colors name, Color color, string hex) : base(name, color, hex)
        {
        }
    }

    public sealed class BackgroundColor : ColorBase
    {
        public static BackgroundColor Gray220 { get; } = new BackgroundColor(Colors.Gray220, Gray220Color, Gray220Hex);
        public static BackgroundColor Gray210 { get; } = new BackgroundColor(Colors.Gray210, Gray210Color, Gray210Hex);
        public static BackgroundColor Gray200 { get; } = new BackgroundColor(Colors.Gray200, Gray200Color, Gray200Hex);
        public static BackgroundColor Gray190 { get; } = new BackgroundColor(Colors.Gray190, Gray190Color, Gray190Hex);
        public static BackgroundColor Gray180 { get; } = new BackgroundColor(Colors.Gray180, Gray180Color, Gray180Hex);
        public static BackgroundColor Gray170 { get; } = new BackgroundColor(Colors.Gray170, Gray170Color, Gray170Hex);
        public static BackgroundColor Gray160 { get; } = new BackgroundColor(Colors.Gray160, Gray160Color, Gray160Hex);
        public static BackgroundColor Gray150 { get; } = new BackgroundColor(Colors.Gray150, Gray150Color, Gray150Hex);
        public static BackgroundColor Gray140 { get; } = new BackgroundColor(Colors.Gray140, Gray140Color, Gray140Hex);
        public static BackgroundColor Gray130 { get; } = new BackgroundColor(Colors.Gray130, Gray130Color, Gray130Hex);
        public static BackgroundColor Gray120 { get; } = new BackgroundColor(Colors.Gray120, Gray120Color, Gray120Hex);
        public static BackgroundColor Gray110 { get; } = new BackgroundColor(Colors.Gray110, Gray110Color, Gray110Hex);
        public static BackgroundColor Gray100 { get; } = new BackgroundColor(Colors.Gray100, Gray100Color, Gray100Hex);
        public static BackgroundColor Gray90 { get; } = new BackgroundColor(Colors.Gray90, Gray90Color, Gray90Hex);
        public static BackgroundColor Gray80 { get; } = new BackgroundColor(Colors.Gray80, Gray80Color, Gray80Hex);
        public static BackgroundColor Gray70 { get; } = new BackgroundColor(Colors.Gray70, Gray70Color, Gray70Hex);
        public static BackgroundColor Gray60 { get; } = new BackgroundColor(Colors.Gray60, Gray60Color, Gray60Hex);
        public static BackgroundColor Gray50 { get; } = new BackgroundColor(Colors.Gray50, Gray50Color, Gray50Hex);
        public static BackgroundColor Gray40 { get; } = new BackgroundColor(Colors.Gray40, Gray40Color, Gray40Hex);
        public static BackgroundColor Gray30 { get; } = new BackgroundColor(Colors.Gray30, Gray30Color, Gray30Hex);
        public static BackgroundColor Gray20 { get; } = new BackgroundColor(Colors.Gray20, Gray20Color, Gray20Hex);
        public static BackgroundColor Gray10 { get; } = new BackgroundColor(Colors.Gray10, Gray10Color, Gray10Hex);

        private BackgroundColor(Colors name, Color color, string hex) : base(name, color, hex)
        {
        }
    }

    private static Color ConvertHexToColor(string hex)
    {
        hex = hex.TrimStart('#');

        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);

        return Color.FromRgb(r, g, b);
    }
}
