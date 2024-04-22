﻿using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;

public abstract class StarTargetedEntityDrawingModuleBase : StarEntityDrawingModuleBase
{
    protected virtual Color OutlineColor { get; } = Colors.Chocolate;

    public override sealed void DrawEntity(int finalX, int finalY, int size, double angle, WriteableBitmap bitmap, bool targeted, Color shade)
    {
        base.DrawEntity(finalX, finalY, size, angle, bitmap, targeted, shade);
        if (targeted)
        {
            var thickness = size / 5;
            this.DrawOutlinedStar(bitmap, finalX, finalY, size, angle, thickness, this.OutlineColor, shade);
        }
    }
}
