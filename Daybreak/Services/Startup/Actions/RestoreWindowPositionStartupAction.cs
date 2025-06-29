using Daybreak.Launch;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Screens;
using Daybreak.Views;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Threading;

namespace Daybreak.Services.Startup.Actions;
internal sealed class RestoreWindowPositionStartupAction(
    MainWindow mainWindow,
    ISplashScreenService splashScreenService,
    IViewManager viewManager,
    IScreenManager screenManager) : StartupActionBase
{
    private readonly MainWindow mainWindow = mainWindow.ThrowIfNull();
    private readonly ISplashScreenService splashScreenService = splashScreenService.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        Task.Factory.StartNew(async () =>
        {
            await this.mainWindow.Dispatcher.InvokeAsync(this.screenManager.MoveWindowToSavedPosition, DispatcherPriority.Normal);
            await this.mainWindow.Dispatcher.InvokeAsync(this.splashScreenService.HideSplashScreen, DispatcherPriority.Normal);
            await this.mainWindow.Dispatcher.InvokeAsync(this.mainWindow.Show, DispatcherPriority.Render);
            await Task.Delay(1000);
            await this.mainWindow.Dispatcher.InvokeAsync(this.viewManager.ShowView<LauncherView>, DispatcherPriority.Render);
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
}
