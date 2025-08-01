using Daybreak.Services.Themes;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Interop;
using TrailBlazr.Services;
using WpfExtended.Blazor.Launch;

namespace Daybreak.ViewModels;
public sealed class AppViewModel
{
    private readonly IViewManager viewManager;
    private readonly BlazorThemeInteropService blazorThemeInteropService;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly IPrivilegeManager privilegeManager;
    private readonly BlazorHostWindow blazorHostWindow;

    private HwndSource? hwndSource;

    public event EventHandler<WindowState>? WindowStateChanged;
    public event EventHandler? RedrawRequested;

    public Type CurrentViewType { get; private set; } = typeof(LaunchView);
    public IDictionary<string, object> CurrentViewParameters { get; private set; } = new Dictionary<string, object>();

    public WindowState WindowState => this.blazorHostWindow.WindowState;
    public bool IsAdmin => this.privilegeManager.AdminPrivileges;
    public string CurrentVersionText => this.applicationUpdater.CurrentVersion.ToString();
    public string CreditText { get; private set; } = string.Empty;

    public string AccentBaseColor { get; set; } = string.Empty;
    public string NeutralBaseColor { get; set; } = string.Empty;
    public float BaseLayerLuminace { get; set; } = 0.0f;

    public AppViewModel(
        IViewManager viewManager,
        BlazorThemeInteropService blazorThemeInteropService,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        BlazorHostWindow blazorHostWindow)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.blazorThemeInteropService = blazorThemeInteropService.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.blazorHostWindow = blazorHostWindow.ThrowIfNull();

        this.blazorHostWindow.StateChanged += this.MainWindow_StateChanged;
    }

    public void InitializeApp()
    {
        this.blazorThemeInteropService.InitializeTheme();
        this.BaseLayerLuminace = this.blazorThemeInteropService.BaseLayerLuminance;
        this.AccentBaseColor = this.blazorThemeInteropService.AccentBaseColor;
        this.NeutralBaseColor = this.blazorThemeInteropService.NeutralBaseColor;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);
        this.viewManager.ShowView<LaunchView>();
        this.hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this.blazorHostWindow).Handle);
    }

    public void Drag()
    {
        NativeMethods.ReleaseCapture();
        NativeMethods.SendMessage(this.hwndSource?.Handle ?? 0, NativeMethods.WM_NCLBUTTONDOWN, new IntPtr(NativeMethods.HTCAPTION), IntPtr.Zero);
    }

    public void StartResize(NativeMethods.ResizeDirection resizeDirection)
    {
        if (this.blazorHostWindow.WindowState == WindowState.Maximized)
        {
            return; // Don't allow resizing when maximized
        }

        NativeMethods.ReleaseCapture();
        NativeMethods.SendMessage(this.hwndSource?.Handle ?? 0, NativeMethods.WM_NCLBUTTONDOWN, new IntPtr((int)resizeDirection), IntPtr.Zero);
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

    public void OpenCreditLink()
    {

    }

    public void OpenIssues()
    {

    }

    public void OpenSynchronizationView()
    {

    }

    public void OpenVersionView()
    {

    }

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        this.WindowStateChanged?.Invoke(this, this.WindowState);
    }
}
