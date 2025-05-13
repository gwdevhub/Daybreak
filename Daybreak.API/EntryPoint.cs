using System.Runtime.InteropServices;
using Net.Sdk.Web;

namespace Daybreak.API;

public static class EntryPoint
{
    [UnmanagedCallersOnly(EntryPoint = "ThreadInit"), STAThread]
    public static int ThreadInit(IntPtr _, int __)
    {
        Task.Run(StartServer);
        return 0;
    }

    static async Task StartServer()
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:5080");
            builder.Logging.AddConsole();
            builder.WithRoutes();

            var app = builder.Build();
            app.UseRoutes();

            await app.RunAsync();   // blocks for the app lifetime
        }
        catch (Exception ex)
        {
            // minimal logging: Debug window or console if one exists
            Console.Error.WriteLine(ex);
        }
    }
}
