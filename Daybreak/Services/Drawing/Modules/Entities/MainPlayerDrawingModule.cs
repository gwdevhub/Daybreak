using Daybreak.Models.Guildwars;
using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;

namespace Daybreak.Services.Drawing.Modules.Entities;
public sealed class MainPlayerDrawingModule : CircleEntityDrawingModuleBase
{
    protected override Color FillColor { get; } = Colors.Green;

    public override bool CanDrawEntity(IEntity entity)
    {
        return entity is MainPlayerInformation;
    }
}
