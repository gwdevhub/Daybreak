using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Bosses;

public sealed class ElementalistBossEntityDrawingModule : BossEntityDrawingModule
{
    protected override Color FillColor { get; } = ColorPalette.DeepOrange;

    public override bool CanDrawEntity(IEntity entity)
    {
        return base.CanDrawEntity(entity) &&
               entity is LivingEntity livingEntity &&
               livingEntity.PrimaryProfession == Profession.Elementalist;
    }
}
