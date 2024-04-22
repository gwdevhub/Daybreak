using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.MapIcons;

public sealed class AreaMapDrawingModule : EmbeddedSvgDrawingModuleBase<AreaMapDrawingModule>
{
    protected override bool HasMinimumSize => true;
    protected override string EmbeddedSvgPath => "Daybreak.Services.Drawing.Resources.Scroll.svg";

    public override bool CanDrawMapIcon(MapIcon mapIcon)
    {
        return mapIcon.Icon == GuildwarsIcon.AreaMap;
    }

    public override void DrawMapIcon(int finalX, int finalY, int size, double angle, WriteableBitmap bitmap, Affiliation affiliation, Color shade)
    {
        this.DrawSvg(bitmap, finalX, finalY, size, Colors.Transparent, Colors.Transparent, shade);
    }
}
