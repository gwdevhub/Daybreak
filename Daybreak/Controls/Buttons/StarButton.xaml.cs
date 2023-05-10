using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for StarButton.xaml
/// </summary>
public partial class StarButton : UserControl
{
    public event EventHandler? Clicked;

    public StarButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
