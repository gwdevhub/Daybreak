using System.Globalization;
using Daybreak.Shared.Services.Formatting;

namespace Daybreak.Services.Formatting;

internal sealed class SkillNumberFormatter : ISkillNumberFormatter
{
    public string Format(double? value)
    {
        if (value is not double d)
        {
            return string.Empty;
        }

        if (d < 0)
        {
            return "-" + this.Format(-d);
        }

        var whole = (long)Math.Floor(d);
        var fractional = d - whole;
        var fractionGlyph = TryGetFractionGlyph(fractional);

        if (fractionGlyph is null)
        {
            return d == whole
                ? whole.ToString(CultureInfo.InvariantCulture)
                : d.ToString("0.##", CultureInfo.InvariantCulture);
        }

        return whole == 0
            ? fractionGlyph
            : whole.ToString(CultureInfo.InvariantCulture) + fractionGlyph;
    }

    public string FormatPercent(double? value)
    {
        if (value is not double d)
        {
            return string.Empty;
        }

        return Math.Round(d * 100).ToString("0", CultureInfo.InvariantCulture) + "%";
    }

    private static string? TryGetFractionGlyph(double fractional) => fractional switch
    {
        < 0.001 => null,
        < 0.26 and > 0.24 => "¹⁄₄",
        < 0.51 and > 0.49 => "¹⁄₂",
        < 0.76 and > 0.74 => "³⁄₄",
        _ => null,
    };
}
