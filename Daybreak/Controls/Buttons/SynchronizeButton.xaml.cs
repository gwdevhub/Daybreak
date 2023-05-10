using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for SynchronizeButton.xaml
/// </summary>
public partial class SynchronizeButton : UserControl
{
    public event EventHandler? Clicked;

    public SynchronizeButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
