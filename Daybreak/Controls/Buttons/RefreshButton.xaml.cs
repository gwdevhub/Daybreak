using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for RefreshButton.xaml
/// </summary>
public partial class RefreshButton : UserControl
{
    public event EventHandler? Clicked;

    public RefreshButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
