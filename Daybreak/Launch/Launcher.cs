﻿using Daybreak.Configuration;
using Daybreak.Models.Progress;
using Daybreak.Services.ApplicationArguments;
using Daybreak.Services.Browser;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Plugins;
using Daybreak.Services.Screens;
using Daybreak.Services.Startup;
using Daybreak.Services.Themes;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Services.Window;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using Slim.Integration.ServiceCollection;
using System;
using System.Core.Extensions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Media;

//The following lines are needed to expose internal objects to the test project
[assembly: InternalsVisibleTo("Daybreak.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Daybreak.Launch;

public sealed class Launcher : ExtendedApplication<MainWindow>
{
    public static Launcher Instance { get; private set; } = default!;

    private readonly ProjectConfiguration projectConfiguration = new();
    private readonly string[] launchArguments;
    private ILogger? logger;
    private IExceptionHandler? exceptionHandler;

    public System.IServiceProvider ApplicationServiceProvider => this.ServiceProvider;

    internal Launcher(string[] args)
    {
        this.launchArguments = args.ThrowIfNull();
    }

    [STAThread]
    public static int Main(string[] args)
    {
        Instance = new Launcher(args);
        RegisterExtraEncodingProviders();
        RegisterMahAppsStyle();
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
        this.projectConfiguration.RegisterServices(services);
    }

    protected override bool HandleException(Exception e)
    {
        return this.exceptionHandler?.HandleException(e) is true;
    }

    protected override void ApplicationStarting()
    {
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
        _ = this.ServiceProvider.GetRequiredService<IThemeManager>().GetCurrentTheme();

        var serviceManager = this.ServiceProvider.GetRequiredService<IServiceManager>();
        var viewProducer = this.ServiceProvider.GetRequiredService<IViewManager>();
        var postUpdateActionProducer = this.ServiceProvider.GetRequiredService<IPostUpdateActionProducer>();
        var startupActionProducer = this.ServiceProvider.GetRequiredService<IStartupActionProducer>();
        var notificationHandlerProducer = this.ServiceProvider.GetRequiredService<INotificationHandlerProducer>();
        var modsManager = this.ServiceProvider.GetRequiredService<IModsManager>();
        var browserExtensionsProducer = this.ServiceProvider.GetRequiredService<IBrowserExtensionsProducer>();
        var argumentHandlerProducer = this.ServiceProvider.GetRequiredService<IArgumentHandlerProducer>();
        
        startupStatus.CurrentStep = StartupStatus.Custom("Loading views");
        this.projectConfiguration.RegisterViews(viewProducer);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading post-update actions");
        this.projectConfiguration.RegisterPostUpdateActions(postUpdateActionProducer);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading startup actions");
        this.projectConfiguration.RegisterStartupActions(startupActionProducer);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading notification handlers");
        this.projectConfiguration.RegisterNotificationHandlers(notificationHandlerProducer);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading mods");
        this.projectConfiguration.RegisterMods(modsManager);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading browser extensions");
        this.projectConfiguration.RegisterBrowserExtensions(browserExtensionsProducer);
        startupStatus.CurrentStep = StartupStatus.Custom("Loading argument handlers");
        this.projectConfiguration.RegisterLaunchArgumentHandlers(argumentHandlerProducer);

        this.logger = this.ServiceProvider.GetRequiredService<ILogger<Launcher>>();
        this.logger.LogInformation($"Running in {Environment.CurrentDirectory}");
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
                    argumentHandlerProducer);
        }
        catch(Exception e)
        {
            this.logger.LogError(e, "Encountered exception while loading plugins. Aborting...");
            this.exceptionHandler.HandleException(e);
        }
        
        // Trigger hooks into MainWindow
        this.ServiceProvider.GetRequiredService<IWindowEventsHook<MainWindow>>();

        startupStatus.CurrentStep = StartupStatus.Custom("Registering view container");
        this.RegisterViewContainer();
        startupStatus.CurrentStep = StartupStatus.Custom("Executing argument handlers");
        this.ServiceProvider.GetRequiredService<IApplicationArgumentService>().HandleArguments(this.launchArguments);
        startupStatus.CurrentStep = StartupStatus.Finished;
        this.ServiceProvider.GetRequiredService<IViewManager>().ShowView<LauncherView>();
    }

    protected override void ApplicationClosing()
    {
    }

    private void RegisterViewContainer()
    {
        var viewManager = this.ServiceProvider.GetRequiredService<IViewManager>();
        var mainWindow = this.ServiceProvider.GetRequiredService<MainWindow>();
        viewManager.RegisterContainer(mainWindow.Container);
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

    private static void RegisterMahAppsStyle()
    {
        RegisterMahAppsComponent("Styles/Fonts.xaml");
        RegisterMahAppsComponent("Styles/Controls.xaml");
        RegisterMahAppsComponent("Styles/Themes/Light.Steel.xaml");
        
        //TODO: Hacky way to initialize a theme before startup and option loading
        RegisterDaybreakComponent();
    }

    private static void RegisterMahAppsComponent(string component)
    {
        Instance.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/MahApps.Metro;component/{component}", UriKind.RelativeOrAbsolute)
        });
    }

    private static void RegisterDaybreakComponent()
    {
        Instance.Resources.MergedDictionaries[2].Add("Daybreak.Brushes.Background", new SolidColorBrush(Colors.Transparent));
    }
}
