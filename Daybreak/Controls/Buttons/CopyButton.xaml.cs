using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for CopyButton.xaml
/// </summary>
public partial class CopyButton : UserControl
{
    public event EventHandler? Clicked;

    public CopyButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
