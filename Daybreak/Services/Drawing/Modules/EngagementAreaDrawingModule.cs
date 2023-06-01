using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;

public sealed class EngagementAreaDrawingModule : CircleDrawingModuleBase
{
    private static readonly Color EngagementAreaColor = Colors.Gray;
    private static readonly Color EngagementAreaBorderColor = Color.FromArgb(
        a: 40,
        r: EngagementAreaColor.R,
        g: EngagementAreaColor.G,
        b: EngagementAreaColor.B);
    private static readonly Color EngagementAreaFillColor = Color.FromArgb(
        a: 20,
        r: EngagementAreaColor.R,
        g: EngagementAreaColor.G,
        b: EngagementAreaColor.B);

    public override bool CanDrawEngagementArea(IEntity entity)
    {
        return entity is MainPlayerInformation ||
            entity is PlayerInformation;
    }

    public override void DrawEngagementArea(int finalX, int finalY, int size, WriteableBitmap bitmap, Color shade)
    {
        this.DrawFilledCircle(bitmap, finalX, finalY, size, EngagementAreaFillColor, shade);
        this.DrawCircle(bitmap, finalX, finalY, size, EngagementAreaBorderColor, shade);
    }
}
