using Daybreak.Views;
using Photino.Blazor;

namespace Daybreak.Launch;

public partial class Launcher
{
    private static PhotinoBlazorAppBuilder CreateMainBuilder(string[] args)
    {
        var mainBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        mainBuilder.RootComponents.Add<App>("#app");
        mainBuilder.Services.AddBlazorDesktop();
        return mainBuilder;
    }

    private static PhotinoBlazorApp CreateMainApp(PhotinoBlazorAppBuilder mainBuilder)
    {
        var app = mainBuilder.Build();
        app.MainWindow.SetTitle("Daybreak");
        return app;
    }

    private static void RunMainApp(PhotinoBlazorApp app)
    {
        app.Run();
    }
}
