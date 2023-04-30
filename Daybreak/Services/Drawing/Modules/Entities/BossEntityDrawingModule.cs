using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Entities;

public sealed class BossEntityDrawingModule : StarTargetedEntityDrawingModuleBase
{
    protected override Color FillColor { get; } = Colors.DarkRed;
    protected override bool HasMinimumSize => true;

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            livingEntity.State is LivingEntityState.Boss;
    }
}
