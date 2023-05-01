using Daybreak.Models;
using Daybreak.Models.Guildwars;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Services.Drawing.Modules.Primitives;
public abstract class ColoredEmbeddedSvgDrawingModuleBase<TDerivingType> : EmbeddedSvgDrawingModuleBase<TDerivingType>
{
    protected abstract Color StrokeColor { get; }

    public override void DrawMapIcon(int finalX, int finalY, int size, WriteableBitmap bitmap, Affiliation affiliation)
    {
        switch (affiliation)
        {
            case Affiliation.Gray:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, Colors.Gray);
                break;
            case Affiliation.Red:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Red);
                break;
            case Affiliation.Blue:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Blue);
                break;
            case Affiliation.Yellow:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Yellow);
                break;
            case Affiliation.Green:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Green);
                break;
            case Affiliation.GrayNeutral:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, Colors.Gray);
                break;
            case Affiliation.Teal:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Teal);
                break;
            case Affiliation.Purple:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, QuestObjectiveColors.Purple);
                break;
            case Affiliation.Any:
                this.DrawSvg(bitmap, finalX, finalY, size, this.StrokeColor, Colors.Gray);
                break;

        }
    }
}
