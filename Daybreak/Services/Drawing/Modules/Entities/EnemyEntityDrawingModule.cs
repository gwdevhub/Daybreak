using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Entities;

public sealed class EnemyEntityDrawingModule : TriangleTargetedEntityDrawingModuleBase
{
    protected override Color FillColor { get; } = Colors.Red;

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            livingEntity.Allegiance is LivingEntityAllegiance.Enemy &&
            livingEntity.State is not LivingEntityState.Boss or LivingEntityState.Dead;
    }
}
