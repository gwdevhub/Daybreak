using Daybreak.Configuration.Options;
using Daybreak.Models.Browser;
using Daybreak.Models.Guildwars;
using Daybreak.Services.BuildTemplates;
using Daybreak.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Controls;

/// <summary>
/// Interaction logic for ChromiumBrowserWrapper.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class ChromiumBrowserWrapper : UserControl
{
    public static readonly DependencyProperty AddressProperty =
        DependencyPropertyExtensions.Register<ChromiumBrowserWrapper, string>(nameof(Address));

    private const string BrowserSearchPlaceholder = "[PLACEHOLDER]";
    private const string BrowserSearchLink = $"https://www.google.com/search?q={BrowserSearchPlaceholder}";
    private const string BrowserDownloadLink = "https://developer.microsoft.com/en-us/microsoft-edge/webview2/";

    private static readonly Regex WebAddressRegex = new("^((http|ftp|https)://)?([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?", RegexOptions.Compiled);

    private static CoreWebView2Environment? coreWebView2Environment;

    public event EventHandler<string>? FavoriteUriChanged;
    public event EventHandler? MaximizeClicked;
    public event EventHandler<Build>? BuildDecoded;
    public event EventHandler<DownloadPayload>? DownloadingFile;

    private readonly Task initializationTask;
    private readonly ILiveOptions<BrowserOptions>? liveOptions;
    private readonly ILogger<ChromiumBrowserWrapper>? logger;
    private readonly IBuildTemplateManager? buildTemplateManager;
    
    [GenerateDependencyProperty(InitialValue = true)]
    private bool canDownloadBuild;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool canNavigate;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool canDownloadFiles;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool controlsEnabled;
    [GenerateDependencyProperty(InitialValue = null)]
    private bool? browserSupported;
    [GenerateDependencyProperty]
    private bool addressBarReadonly;
    [GenerateDependencyProperty]
    private bool browserEnabled;
    [GenerateDependencyProperty]
    private bool navigating;
    [GenerateDependencyProperty]
    private string favoriteAddress = string.Empty;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool preventDispose;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool homeButtonVisible = true;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool favoriteButtonVisible = true;
    [GenerateDependencyProperty]
    private string downloadsDirectory = string.Empty;

    public string Address
    {
        get => this.GetTypedValue<string>(AddressProperty);
        set
        {
            if (this.BrowserSupported is true)
            {
                this.SetValue(AddressProperty, value);
            }
        }
    }

    public ChromiumBrowserWrapper()
        : this(Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<BrowserOptions>>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILogger<ChromiumBrowserWrapper>>())
    {
    }

    public ChromiumBrowserWrapper(
        ILiveOptions<BrowserOptions> liveOptions,
        IBuildTemplateManager buildTemplateManager,
        ILogger<ChromiumBrowserWrapper> logger)
    {
        this.liveOptions = liveOptions.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.initializationTask = this.InitializeBrowser();

        this.InitializeComponent();
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == AddressProperty || e.Property == FavoriteAddressProperty)
        {
            this.CheckFavoriteAddress();
        }
    }

    public async Task ReinitializeBrowser()
    {
        await this.InitializeBrowser();
    }

    public Task InitializationTask() => this.initializationTask;

    private async void UserControl_Initialized(object sender, EventArgs e)
    {
        this.InitializeEnvironment();
        await this.InitializeBrowser();
    }

    private void InitializeEnvironment()
    {
        if (this.liveOptions!.Value.Enabled is false)
        {
            this.BrowserSupported = false;
            return;
        }

        try
        {
            coreWebView2Environment ??= System.Extensions.TaskExtensions.RunSync(() => CoreWebView2Environment.CreateAsync(null, "BrowserData", new CoreWebView2EnvironmentOptions
            {
                EnableTrackingPrevention = true,
                AllowSingleSignOnUsingOSPrimaryAccount = true
            }));

            this.BrowserSupported = true;
        }
        catch(Exception e)
        {
            this.logger!.LogWarning($"Browser initialization failed. Details: {e}");
            this.BrowserSupported = false;
        }
    }

    private async Task InitializeBrowser()
    {
        if (this.BrowserSupported is true)
        {
            this.WebBrowser.IsEnabled = true;
            this.BrowserEnabled = true;
            await this.WebBrowser.EnsureCoreWebView2Async(coreWebView2Environment);
            this.AddressBarReadonly = this.liveOptions!.Value.AddressBarReadonly;
            this.CanDownloadBuild = this.liveOptions.Value.DynamicBuildLoading;
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
            this.WebBrowser.CoreWebView2.DownloadStarting += (browser, args) =>
            {
                if (this.CanDownloadFiles is false)
                {
                    this.logger?.LogInformation("Downloads are disallowed. Cancelling download");
                    args.Cancel = true;
                }

                if (this.DownloadsDirectory.IsNullOrWhiteSpace())
                {
                    this.logger?.LogInformation("No downloads directory specified. Downloading to default directory");
                    return;
                }

                var fileName = Path.GetFileName(args.ResultFilePath);
                var finalPath = Path.GetFullPath(Path.Combine(this.DownloadsDirectory, fileName));
                var downloadPayload = new DownloadPayload { ResultingFilePath = finalPath, CanDownload = true };
                this.DownloadingFile?.Invoke(this, downloadPayload);
                if (!downloadPayload.CanDownload)
                {
                    args.Cancel = true;
                    this.logger?.LogInformation("Download blocked by handler. Cancelling download");
                    return;
                }

                args.ResultFilePath = finalPath;
            };
            this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
            this.WebBrowser.WebMessageReceived += this.CoreWebView2_WebMessageReceived!;
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
            var input = sender.As<TextBox>().Text;
            var maybeAddress = SanitizeAddress(input);
            // If input is address, navigate to address. Otherwise search for the text input in Google
            if (Uri.TryCreate(maybeAddress, UriKind.Absolute, out _))
            {
                this.Address = maybeAddress;
            }
            else
            {
                var address = BrowserSearchLink.Replace(BrowserSearchPlaceholder, maybeAddress);
                this.Address = address;
            }

            this.WebBrowser.CoreWebView2.Navigate(this.Address);
            e.Handled = true;
        }
    }

    private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
    {
        BrowserPayload payload = default!;
        try
        {
            payload = args.WebMessageAsJson.Deserialize<BrowserPayload>();
        }
        catch(Exception e)
        {
            this.logger!.LogError(e, $"Exception encountered when deserializing {nameof(BrowserPayload)}");
        }

        if (payload?.Key == BrowserPayload.PayloadKeys.ContextMenu)
        {
            var contextMenuPayload = args.WebMessageAsJson.Deserialize<BrowserPayload<OnContextMenuPayload>>();
            var maybeTemplate = contextMenuPayload.Value!.Selection;
            if (string.IsNullOrWhiteSpace(maybeTemplate))
            {
                return;
            }

            if (this.buildTemplateManager is null)
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
                    this.logger!.LogWarning($"Exception when decoding template {maybeTemplate}. Details {e}");
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
        if (this.PreventDispose)
        {
            return;
        }

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
            return default!;
        }

        if (WebAddressRegex.IsMatch(address) is false)
        {
            return address;
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
        return uri?.ToString() ?? string.Empty;
    }
}
