using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;

public sealed class PathfindingDrawingModule : CircleDrawingModuleBase
{
    public override bool CanDrawPathfinding => true;

    public override void DrawPathFinding(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color)
    {
        this.DrawFilledCircle(bitmap, finalX, finalY, size, color);
    }
}
