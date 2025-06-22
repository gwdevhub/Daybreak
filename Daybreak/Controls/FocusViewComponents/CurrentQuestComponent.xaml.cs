using Daybreak.Shared.Models.FocusView;
using Daybreak.Shared.Models.Guildwars;
using System;
using System.Windows.Controls;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for CurrentQuestComponent.xaml
/// </summary>
public partial class CurrentQuestComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;

    public CurrentQuestComponent()
    {
        this.InitializeComponent();
    }

    private void CurrentQuest_MouseLeftButtonDown(object _, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (this.DataContext is not CurrentQuestComponentContext context)
        {
            return;
        }

        if (context.Quest.WikiUrl is not string url)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, url);
    }
}
