using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Utils;
using System.Core.Extensions;
using System.Extensions;
using System.Windows.Interop;
using System.Windows.Threading;
using WpfExtended.Blazor.Launch;

namespace Daybreak.Services.Startup.Actions;
internal sealed class RestoreWindowPositionStartupAction(
    BlazorHostWindow mainWindow,
    ISplashScreenService splashScreenService,
    IScreenManager screenManager) : StartupActionBase
{
    private readonly BlazorHostWindow mainWindow = mainWindow.ThrowIfNull();
    private readonly ISplashScreenService splashScreenService = splashScreenService.ThrowIfNull();
    private readonly IScreenManager screenManager = screenManager.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        Task.Factory.StartNew(async () =>
        {
            await this.mainWindow.Dispatcher.InvokeAsync(this.screenManager.MoveWindowToSavedPosition, DispatcherPriority.Normal);
            await this.mainWindow.Dispatcher.InvokeAsync(this.splashScreenService.HideSplashScreen, DispatcherPriority.Normal);
            await this.mainWindow.Dispatcher.InvokeAsync(this.mainWindow.Show, DispatcherPriority.Render);
            
            await Task.Delay(1000);
            await this.mainWindow.Dispatcher.InvokeAsync(() =>
            {
                var hwnd = new WindowInteropHelper(this.mainWindow).Handle;
                var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref preference, sizeof(uint));
            });
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
}
