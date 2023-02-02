﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for AddButton.xaml
    /// </summary>
    public partial class HelpButton : UserControl
    {
        public event EventHandler? Clicked;

        public HelpButton()
        {
            this.InitializeComponent();
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            this.BackgroundEllipse.Opacity = 0.6;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            this.BackgroundEllipse.Opacity = 0;
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.Clicked?.Invoke(this, e);
            }
        }
    }
}
