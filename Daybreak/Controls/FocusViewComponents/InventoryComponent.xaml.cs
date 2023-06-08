using System;
using System.Windows.Controls;

namespace Daybreak.Controls.FocusViewComponents;

/// <summary>
/// Interaction logic for InventoryComponent.xaml
/// </summary>
public partial class InventoryComponent : UserControl
{
    public event EventHandler? MaximizeClicked;

    public InventoryComponent()
    {
        this.InitializeComponent();
    }

    private void MaximizeButton_Clicked(object sender, EventArgs e)
    {
        this.MaximizeClicked?.Invoke(this, e);
    }
}
