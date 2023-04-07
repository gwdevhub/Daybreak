using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for LivingEntityContextMenu.xaml
/// </summary>
public partial class LivingEntityContextMenu : UserControl
{
    public event EventHandler<LivingEntity?>? LivingEntityContextMenuClicked;

    public LivingEntityContextMenu()
    {
        this.InitializeComponent();
    }

    private void TextBlock_MouseLeftButtonDown(object _, MouseButtonEventArgs e)
    {
        this.LivingEntityContextMenuClicked?.Invoke(this, this.DataContext as LivingEntity? ?? default);
    }
}
