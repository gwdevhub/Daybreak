using Daybreak.Configuration;
using Daybreak.Services.ExceptionHandling;
using Daybreak.Services.Navigation;
using Daybreak.Services.Startup;
using Daybreak.Services.Updater.PostUpdate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using Slim.Integration.ServiceCollection;
using System;
using System.Windows.Extensions;

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
        return LaunchMainWindow();
    }

    protected override System.IServiceProvider SetupServiceProvider(IServiceCollection services)
    {
        var serviceManager = new ServiceManager();
        ProjectConfiguration.RegisterResolvers(serviceManager);
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
        ProjectConfiguration.RegisterViews(this.ServiceProvider.GetRequiredService<IViewManager>()!);
        ProjectConfiguration.RegisterPostUpdateActions(this.ServiceProvider.GetRequiredService<IPostUpdateActionProducer>()!);
        ProjectConfiguration.RegisterStartupActions(this.ServiceProvider.GetRequiredService<IStartupActionProducer>()!);

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
}
