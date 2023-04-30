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

    public override void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap)
    {
        this.DrawSvg(bitmap, finalX, finalY, size, Colors.Transparent, Colors.Transparent);
    }
}
