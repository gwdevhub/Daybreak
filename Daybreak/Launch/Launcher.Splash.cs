using Daybreak.Shared.Utils;
using Daybreak.Views;
using Photino.Blazor;

namespace Daybreak.Launch;

public static partial class Launcher
{
    private static PhotinoBlazorApp? SplashWindowApp;

    private static void CreateAndShowSplash(string[] args, EventHandler splashShownContinue)
    {
        var splashBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        splashBuilder.RootComponents.Add<SplashView>("#app");
        splashBuilder.Services.AddBlazorDesktop();
        SetupLogging(splashBuilder);

        SplashWindowApp = splashBuilder.Build();
        SplashWindowApp.MainWindow
            .SetChromeless(false)
            .SetTitle("Daybreak - Loading...")
            .SetResizable(true)
            .Center()
            .RegisterWindowCreatedHandler((sender, args) =>
            {
                var hwnd = SplashWindowApp.MainWindow.WindowHandle;
                var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                NativeMethods.DwmSetWindowAttribute(
                    hwnd,
                    NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                    ref preference,
                    sizeof(uint));
            });

        SplashWindowApp.MainWindow.RegisterWindowCreatedHandler(splashShownContinue);
        SplashWindowApp.Run();
    }
}
