using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CircleDrawingModuleBase : DrawingModuleBase
{
    protected void DrawCircle(WriteableBitmap bitmap, int x, int y, int entitySize, Color color)
    {
        bitmap.FillEllipseCentered(
                x,
                y,
                entitySize,
                entitySize,
                color);
    }
}
