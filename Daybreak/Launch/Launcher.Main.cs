using Daybreak.Views;
using Photino.Blazor;

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
        app.MainWindow.RegisterWindowCreatedHandler((_, __) => StartHostedServices(app, cts));
        app.MainWindow.RegisterWindowClosingHandler((_, __) => StopHostedServices(app, cts));
        MainApp = app;
        return app;
    }

    private static void RunMainApp(PhotinoBlazorApp app)
    {
        app.Run();
    }
}
