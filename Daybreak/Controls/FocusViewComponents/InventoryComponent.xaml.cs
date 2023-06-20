using Daybreak.Models.Guildwars;
using System;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls.FocusViewComponents;

/// <summary>
/// Interaction logic for InventoryComponent.xaml
/// </summary>
public partial class InventoryComponent : UserControl
{
    public event EventHandler? MaximizeClicked;
    public event EventHandler<ItemBase>? ItemWikiClicked;
    public event EventHandler<ItemBase>? PriceHistoryClicked;

    [GenerateDependencyProperty]
    private bool bagItemMenuVisible;

    [GenerateDependencyProperty]
    private ItemBase selectedItem = default!;

    public InventoryComponent()
    {
        this.InitializeComponent();
    }

    private void MaximizeButton_Clicked(object _, EventArgs e)
    {
        this.MaximizeClicked?.Invoke(this, e);
    }

    private void BagTemplate_ItemWikiClicked(object _, ItemBase e)
    {
        this.ItemWikiClicked?.Invoke(this, e);
    }

    private void BagTemplate_PriceHistoryClicked(object _, ItemBase e)
    {
        this.PriceHistoryClicked?.Invoke(this, e);
    }

    private void BagTemplate_ItemClicked(object _, ItemBase e)
    {
        this.BagItemMenuVisible = true;
        this.SelectedItem = e;
    }

    private void BagItemMenu_CloseClicked(object _, EventArgs e)
    {
        this.BagItemMenuVisible = false;
    }

    private void BagItemMenu_ItemWikiClicked(object _, ItemBase e)
    {
        this.ItemWikiClicked?.Invoke(this, e);
    }
}
