using System;
using System.Windows.Controls;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for FilePickerGlyph.xaml
/// </summary>
public partial class FilePickerButton : UserControl
{
    public event EventHandler? Clicked;

    public FilePickerButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
    }
}
