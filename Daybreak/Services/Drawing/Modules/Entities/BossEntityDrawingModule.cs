﻿using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Entities;

public sealed class BossEntityDrawingModule : EmbeddedSvgDrawingModuleBase<BossEntityDrawingModule>
{
    private Color OutlineColor { get; } = Colors.Chocolate;
    private Color FillColor { get; } = Colors.DarkRed;

    protected override bool HasMinimumSize => true;

    protected override string EmbeddedSvgPath => "Daybreak.Services.Drawing.Resources.Skull.svg";

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is LivingEntity livingEntity &&
            livingEntity.State is LivingEntityState.Boss;
    }

    public override void DrawEntity(int finalX, int finalY, int size, WriteableBitmap bitmap, bool targeted)
    {
        this.DrawSvg(bitmap, finalX, finalY, size, targeted ? this.OutlineColor : this.FillColor, this.FillColor);
    }
}
