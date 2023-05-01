using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.MapIcons;

public sealed class CircledStarDrawingModule : ColoredEmbeddedSvgDrawingModuleBase<CircledStarDrawingModule>
{
    protected override bool HasMinimumSize => true;
    protected override string EmbeddedSvgPath => "Daybreak.Services.Drawing.Resources.CircledStar.svg";
    protected override Color StrokeColor { get; } = Colors.Black;

    public override bool CanDrawMapIcon(MapIcon mapIcon)
    {
        return mapIcon.Icon == GuildwarsIcon.Star;
    }
}
