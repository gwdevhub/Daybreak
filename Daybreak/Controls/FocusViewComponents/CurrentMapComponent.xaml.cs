using Daybreak.Shared.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls.FocusViewComponents;
/// <summary>
/// Interaction logic for CurrentMapComponent.xaml
/// </summary>
public partial class CurrentMapComponent : UserControl
{
    public event EventHandler<string>? NavigateToClicked;

    public CurrentMapComponent()
    {
        this.InitializeComponent();
    }

    private void CurrentMap_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        if (this.DataContext is not Map currentMap)
        {
            return;
        }

        if (currentMap.WikiUrl is not string url)
        {
            return;
        }

        this.NavigateToClicked?.Invoke(this, url);
    }
}
