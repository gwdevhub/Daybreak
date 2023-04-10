using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;

public sealed class PlayerPositionHistoryDrawingModule : CircleDrawingModuleBase
{
    private readonly Color positionHistoryColor = Color.FromArgb(155, Colors.Red.R, Colors.Red.G, Colors.Red.B);

    public override bool CanDrawPlayerPositionHistory => true;

    public override void DrawPlayerPositionHistory(int finalX, int finalY, int size, WriteableBitmap bitmap)
    {
        this.DrawCircle(bitmap, finalX, finalY, size, this.positionHistoryColor);
    }
}
