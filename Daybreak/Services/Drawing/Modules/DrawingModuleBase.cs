using Daybreak.Models.Guildwars;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;
public abstract class DrawingModuleBase
{
    public virtual bool CanDrawPlayerPositionHistory => false;

    public virtual bool CanDrawPathfinding => false;

    public virtual bool CanDrawQuestObjectives => false;

    public virtual bool CanDrawEntity(IEntity entity) => false;

    public virtual bool CanDrawMapIcon(MapIcon mapIcon) => false;

    public virtual void DrawEntity(int finalX, int finalY, int size, WriteableBitmap bitmap, bool targeted) { }

    public virtual void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap) { }

    public virtual void DrawPlayerPositionHistory(int finalX, int finalY, int size, WriteableBitmap bitmap) { }

    public virtual void DrawPathFinding(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color) { }

    public virtual void DrawQuestObjective(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color) { }
}
