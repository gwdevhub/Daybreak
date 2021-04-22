using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for SaveButton.xaml
    /// </summary>
    public partial class SaveButton : UserControl
    {
        public event EventHandler Clicked;

        public SaveButton()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            BackgroundEllipse.Visibility = System.Windows.Visibility.Visible;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            BackgroundEllipse.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Clicked?.Invoke(this, e);
            }
        }
    }
}
