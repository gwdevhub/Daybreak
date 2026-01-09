using Daybreak.Views;
using Photino.Blazor;

namespace Daybreak.Launch;

public static partial class Launcher
{
    private static PhotinoBlazorAppBuilder CreateMainBuilder(string[] args)
    {
        var mainBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
        mainBuilder.RootComponents.Add<App>("#app");
        mainBuilder.Services.AddBlazorDesktop();
        return mainBuilder;
    }
}
