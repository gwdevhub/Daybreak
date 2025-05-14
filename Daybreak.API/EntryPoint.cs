using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Daybreak.API.Extensions;
using Daybreak.API.Serialization;
using Net.Sdk.Web;

namespace Daybreak.API;

public static class EntryPoint
{
    private const int StartPort = 5080;

    [UnmanagedCallersOnly(EntryPoint = "ThreadInit"), STAThread]
    public static int ThreadInit(IntPtr _, int __)
    {
        ConsoleExtensions.AllocateAnsiConsole();
        var port = FindAvailablePort(StartPort);
        if (port <= 0)
        {
            Console.WriteLine($"No available port found starting from {StartPort}");
            return -1;
        }

        Console.WriteLine($"Starting Daybreak API on port {port}");
        Task.Run(() => StartServer(port));
        return port;
    }

    private static async Task StartServer(int port)
    {
        try
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls($"http://127.0.0.1:{port}");
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, new ApiJsonSerializerContext());
            });
            builder.Logging.AddConsole();
            builder.WithRoutes();

            var app = builder.Build();
            app.UseRoutes();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }
    }

    private static int FindAvailablePort(int startPort)
    {
        var port = startPort;
        while (!IsPortAvailable(port))
        {
            port++;
            if (port > 65535 || port < 0)
            {
                return -1;
            }
        }

        return port;
    }

    private static bool IsPortAvailable(int port)
    {
        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpListeners = ipProperties.GetActiveTcpListeners();
        if (tcpListeners.Any(endpoint => endpoint.Port == port))
        {
            return false;
        }

        var tcpConnections = ipProperties.GetActiveTcpConnections();
        if (tcpConnections.Any(conn => conn.LocalEndPoint.Port == port))
        {
            return false;
        }

        return true;
    }
}
