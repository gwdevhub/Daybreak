using Daybreak.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        public ImageSource ImageSource
        {
            get => this.CurrentVisible().Source;
            set => this.ShowImage(value);
        }

        public ImageViewer()
        {
            InitializeComponent();
            this.Image1.Visibility = Visibility.Visible;
            this.Image1.Opacity = 1;
            this.Image2.Visibility = Visibility.Hidden;
            this.Image2.Opacity = 0;
        }

        public async void ShowImage(ImageSource imageSource)
        {
            var currentVisible = this.CurrentVisible();
            var nextVisible = this.NextVisible();

            if (nextVisible.Source is BitmapImage bitmapImage)
            {
                await bitmapImage.StreamSource.DisposeAsync().ConfigureAwait(true);
            }

            nextVisible.Source = imageSource;
            this.Transition(currentVisible, nextVisible);
        }

        private Image CurrentVisible()
        {
            if (this.Image1.Visibility == Visibility.Visible)
            {
                return this.Image1;
            }
            else
            {
                return this.Image2;
            }
        }
        private Image NextVisible()
        {
            if (this.Image1.Visibility == Visibility.Visible)
            {
                return this.Image2;
            }
            else
            {
                return this.Image1;
            }
        }
        private void Transition(Image from, Image to)
        {
            to.Visibility = Visibility.Visible;
            to.Opacity = 0;
            from.Visibility = Visibility.Visible;
            from.Opacity = 1;
            var showAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1.5)
            };
            var hideAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(1.5)
            };
            hideAnimation.Completed += (s, e) =>
            {
                from.Visibility = Visibility.Hidden;
                to.BeginAnimation(Image.OpacityProperty, showAnimation);
            };
            from.BeginAnimation(Image.OpacityProperty, hideAnimation);
        }
    }
}
