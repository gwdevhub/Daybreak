using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CircleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledCircle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        bitmap.FillEllipseCentered(
                x,
                y,
                entitySize,
                entitySize,
                color);
    }

    protected void DrawCircle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        bitmap.DrawEllipseCentered(
                x,
                y,
                entitySize,
                entitySize,
                color);
    }
}
