using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls.Buttons;

/// <summary>
/// Interaction logic for AddButton.xaml
/// </summary>
public partial class AddButton : UserControl
{
    public event EventHandler? Clicked;

    public ICommand? Click { get; set; }

    public AddButton()
    {
        this.InitializeComponent();
    }

    private void HighlightButton_Clicked(object sender, EventArgs e)
    {
        this.Clicked?.Invoke(this, e);
        if (this.Click?.CanExecute(e) is true)
        {
            this.Click?.Execute(e);
        }
    }
}
