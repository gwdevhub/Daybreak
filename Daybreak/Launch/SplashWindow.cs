using Daybreak.Shared.Utils;
using Daybreak.Views;
using Microsoft.Extensions.FileProviders;
using Photino.Blazor;
using System.Reflection;

namespace Daybreak.Launch;

public sealed class SplashWindow
{
    public PhotinoBlazorApp SplashWindowApp { get; }
    
    public SplashWindow(string[] launchArguments)
    {
        var fileProvider = new ManifestEmbeddedFileProvider(
            Assembly.GetExecutingAssembly(),
            "wwwroot");

        var splashBuilder = PhotinoBlazorAppBuilder.CreateDefault(fileProvider, launchArguments);
        splashBuilder.RootComponents.Add<SplashView>("#app");
        splashBuilder.Services.AddBlazorDesktop();

        this.SplashWindowApp = splashBuilder.Build();
        this.SplashWindowApp.MainWindow
            .SetChromeless(true)
            .SetTitle("Daybreak - Loading...")
            .SetResizable(false)
            .Center()
            .RegisterWindowCreatedHandler((sender, args) =>
            {
                var hwnd = this.SplashWindowApp.MainWindow.WindowHandle;
                var preference = NativeMethods.DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                NativeMethods.DwmSetWindowAttribute(
                    hwnd,
                    NativeMethods.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
                    ref preference,
                    sizeof(uint));
            });
    }
}
