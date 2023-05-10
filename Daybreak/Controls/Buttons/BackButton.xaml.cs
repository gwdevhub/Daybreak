using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for BackButton.xaml
/// </summary>
public partial class BackButton : UserControl
{
    public event EventHandler? Clicked;

    public BackButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
