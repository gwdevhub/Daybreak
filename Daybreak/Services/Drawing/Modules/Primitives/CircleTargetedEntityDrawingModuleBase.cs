using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CircleTargetedEntityDrawingModuleBase : CircleEntityDrawingModuleBase
{
    protected virtual Color OutlineColor { get; } = Colors.Chocolate;

    public override sealed void DrawEntity(int finalX, int finalY, int size, WriteableBitmap bitmap, bool targeted)
    {
        if (targeted)
        {
            var outlineSize = size + (size / 5);
            this.DrawCircle(bitmap, finalX, finalY, outlineSize, this.OutlineColor);
        }

        base.DrawEntity(finalX, finalY, size, bitmap, targeted);
    }
}
