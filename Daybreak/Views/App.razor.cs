using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Interop;
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
using WpfExtended.Blazor.Launch;

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
    private readonly BlazorHostWindow blazorHostWindow;
    private readonly JSConsoleInterop jsConsoleInterop;
    private readonly INotificationService notificationService;
    private readonly ILogger<App> logger;

    private HwndSource? hwndSource;

    public event EventHandler<WindowState>? WindowStateChanged;
    public event EventHandler? RedrawRequested;

    public Type CurrentViewType { get; private set; } = typeof(LaunchView);
    public IDictionary<string, object> CurrentViewParameters { get; private set; } =
        new Dictionary<string, object>();

    public List<MenuCategory> MenuCategories { get; private set; } = [];

    public WindowState WindowState => this.blazorHostWindow.WindowState;
    public bool IsAdmin => this.privilegeManager.AdminPrivileges;
    public bool IsNavigationOpen { get; private set; }
    public string CurrentVersionText => this.applicationUpdater.CurrentVersion.ToString();
    public INotificationProducer NotificationProducer { get; }

    public string AccentBaseColor { get; set; } = string.Empty;
    public string NeutralBaseColor { get; set; } = string.Empty;
    public float BaseLayerLuminace { get; set; } = 0.0f;
    public string BackdropImage { get; set; } = string.Empty;

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
        BlazorHostWindow blazorHostWindow,
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
        this.blazorHostWindow = blazorHostWindow.ThrowIfNull();
        this.jsConsoleInterop = jsConsoleInterop.ThrowIfNull();
        this.NotificationProducer = notificationProducer.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.blazorHostWindow.StateChanged += this.MainWindow_StateChanged;
        menuServiceInitializer.InitializeMenuService(
            this.OpenNavigationMenu,
            this.CloseNavigationMenu,
            this.ToggleNavigationMenu
        );
    }

    public async ValueTask InitializeApp(IJSRuntime jsRuntime)
    {
        this.themeManager.ThemeChanged += (s, e) => this.OnThemeChange();
        this.OnThemeChange();
        this.LoadMenuCategories();
        await this.jsConsoleInterop.InitializeConsoleRedirection(jsRuntime);
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
        this.viewManager.ShowView<LaunchView>();
        this.viewManager.ShowViewRequested += (s, e) => this.CloseNavigationMenu();
        this.hwndSource = HwndSource.FromHwnd(
            new WindowInteropHelper(this.blazorHostWindow).Handle
        );
    }

    public void Drag()
    {
        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */
        this.blazorHostWindow.Dispatcher.BeginInvoke(() =>
        {
            var hwnd = new WindowInteropHelper(this.blazorHostWindow).Handle;
            if (hwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.ReleaseCapture();
            NativeMethods.PostMessage(
                hwnd,
                NativeMethods.WM_SYSCOMMAND,
                (IntPtr)(NativeMethods.SC_MOVE | NativeMethods.HTCAPTION),
                IntPtr.Zero
            );
        });
    }

    public void StartResize(NativeMethods.ResizeDirection resizeDirection)
    {
        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */

        if (this.blazorHostWindow.WindowState == WindowState.Maximized)
        {
            return;
        }

        this.blazorHostWindow.Dispatcher.BeginInvoke(() =>
        {
            var hwnd = new WindowInteropHelper(this.blazorHostWindow).Handle;
            if (hwnd == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.ReleaseCapture();
            NativeMethods.PostMessage(
                hwnd,
                NativeMethods.WM_NCLBUTTONDOWN,
                (IntPtr)(int)resizeDirection,
                IntPtr.Zero
            );
        });
    }

    public void Minimize()
    {
        this.blazorHostWindow.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
        this.blazorHostWindow.WindowState = WindowState.Maximized;
    }

    public void Restore()
    {
        this.blazorHostWindow.WindowState = WindowState.Normal;
    }

    public void Close()
    {
        this.blazorHostWindow.Close();
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

    private void OnThemeChange()
    {
        this.BaseLayerLuminace = this.themeManager.BaseLayerLuminance;
        this.AccentBaseColor = this.themeManager.AccentBaseColorHex;
        this.NeutralBaseColor = this.themeManager.NeutralBaseColorHex;
        this.BackdropImage = this.themeManager.BackdropImage;
        this.XXSmallFontSize = this.themeManager.XXSmallFontSize;
        this.XSmallFontSize = this.themeManager.XSmallFontSize;
        this.SmallFontSize = this.themeManager.SmallFontSize;
        this.MediumFontSize = this.themeManager.MediumFontSize;
        this.LargeFontSize = this.themeManager.LargeFontSize;
        this.XLargeFontSize = this.themeManager.XLargeFontSize;
        this.XXLargeFontSize = this.themeManager.XXLargeFontSize;
        this.UIScale = this.themeManager.UIScale;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
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

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        this.WindowStateChanged?.Invoke(this, this.WindowState);
    }
}
