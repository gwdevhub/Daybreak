using Daybreak.Models.Guildwars;
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
        if (this.DataContext is not Quest quest)
        {
            return;
        }

        if (quest.WikiUrl is not string url)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, url);
    }
}
