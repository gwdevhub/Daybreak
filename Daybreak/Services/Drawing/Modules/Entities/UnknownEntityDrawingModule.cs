using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Entities;
public sealed class UnknownEntityDrawingModule : ColoredEmbeddedSvgDrawingModuleBase<UnknownEntityDrawingModule>
{
    protected override bool HasMinimumSize => false;
    protected override Color StrokeColor { get; } = Colors.IndianRed;
    protected override string EmbeddedSvgPath { get; } = "Daybreak.Services.Drawing.Resources.QuestionMark.svg";

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            (livingEntity.NpcDefinition is null ||
            livingEntity.NpcDefinition == Npc.Unknown);
    }

    public override void DrawEntity(int finalX, int finalY, int size, double cameraAngle, double entityAngle, WriteableBitmap bitmap, bool targeted, Color shade)
    {
        this.DrawSvg(bitmap, finalX, finalY, (int)(size / 1.3), cameraAngle, this.StrokeColor, Colors.Transparent, shade);
    }
}
