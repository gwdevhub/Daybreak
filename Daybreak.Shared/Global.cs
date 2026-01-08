using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak")]

namespace Daybreak.Shared;

public static class Global
{
    //Will get set by Daybreak on application startup
    public static IServiceProvider GlobalServiceProvider { get; internal set; } = default!;

    //Will get set by Daybreak on successful WebView2 initialization
    public static object? CoreWebView2 { get; internal set; }
}
