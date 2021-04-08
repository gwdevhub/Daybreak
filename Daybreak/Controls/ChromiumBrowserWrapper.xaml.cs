using Daybreak.Launch;
using Daybreak.Services.Configuration;
using Microsoft.Web.WebView2.Core;
using System;
using System.Extensions;
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
        public readonly static DependencyProperty AddressProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, string>(nameof(Address));
        public readonly static DependencyProperty FavoriteAddressProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, string>(nameof(FavoriteAddress));
        public readonly static DependencyProperty NavigatingProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(Navigating));
        public readonly static DependencyProperty AddressBarReadonlyProperty = DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, bool>(nameof(AddressBarReadonly));

        public event EventHandler<string> FavoriteUriChanged;
        public event EventHandler MaximizeClicked;

        private readonly CoreWebView2Environment coreWebView2Environment;
        private readonly IConfigurationManager configurationManager;

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

        public ChromiumBrowserWrapper()
        {
            this.coreWebView2Environment = Launcher.ApplicationServiceManager.GetService<CoreWebView2Environment>();
            this.configurationManager = Launcher.ApplicationServiceManager.GetService<IConfigurationManager>();
            this.InitializeComponent();
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

        public void ReinitializeBrowser()
        {
            this.InitializeBrowser();
        }

        private async void InitializeBrowser()
        {
            await this.WebBrowser.EnsureCoreWebView2Async(this.coreWebView2Environment);
            this.AddressBarReadonly = this.configurationManager.GetConfiguration().AddressBarReadonly;
            this.WebBrowser.CoreWebView2.NewWindowRequested += (browser, args) => args.Handled = true;
            this.WebBrowser.NavigationStarting += (browser, args) => this.Navigating = true;
            this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
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
