using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class StarEntityDrawingModuleBase : StarDrawingModuleBase
{
    protected abstract Color FillColor { get; }

    public override void DrawEntity(int finalX, int finalY, int size, double angle, WriteableBitmap bitmap, bool targeted, Color shade)
    {
        this.DrawFilledStar(bitmap, finalX, finalY, size, angle, this.FillColor, shade);
    }
}
