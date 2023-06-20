using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for BagItemContextMenu.xaml
/// </summary>
public partial class BagItemContextMenu : UserControl
{
    public event EventHandler<ItemBase>? PriceHistoryClicked;
    public event EventHandler<ItemBase>? WikiClicked;

    public BagItemContextMenu()
    {
        this.InitializeComponent();
    }

    private void WikiTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (this.DataContext is not BagItem bagItem ||
            bagItem.Item is not ItemBase itemBase)
        {
            return;
        }

        this.WikiClicked?.Invoke(this, itemBase);
    }

    private void PriceHistoryTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (this.DataContext is not BagItem bagItem ||
            bagItem.Item is not ItemBase itemBase)
        {
            return;
        }

        this.PriceHistoryClicked?.Invoke(this, itemBase);
    }
}
