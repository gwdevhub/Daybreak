using Daybreak.Services.Drawing.Modules.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules;

public sealed class QuestObjectiveDrawingModule : StarDrawingModuleBase
{
    protected override bool HasMinimumSize => true;

    public override bool CanDrawQuestObjectives => true;

    public override void DrawQuestObjective(int finalX, int finalY, int size, double angle, WriteableBitmap bitmap, Color color, Color shade)
    {
        this.DrawFilledStar(bitmap, finalX, finalY, size, angle, color, shade);
    }
}
