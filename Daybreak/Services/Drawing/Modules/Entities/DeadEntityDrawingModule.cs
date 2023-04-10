using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Entities;

public sealed class DeadEntityDrawingModule : CircleTargetedEntityDrawingModuleBase
{
    protected override Color FillColor { get; } = Colors.Gray;

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            livingEntity.State is LivingEntityState.Dead;
    }
}
