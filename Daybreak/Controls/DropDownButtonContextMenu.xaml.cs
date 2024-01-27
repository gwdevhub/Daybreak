using Daybreak.Controls.Buttons;
using System;
using System.Collections;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;
/// <summary>
/// Interaction logic for DropDownButtonContextMenu.xaml
/// </summary>
public partial class DropDownButtonContextMenu : UserControl
{
    [GenerateDependencyProperty]
    private DataTemplate itemTemplate = default!;

    [GenerateDependencyProperty]
    private IEnumerable items = default!;

    public event EventHandler<object> ItemClicked = default!;

    public DropDownButtonContextMenu()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.ItemClicked?.Invoke(this, sender.Cast<HighlightButton>().DataContext);
    }
}
