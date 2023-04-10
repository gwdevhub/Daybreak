using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;

public sealed class QuestObjectiveDrawingModule : StarDrawingModuleBase
{
    public override bool CanDrawQuestObjectives => true;

    public override void DrawQuestObjective(int finalX, int finalY, int size, WriteableBitmap bitmap, Color color)
    {
        this.DrawFilledStar(bitmap, finalX, finalY, size, color);
    }
}
