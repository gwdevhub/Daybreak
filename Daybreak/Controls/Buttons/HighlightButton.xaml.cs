using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;

namespace Daybreak.Controls.Buttons;
/// <summary>
/// HighlightButton control
/// </summary>
public partial class HighlightButton : Control
{
    public event EventHandler? Clicked;

    [GenerateDependencyProperty]
    private ICommand click = default!;

    [GenerateDependencyProperty]
    private bool highlighted;
    [GenerateDependencyProperty]
    private Brush highlightBrush = default!;
    [GenerateDependencyProperty]
    private Brush disabledBrush = default!;
    [GenerateDependencyProperty(InitialValue = "")]
    private string title = string.Empty;
    [GenerateDependencyProperty(InitialValue = HorizontalAlignment.Left)]
    private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Left;
    [GenerateDependencyProperty(InitialValue = VerticalAlignment.Top)]
    private VerticalAlignment verticalContentAlignment = VerticalAlignment.Top;
    [GenerateDependencyProperty(InitialValue = TextAlignment.Center)]
    private TextAlignment textAlignment = TextAlignment.Center;
    [GenerateDependencyProperty(InitialValue = TextWrapping.Wrap)]
    private TextWrapping textWrapping = TextWrapping.Wrap;
    [GenerateDependencyProperty]
    private Thickness textPadding = new(0);
    [GenerateDependencyProperty]
    private UIElement buttonContent = default!;

    static HighlightButton()
    {
        // connect to default style in Themes/Generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HighlightButton),
            new FrameworkPropertyMetadata(typeof(HighlightButton)));
    }

    public HighlightButton()
    {
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        
        // Find the transparent rectangle for mouse handling
        if (this.GetTemplateChild("PART_MouseHandler") is FrameworkElement mouseHandler)
        {
            mouseHandler.MouseEnter += this.Grid_MouseEnter;
            mouseHandler.MouseLeave += this.Grid_MouseLeave;
            mouseHandler.MouseLeftButtonDown += this.Rectangle_MouseLeftButtonDown;
        }
    }

    private void Grid_MouseEnter(object sender, MouseEventArgs e)
    {
        this.Highlighted = true;
    }

    private void Grid_MouseLeave(object sender, MouseEventArgs e)
    {
        this.Highlighted = false;
    }

    private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.Clicked?.Invoke(this, e);
        if (this.Click?.CanExecute(e) is true)
        {
            this.Click?.Execute(e);
        }
    }
}
