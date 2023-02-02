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
        public event EventHandler? Clicked;

        [GenerateDependencyProperty(InitialValue = "")]
        private string text = string.Empty;
        [GenerateDependencyProperty]
        private Brush transparentBackground = default!;
        [GenerateDependencyProperty]
        private Brush highlight = default!;
        [GenerateDependencyProperty]
        public double backgroundOpacity;
        [GenerateDependencyProperty]
        public double highlightOpacity;
        [GenerateDependencyProperty]
        public bool highlightVisible;

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
