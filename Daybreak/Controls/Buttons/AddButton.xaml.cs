using System;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for AddButton.xaml
/// </summary>
public partial class AddButton : UserControl
{
    public event EventHandler? Clicked;

    public AddButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
