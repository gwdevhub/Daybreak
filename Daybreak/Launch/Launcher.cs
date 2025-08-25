using Daybreak.Configuration;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Navigation;
using Daybreak.Services.Telemetry;
using Daybreak.Shared;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.ApplicationArguments;
using Daybreak.Shared.Services.Browser;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Slim;
using Slim.Integration.ServiceCollection;
using System.Core.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
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
    public static Launcher Instance { get; private set; } = default!;
    public override bool DevToolsEnabled { get; } = true;
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
        serviceManager.RegisterSingleton<StartupStatus>();
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
        var startupStatus = this.ServiceProvider.GetRequiredService<StartupStatus>();
        startupStatus.CurrentStep = StartupStatus.Custom("Loading options");
        this.projectConfiguration.RegisterOptions(optionsProducer);

        /*
         * SplashScreenService has a dependency on IOptionsProducer, due to needing to style the
         * SplashScreen based on the theme in the options. Thus, it can only be called after
         * initializing the options.
         */
        this.ServiceProvider.GetRequiredService<ISplashScreenService>().ShowSplashScreen();

        /*
         * Hook into WPF traces and output them to logs
         */

        PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
        PresentationTraceSources.DataBindingSource.Listeners.Add(new BindingErrorTraceListener(this.ServiceProvider.GetRequiredService<ILogger<Launcher>>()));

        await this.InitializeApplicationServices(startupStatus, optionsProducer);
    }

    protected override void ApplicationClosing()
    {
    }

    protected override void Host_CoreWebView2Initialized(CoreWebView2 e)
    {
        base.Host_CoreWebView2Initialized(e);
        Global.CoreWebView2 = e;
    }

    private async ValueTask InitializeApplicationServices(StartupStatus startupStatus, IOptionsProducer optionsProducer)
    {
        var telemetryHost = this.ServiceProvider.GetRequiredService<TelemetryHost>();
        var serviceManager = this.ServiceProvider.GetRequiredService<IServiceManager>();
        var viewProducer = new TrailBlazrViewProducer(serviceManager);
        var postUpdateActionProducer = this.ServiceProvider.GetRequiredService<IPostUpdateActionProducer>();
        var startupActionProducer = this.ServiceProvider.GetRequiredService<IStartupActionProducer>();
        var notificationHandlerProducer = this.ServiceProvider.GetRequiredService<INotificationHandlerProducer>();
        var modsManager = this.ServiceProvider.GetRequiredService<IModsManager>();
        var browserExtensionsProducer = this.ServiceProvider.GetRequiredService<IBrowserExtensionsProducer>();
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

        startupStatus.CurrentStep = StartupStatus.Custom("Loading themes");
        this.projectConfiguration.RegisterThemes(themeProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading views");
        this.projectConfiguration.RegisterViews(viewProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading post-update actions");
        this.projectConfiguration.RegisterPostUpdateActions(postUpdateActionProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading startup actions");
        this.projectConfiguration.RegisterStartupActions(startupActionProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading notification handlers");
        this.projectConfiguration.RegisterNotificationHandlers(notificationHandlerProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading mods");
        this.projectConfiguration.RegisterMods(modsManager);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading browser extensions");
        this.projectConfiguration.RegisterBrowserExtensions(browserExtensionsProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading argument handlers");
        this.projectConfiguration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Custom("Loading menu buttons");
        this.projectConfiguration.RegisterMenuButtons(menuServiceProducer);
        await Task.Delay(10);

        this.logger = this.ServiceProvider.GetRequiredService<ILogger<Launcher>>();
        this.logger.LogDebug($"Running in {Environment.CurrentDirectory}");
        this.exceptionHandler = this.ServiceProvider.GetRequiredService<IExceptionHandler>();
        try
        {
            startupStatus.CurrentStep = StartupStatus.Custom("Loading plugins");
            this.ServiceProvider.GetRequiredService<IPluginsService>()
                .LoadPlugins(
                    serviceManager,
                    optionsProducer,
                    viewProducer,
                    postUpdateActionProducer,
                    startupActionProducer,
                    notificationHandlerProducer,
                    modsManager,
                    browserExtensionsProducer,
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

        startupStatus.CurrentStep = StartupStatus.Custom("Executing argument handlers");
        this.ServiceProvider.GetRequiredService<IApplicationArgumentService>().HandleArguments(this.launchArguments);
        await Task.Delay(10);

        startupStatus.CurrentStep = StartupStatus.Finished;
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

    internal sealed class BindingErrorTraceListener(ILogger logger) : TraceListener
    {
        private readonly ILogger logger = logger;

        public override void Write(string? message)
        {
            this.logger.LogWarning("WPF Binding error: {bindingMessage}", message);
        }

        public override void WriteLine(string? message)
        {
            this.logger.LogWarning("WPF Binding error: {bindingMessage}", message);
        }
    }
}
