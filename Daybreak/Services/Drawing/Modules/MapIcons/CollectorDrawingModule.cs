using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.MapIcons;
public sealed class CollectorDrawingModule : EmbeddedSvgDrawingModuleBase<CollectorDrawingModule>
{
    protected override string EmbeddedSvgPath => "Daybreak.Services.Drawing.Resources.Bag.svg";

    public override bool CanDrawMapIcon(MapIcon mapIcon)
    {
        return mapIcon.Icon == GuildwarsIcon.Collector;
    }

    public override void DrawMapIcon(int finalX, int finalY, int size, double angle, WriteableBitmap bitmap, Affiliation _, Color shade)
    {
        this.DrawSvg(bitmap, finalX, finalY, size, angle, Colors.Transparent, Colors.Transparent, shade);
    }
}
