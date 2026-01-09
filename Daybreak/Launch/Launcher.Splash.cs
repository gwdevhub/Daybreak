using Daybreak.Configuration.Options;
using Daybreak.Services.Options;
using Daybreak.Services.Screens;
using Daybreak.Services.Themes;
using Daybreak.Shared.Services.Screens;
using Daybreak.Shared.Services.Themes;
using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Photino.Blazor;
using System.Drawing;
using System.Extensions;

namespace Daybreak.Launch;

public static partial class Launcher
{
    private static PhotinoBlazorApp? SplashWindowApp;

    private static void CreateAndShowSplash(string[] args, Action<string[], PhotinoBlazorApp> bootstrapingProvider)
    {
        var cts = new CancellationTokenSource();
        var splashBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        splashBuilder.RootComponents.Add<SplashView>("#app");
        splashBuilder.Services.AddBlazorDesktop();
        SetupLogging(splashBuilder);
        BootstrapOptionsManager(splashBuilder);
        BootstrapThemeOptions(splashBuilder);
        BootstrapScreenManager(splashBuilder);
        SplashWindowApp = splashBuilder.Build();
        SplashWindowApp.MainWindow
            .SetChromeless(true)
            .SetTitle("Daybreak")
            .SetResizable(false)
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

        SetupWindowSize(SplashWindowApp);
        SplashWindowApp.MainWindow.RegisterWindowCreatingHandler((_, __) => StartHostedServices(SplashWindowApp, cts));
        SplashWindowApp.MainWindow.RegisterWindowCreatedHandler((_, __) => bootstrapingProvider(args, SplashWindowApp));
        SplashWindowApp.MainWindow.RegisterWindowClosingHandler((_, __) => StopHostedServicesAsync(SplashWindowApp, cts));
        SplashWindowApp.Run();
    }

    private static void BootstrapOptionsManager(PhotinoBlazorAppBuilder builder)
    {
        builder.Services.AddSingleton<OptionsManager>();
    }

    private static void BootstrapThemeOptions(PhotinoBlazorAppBuilder builder)
    {
        builder.Services.AddDaybreakOptions<ThemeOptions>();
        builder.Services.AddSingleton<IThemeManager, BlazorThemeInteropService>();
        builder.Services.AddHostedService(sp => sp.GetRequiredService<IThemeManager>().Cast<BlazorThemeInteropService>());
    }

    private static void BootstrapScreenManager(PhotinoBlazorAppBuilder builder)
    {
        builder.Services.AddDaybreakOptions<ScreenManagerOptions>();
        builder.Services.AddSingleton<IScreenManager, ScreenManager>();
        builder.Services.AddHostedService(sp => sp.GetRequiredService<IScreenManager>().Cast<ScreenManager>());
    }

    private static void SetupWindowSize(PhotinoBlazorApp app)
    {
        var screenManager = app.Services.GetRequiredService<IScreenManager>();
        var screenOptions = app.Services.GetRequiredService<IOptions<ScreenManagerOptions>>();

        var windowPosition = new Rectangle((int)screenOptions.Value.X, (int)screenOptions.Value.Y, (int)screenOptions.Value.Width, (int)screenOptions.Value.Height);
        var desiredScreen = screenManager.Screens.FirstOrDefault(s => s.Size.IntersectsWith(windowPosition));
        if (desiredScreen.Size.Width == 0 || desiredScreen.Size.Height == 0)
        {
            desiredScreen = screenManager.Screens.FirstOrDefault();
        }

        app.MainWindow.SetLocation(desiredScreen.Size.Location + (desiredScreen.Size.Size / 2));
        app.MainWindow.SetSize(desiredScreen.Size.Size / 2);
    }
}
