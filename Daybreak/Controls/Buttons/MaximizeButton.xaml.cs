using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for SaveButton.xaml
/// </summary>
public partial class MaximizeButton : UserControl
{
    public event EventHandler? Clicked;

    public MaximizeButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
