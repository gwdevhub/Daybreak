using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Menu;
using Daybreak.Shared.Services.Keyboard;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Updater;
using Daybreak.Shared.Services.Window;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Photino.Blazor;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using TrailBlazr.Services;

namespace Daybreak.Views;

public sealed class AppViewModel
{
    private const string IssueUrl = "https://github.com/gwdevhub/Daybreak/issues/new";

    private readonly IKeyboardHookService keyboardHookService;
    private readonly IOptionsProvider optionsProvider;
    private readonly IMenuServiceInitializer menuServiceInitializer;
    private readonly IMenuServiceButtonHandler menuServiceButtonHandler;
    private readonly IViewManager viewManager;
    private readonly IThemeManager themeManager;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly IPrivilegeManager privilegeManager;
    private readonly PhotinoBlazorApp photinoApp;
    private readonly JSConsoleInterop jsConsoleInterop;
    private readonly INotificationService notificationService;
    private readonly IWindowManipulationService windowManipulationService;
    private readonly ILogger<App> logger;

    private SemaphoreSlim themeChangeSemaphore = new(1, 1);
    private bool isInitialized = false;

    public event EventHandler<WindowState>? WindowStateChanged;
    public event EventHandler? RedrawRequested;

    public Type CurrentViewType { get; private set; } = typeof(LaunchView);
    public IDictionary<string, object> CurrentViewParameters { get; private set; } =
        new Dictionary<string, object>();

    public List<MenuCategory> MenuCategories { get; private set; } = [];

    public WindowState WindowState { get; private set; }
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
        IKeyboardHookService keyboardHookService,
        IOptionsProvider optionsProvider,
        IMenuServiceInitializer menuServiceInitializer,
        IMenuServiceButtonHandler menuServiceButtonHandler,
        IViewManager viewManager,
        IThemeManager themeManager,
        IApplicationUpdater applicationUpdater,
        IPrivilegeManager privilegeManager,
        PhotinoBlazorApp photinoApp,
        JSConsoleInterop jsConsoleInterop,
        INotificationProducer notificationProducer,
        INotificationService notificationService,
        IWindowManipulationService windowManipulationService,
        ILogger<App> logger)
    {
        this.keyboardHookService = keyboardHookService.ThrowIfNull();
        this.optionsProvider = optionsProvider.ThrowIfNull();
        this.menuServiceInitializer = menuServiceInitializer.ThrowIfNull();
        this.menuServiceButtonHandler = menuServiceButtonHandler.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.themeManager = themeManager.ThrowIfNull();
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.photinoApp = photinoApp.ThrowIfNull();
        this.jsConsoleInterop = jsConsoleInterop.ThrowIfNull();
        this.NotificationProducer = notificationProducer.ThrowIfNull();
        this.notificationService = notificationService.ThrowIfNull();
        this.windowManipulationService = windowManipulationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        
        this.photinoApp.MainWindow.RegisterMaximizedHandler(this.OnWindowStateChanged)
            .RegisterMinimizedHandler(this.OnWindowStateChanged)
            .RegisterRestoredHandler(this.OnWindowStateChanged);

        this.menuServiceInitializer.InitializeMenuService(
            this.OpenNavigationMenu,
            this.CloseNavigationMenu,
            this.ToggleNavigationMenu
        );

        this.keyboardHookService.KeyUp += this.KeyboardHook_PreviewKeyUp;
    }

    private void KeyboardHook_PreviewKeyUp(object? _, KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key is VirtualKey.F1)
        {
            this.viewManager.ShowView<WikiView>((nameof(WikiView.Page), "Home"));
        }
    }

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
        this.isInitialized = true;
    }

    public void Drag()
    {
        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */
        this.photinoApp.WindowManager.Dispatcher.InvokeAsync(() =>
        {
            this.windowManipulationService.DragWindow();
        });
    }

    public void StartResize(ResizeDirection resizeDirection)
    {
        /*
         * This operation has to be queued on the UI thread and has to be done via PostMessage to avoid re-entrancy.
         * Using SendMessage or doing it directly can cause crashes when WebView2 is handling a user callback while another sendmessage appears.
         */

        if (this.WindowState == WindowState.Maximized)
        {
            return;
        }

        this.photinoApp.WindowManager.Dispatcher.InvokeAsync(() =>
        {
            this.windowManipulationService.ResizeWindow(resizeDirection);
        });
    }

    public void Minimize()
    {
        this.photinoApp.MainWindow.SetMinimized(true);
    }

    public void Maximize()
    {
        this.photinoApp.MainWindow.SetMaximized(true);
    }

    public void Restore()
    {
        this.photinoApp.MainWindow.SetMaximized(false);
    }

    public void Close()
    {
        this.photinoApp.MainWindow.Close();
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
    }

    private void LoadMenuCategories()
    {
        foreach (var category in this.menuServiceInitializer.GetCategories())
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
                            sp.GetRequiredService<IViewManager>()
                                .ShowView<OptionView>(("optionName", option.Name))
                    );
                }
            }
        }
    }

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

    private void OnWindowStateChanged(object? _, EventArgs __)
    {
        this.WindowState = this.photinoApp.MainWindow.Maximized ?
            WindowState.Maximized :
            this.photinoApp.MainWindow.Minimized ?
                WindowState.Minimized :
                WindowState.Normal;

        this.photinoApp.WindowManager.Dispatcher.InvokeAsync(() =>
        {
            this.WindowStateChanged?.Invoke(this, this.WindowState);
        });
    }
}
