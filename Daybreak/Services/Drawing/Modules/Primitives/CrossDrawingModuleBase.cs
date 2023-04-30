using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CrossDrawingModuleBase : DrawingModuleBase
{
    protected void DrawCross(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        if (this.HasMinimumSize &&
            entitySize < MinimumSize)
        {
            entitySize = MinimumSize;
        }

        var thickness = entitySize / 3;
        var height = entitySize * 3;
        var width = entitySize * 2;
        var height3 = height / 3;
        var height2 = height3 * 2;

        bitmap.FillRectangle(
            x - thickness, y - height3,
            x + thickness, y + height2,
            color);

        bitmap.FillRectangle(
            x - width / 2, y - thickness,
            x + width / 2, y + thickness,
            color);
    }
}
