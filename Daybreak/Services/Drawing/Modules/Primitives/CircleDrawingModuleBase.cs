using System.Windows.Media;
using System.Windows.Media.Imaging;
using Daybreak.Utils;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CircleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledCircle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.FillEllipseCentered(
                x,
                y,
                entitySize,
                entitySize,
                finalColor);
    }

    protected void DrawCircle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color, Color shade)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var finalColor = ColorExtensions.AlphaBlend(color, shade);
        bitmap.DrawEllipseCentered(
                x,
                y,
                entitySize,
                entitySize,
                finalColor);
    }
}
