using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Daybreak.API.Configuration;
using Daybreak.API.Controllers;
using Daybreak.API.Extensions;
using Daybreak.API.Health;
using Daybreak.API.Hosting;
using Daybreak.API.Logging;
using Daybreak.API.Serialization;
using Daybreak.API.Services;
using Daybreak.API.Swagger;
using Daybreak.API.WebSockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Net.Sdk.Web;
using Net.Sdk.Web.Websockets.Extensions;

namespace Daybreak.API;

public static class EntryPoint
{
    private const int StartPort = 5080;
    private const int InitializationAttempts = 300;

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
        var app = CreateApplication(port);
        var runTask = Task.Run(() => StartServer(app));
        var healthCheck = app.Services.GetRequiredService<HealthCheckService>();
        for(var i = 0; i < InitializationAttempts; i++)
        {
            var status = Task.Run(() => healthCheck.CheckHealthAsync()).Result;
            Console.WriteLine($"Health check status: {status.Status} in {status.TotalDuration.TotalMilliseconds}ms. Report:\n{string.Join("\n", status.Entries.Select(e => $"{e.Key}: {e.Value.Status}"))}");
            if (status.Status is not HealthStatus.Healthy)
            {
                Thread.Sleep(100);
            }
            else
            {
                Console.WriteLine($"Daybreak API is healthy. Initialization succeeded");
                break;
            }
        }

        return port;
    }

    private static WebApplication CreateApplication(int port)
    {
        var app = WebApplication.CreateBuilder()
                .WithConfiguration()
                .WithHosting(port)
                .WithSwagger()
                .WithSerializationContext()
                .WithLogging()
                .WithDaybreakServices()
                .WithWebSocketRoutes()
                .WithRoutes()
                .WithHealthChecks()
                .Build();

        app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(30) });

        return app
            .UseHealthChecks()
            .UseLogging()
            .UseRoutes()
            .UseWebSocketRoutes()
            .UseSwaggerWithUI();

    }

    private static async Task StartServer(WebApplication app)
    {
        try
        {
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
