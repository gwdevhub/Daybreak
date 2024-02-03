using Daybreak.Configuration;
using Daybreak.Configuration.Options;
using Daybreak.Models;
using Daybreak.Models.Browser;
using Daybreak.Models.Builds;
using Daybreak.Models.Guildwars;
using Daybreak.Services.Browser;
using Daybreak.Services.BuildTemplates;
using Daybreak.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

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

    private const long MaxBuildBytes = 200;
    private const string BrowserSearchPlaceholder = "[PLACEHOLDER]";
    private const string BrowserSearchLink = $"https://www.google.com/search?q={BrowserSearchPlaceholder}";
    private const string BrowserDownloadLink = "https://developer.microsoft.com/en-us/microsoft-edge/webview2/";

    private static readonly Regex WebAddressRegex = BuildWebAddressRegex();
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    private static CoreWebView2Environment? CoreWebView2Environment;

    public event EventHandler<string>? FavoriteUriChanged;
    public event EventHandler? MaximizeClicked;
    public event EventHandler<DownloadedBuild>? BuildDecoded;
    public event EventHandler<DownloadPayload>? DownloadingFile;
    public event EventHandler<string>? DownloadedFile;

    private readonly HashSet<ulong> domNavigationIds = [];
    private readonly Task initializationTask;
    private readonly IHttpClient<ChromiumBrowserWrapper> httpClient;
    private readonly ILiveOptions<BrowserOptions> liveOptions;
    private readonly ILogger<ChromiumBrowserWrapper> logger;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IBrowserHistoryManager historyManager;
    
    private readonly IBrowserExtensionsManager browserExtensionsManager;

    [GenerateDependencyProperty(InitialValue = true)]
    private bool canDownloadBuild;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool canNavigate;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool canDownloadFiles;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool controlsEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool browserSupported = false;
    [GenerateDependencyProperty]
    private bool addressBarReadonly;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool browserEnabled = false;
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
    [GenerateDependencyProperty]
    private bool showBrowserDisabledMessage;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool showDownloadsDialog = true;

    private bool browserInitialized = false;

    public string Address
    {
        get => this.GetTypedValue<string>(AddressProperty);
        set => this.SetValue(AddressProperty, value);
    }

    public IBrowserHistoryManager BrowserHistoryManager => this.historyManager;

    public ChromiumBrowserWrapper()
        : this(
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBrowserHistoryManager>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBrowserExtensionsManager>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IHttpClient<ChromiumBrowserWrapper>>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILiveOptions<BrowserOptions>>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<IBuildTemplateManager>(),
              Launch.Launcher.Instance.ApplicationServiceProvider.GetRequiredService<ILogger<ChromiumBrowserWrapper>>())
    {
    }

    public ChromiumBrowserWrapper(
        IBrowserHistoryManager historyManager,
        IBrowserExtensionsManager browserExtensionsManager,
        IHttpClient<ChromiumBrowserWrapper> httpClient,
        ILiveOptions<BrowserOptions> liveOptions,
        IBuildTemplateManager buildTemplateManager,
        ILogger<ChromiumBrowserWrapper> logger)
    {
        this.historyManager = historyManager.ThrowIfNull();
        this.browserExtensionsManager = browserExtensionsManager.ThrowIfNull();
        this.httpClient = httpClient.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();

        this.historyManager.InitializeHistoryManager(this);
        this.initializationTask = Task.Run(this.InitializeBrowserSafe);
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == AddressProperty)
        {
            if (this.BrowserSupported is true &&
                this.browserInitialized is true &&
                this.WebBrowser is not null &&
                Uri.TryCreate(this.Address, UriKind.RelativeOrAbsolute, out var uri))
            {
                this.WebBrowser.Source = uri;
            }
        }
        else if (e.Property == FavoriteAddressProperty)
        {
            this.CheckFavoriteAddress();
        }
    }

    public async Task ReinitializeBrowser()
    {
        await this.InitializeBrowserSafe();
    }

    public Task InitializationTask() => this.initializationTask;

    private void InitializeEnvironment()
    {
        try
        {
            if (CoreWebView2Environment is not null)
            {
                this.BrowserSupported = true;
                return;
            }

            CoreWebView2Environment ??= System.Extensions.TaskExtensions.RunSync(() => CoreWebView2Environment.CreateAsync(null, "BrowserData", new CoreWebView2EnvironmentOptions
            {
                EnableTrackingPrevention = true,
                AllowSingleSignOnUsingOSPrimaryAccount = true,
                AreBrowserExtensionsEnabled = true,
            }));

            this.BrowserSupported = true;
        }
        catch (Exception e)
        {
            this.logger!.LogWarning($"Browser initialization failed. Details: {e}");
            this.BrowserSupported = false;
        }
    }

    private async Task InitializeBrowserSafe()
    {
        await this.Dispatcher.InvokeAsync(async () =>
        {
            await SemaphoreSlim.WaitAsync();

            try
            {
                this.InitializeEnvironment();
                await this.InitializeBrowser().ConfigureAwait(true);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        });
    }

    private async Task InitializeBrowser()
    {
        this.BrowserEnabled = this.liveOptions!.Value.Enabled;
        this.ShowBrowserDisabledMessage = this.BrowserSupported && !this.BrowserEnabled;
        if (!this.BrowserSupported ||
            !this.BrowserEnabled)
        {

            return;
        }
        
        if (this.WebBrowser.CoreWebView2 is not null)
        {
            return;
        }

        this.ShowBrowserDisabledMessage = false;
        await this.WebBrowser.EnsureCoreWebView2Async(CoreWebView2Environment).ConfigureAwait(true);
        await this.browserExtensionsManager.InitializeBrowserEnvironment(this.WebBrowser.CoreWebView2!.Profile, CoreWebView2Environment!.BrowserVersionString).ConfigureAwait(true);
        if (this.Address is not null &&
            Uri.TryCreate(this.Address, UriKind.RelativeOrAbsolute, out var uri))
        {
            this.WebBrowser.Source = uri;
        }

        this.WebBrowser.SourceChanged += (_, args) => this.Address = this.WebBrowser.Source.ToString();
        this.AddressBarReadonly = this.liveOptions!.Value.AddressBarReadonly;
        this.CanDownloadBuild = this.liveOptions.Value.DynamicBuildLoading;
        this.WebBrowser.CoreWebView2.NewWindowRequested += (browser, args) => args.Handled = true;
        this.WebBrowser.KeyDown += (sender, e) =>
        {
            if ((e.Key == Key.Left && Keyboard.Modifiers == ModifierKeys.Alt) ||
                (e.Key == Key.BrowserBack))
            {
                this.BrowserHistoryManager.GoBack();
                e.Handled = true; // Prevent default behavior
            }
            else if ((e.Key == Key.Right && Keyboard.Modifiers == ModifierKeys.Alt) ||
                     (e.Key == Key.BrowserForward))
            {
                this.BrowserHistoryManager.GoForward();
                e.Handled = true; // Prevent default behavior
            }
        };
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
            if (!this.CanDownloadFiles)
            {
                this.logger?.LogInformation("Downloads are disallowed. Cancelling download");
                args.Cancel = true;
                this.CheckForBuildFile(args.DownloadOperation.Uri);
                return;
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

            args.DownloadOperation.StateChanged += this.DownloadOperation_StateChanged;
        };
        this.WebBrowser.NavigationCompleted += (browser, args) => this.Navigating = false;
        this.WebBrowser.WebMessageReceived += this.CoreWebView2_WebMessageReceived!;
        this.WebBrowser.CoreWebView2.IsDefaultDownloadDialogOpenChanged += (browser, args) =>
        {
            if (this.WebBrowser.CoreWebView2.IsDefaultDownloadDialogOpen &&
                !this.ShowDownloadsDialog)
            {
                this.WebBrowser.CoreWebView2.CloseDefaultDownloadDialog();
            }
        };
        this.WebBrowser.CoreWebView2.DOMContentLoaded += async (browser, args) =>
        {
            if (this.domNavigationIds.Contains(args.NavigationId))
            {
                return;
            }

            this.domNavigationIds.Add(args.NavigationId);
            await this.WebBrowser.CoreWebView2.ExecuteScriptAsync(Scripts.CaptureNavigationButtons);
            if (this.CanDownloadBuild)
            {
                await this.WebBrowser.CoreWebView2.ExecuteScriptAsync(Scripts.SendSelectionOnContextMenu);
            }
        };

        this.WebBrowser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
        if (this.CanDownloadBuild)
        {
            await this.WebBrowser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(Scripts.SendSelectionOnContextMenu);
        }

        this.browserInitialized = true;
    }

    private async void CheckForBuildFile(string source)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckForBuildFile), source);
        if (!this.CanDownloadBuild)
        {
            return;
        }

        if (source.IsNullOrWhiteSpace())
        {
            return;
        }

        var result = await this.httpClient.GetAsync(source);
        if (result.Content.Headers.ContentLength > MaxBuildBytes)
        {
            this.logger.LogInformation($"Content size [{result.Content.Headers.ContentLength}] exceeds max size [{MaxBuildBytes}]");
            result.Dispose();
            return;
        }

        var content = await result.Content.ReadAsStringAsync();
        if (!this.buildTemplateManager.TryDecodeTemplate(content, out var build))
        {
            this.logger.LogInformation("Could not decode downloaded file");
            return;
        }

        build.SourceUrl = this.Address;
        this.BuildDecoded?.Invoke(this, new DownloadedBuild
        {
            Build = build,
            PreferredName = result.Content.Headers.ContentDisposition?.FileName
        });
    }

    private async void RetryInitializeButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await this.InitializeBrowserSafe();
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
            var input = sender.As<TextBox>()?.Text;
            if (input is null)
            {
                return;
            }

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
        BrowserPayload? payload = default!;
        try
        {
            payload = args.WebMessageAsJson.Deserialize<BrowserPayload>();
        }
        catch (Exception e)
        {
            this.logger!.LogError(e, $"Exception encountered when deserializing {nameof(BrowserPayload)}");
        }

        if (payload?.Key is BrowserPayload.PayloadKeys.ContextMenu)
        {
            var contextMenuPayload = args.WebMessageAsJson.Deserialize<BrowserPayload<OnContextMenuPayload>>();
            var maybeTemplate = contextMenuPayload?.Value?.Selection?.Trim();
            if (string.IsNullOrWhiteSpace(maybeTemplate))
            {
                return;
            }

            if (this.buildTemplateManager is null)
            {
                return;
            }

            if (this.buildTemplateManager.TryDecodeTemplate(maybeTemplate, out var build) is false)
            {
                return;
            }

            build.SourceUrl = contextMenuPayload?.Value?.Url;
            Task.Run(() =>
            {
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ContextMenu.DataContext = build;
                        this.ContextMenu.IsOpen = true;
                    });
                }
                catch (Exception e)
                {
                    this.logger!.LogWarning($"Exception when decoding template {maybeTemplate}. Details {e}");
                }
            });
        }
        else if (payload?.Key is BrowserPayload.PayloadKeys.XButton1Pressed)
        {
            this.BrowserHistoryManager.GoBack();
        }
        else if (payload?.Key is BrowserPayload.PayloadKeys.XButton2Pressed)
        {
            this.BrowserHistoryManager.GoForward();
        }
    }

    private void DownloadOperation_StateChanged(object? sender, object? e)
    {
        if (sender is not CoreWebView2DownloadOperation downloadOperation)
        {
            return;
        }

        if (downloadOperation.State != CoreWebView2DownloadState.Completed &&
            downloadOperation.State != CoreWebView2DownloadState.Interrupted)
        {
            return;
        }

        downloadOperation.StateChanged -= this.DownloadOperation_StateChanged;
        var filePath = Path.GetFullPath(downloadOperation.ResultFilePath);
        this.DownloadedFile?.Invoke(this, filePath);
    }

    private void LoadBuildTemplateButton_Click(object sender, EventArgs e)
    {
        var build = this.ContextMenu.DataContext.As<IBuildEntry>();
        this.ContextMenu.IsOpen = false;
        if (build is null)
        {
            return;
        }

        this.BuildDecoded?.Invoke(this, new DownloadedBuild { Build = build, PreferredName = this.WebBrowser.CoreWebView2.DocumentTitle });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await this.InitializeBrowserSafe();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        if (this.PreventDispose)
        {
            return;
        }

        this.historyManager.UnInitializeHistoryManager();
        this.WebBrowser?.Dispose();
        this.browserInitialized = false;
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.historyManager.GoBack();
    }

    private void ForwardButton_Clicked(object sender, EventArgs e)
    {
        this.historyManager.GoForward();
    }

    private void RefreshGlyph_Clicked(object sender, EventArgs e)
    {
        this.BrowserHistoryManager.Reload();
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

    private void Browser_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        
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

    [GeneratedRegex("^((http|ftp|https)://)?([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?", RegexOptions.Compiled)]
    private static partial Regex BuildWebAddressRegex();
}
