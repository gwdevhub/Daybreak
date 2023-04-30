using System;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Models;
public sealed class ColorCombination
{
    public Color StrokeColor { get; init; }
    public Color FillColor { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is not ColorCombination otherCombination)
        {
            return base.Equals(obj);
        }

        return this.StrokeColor == otherCombination.StrokeColor &&
            this.FillColor == otherCombination.FillColor;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.StrokeColor, this.FillColor);
    }
}
