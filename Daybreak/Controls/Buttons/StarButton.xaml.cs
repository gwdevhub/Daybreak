using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// Interaction logic for StarButton.xaml
/// </summary>
public partial class StarButton : UserControl
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private ICommand click = default!;

    public StarButton()
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
