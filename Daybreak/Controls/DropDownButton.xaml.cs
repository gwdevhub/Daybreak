using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for DropdownButton.xaml
/// </summary>
public partial class DropDownButton : UserControl
{
    [GenerateDependencyProperty]
    private DataTemplate itemTemplate = default!;

    [GenerateDependencyProperty]
    private object selectedItem = default!;

    [GenerateDependencyProperty]
    private IEnumerable items = default!;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool clickEnabled = true;

    public event EventHandler<object> Clicked = default!;
    public event EventHandler<object> SelectionChanged = default!;

    public DropDownButton()
    {
        this.InitializeComponent();
    }

    private void MainButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, this.SelectedItem);
    }

    private void ArrowButton_Clicked(object sender, EventArgs e)
    {
        var contextMenu = this.ContextMenu;
        contextMenu.PlacementTarget = this;
        contextMenu.IsOpen = true;
    }

    private void DropDownButtonContextMenu_ItemClicked(object _, object e)
    {
        this.SelectedItem = e;
        this.ContextMenu.IsOpen = false;
        this.SelectionChanged?.Invoke(this, e);
    }

    private void IgnoreRightMouseButton(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is MouseButton.Right)
        {
            e.Handled = true;
        }
    }
}
