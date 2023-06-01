using Daybreak.Utils;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class TriangleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.FillTriangle(
            x - entitySize, y - entitySize,
            x + entitySize, y - entitySize,
            x, y + entitySize,
            finalColor);
    }

    protected void DrawOutlinedTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, int thickness, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.DrawLineAa(
            x - entitySize, y - entitySize,
            x + entitySize, y - entitySize,
            finalColor,
            thickness);

        bitmap.DrawLineAa(
            x + entitySize, y - entitySize,
            x, y + entitySize,
            finalColor,
            thickness);

        bitmap.DrawLineAa(
            x, y + entitySize,
            x - entitySize, y - entitySize,
            finalColor,
            thickness);
    }
}
