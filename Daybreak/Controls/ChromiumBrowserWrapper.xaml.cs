using Daybreak.Launch;
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

        public event EventHandler<string> FavoriteUriChanged;

        private readonly CoreWebView2Environment coreWebView2Environment;

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

        public ChromiumBrowserWrapper()
        {
            this.coreWebView2Environment = Launcher.ApplicationServiceManager.GetService<CoreWebView2Environment>();
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

        private async void InitializeBrowser()
        {
            await this.WebBrowser.EnsureCoreWebView2Async(this.coreWebView2Environment);
            this.WebBrowser.CoreWebView2.NewWindowRequested += (browser, args) => args.Handled = true;
            this.WebBrowser.NavigationStarting += (browser, args) => this.Navigating = true;
            this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.WebBrowser.Dispose();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.GoBack();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.GoForward();
        }

        private void RefreshGlyph_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.Reload();
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
    }
}
