﻿using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Entities;

public sealed class NeutralEntityDrawingModule : CircleTargetedEntityDrawingModuleBase
{
    protected override Color FillColor { get; } = Colors.LightSteelBlue;

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            livingEntity.Allegiance is LivingEntityAllegiance.Neutral;
    }
}
