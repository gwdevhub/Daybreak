using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for PlayerContextMenu.xaml
/// </summary>
public partial class PlayerContextMenu : UserControl
{
    public event EventHandler<PlayerInformation?>? PlayerContextMenuClicked;

    public PlayerContextMenu()
    {
        this.InitializeComponent();
    }

    private void TextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.PlayerContextMenuClicked?.Invoke(this, this.DataContext as PlayerInformation? ?? default);
    }
}
