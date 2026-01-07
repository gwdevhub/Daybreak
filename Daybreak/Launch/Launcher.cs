using System.Core.Extensions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Daybreak.Configuration;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Navigation;
using Daybreak.Services.Telemetry;
using Daybreak.Shared;
using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Menu;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using Daybreak.Shared.Services.Plugins;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Startup;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Services.Updater.PostUpdate;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Slim;
using Slim.Integration.ServiceCollection;
using WpfExtended.Blazor.Launch;

//The following lines are needed to expose internal objects to the test project
[assembly: InternalsVisibleTo("Daybreak.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly)
]
namespace Daybreak.Launch;

public sealed class Launcher : BlazorHybridApplication<App>
{
    private static readonly ProgressUpdate ProgressLoadOptions = new(0, "Loading options");
    private static readonly ProgressUpdate ProgressLoadThemes = new(0.1, "Loading themes");
    private static readonly ProgressUpdate ProgressLoadingViews = new(0.2, "Loading views");
    private static readonly ProgressUpdate ProgressLoadPostUpdateActions = new(0.3, "Loading post-update actions");
    private static readonly ProgressUpdate ProgressLoadStartupActions = new(0.4, "Loading startup actions");
    private static readonly ProgressUpdate ProgressLoadNotificationHandlers = new(0.5, "Loading notification handlers");
    private static readonly ProgressUpdate ProgressLoadMods = new(0.6, "Loading mods");
    private static readonly ProgressUpdate ProgressLoadArgumentHandlers = new(0.8, "Loading argument handlers");
    private static readonly ProgressUpdate ProgressLoadMenuButtons = new(0.9, "Loading menu buttons");
    private static readonly ProgressUpdate ProgressLoadPlugins = new(0.95, "Loading plugins");
    private static readonly ProgressUpdate ProgressExecuteArgumentHandlers = new(0.99, "Executing argument handlers");
    private static readonly ProgressUpdate ProgressFinished = new(1.0, "Finished");

    public static Launcher Instance { get; private set; } = default!;

#if DEBUG
    public override bool DevToolsEnabled { get; } = true;
#else
    public override bool DevToolsEnabled { get; } = false;
#endif

    public override string HostPage { get; } = "wwwroot/Index.html";
    public override bool ShowTitleBar => false;

    private readonly ProjectConfiguration projectConfiguration = new();
    private readonly string[] launchArguments;
    private ILogger? logger;
    private IExceptionHandler? exceptionHandler;

    internal Launcher(string[] args)
    {
        this.launchArguments = args.ThrowIfNull();
        this.ShowWindowOnStartup = false;
    }

    [STAThread]
    public static int Main(string[] args)
    {
#if DEBUG
        AllocateAnsiConsole();
#endif

        Instance = new Launcher(args);
        RegisterExtraEncodingProviders();
        return LaunchMainWindow();
    }

    protected override System.IServiceProvider SetupServiceProvider(IServiceCollection services)
    {
        var serviceManager = new ServiceManager();
        this.projectConfiguration.RegisterResolvers(serviceManager);
        serviceManager.RegisterSingleton<SplashWindow>();
        serviceManager.RegisterSingleton<StartupContext>();
        return services.BuildSlimServiceProvider(serviceManager);
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        base.RegisterServices(services);
        this.projectConfiguration.RegisterServices(services);
    }

    protected override bool HandleException(Exception e)
    {
        return this.exceptionHandler?.HandleException(e) is true;
    }

    protected override async ValueTask ApplicationStarting()
    {
        Global.GlobalServiceProvider = Instance.ServiceProvider;
        await base.ApplicationStarting();
        /*
         * Show splash screen before beginning to load the rest of the application.
         * MainWindow will call HideSplashScreen() on Loaded event
         * 
         * OptionsProducer needs to be created before everything else, otherwise all
         * the other services will fail to get options for their needs.
         */

        var optionsProducer = this.ServiceProvider.GetRequiredService<IOptionsProducer>();
        var startupContext = this.ServiceProvider.GetRequiredService<StartupContext>();
        startupContext.ProgressUpdate = ProgressLoadOptions;
        this.projectConfiguration.RegisterOptions(optionsProducer);

        /*
         * SplashScreenService has a dependency on IOptionsProducer, due to needing to style the
         * SplashScreen based on the theme in the options. Thus, it can only be called after
         * initializing the options.
         */
        this.ServiceProvider.GetRequiredService<ISplashScreenService>().ShowSplashScreen();
        await this.InitializeApplicationServices(startupContext, optionsProducer);
    }

    protected override void ApplicationClosing()
    {
    }

    protected override void Host_BlazorWebViewInitialized(BlazorWebViewInitializedEventArgs e)
    {
        e.WebView.CoreWebView2.ProcessFailed += this.CoreWebView2_ProcessFailed;
        Global.CoreWebView2 = e.WebView.CoreWebView2;
        this.logger?.LogInformation("WebView2 initialized with version {version}", e.WebView.CoreWebView2.Environment.BrowserVersionString);
        this.logger?.LogInformation("Process: {architecture}", Environment.Is64BitProcess ? "x64" : "x86");
        base.Host_BlazorWebViewInitialized(e);
    }

    private void CoreWebView2_ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
    {
        this.logger?.LogCritical("WebView2 process failed.\nExit Code: {exitCode}\nSource: {source}\nKind: {kind}\nReason: {reason}\nFrame Infos: {frameInfos}\nProcess Description: {processDescription}", e.ExitCode, e.FailureSourceModulePath, e.ProcessFailedKind, e.Reason, e.FrameInfosForFailedProcess, e.ProcessDescription);
    }

    private async ValueTask InitializeApplicationServices(StartupContext startupContext, IOptionsProducer optionsProducer)
    {
        var telemetryHost = this.ServiceProvider.GetRequiredService<TelemetryHost>();
        var serviceManager = this.ServiceProvider.GetRequiredService<IServiceManager>();
        var viewProducer = new TrailBlazrViewProducer(serviceManager);
        var postUpdateActionProducer = this.ServiceProvider.GetRequiredService<IPostUpdateActionProducer>();
        var startupActionProducer = this.ServiceProvider.GetRequiredService<IStartupActionProducer>();
        var notificationHandlerProducer = this.ServiceProvider.GetRequiredService<INotificationHandlerProducer>();
        var modsManager = this.ServiceProvider.GetRequiredService<IModsManager>();
        var argumentHandlerProducer = this.ServiceProvider.GetRequiredService<IArgumentHandlerProducer>();
        var menuServiceProducer = this.ServiceProvider.GetRequiredService<IMenuServiceProducer>();
        var themeProducer = this.ServiceProvider.GetRequiredService<IThemeProducer>();

        await this.Dispatcher.InvokeAsync(() =>
        {
            // Hide the main window until the application is fully loaded. Main window will be shown by the RestoreWindowPositionStartupAction
            var mainWindow = this.ServiceProvider.GetRequiredService<BlazorHostWindow>();
            mainWindow.Hide();
        });
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadThemes;

        // TODO: This is pretty hacky so it should be reworked
        var gameScreenshotsTheme = this.ServiceProvider.GetRequiredService<GameScreenshotsTheme>();
        themeProducer.RegisterTheme(gameScreenshotsTheme);

        this.projectConfiguration.RegisterThemes(themeProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadingViews;
        this.projectConfiguration.RegisterViews(viewProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadPostUpdateActions;
        this.projectConfiguration.RegisterPostUpdateActions(postUpdateActionProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadStartupActions;
        this.projectConfiguration.RegisterStartupActions(startupActionProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadNotificationHandlers;
        this.projectConfiguration.RegisterNotificationHandlers(notificationHandlerProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadMods;
        this.projectConfiguration.RegisterMods(modsManager);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadArgumentHandlers;
        this.projectConfiguration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressLoadMenuButtons;
        this.projectConfiguration.RegisterMenuButtons(menuServiceProducer);
        await Task.Delay(10);

        this.logger = this.ServiceProvider.GetRequiredService<ILogger<Launcher>>();
        this.logger.LogDebug("Running in {Environment.CurrentDirectory}", Environment.CurrentDirectory);
        this.exceptionHandler = this.ServiceProvider.GetRequiredService<IExceptionHandler>();
        try
        {
            startupContext.ProgressUpdate = ProgressLoadPlugins;
            this.ServiceProvider.GetRequiredService<IPluginsService>()
                .LoadPlugins(
                    serviceManager,
                    optionsProducer,
                    viewProducer,
                    postUpdateActionProducer,
                    startupActionProducer,
                    notificationHandlerProducer,
                    modsManager,
                    argumentHandlerProducer,
                    menuServiceProducer,
                    themeProducer);
            await Task.Delay(10);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Encountered exception while loading plugins. Aborting...");
            this.exceptionHandler.HandleException(e);
        }

        startupContext.ProgressUpdate = ProgressExecuteArgumentHandlers;
        this.ServiceProvider.GetRequiredService<IApplicationArgumentService>().HandleArguments(this.launchArguments);
        await Task.Delay(10);

        startupContext.ProgressUpdate = ProgressFinished;
        await Task.Delay(10);
    }

    private static int LaunchMainWindow()
    {
        return Instance.Run();
    }

    private static void RegisterExtraEncodingProviders()
    {
        /*
         * This is a fix for encoding issues with zip files
         */
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    private static void AllocateAnsiConsole()
    {
        NativeMethods.AllocConsole();
        var handle = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);
        if (!NativeMethods.GetConsoleMode(handle, out var mode))
        {
            Console.WriteLine("Failed to get console mode");
        }

        if (!NativeMethods.SetConsoleMode(handle, mode | NativeMethods.ENABLE_VIRTUAL_TERMINAL_PROCESSING | NativeMethods.ENABLE_PROCESSED_OUTPUT))
        {
            Console.WriteLine("Failed to enable virtual terminal processing");
        }
    }
}
