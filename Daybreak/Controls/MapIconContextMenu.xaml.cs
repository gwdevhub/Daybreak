using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for MapIconContextMenu.xaml
/// </summary>
public partial class MapIconContextMenu : UserControl
{
    public event EventHandler<MapIcon?>? MapIconContextMenuClicked;

    public MapIconContextMenu()
    {
        this.InitializeComponent();
    }

    private void TextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.MapIconContextMenuClicked?.Invoke(this, this.DataContext as MapIcon? ?? default);
    }
}
