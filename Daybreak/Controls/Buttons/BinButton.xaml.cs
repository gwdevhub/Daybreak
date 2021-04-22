using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for BinButton.xaml
    /// </summary>
    public partial class BinButton : UserControl
    {
        public event EventHandler Clicked;
        public BinButton()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Clicked?.Invoke(this, e);
            }
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            this.BackgroundEllipse.Visibility = System.Windows.Visibility.Visible;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BackgroundEllipse.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
