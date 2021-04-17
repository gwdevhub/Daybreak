using Daybreak.Launch;
using Daybreak.Services.Configuration;
using Daybreak.Services.Logging;
using Daybreak.Utils;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Extensions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls
{
    /// <summary>
    /// Interaction logic for ChromiumBrowserWrapper.xaml
    /// </summary>
    public partial class ChromiumBrowserWrapper : UserControl
    {
        private const string BrowserDownloadLink = "https://developer.microsoft.com/en-us/microsoft-edge/webview2/";

        public readonly static DependencyProperty AddressProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, string>(nameof(Address));
        public readonly static DependencyProperty FavoriteAddressProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, string>(nameof(FavoriteAddress));
        public readonly static DependencyProperty NavigatingProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(Navigating));
        public readonly static DependencyProperty BrowserEnabledProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(BrowserEnabled));
        public readonly static DependencyProperty AddressBarReadonlyProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(AddressBarReadonly));
        public readonly static DependencyProperty BrowserSupportedProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(BrowserSupported), new PropertyMetadata(true));

        public event EventHandler<string> FavoriteUriChanged;
        public event EventHandler MaximizeClicked;

        private readonly IConfigurationManager configurationManager;
        private readonly ILogger logger;
        private CoreWebView2Environment coreWebView2Environment;

        public string Address
        {
            get => this.GetTypedValue<string>(AddressProperty);
            set => this.SetTypedValue(AddressProperty, value);
        }
        public string FavoriteAddress
        {
            get => this.GetTypedValue<string>(FavoriteAddressProperty);
            set => this.SetTypedValue(FavoriteAddressProperty, value);
        }
        public bool Navigating
        {
            get => this.GetTypedValue<bool>(NavigatingProperty);
            private set => this.SetTypedValue<bool>(NavigatingProperty, value);
        }
        public bool AddressBarReadonly
        {
            get => this.GetTypedValue<bool>(AddressBarReadonlyProperty);
            private set => this.SetTypedValue<bool>(AddressBarReadonlyProperty, value);
        }
        public bool BrowserEnabled
        {
            get => this.GetTypedValue<bool>(BrowserEnabledProperty);
            private set => this.SetTypedValue<bool>(BrowserEnabledProperty, value);
        }
        public bool BrowserSupported
        {
            get => this.GetTypedValue<bool>(BrowserSupportedProperty);
            private set => this.SetTypedValue<bool>(BrowserSupportedProperty, value);
        }

        public ChromiumBrowserWrapper()
        {
            this.configurationManager = Launcher.ApplicationServiceManager.GetService<IConfigurationManager>();
            this.logger = Launcher.ApplicationServiceManager.GetService<ILogger>();
            this.InitializeComponent();
            this.InitializeEnvironment();
            this.InitializeBrowser();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == AddressProperty || e.Property == FavoriteAddressProperty)
            {
                this.CheckFavoriteAddress();
            }
        }

        public async void ReinitializeBrowser()
        {
            await this.InitializeBrowser();
        }

        private void InitializeEnvironment()
        {
            if (this.configurationManager.GetConfiguration().BrowsersEnabled is false)
            {
                this.BrowserSupported = false;
                return;
            }

            try
            {
                this.coreWebView2Environment = System.Extensions.TaskExtensions.RunSync(() => CoreWebView2Environment.CreateAsync(null, "BrowserData", null));
                this.BrowserSupported = true;
            }
            catch(Exception e)
            {
                this.logger.LogWarning($"Browser initialization failed. Details: {e}");
                this.BrowserSupported = false;
            }
        }

        private async Task InitializeBrowser()
        {
            if (this.BrowserSupported is true)
            {
                await this.WebBrowser.EnsureCoreWebView2Async(this.coreWebView2Environment);
                this.AddressBarReadonly = this.configurationManager.GetConfiguration().AddressBarReadonly;
                this.WebBrowser.CoreWebView2.NewWindowRequested += (browser, args) => args.Handled = true;
                this.WebBrowser.NavigationStarting += (browser, args) => this.Navigating = true;
                this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
            }
        }

        private async void RetryInitializeButton_Clicked(object sender, EventArgs e)
        {
            this.InitializeEnvironment();
            try
            {
                await this.InitializeBrowser();
            }
            catch
            {
                this.BrowserSupported = false;
            }
        }

        private void Hyperlink_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Uri.TryCreate(BrowserDownloadLink, UriKind.Absolute, out var uri))
            {
                Process.Start("explorer.exe", uri.ToString());
            }
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var newAddress = sender.As<TextBox>().Text;
                newAddress = SanitizeAddress(newAddress);
                if (newAddress is null)
                {
                    return;
                }

                this.Address = newAddress;
                this.WebBrowser.CoreWebView2.Navigate(this.Address);
                e.Handled = true;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.Dispose();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.GoBack();
            this.Address = this.WebBrowser.Source.ToString();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.GoForward();
            this.Address = this.WebBrowser.Source.ToString();
        }

        private void RefreshGlyph_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.Reload();
            this.Address = this.WebBrowser.Source.ToString();
        }

        private void CancelGlyph_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.Stop();
        }

        private void StarGlyph_Clicked(object sender, EventArgs e)
        {
            this.FavoriteAddress = this.Address;
            this.FavoriteUriChanged?.Invoke(this, this.FavoriteAddress);
        }

        private void MaximizeButton_Clicked(object sender, EventArgs e)
        {
            this.MaximizeClicked?.Invoke(this, e);
        }

        private void CheckFavoriteAddress()
        {
            if (this.Address == this.FavoriteAddress)
            {
                this.FavoriteButton.IsEnabled = false;
            }
            else
            {
                this.FavoriteButton.IsEnabled = true;
            }
        }

        private static string SanitizeAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            if (address.StartsWith("www") is false &&
                address.StartsWith("http") is false)
            {
                address = "https://www." + address;
            }
            else if (address.StartsWith("www"))
            {
                address = "https://" + address;
            }

            Uri.TryCreate(address, UriKind.Absolute, out var uri);
            return uri?.ToString();
        }
    }
}
