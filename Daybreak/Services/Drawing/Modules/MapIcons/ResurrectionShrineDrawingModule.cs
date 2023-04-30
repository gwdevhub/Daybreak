using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.MapIcons;

public sealed class ResurrectionShrineDrawingModule : CrossDrawingModuleBase
{
    protected override bool HasMinimumSize => true;

    public override bool CanDrawMapIcon(MapIcon mapIcon)
    {
        return mapIcon.Icon == GuildwarsIcon.ResurrectionShrine;
    }

    public override void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap)
    {
        this.DrawCross(bitmap, finalX, finalY, size, Colors.CornflowerBlue);
    }
}
