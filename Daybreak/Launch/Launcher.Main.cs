using Daybreak.Shared.Services.Screens;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Photino.Blazor;
using System.Extensions.Core;

namespace Daybreak.Launch;

public partial class Launcher
{
#if DEBUG
    public const bool IsDebug = true;
#else
    public const bool IsDebug = false;
#endif

    public static PhotinoBlazorApp? MainApp { get; private set; }
    public static PhotinoBlazorAppBuilder? MainBuilder { get; private set; }

    private static PhotinoBlazorAppBuilder CreateMainBuilder(string[] args)
    {
        var mainBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        mainBuilder.RootComponents.Add<App>("#app");
        mainBuilder.Services.AddBlazorDesktop();
        SetupLogging(mainBuilder.Services);
        MainBuilder = mainBuilder;
        return mainBuilder;
    }

    private static PhotinoBlazorApp CreateMainApp(PhotinoBlazorAppBuilder mainBuilder)
    {
        var app = mainBuilder.Build();

        var cts = new CancellationTokenSource();
        app.MainWindow.SetTitle("Daybreak")
            .SetContextMenuEnabled(IsDebug)
            .SetDevToolsEnabled(IsDebug)
            .SetSmoothScrollingEnabled(true)
            .SetChromeless(true);
        app.MainWindow.SetLogVerbosity(0);
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => SetupRoundedWindows(app));
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => SetupBorderless(app));
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => StartHostedServices(app, cts));
        app.MainWindow.RegisterWindowClosingHandler((_, __) => StopHostedServices(app, cts));

        SetWindowPosition(app);
        MainApp = app;
        return app;
    }

    private static void RunMainApp(PhotinoBlazorApp app)
    {
        app.Run();
    }

    private static void SetWindowPosition(PhotinoBlazorApp app)
    {
        var scopedLogger = app.Services.GetRequiredService<ILogger<Launcher>>().CreateScopedLogger();
        var screenManager = app.Services.GetRequiredService<IScreenManager>();
        var savedPosition = screenManager.GetSavedPosition();

        if (savedPosition.Width is 0 || savedPosition.Height is 0)
        {
            scopedLogger.LogDebug("No saved window position found, centering the window");
            var firstScreen = screenManager.Screens.FirstOrDefault();
            if (firstScreen.Size.IsEmpty)
            {
                scopedLogger.LogWarning("No valid screen found to center the window. Window will spawn with minimal size");
                return;
            }

            app.MainWindow.Center();
            app.MainWindow.Width = firstScreen.Size.Width / 2;
            app.MainWindow.Height = firstScreen.Size.Height / 2;
        }
        else
        {
            scopedLogger.LogDebug("Restoring saved window position");
            app.MainWindow.Left = savedPosition.X;
            app.MainWindow.Top = savedPosition.Y;
            app.MainWindow.Width = savedPosition.Width;
            app.MainWindow.Height = savedPosition.Height;
        }
    }
}
