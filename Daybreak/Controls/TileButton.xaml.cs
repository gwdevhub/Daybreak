using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for TileButton.xaml
    /// </summary>
    public partial class TileButton : UserControl
    {
        public event EventHandler Clicked;

        public static readonly DependencyProperty HighlightedProperty =
            DependencyProperty.Register("Highlighted", typeof(bool), typeof(TileButton), null);
        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.Register("HighlightColor", typeof(Brush), typeof(TileButton), null);
        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(FrameworkElement), typeof(TileButton), null);
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TileButton), null);

        public TileButton()
        {
            this.InitializeComponent();
        }

        public bool Highlighted
        {
            get => (bool)this.GetValue(HighlightedProperty);
            set => this.SetValue(HighlightedProperty, value);
        }

        public string Title
        {
            get => this.GetValue(TitleProperty) as string;
            set => this.SetValue(TitleProperty, value);
        }

        public FrameworkElement InnerContent
        {
            get => this.GetValue(InnerContentProperty) as FrameworkElement;
            set => this.SetValue(InnerContentProperty, value);
        }

        public Brush HighlightColor
        {
            get => this.GetValue(HighlightColorProperty) as Brush;
            set => this.SetValue(HighlightColorProperty, value);
        }

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Highlighted = true;
        }

        private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Highlighted = false;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}
