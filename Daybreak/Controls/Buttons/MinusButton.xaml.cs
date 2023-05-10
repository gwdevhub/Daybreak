using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for AddButton.xaml
/// </summary>
public partial class MinusButton : UserControl
{
    public event EventHandler? Clicked;

    public MinusButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
