using System.Windows.Media;

namespace Daybreak.Shared.Utils;

public static class ColorExtensions
{
    public static Color AlphaBlend(this Color backColor, Color foreColor)
    {
        var fa = (float)foreColor.A / byte.MaxValue;
        var fr = (float)foreColor.R / byte.MaxValue;
        var fg = (float)foreColor.G / byte.MaxValue;
        var fb = (float)foreColor.B / byte.MaxValue;

        var ba = (float)backColor.A / byte.MaxValue;
        var br = (float)backColor.R / byte.MaxValue;
        var bg = (float)backColor.G / byte.MaxValue;
        var bb = (float)backColor.B / byte.MaxValue;

        var a = fa + ba - fa * ba;

        if (a <= 0)
            return Colors.Transparent;

        var r = (fa * (1 - ba) * fr + fa * ba * fa + (1 - fa) * ba * br) / a;
        var g = (fa * (1 - ba) * fg + fa * ba * fa + (1 - fa) * ba * bg) / a;
        var b = (fa * (1 - ba) * fb + fa * ba * fa + (1 - fa) * ba * bb) / a;

        return Color.FromArgb(
            (byte)(a * byte.MaxValue),
            (byte)(r * byte.MaxValue),
            (byte)(g * byte.MaxValue),
            (byte)(b * byte.MaxValue));
    }
}
