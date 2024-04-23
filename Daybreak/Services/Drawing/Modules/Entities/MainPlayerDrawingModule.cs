using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Entities;
public sealed class MainPlayerDrawingModule : EmbeddedSvgDrawingModuleBase<MainPlayerDrawingModule>
{
    private Color OutlineColor { get; } = Colors.Chocolate;
    private Color FillColor { get; } = Colors.Green;
    protected override string EmbeddedSvgPath { get; } = "Daybreak.Services.Drawing.Resources.CircledArrow.svg";

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is MainPlayerInformation;
    }

    public override void DrawEntity(int finalX, int finalY, int size, double cameraAngle, double entityAngle, WriteableBitmap bitmap, bool targeted, Color shade)
    {
        var angle = entityAngle != 0 ? entityAngle : cameraAngle;
        this.DrawSvg(bitmap, finalX, finalY, size, angle, targeted ? this.OutlineColor : this.FillColor, this.FillColor, shade);
    }
}
