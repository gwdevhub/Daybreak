using Daybreak.Launch;
using Daybreak.Services.Themes;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.AspNetCore.Components;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Interop;

namespace Daybreak.ViewModels;
public sealed class AppViewModel
{
    private readonly IBlazorViewManager blazorViewManager;
    private readonly BlazorThemeInteropService blazorThemeInteropService;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly IPrivilegeManager privilegeManager;
    private readonly MainWindow mainWindow;

    public event EventHandler<WindowState>? WindowStateChanged;
    public event EventHandler? RedrawRequested;

    public Type CurrentViewType { get; private set; } = typeof(LaunchView);
    public IDictionary<string, object> CurrentViewParameters { get; private set; } = new Dictionary<string, object>();

    public WindowState WindowState => this.mainWindow.WindowState;
    public bool IsAdmin => this.privilegeManager.AdminPrivileges;
    public string CurrentVersionText => this.applicationUpdater.CurrentVersion.ToString();
    public string CreditText { get; private set; } = string.Empty;

    public string AccentBaseColor { get; set; } = string.Empty;
    public string NeutralBaseColor { get; set; } = string.Empty;
    public float BaseLayerLuminace { get; set; } = 0.0f;

    public AppViewModel(
        IBlazorViewManager blazorViewManager,
        BlazorThemeInteropService blazorThemeInteropService,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        MainWindow mainWindow)
    {
        this.blazorViewManager = blazorViewManager.ThrowIfNull();
        this.blazorThemeInteropService = blazorThemeInteropService.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.mainWindow = mainWindow.ThrowIfNull();

        this.mainWindow.StateChanged += this.MainWindow_StateChanged;
    }

    public async Task InitializeApp()
    {
        this.blazorThemeInteropService.InitializeTheme();
        this.BaseLayerLuminace = this.blazorThemeInteropService.BaseLayerLuminance;
        this.AccentBaseColor = this.blazorThemeInteropService.AccentBaseColor;
        this.NeutralBaseColor = this.blazorThemeInteropService.NeutralBaseColor;
        this.RedrawRequested?.Invoke(this, EventArgs.Empty);

        await Task.Delay(1500);
        this.blazorViewManager.ShowView<LaunchView, LaunchViewModel>();
    }

    public void Drag()
    {
        var handle = new WindowInteropHelper(this.mainWindow).Handle;
        NativeMethods.ReleaseCapture();
        NativeMethods.SendMessage(handle, NativeMethods.WM_NCLBUTTONDOWN, new IntPtr(NativeMethods.HTCAPTION), IntPtr.Zero);
    }

    public void Minimize()
    {
        this.mainWindow.WindowState = WindowState.Minimized;
    }

    public void Maximize()
    {
        this.mainWindow.WindowState = WindowState.Maximized;
    }

    public void Restore()
    {
        this.mainWindow.WindowState = WindowState.Normal;
    }

    public void Close()
    {
        this.mainWindow.Close();
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

    public void SetView<TView>(object? dataContext)
        where TView : ComponentBase
    {
        this.CurrentViewParameters = new Dictionary<string, object>() { { "DataContext", dataContext ?? new object() } };
        this.CurrentViewType = typeof(TView);
    }

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        this.WindowStateChanged?.Invoke(this, this.WindowState);
    }
}
