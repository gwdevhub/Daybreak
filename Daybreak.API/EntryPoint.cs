using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Extensions.Core;
using System.Logging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Daybreak.API.Configuration;
using Daybreak.API.Extensions;
using Daybreak.API.Health;
using Daybreak.API.Hosting;
using Daybreak.API.Interop;
using Daybreak.API.Logging;
using Daybreak.API.Serialization;
using Daybreak.API.Services;
using Daybreak.API.Swagger;
using Daybreak.API.WebSockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Net.Sdk.Web;

namespace Daybreak.API;

public class EntryPoint
{
    private const int StartPort = 5080;
    private static readonly TimeSpan InitializationTimeout = TimeSpan.FromSeconds(5);
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    [UnmanagedCallersOnly(EntryPoint = "ThreadInit"), STAThread]
    [RequiresUnreferencedCode("The handler uses a static method that gets referenced, so there's no unreferenced code to worry about")]
    [RequiresDynamicCode("The handler uses a static method, so there's no dynamic code to worry about")]
    [SuppressMessage("Trimming", "IL2123:The use of 'RequiresUnreferencedCodeAttribute' on entry points is disallowed since the method will be called from outside the visible app.", Justification = "<Pending>")]
    [SuppressMessage("AOT", "IL3057:The use of 'RequiresDynamicCodeAttribute' on entry points is disallowed since the method will be called from outside the visible app.", Justification = "<Pending>")]
    public static int ThreadInit(IntPtr _, int __)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES", null, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("ASPNETCORE_PREVENTHOSTINGSTARTUP", "true", EnvironmentVariableTarget.Process);
#if DEBUG
        ConsoleExtensions.AllocateAnsiConsole();
#endif
        var port = FindAvailablePort(StartPort);
        var app = CreateApplication(port);
        var runTask = Task.Run(() => StartServer(app), CancellationTokenSource.Token);
        var scopedLogger = app.Services.GetRequiredService<ILogger<EntryPoint>>().CreateScopedLogger();
        var healthCheck = app.Services.GetRequiredService<HealthCheckService>();
        var sw = Stopwatch.StartNew();
        var healthy = false;
        while (sw.Elapsed < InitializationTimeout)
        {
            var status = Task.Run(() => healthCheck.CheckHealthAsync()).Result;
            scopedLogger.LogWarning("HealthCheck status: {status} in {duration}. Report: {report}", status.Status, status.TotalDuration, string.Join("\n", status.Entries.Select(e => $"{e.Key}: {e.Value.Status}")));
            if (status.Status is HealthStatus.Healthy)
            {
                healthy = true;
                break;
            }
            else
            {
                Thread.Sleep(100);
            }
        }

        if (healthy)
        {
            scopedLogger.LogInformation("Daybreak API is healthy on port {port}. Initialization succeeded in {duration}", port, sw.Elapsed);
            return port;
        }
        else
        {
            scopedLogger.LogError("Daybreak API failed to initialize in {duration}", sw.Elapsed);
            CancellationTokenSource.Cancel();
            return -1;
        }
    }

    [RequiresUnreferencedCode("The handler uses a static method that gets referenced, so there's no unreferenced code to worry about")]
    [RequiresDynamicCode("The handler uses a static method, so there's no dynamic code to worry about")]
    private static WebApplication CreateApplication(int port)
    {
        var builder = WebApplication.CreateBuilder()
                .WithConfiguration()
                .WithHosting(port)
                .WithSwagger()
                .WithSerializationContext()
                .WithLogging()
                .WithDaybreakServices()
                .WithWebSocketRoutes()
                .WithRoutes()
                .WithHealthChecks();

        // Add CORS policy - allow localhost and trusted community sites
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                    {
                        if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                        {
                            // Always allow localhost
                            if (uri.Host is "localhost" or "127.0.0.1")
                            {
                                return true;
                            }

                            // Allow known Guild Wars community sites
                            return uri.Host.EndsWith("guildwars.app", StringComparison.OrdinalIgnoreCase)
                                || uri.Host.EndsWith("gwmarket.net", StringComparison.OrdinalIgnoreCase);
                        }

                        return false;
                    })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseCors();
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
        var scopedLogger = app.Services.GetRequiredService<ILogger<EntryPoint>>().CreateScopedLogger();
        try
        {
            scopedLogger.LogDebug("Initializing GWCA");
            var gwcaAlreadyInitialized = PreloadNativeDependencies(scopedLogger);
            if (!gwcaAlreadyInitialized)
            {
                var result = GWCA.GW.Initialize();
                scopedLogger.LogDebug("GWCA.GW.Initialize() returned: {Result}", result);
            }

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            scopedLogger.LogCritical(ex, "Encountered fatal error while starting API server {Exception}", ex);
            throw;
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

    // Returns true if gwca.dll was already loaded by another tool (e.g. GWToolbox++),
    // meaning the caller must NOT call GW::Initialize() — doing so installs a second
    // set of inline detour hooks on the same GW packet dispatch functions, corrupting
    // the trampoline chain and crashing GW mid-session.
    private static bool PreloadNativeDependencies(ScopedLogger<EntryPoint> logger)
    {
        string? existingGwcaPath = null;
        string? daybreakGwcaPath = null;

        foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
        {
            if (existingGwcaPath is null
                && module.ModuleName?.Equals("gwca.dll", StringComparison.OrdinalIgnoreCase) is true)
            {
                existingGwcaPath = module.FileName;
            }

            if (daybreakGwcaPath is null
                && module.ModuleName?.Contains("Daybreak.API", StringComparison.OrdinalIgnoreCase) is true)
            {
                var moduleDir = Path.GetDirectoryName(module.FileName)!;
                var candidate = Path.Combine(moduleDir, "gwca.dll");
                if (File.Exists(candidate))
                {
                    daybreakGwcaPath = candidate;
                }
            }
        }

        if (existingGwcaPath is not null)
        {
            logger.LogWarning(
                "gwca.dll already loaded at {Path} — reusing existing instance to avoid hook conflicts with co-loaded tools.",
                existingGwcaPath);
            // Ensure [DllImport("gwca")] resolves to this instance; LoadLibrary
            // deduplicates by path so this is a no-op ref-count bump if it's the same file.
            NativeLibrary.Load(existingGwcaPath);
            return true;
        }

        if (daybreakGwcaPath is null)
        {
            logger.LogError("gwca.dll not found alongside Daybreak.API — native library not loaded.");
            return false;
        }

        NativeLibrary.Load(daybreakGwcaPath);
        logger.LogDebug("Preloaded gwca.dll from {Path}", daybreakGwcaPath);
        return false;
    }
}
