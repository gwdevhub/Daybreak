using Daybreak.Configuration;
using Daybreak.Services.Bloogum;
using Daybreak.Services.IconRetrieve;
using Daybreak.Services.Menu;
using Daybreak.Services.Navigation;
using Daybreak.Services.Privilege;
using Daybreak.Services.Screens;
using Daybreak.Services.Screenshots;
using Daybreak.Services.Updater;
using Daybreak.Utils;
using Daybreak.Views;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Daybreak.Launch;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add read-only modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class MainWindow : Window
{
    private readonly IMenuServiceInitializer menuServiceInitializer;
    private readonly IViewManager viewManager;
    private readonly IScreenshotProvider screenshotProvider;
    private readonly IBloogumClient bloogumClient;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly IPrivilegeManager privilegeManager;
    private readonly IIconDownloader iconDownloader;
    private readonly ILiveOptions<ApplicationConfiguration> liveOptions;
    private readonly CancellationTokenSource cancellationToken = new();

    [GenerateDependencyProperty]
    private string creditText = string.Empty;
    [GenerateDependencyProperty]
    private string currentVersionText = string.Empty;
    [GenerateDependencyProperty]
    private bool isRunningAsAdmin;
    [GenerateDependencyProperty]
    private bool isShowingDropdown;

    public event EventHandler<MainWindow>? WindowParametersChanged;

    public MainWindow(
        IMenuServiceInitializer menuServiceInitializer,
        IViewManager viewManager,
        IScreenshotProvider screenshotProvider,
        IBloogumClient bloogumClient,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        IIconDownloader iconDownloader,
        ILiveOptions<ApplicationConfiguration> liveOptions)
    {
        this.menuServiceInitializer = menuServiceInitializer.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.screenshotProvider = screenshotProvider.ThrowIfNull();
        this.bloogumClient = bloogumClient.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.iconDownloader = iconDownloader.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.InitializeComponent();
        this.CurrentVersionText = this.applicationUpdater.CurrentVersion.ToString();
        this.IsRunningAsAdmin = this.privilegeManager.AdminPrivileges;
        this.SetupMenuService();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (this.IsLoaded is false)
        {
            return;
        }

        if (e.Property == LeftProperty ||
            e.Property == TopProperty ||
            e.Property == ActualWidthProperty ||
            e.Property == ActualHeightProperty)
        {
            this.WindowParametersChanged?.Invoke(this, this);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.SetupImageCycle();
        this.CheckForUpdates();
        this.SetupBackgroundBrowser();
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton is not MouseButton.Left &&
            e.LeftButton is not MouseButtonState.Pressed)
        {
            return;
        }

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

    private void SetupBackgroundBrowser()
    {
        this.iconDownloader.SetBrowser(this.BackgroundWebView);
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
        this.ToggleDropdownMenu();
    }

    private void OpenDropdownMenu()
    {
        if (this.IsShowingDropdown)
        {
            return;
        }

        this.ToggleDropdownMenu();
    }

    private void CloseDropdownMenu()
    {
        if (!this.IsShowingDropdown)
        {
            return;
        }

        this.ToggleDropdownMenu();
    }

    private void ToggleDropdownMenu()
    {
        var button = this.IsShowingDropdown ?
            this.ClosingSettingsButton :
            this.OpeningSettingsButton;
        button.IsEnabled = false;
        var widthAnimation = new DoubleAnimation
        {
            From = this.IsShowingDropdown ?
                this.MenuContainer.ActualWidth :
                0,
            To = this.IsShowingDropdown ?
                0 :
                300,
            Duration = new Duration(TimeSpan.FromMilliseconds(200)),
            DecelerationRatio = 0.7
        };

        var opacityAnimation = new DoubleAnimation
        {
            From = this.IsShowingDropdown ?
                this.MenuContainer.Opacity :
                0,
            To = this.IsShowingDropdown ?
                0 :
                1,
            Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            DecelerationRatio = 0.7
        };

        var storyBoard = new Storyboard();
        storyBoard.Children.Add(widthAnimation);
        Storyboard.SetTarget(widthAnimation, this.MenuContainer);
        Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Grid.WidthProperty));
        storyBoard.Children.Add(opacityAnimation);
        Storyboard.SetTarget(opacityAnimation, this.MenuContainer);
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Grid.OpacityProperty));

        storyBoard.Completed += (_, _) =>
        {
            button.IsEnabled = true;
            this.IsShowingDropdown = !this.IsShowingDropdown;
        };

        storyBoard.Begin();
    }

    private void CreditTextBox_MouseLeftButtonDown(object sender, EventArgs e)
    {
        if (Uri.TryCreate(this.CreditText, UriKind.Absolute, out var uri))
        {
            Process.Start("explorer.exe", uri.ToString());
        }
    }

    private void VersionText_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<VersionManagementView>();
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

    private async void CheckForUpdates()
    {
        var updateAvailable = this.liveOptions.Value.AutoCheckUpdate is true && await this.applicationUpdater.UpdateAvailable().ConfigureAwait(true);
        if (updateAvailable)
        {
            this.viewManager.ShowView<AskUpdateView>();
        }
        else
        {
            this.viewManager.ShowView<LauncherView>();
            this.PeriodicallyCheckForUpdates();
        }
    }

    private void PeriodicallyCheckForUpdates()
    {
        this.applicationUpdater.PeriodicallyCheckForUpdates();
    }

    private void SetupMenuService()
    {
        this.menuServiceInitializer.InitializeMenuService(this.OpenDropdownMenu, this.CloseDropdownMenu, this.ToggleDropdownMenu);
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
        return (0.299 * R) + (0.587 * G) + (0.114 * B);
    }
}
