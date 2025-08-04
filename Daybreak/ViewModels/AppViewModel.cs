using Daybreak.Attributes;
using Daybreak.Services.Themes;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using TrailBlazr.Services;
using WpfExtended.Blazor.Launch;

namespace Daybreak.ViewModels;
public sealed class AppViewModel
{
    private readonly IOptionsProvider optionsProvider;
    private readonly IMenuServiceProducer menuServiceProducer;
    private readonly IMenuServiceButtonHandler menuServiceButtonHandler;
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

    public List<MenuCategory> MenuCategories { get; private set; } = [];

    public WindowState WindowState => this.blazorHostWindow.WindowState;
    public bool IsAdmin => this.privilegeManager.AdminPrivileges;
    public string CurrentVersionText => this.applicationUpdater.CurrentVersion.ToString();
    public string CreditText { get; private set; } = string.Empty;

    public string AccentBaseColor { get; set; } = string.Empty;
    public string NeutralBaseColor { get; set; } = string.Empty;
    public float BaseLayerLuminace { get; set; } = 0.0f;

    public AppViewModel(
        IOptionsProvider optionsProvider,
        IMenuServiceProducer menuServiceProducer,
        IMenuServiceButtonHandler menuServiceButtonHandler,
        IViewManager viewManager,
        BlazorThemeInteropService blazorThemeInteropService,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        BlazorHostWindow blazorHostWindow)
    {
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.menuServiceProducer = menuServiceProducer.ThrowIfNull();
        this.menuServiceButtonHandler = menuServiceButtonHandler.ThrowIfNull();
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
        this.AsynchronouslyLoadMenu();
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

    public void OnError(ErrorEventArgs _)
    {
    }

    public void MenuButtonClicked(MenuButton menuButton)
    {
        this.menuServiceButtonHandler.HandleButton(menuButton);
    }

    private Task AsynchronouslyLoadMenu()
    {
        return Task.Factory.StartNew(() =>
        {
            this.LoadMenuCategories();
            this.RedrawRequested?.Invoke(this, EventArgs.Empty);
        }, TaskCreationOptions.LongRunning);
    }

    private void LoadMenuCategories()
    {
        foreach(var category in this.menuServiceProducer.GetCategories())
        {
            this.MenuCategories.Add(category);
            if (category.Name is "Settings")
            {
                foreach (var option in this.optionsProvider.GetRegisteredOptionDefinitions())
                {
                    //TODO: Handle option click
                    category.RegisterButton(
                        option.Name,
                        option.Description,
                        sp => sp.GetRequiredService<ViewManager>().ShowView<OptionView>(new RouteValueDictionary
                        {
                            ["optionName"] = option.Name
                        }));
                }
            }
        }
    }

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        this.WindowStateChanged?.Invoke(this, this.WindowState);
    }
}
