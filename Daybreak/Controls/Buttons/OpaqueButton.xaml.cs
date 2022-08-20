using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Media;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for OpaqueButton.xaml
    /// </summary>
    public partial class OpaqueButton : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, string>(nameof(Text));

        public static readonly DependencyProperty TransparentBackgroundProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, Brush>(nameof(TransparentBackground));

        public static readonly DependencyProperty HighlightProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, Brush>(nameof(Highlight));

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, double>(nameof(BackgroundOpacity));

        public static readonly DependencyProperty HighlightOpacityProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, double>(nameof(HighlightOpacity));

        public static readonly DependencyProperty HighlightVisibleProperty =
            DependencyPropertyExtensions.Register<OpaqueButton, bool>(nameof(HighlightVisible));

        public event EventHandler Clicked;

        public string Text
        {
            get => this.GetTypedValue<string>(TextProperty);
            set => this.SetTypedValue(TextProperty, value);
        }

        public Brush TransparentBackground
        {
            get => this.GetTypedValue<Brush>(TransparentBackgroundProperty);
            set => this.SetTypedValue(TransparentBackgroundProperty, value);
        }

        public Brush Highlight
        {
            get => this.GetTypedValue<Brush>(HighlightProperty);
            set => this.SetTypedValue(HighlightProperty, value);
        }

        public double BackgroundOpacity
        {
            get => this.GetTypedValue<double>(BackgroundOpacityProperty);
            set => this.SetTypedValue(BackgroundOpacityProperty, value);
        }

        public double HighlightOpacity
        {
            get => this.GetTypedValue<double>(HighlightOpacityProperty);
            set => this.SetTypedValue(HighlightOpacityProperty, value);
        }

        public bool HighlightVisible
        {
            get => this.GetTypedValue<bool>(HighlightVisibleProperty);
            set => this.SetTypedValue(HighlightVisibleProperty, value);
        }

        public OpaqueButton()
        {
            this.InitializeComponent();
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            this.HighlightVisible = true;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            this.HighlightVisible = false;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Clicked?.Invoke(this, e);
        }
    }
}
