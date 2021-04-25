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
    public partial class ScreenTemplate : UserControl
    {
        public static readonly DependencyProperty ScreenIdProperty =
            DependencyPropertyExtensions.Register<ScreenTemplate, string>(nameof(ScreenId));
        public static readonly DependencyProperty HighlightProperty =
            DependencyPropertyExtensions.Register<ScreenTemplate, Brush>(nameof(Highlight));

        public event EventHandler<Screen> Clicked;

        public string ScreenId
        {
            get => this.GetTypedValue<string>(ScreenIdProperty);
            set => this.SetValue(ScreenIdProperty, value);
        }

        public Brush Highlight
        {
            get => this.GetTypedValue<Brush>(HighlightProperty);
            set => this.SetValue(HighlightProperty, value);
        }

        public ScreenTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += ScreenTemplate_DataContextChanged;
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
