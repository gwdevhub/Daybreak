using Daybreak.Services.Bloogum;
using Daybreak.Services.Screenshots;
using Daybreak.Services.ViewManagement;
using Daybreak.Views;
using Pepa.Wpf.Utilities;
using System;
using System.Diagnostics;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Daybreak.Launch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty CreditTextProperty = DependencyPropertyExtensions.Register<MainWindow, string>(nameof(CreditText));

        private readonly IViewManager viewManager;
        private readonly IScreenshotProvider screenshotProvider;
        private readonly IBloogumClient bloogumClient;
        private readonly CancellationTokenSource cancellationToken = new();

        public string CreditText
        {
            get => this.GetTypedValue<string>(CreditTextProperty);
            set => this.SetValue(CreditTextProperty, value);
        }

        public MainWindow(
            IViewManager viewManager,
            IScreenshotProvider screenshotProvider,
            IBloogumClient bloogumClient)
        {
            this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
            this.screenshotProvider = screenshotProvider.ThrowIfNull(nameof(screenshotProvider));
            this.bloogumClient = bloogumClient.ThrowIfNull(nameof(bloogumClient));
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewManager.ShowView<MainView>();
            this.SetupImageCycle();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TitleBar_MinimizeButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.Titlebar.WindowState = WindowState.Minimized;
        }

        private void TitleBar_MaximizeButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            this.Titlebar.WindowState = WindowState.Maximized;
        }

        private void TitleBar_RestoreButtonClicked(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Titlebar.WindowState = WindowState.Normal;
        }

        private void TitleBar_CloseButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Border_OnResize(object sender, WCL.Border.ResizeDirection e)
        {
            NativeMethods.SendMessage(new WindowInteropHelper(this).Handle, NativeMethods.WM_SYSCOMMAND, (IntPtr)e, IntPtr.Zero);
        }

        private void SetupImageCycle()
        {
            TaskExtensions.RunPeriodicAsync(() => this.Dispatcher.Invoke(() => this.UpdateRandomImage()), TimeSpan.Zero, TimeSpan.FromSeconds(15), this.cancellationToken.Token);
        }

        private void UpdateRandomImage()
        {
            var maybeImage = this.screenshotProvider.GetRandomScreenShot();
            maybeImage.Do(
                onSome: (image) =>
                {
                    this.SetImage(image);
                    this.CreditText = string.Empty;
                },
                onNone: async () =>
                {
                    var maybeImageStream = await this.bloogumClient.GetRandomScreenShot().ConfigureAwait(true);
                    maybeImageStream.DoAny(
                        onSome: (stream) =>
                        {
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                            this.SetImage(bitmapImage);
                            this.CreditText = "http://bloogum.net/guildwars";
                        });
                });
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            this.viewManager.ShowView<SettingsView>();
        }

        private void CreditTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Uri.TryCreate(this.CreditText, UriKind.Absolute, out var uri))
            {
                Process.Start("explorer.exe", uri.ToString());
            }
        }

        private void SetImage(ImageSource imageSource)
        {
            this.ImageViewer.ShowImage(imageSource);
            if (imageSource is BitmapImage bitmapImage)
            {
                var avgColor = GetAverageColor(bitmapImage);
                var luminace = GetLuminace(avgColor);
                if (luminace < 0.15)
                {
                    this.Foreground = Brushes.White;
                }
                else
                {
                    this.Foreground = Brushes.Black;
                }
            }
        }

        private static Color GetAverageColor(BitmapSource bitmap)
        {
            var format = bitmap.Format;
            if (format != PixelFormats.Bgr24 &&
                    format != PixelFormats.Bgr32 &&
                    format != PixelFormats.Bgra32 &&
                    format != PixelFormats.Pbgra32)
            {
                throw new InvalidOperationException("BitmapSource must have Bgr24, Bgr32, Bgra32 or Pbgra32 format");
            }

            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;
            var numPixels = width * height;
            var bytesPerPixel = format.BitsPerPixel / 8;
            var pixelBuffer = new byte[numPixels * bytesPerPixel];
            bitmap.CopyPixels(pixelBuffer, width * bytesPerPixel, 0);
            long blue = 0;
            long green = 0;
            long red = 0;
            for (int i = 0; i < pixelBuffer.Length; i += bytesPerPixel)
            {
                blue += pixelBuffer[i];
                green += pixelBuffer[i + 1];
                red += pixelBuffer[i + 2];
            }

            return Color.FromRgb((byte)(red / numPixels), (byte)(green / numPixels), (byte)(blue / numPixels));
        }

        private static double GetLuminace(Color color)
        {
            double R = color.ScR;
            double G = color.ScG;
            double B = color.ScB;
            return 0.299 * R + 0.587 * G + 0.114 * B;
        }
    }
}
