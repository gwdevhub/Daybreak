using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using System.Windows.Controls;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for QuestLogComponent.xaml
/// </summary>
public partial class QuestLogComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;

    public QuestLogComponent()
    {
        this.InitializeComponent();
    }

    private void QuestLogTemplate_MapClicked(object _, Map e)
    {
        if (e is null)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, e.WikiUrl ?? string.Empty);
    }

    private void QuestLogTemplate_QuestClicked(object _, Quest e)
    {
        if (e is null)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, e.WikiUrl ?? string.Empty);
    }

    private void ActiveQuest_Clicked(object sender, EventArgs e)
    {
        if (this.DataContext is not QuestLogComponentContext context ||
            context.CurrentQuest?.Quest is null)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, context.CurrentQuest.Quest.WikiUrl ?? string.Empty);
    }
}
