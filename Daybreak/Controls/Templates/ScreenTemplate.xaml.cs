using Daybreak.Models;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Controls.Templates
{
    /// <summary>
    /// Interaction logic for ScreenTemplate.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class ScreenTemplate : UserControl
    {
        public event EventHandler<Screen>? Clicked;

        [GenerateDependencyProperty]
        private string screenId = string.Empty;
        [GenerateDependencyProperty]
        private Brush highlight = default!;

        public ScreenTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.ScreenTemplate_DataContextChanged;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ForegroundProperty)
            {
                this.Highlight = e.NewValue.As<Brush>();
            }
        }

        private void ScreenTemplate_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Screen screen)
            {
                this.ScreenId = screen.Id.ToString();
            }
        }

        private void Rectangle_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Highlight = Brushes.LightSteelBlue;
        }

        private void Rectangle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.Highlight = this.Foreground;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Clicked?.Invoke(this, this.DataContext.As<Screen>());
        }
    }
}
