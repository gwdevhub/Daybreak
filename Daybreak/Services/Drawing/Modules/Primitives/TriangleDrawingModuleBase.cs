using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class TriangleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawFilledTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        var halfSize = entitySize / 2;
        bitmap.FillTriangle(
            x - halfSize, y - halfSize,
            x + halfSize, y - halfSize,
            x, y + halfSize,
            color);
    }

    protected void DrawOutlinedTriangle(WriteableBitmap bitmap, int x, int y, int entitySize, int thickness, Color color)
    {
        var halfSize = entitySize / 2;
        bitmap.DrawLineAa(
            x - halfSize, y - halfSize,
            x + halfSize, y - halfSize,
            color,
            thickness);

        bitmap.DrawLineAa(
            x + halfSize, y - halfSize,
            x, y + halfSize,
            color,
            thickness);

        bitmap.DrawLineAa(
            x, y + halfSize,
            x - halfSize, y - halfSize,
            color,
            thickness);
    }
}
