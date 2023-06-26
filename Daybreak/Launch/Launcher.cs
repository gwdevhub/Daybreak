using Daybreak.Configuration;
using Daybreak.Services.Drawing;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Mods;
using Daybreak.Services.Navigation;
using Daybreak.Services.Notifications;
using Daybreak.Services.Options;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using Slim.Integration.ServiceCollection;
using System;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Media;

namespace Daybreak.Launch;

public sealed class Launcher : ExtendedApplication<MainWindow>
{
    public readonly static Launcher Instance = new();

    private ILogger? logger;
    private IExceptionHandler? exceptionHandler;

    public System.IServiceProvider ApplicationServiceProvider => this.ServiceProvider;

    [STAThread]
    public static int Main()
    {
        RegisterMahAppsStyle();
        return LaunchMainWindow();
    }

    protected override System.IServiceProvider SetupServiceProvider(IServiceCollection services)
    {
        var serviceManager = new ServiceManager();
        ProjectConfiguration.RegisterResolvers(serviceManager);
        ProjectConfiguration.RegisterLiteCollections(services);
        return services.BuildSlimServiceProvider(serviceManager);
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        ProjectConfiguration.RegisterServices(services);
    }

    protected override bool HandleException(Exception e)
    {
        return this.exceptionHandler?.HandleException(e) is true;
    }

    protected override void ApplicationStarting()
    {
        ProjectConfiguration.RegisterOptions(this.ServiceProvider.GetRequiredService<IOptionsProducer>()!);
        ProjectConfiguration.RegisterViews(this.ServiceProvider.GetRequiredService<IViewManager>()!);
        ProjectConfiguration.RegisterPostUpdateActions(this.ServiceProvider.GetRequiredService<IPostUpdateActionProducer>()!);
        ProjectConfiguration.RegisterStartupActions(this.ServiceProvider.GetRequiredService<IStartupActionProducer>()!);
        ProjectConfiguration.RegisterDrawingModules(this.ServiceProvider.GetRequiredService<IDrawingModuleProducer>()!);
        ProjectConfiguration.RegisterNotificationHandlers(this.ServiceProvider.GetRequiredService<INotificationHandlerProducer>()!);
        ProjectConfiguration.RegisterMods(this.ServiceProvider.GetRequiredService<IModsManager>()!);

        this.logger = this.ServiceProvider.GetRequiredService<ILogger<Launcher>>();
        this.exceptionHandler = this.ServiceProvider.GetRequiredService<IExceptionHandler>();
        this.RegisterViewContainer();
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
