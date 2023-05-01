using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.MapIcons;
public sealed class StairsDownDrawingModule : EmbeddedSvgDrawingModuleBase<StairsDownDrawingModule>
{
    protected override bool HasMinimumSize => true;
    protected override string EmbeddedSvgPath => "Daybreak.Services.Drawing.Resources.StairsDown.svg";

    public override bool CanDrawMapIcon(MapIcon mapIcon)
    {
        return mapIcon.Icon == GuildwarsIcon.StairsDown;
    }

    public override void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap, Affiliation _)
    {
        this.DrawSvg(bitmap, finalX, finalY, size, Colors.White, Colors.Red);
    }
}
