using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for HomeButton.xaml
/// </summary>
public partial class HomeButton : UserControl
{
    public event EventHandler? Clicked;

    public HomeButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
