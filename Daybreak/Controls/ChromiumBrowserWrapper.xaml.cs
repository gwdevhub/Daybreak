using Daybreak.Launch;
using Daybreak.Models.Browser;
using Daybreak.Models.Builds;
using Daybreak.Services.BuildTemplates;
using Daybreak.Services.Configuration;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
    public partial class ChromiumBrowserWrapper : UserControl
    {
        private const string BrowserDownloadLink = "https://developer.microsoft.com/en-us/microsoft-edge/webview2/";

        public event EventHandler<string> FavoriteUriChanged;
        public event EventHandler MaximizeClicked;
        public event EventHandler<Build> BuildDecoded;

        private readonly IConfigurationManager configurationManager;
        private readonly ILogger<ChromiumBrowserWrapper> logger;
        private readonly IBuildTemplateManager buildTemplateManager;
        private CoreWebView2Environment coreWebView2Environment;

        [GenerateDependencyProperty(InitialValue = true)]
        private bool canDownloadBuild;
        [GenerateDependencyProperty(InitialValue = true)]
        private bool canNavigate;
        [GenerateDependencyProperty(InitialValue = true)]
        private bool controlsEnabled;
        [GenerateDependencyProperty(InitialValue = true)]
        private bool browserSupported;
        [GenerateDependencyProperty]
        private bool addressBarReadonly;
        [GenerateDependencyProperty]
        private bool browserEnabled;
        [GenerateDependencyProperty]
        private bool navigating;
        [GenerateDependencyProperty]
        private string favoriteAddress;
        [GenerateDependencyProperty]
        private string address;

        public ChromiumBrowserWrapper()
        {
            this.configurationManager = Launcher.ApplicationServiceManager.GetService<IConfigurationManager>();
            this.logger = Launcher.ApplicationServiceManager.GetService<ILogger<ChromiumBrowserWrapper>>();
            this.buildTemplateManager = Launcher.ApplicationServiceManager.GetService<IBuildTemplateManager>();
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
                this.CanDownloadBuild = this.configurationManager.GetConfiguration().ExperimentalFeatures.DynamicBuildLoading;
                this.WebBrowser.CoreWebView2.NewWindowRequested += (browser, args) => args.Handled = true;
                this.WebBrowser.NavigationStarting += (browser, args) =>
                {
                    if (this.CanNavigate is false && args.Uri != this.Address)
                    {
                        args.Cancel = true;
                    }
                    else
                    {
                        this.Navigating = true;
                    }
                };
                this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
                this.WebBrowser.WebMessageReceived += this.CoreWebView2_WebMessageReceived;
                this.WebBrowser.CoreWebView2.Settings.AreDevToolsEnabled = false;
                this.WebBrowser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                if (this.CanDownloadBuild)
                {
                    await this.WebBrowser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(Scripts.SendSelectionOnContextMenu);
                }
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

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            BrowserPayload payload = default;
            try
            {
                payload = args.WebMessageAsJson.Deserialize<BrowserPayload>();
            }
            catch(Exception e)
            {
                this.logger.LogError(e, $"Exception encountered when deserializing {nameof(BrowserPayload)}");
            }
            if (payload?.Key == BrowserPayload.PayloadKeys.ContextMenu)
            {
                var contextMenuPayload = args.WebMessageAsJson.Deserialize<BrowserPayload<OnContextMenuPayload>>();
                var maybeTemplate = contextMenuPayload.Value.Selection;
                if (string.IsNullOrWhiteSpace(maybeTemplate))
                {
                    return;
                }

                if (this.buildTemplateManager.IsTemplate(maybeTemplate) is false)
                {
                    return;
                }

                Task.Run(() =>
                {
                    try
                    {
                        var build = this.buildTemplateManager.DecodeTemplate(maybeTemplate);
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ContextMenu.DataContext = build;
                            this.ContextMenu.IsOpen = true;
                        });
                    }
                    catch(Exception e)
                    {
                        this.logger.LogWarning($"Exception when decoding template {maybeTemplate}. Details {e}");
                    }
                });
            }
        }

        private void LoadBuildTemplateButton_Click(object sender, EventArgs e)
        {
            var build = this.ContextMenu.DataContext.As<Build>();
            this.ContextMenu.IsOpen = false;
            this.BuildDecoded?.Invoke(this, build);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.WebBrowser?.Dispose();
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

        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            this.WebBrowser.CoreWebView2.Navigate(this.FavoriteAddress);
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
