using System.Core.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Extensions;
using Daybreak.Services.Logging;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using TrailBlazr.Services;

namespace Daybreak.Views;

public sealed class AppViewModel
{
    private const string IssueUrl = "https://github.com/gwdevhub/Daybreak/issues/new";

    private readonly IOptionsProvider optionsProvider;
    private readonly IMenuServiceProducer menuServiceProducer;
    private readonly IMenuServiceButtonHandler menuServiceButtonHandler;
    private readonly IViewManager viewManager;
    private readonly IThemeManager themeManager;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly IPrivilegeManager privilegeManager;
    private readonly JSConsoleInterop jsConsoleInterop;
    private readonly INotificationService notificationService;
    private readonly ILogger<App> logger;

    private SemaphoreSlim themeChangeSemaphore = new(1, 1);
    private string? tradeChatThemeSetterScript = default;
    private bool isInitialized = false;
    //private HwndSource? hwndSource;

    //public event EventHandler<WindowState>? WindowStateChanged;
    public event EventHandler? RedrawRequested;

    public Type CurrentViewType { get; private set; } = typeof(LaunchView);
    public IDictionary<string, object> CurrentViewParameters { get; private set; } =
        new Dictionary<string, object>();

    public List<MenuCategory> MenuCategories { get; private set; } = [];

    //public WindowState WindowState { get; }
    public bool IsAdmin => this.privilegeManager.AdminPrivileges;
    public bool IsNavigationOpen { get; private set; }
    public string CurrentVersionText => this.applicationUpdater.CurrentVersion.ToString();
    public INotificationProducer NotificationProducer { get; }

    public string AccentBaseColor { get; set; } = string.Empty;
    public string NeutralBaseColor { get; set; } = string.Empty;
    public float BaseLayerLuminace { get; set; } = 0.0f;
    public string? BackdropImage { get; set; } = string.Empty;
    public string? BackdropImageFilter { get; set; } = string.Empty;
    public string? BackdropEmbed { get; set; } = string.Empty;

    public double UIScale { get; set; }
    public double XXSmallFontSize { get; set; }
    public double XSmallFontSize { get; set; }
    public double SmallFontSize { get; set; }
    public double MediumFontSize { get; set; }
    public double LargeFontSize { get; set; }
    public double XLargeFontSize { get; set; }
    public double XXLargeFontSize { get; set; }

    public AppViewModel(
        IOptionsProvider optionsProvider,
        IMenuServiceInitializer menuServiceInitializer,
        IMenuServiceProducer menuServiceProducer,
        IMenuServiceButtonHandler menuServiceButtonHandler,
        IViewManager viewManager,
        IThemeManager themeManager,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        //BlazorHostWindow blazorHostWindow,
        JSConsoleInterop jsConsoleInterop,
        INotificationProducer notificationProducer,
        INotificationService notificationService,
        ILogger<App> logger
    )
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.menuServiceProducer = menuServiceProducer.ThrowIfNull();
        this.menuServiceButtonHandler = menuServiceButtonHandler.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.themeManager = themeManager.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        //this.blazorHostWindow = blazorHostWindow.ThrowIfNull();
        this.jsConsoleInterop = jsConsoleInterop.ThrowIfNull();
        this.NotificationProducer = notificationProducer.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        //this.blazorHostWindow.StateChanged += this.MainWindow_StateChanged;
        menuServiceInitializer.InitializeMenuService(
            this.OpenNavigationMenu,
            this.CloseNavigationMenu,
            this.ToggleNavigationMenu
        );
        //this.blazorHostWindow.PreviewKeyUp += this.BlazorHostWindow_PreviewKeyDown;
    }

    //TODO: Fix F1 press handling
    //private void BlazorHostWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    //{
    //    if (e.Key is Key.F1)
    //    {
    //        this.viewManager.ShowView<WikiView>((nameof(WikiView.Page), "Home"));
    //    }
    //}

    public async ValueTask InitializeApp(IJSRuntime jsRuntime)
    {
        if (this.isInitialized)
        {
            return;
        }

        this.themeManager.ThemeChanged += (s, e) => this.OnThemeChange();
        this.OnThemeChange();
        this.LoadMenuCategories();
        await this.jsConsoleInterop.InitializeConsoleRedirection(jsRuntime);
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
        this.viewManager.ShowView<LaunchView>();
        this.viewManager.ShowViewRequested += (s, e) => this.CloseNavigationMenu();
        //this.hwndSource = HwndSource.FromHwnd(
        //    new WindowInteropHelper(this.blazorHostWindow).Handle
        //);

        //// Hook WndProc to handle WM_NCHITTEST for custom title bar
        //this.hwndSource?.AddHook(this.WndProc);

        this.isInitialized = true;
    }

    /// <summary>
    /// Handles Windows messages to ensure the custom title bar area is treated as client area,
    /// preventing Windows from intercepting mouse events for native window buttons.
    /// </summary>
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        //TODO: Fix WndProc
        if (msg == NativeMethods.WM_NCHITTEST)
        {
            // Get the mouse position from lParam
            var x = (short)(lParam.ToInt32() & 0xFFFF);
            var y = (short)((lParam.ToInt32() >> 16) & 0xFFFF);

            // Convert screen coordinates to window coordinates
            var point = new Point(x, y);
            //point = this.blazorHostWindow.PointFromScreen(point);

            // Define the title bar height (should match the CSS title bar height of 40px)
            // Account for DPI scaling
            //var dpi = VisualTreeHelper.GetDpi(this.blazorHostWindow);
            //var titleBarHeight = 40 * dpi.DpiScaleY;

            // If the mouse is in the title bar area, return HTCLIENT to let WebView2 handle it
            // This prevents Windows from returning HTMINBUTTON, HTMAXBUTTON, or HTCLOSE
            //if (point.Y >= 0 && point.Y < titleBarHeight)
            //{
            //    handled = true;
            //    return new IntPtr(NativeMethods.HTCLIENT);
            //}
        }

        return IntPtr.Zero;
    }

    public void Drag()
    {
        //TODO: Fix Drag

        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */
        //this.blazorHostWindow.Dispatcher.BeginInvoke(() =>
        //{
        //    var hwnd = new WindowInteropHelper(this.blazorHostWindow).Handle;
        //    if (hwnd == IntPtr.Zero)
        //    {
        //        return;
        //    }

        //    NativeMethods.ReleaseCapture();
        //    NativeMethods.PostMessage(
        //        hwnd,
        //        NativeMethods.WM_SYSCOMMAND,
        //        (IntPtr)(NativeMethods.SC_MOVE | NativeMethods.HTCAPTION),
        //        IntPtr.Zero
        //    );
        //});
    }

    public void StartResize(NativeMethods.ResizeDirection resizeDirection)
    {
        //TODO: Fix StartResize

        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */

        //if (this.blazorHostWindow.WindowState == WindowState.Maximized)
        //{
        //    return;
        //}

        //this.blazorHostWindow.Dispatcher.BeginInvoke(() =>
        //{
        //    var hwnd = new WindowInteropHelper(this.blazorHostWindow).Handle;
        //    if (hwnd == IntPtr.Zero)
        //    {
        //        return;
        //    }

        //    NativeMethods.ReleaseCapture();
        //    NativeMethods.PostMessage(
        //        hwnd,
        //        NativeMethods.WM_NCLBUTTONDOWN,
        //        (IntPtr)(int)resizeDirection,
        //        IntPtr.Zero
        //    );
        //});
    }

    public void Minimize()
    {
        //TODO: Fix Minimize
        // this.blazorHostWindow.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
        //TODO: Fix Maximize
        // this.blazorHostWindow.WindowState = WindowState.Maximized;
    }

    public void Restore()
    {
        //TODO: Fix Restore
        //this.blazorHostWindow.WindowState = WindowState.Normal;
    }

    public void Close()
    {
        //TODO: Fix Close
        //this.blazorHostWindow.Close();
    }

    public void ToggleNavigationMenu()
    {
        this.IsNavigationOpen = !this.IsNavigationOpen;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
    }

    public void OpenNavigationMenu()
    {
        this.IsNavigationOpen = true;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
    }

    public void CloseNavigationMenu()
    {
        this.IsNavigationOpen = false;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
    }

    public void OpenIssues()
    {
        try
        {
            Process.Start(new ProcessStartInfo { FileName = IssueUrl, UseShellExecute = true });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Encountered exception while opening issues page");
        }
    }

    public void OpenSynchronizationView()
    {
        this.viewManager.ShowView<SettingsSynchronizationView>();
    }

    public void OnError(ErrorEventArgs eventArgs)
    {
        if (eventArgs.GetException() is Exception ex)
        {
            this.notificationService.NotifyError<MessageBoxHandler>(
                title: "Caught unhandled render exception",
                description: ex.ToString()
            );
        }
        else
        {
            this.notificationService.NotifyError<MessageBoxHandler>(
                title: "Caught unhandled render exception",
                description: "Could not retrieve exception"
            );
        }
    }

    public void MenuButtonClicked(MenuButton menuButton)
    {
        this.menuServiceButtonHandler.HandleButton(menuButton);
    }

    public void RestartAsNormalUser()
    {
        this.privilegeManager.RequestNormalPrivileges<LaunchView>(
            "You are currently running Daybreak as administrator",
            default,
            CancellationToken.None
        );
    }

    private async void OnThemeChange()
    {
        using var ctx = await this.themeChangeSemaphore.Acquire();

        this.BackdropImage = !string.IsNullOrWhiteSpace(this.themeManager.BackdropImage) ? this.GetBackdropImageUrl(this.themeManager.BackdropImage) : default;
        this.BackdropEmbed = !string.IsNullOrWhiteSpace(this.themeManager.BackdropEmbed) ? this.themeManager.BackdropEmbed : default;
        this.BackdropImageFilter = this.themeManager.CurrentTheme?.Filter ?? string.Empty;
        this.BaseLayerLuminace = this.themeManager.BaseLayerLuminance;
        this.AccentBaseColor = this.themeManager.AccentBaseColorHex;
        this.NeutralBaseColor = this.themeManager.NeutralBaseColorHex;
        this.XXSmallFontSize = this.themeManager.XXSmallFontSize;
        this.XSmallFontSize = this.themeManager.XSmallFontSize;
        this.SmallFontSize = this.themeManager.SmallFontSize;
        this.MediumFontSize = this.themeManager.MediumFontSize;
        this.LargeFontSize = this.themeManager.LargeFontSize;
        this.XLargeFontSize = this.themeManager.XLargeFontSize;
        this.XXLargeFontSize = this.themeManager.XXLargeFontSize;
        this.UIScale = this.themeManager.UIScale;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);


        //TODO: Fix Trade Chat Theme Setter
        /*
         * Below code hooks into the webview to set the trade chat theme based on the current application theme.
         */
        //await this.blazorHostWindow.Dispatcher.InvokeAsync(async () =>
        //{
        //    if (Global.CoreWebView2 is null)
        //    {
        //        return;
        //    }

        //    if (this.tradeChatThemeSetterScript is not null)
        //    {
        //        Global.CoreWebView2.Cast<CoreWebView2>().RemoveScriptToExecuteOnDocumentCreated(this.tradeChatThemeSetterScript);
        //        this.tradeChatThemeSetterScript = default;
        //    }

        //    this.tradeChatThemeSetterScript = await Global.CoreWebView2.Cast<CoreWebView2>().AddScriptToExecuteOnDocumentCreatedAsync(@$"
        //    if (location.origin === 'https://kamadan.gwtoolbox.com' || 
        //        location.origin === 'https://ascalon.gwtoolbox.com') {{
        //        localStorage.setItem('mode', '{(this.themeManager.CurrentTheme?.Mode is LightDarkMode.Light ? "light" : "dark")}');
        //    }}");
        //});
    }

    private void LoadMenuCategories()
    {
        foreach (var category in this.menuServiceProducer.GetCategories())
        {
            this.MenuCategories.Add(category);
            if (category.Name is "Settings")
            {
                foreach (var option in this.optionsProvider.GetRegisteredOptionDefinitions())
                {
                    category.RegisterButton(
                        option.Name,
                        option.Description,
                        sp =>
                            sp.GetRequiredService<ViewManager>()
                                .ShowView<OptionView>(("optionName", option.Name))
                    );
                }
            }
        }
    }

    // Fix window state change handling
    //private void MainWindow_StateChanged(object? sender, EventArgs e)
    //{
    //    this.WindowStateChanged?.Invoke(this, this.WindowState);
    //}

    private string GetBackdropImageUrl(string backdropPath)
    {
        if (string.IsNullOrWhiteSpace(backdropPath))
        {
            return string.Empty;
        }

        // Relative paths are served from wwwroot
        if (!Path.IsPathRooted(backdropPath))
        {
            var normalizedPath = backdropPath.Replace('\\', '/');
            if (!normalizedPath.StartsWith('/'))
            {
                normalizedPath = "/" + normalizedPath;
            }

            return $"url('{normalizedPath}')";
        }

        // Absolute paths need to be embedded as base64
        return this.ConvertImageToBase64DataUri(backdropPath);
    }

    private string ConvertImageToBase64DataUri(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                this.logger.LogWarning("Backdrop file does not exist: {FilePath}", filePath);
                return string.Empty;
            }

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var mimeType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };

            var imageBytes = File.ReadAllBytes(filePath);
            var base64 = System.Convert.ToBase64String(imageBytes);
            return $"url('data:{mimeType};base64,{base64}')";
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to convert image to base64: {FilePath}", filePath);
            return string.Empty;
        }
    }
}
