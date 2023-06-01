using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class CircleTargetedEntityDrawingModuleBase : CircleEntityDrawingModuleBase
{
    protected virtual Color OutlineColor { get; } = Colors.Chocolate;

    public override sealed void DrawEntity(int finalX, int finalY, int size, WriteableBitmap bitmap, bool targeted, Color shade)
    {
        if (targeted)
        {
            var outlineSize = size + (size / 5);
            this.DrawFilledCircle(bitmap, finalX, finalY, outlineSize, this.OutlineColor, shade);
        }

        base.DrawEntity(finalX, finalY, size, bitmap, targeted, shade);
    }
}
