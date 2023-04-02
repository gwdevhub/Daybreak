using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for QuestContextMenu.xaml
/// </summary>
public partial class QuestContextMenu : UserControl
{
    public event EventHandler<QuestMetadata?>? QuestContextMenuClicked;

    public QuestContextMenu()
    {
        this.InitializeComponent();
    }

    private void TextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.QuestContextMenuClicked?.Invoke(this, this.DataContext as QuestMetadata? ?? default);
    }
}
