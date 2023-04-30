using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class TriangleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        bitmap.FillTriangle(
            x - entitySize, y - entitySize,
            x + entitySize, y - entitySize,
            x, y + entitySize,
            color);
    }

    protected void DrawOutlinedTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, int thickness, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        bitmap.DrawLineAa(
            x - entitySize, y - entitySize,
            x + entitySize, y - entitySize,
            color,
            thickness);

        bitmap.DrawLineAa(
            x + entitySize, y - entitySize,
            x, y + entitySize,
            color,
            thickness);

        bitmap.DrawLineAa(
            x, y + entitySize,
            x - entitySize, y - entitySize,
            color,
            thickness);
    }
}
