using Daybreak.Models.Guildwars;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;
public abstract class DrawingModuleBase
{
    protected const int MinimumSize = 10;

    protected virtual bool HasMinimumSize => false;

    public virtual bool CanDrawPlayerPositionHistory => false;

    public virtual bool CanDrawPathfinding => false;

    public virtual bool CanDrawQuestObjectives => false;

    public virtual bool CanDrawEntity(IEntity entity) => false;

    public virtual bool CanDrawMapIcon(MapIcon mapIcon) => false;

    public virtual bool CanDrawEngagementArea(IEntity entity) => false;

    public virtual void DrawEntity(int finalX, int finalY, int size, WriteableBitmap bitmap, bool targeted, Color shade) { }

    public virtual void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap, Affiliation affiliation, Color shade) { }

    public virtual void DrawPlayerPositionHistory(int finalX, int finalY, int size, WriteableBitmap bitmap, Color shade) { }

    public virtual void DrawPathFinding(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color, Color shade) { }

    public virtual void DrawQuestObjective(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color, Color shade) { }

    public virtual void DrawEngagementArea(int finalX, int finalY, int size, WriteableBitmap bitmap, Color shade) { }
}
