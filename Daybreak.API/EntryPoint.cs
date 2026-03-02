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

public static class EntryPoint
{
    private const bool IsDebug =
#if DEBUG
        true;
#else
        false;
#endif

    private const int StartPort = 5080;
    private static readonly TimeSpan InitializationTimeout = TimeSpan.FromSeconds(5);
    private static readonly CancellationTokenSource CancellationTokenSource = new();

    // GWCA log handler - must be static to prevent GC
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GWCALogHandler(nint context, int level, nint msg, nint file, uint line, nint function);
    private static GWCALogHandler? gwcaLogHandlerDelegate;
    private static ILogger? gwcaLogger;

    [UnmanagedCallersOnly(EntryPoint = "ThreadInit"), STAThread]
    [RequiresUnreferencedCode("The handler uses a static method that gets referenced, so there's no unreferenced code to worry about")]
    [RequiresDynamicCode("The handler uses a static method, so there's no dynamic code to worry about")]
    [SuppressMessage("Trimming", "IL2123:The use of 'RequiresUnreferencedCodeAttribute' on entry points is disallowed since the method will be called from outside the visible app.", Justification = "<Pending>")]
    [SuppressMessage("AOT", "IL3057:The use of 'RequiresDynamicCodeAttribute' on entry points is disallowed since the method will be called from outside the visible app.", Justification = "<Pending>")]
    public static int ThreadInit(IntPtr _, int __)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES", null, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("ASPNETCORE_PREVENTHOSTINGSTARTUP", "true", EnvironmentVariableTarget.Process);

        // Enable console if GW was launched with --debug-api or if we're running a debug build
        var args = Environment.GetCommandLineArgs();
        if (IsDebug ||
            args.Any(arg => arg.Equals("--debug-api", StringComparison.OrdinalIgnoreCase) || 
                            arg.Equals("-debug-api", StringComparison.OrdinalIgnoreCase)))
        {
            NativeMethods.AllocConsole();
        }

        var port = FindAvailablePort(StartPort);
        var app = CreateApplication(port);
        var runTask = Task.Run(() => StartServer(app), CancellationTokenSource.Token);
        var scopedLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(EntryPoint));
        var healthCheck = app.Services.GetRequiredService<HealthCheckService>();
        var sw = Stopwatch.StartNew();
        var healthy = false;
        while (sw.Elapsed < InitializationTimeout)
        {
            var status = Task.Run(() => healthCheck.CheckHealthAsync()).Result;
            scopedLogger.LogWarning("{methodName}: HealthCheck status: {status} in {duration}. Report: {report}", nameof(ThreadInit), status.Status, status.TotalDuration, string.Join("\n", status.Entries.Select(e => $"{e.Key}: {e.Value.Status}")));
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
            scopedLogger.LogInformation("{methodName}: Daybreak API is healthy on port {port}. Initialization succeeded in {duration}", nameof(ThreadInit), port, sw.Elapsed);
            return port;
        }
        else
        {
            scopedLogger.LogError("{methodName}: Daybreak API failed to initialize in {duration}", nameof(ThreadInit), sw.Elapsed);
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

        builder.Services.AddOpenApi();

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
        var scopedLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(EntryPoint));
        try
        {
            scopedLogger.LogDebug("{methodName}: Initializing GWCA", nameof(StartServer));
            PreloadNativeDependencies(scopedLogger);

            SetupGWCALogging(app);
            scopedLogger.LogDebug("{methodName}: GWCA log handler registered", nameof(StartServer));
            
            var result = GWCA.GW.Initialize();
            scopedLogger.LogDebug("{methodName}: GWCA.GW.Initialize() returned: {result}", nameof(StartServer), result);
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            scopedLogger.LogCritical(ex, "{methodName}: Encountered fatal error while starting API server {exception}", nameof(StartServer), ex);
            throw;
        }
    }

    private static void SetupGWCALogging(WebApplication app)
    {
        gwcaLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("GWCA");
        gwcaLogHandlerDelegate = OnGWCALog;
        var handlerPtr = Marshal.GetFunctionPointerForDelegate(gwcaLogHandlerDelegate);
        GWCA.GW.RegisterLogHandler(handlerPtr, nint.Zero);
    }

    private static void OnGWCALog(nint context, int level, nint msg, nint file, uint line, nint function)
    {
        var msgStr = msg != nint.Zero ? Marshal.PtrToStringAnsi(msg) : "<null>";
        var fileStr = file != nint.Zero ? Marshal.PtrToStringAnsi(file) : "<null>";
        var funcStr = function != nint.Zero ? Marshal.PtrToStringAnsi(function) : "<null>";

        var logLevel = level switch
        {
            0 => LogLevel.Trace,    // LEVEL_TRACE
            1 => LogLevel.Debug,    // LEVEL_DEBUG
            2 => LogLevel.Information, // LEVEL_INFO
            3 => LogLevel.Warning,  // LEVEL_WARN
            4 => LogLevel.Error,    // LEVEL_ERR
            5 => LogLevel.Critical, // LEVEL_CRITICAL
            _ => LogLevel.Debug
        };

        gwcaLogger?.Log(logLevel, "{gwcaSource}: {file}:{line} ({function}) {message}", nameof(GWCA), fileStr, line, funcStr, msgStr);
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

    private static void PreloadNativeDependencies(ILogger logger)
    {
        foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
        {
            if (module.ModuleName?.Contains("Daybreak.API", StringComparison.OrdinalIgnoreCase) is true)
            {
                var moduleDir = Path.GetDirectoryName(module.FileName)!;
                var gwcaPath = Path.Combine(moduleDir, "gwca.dll");
                if (File.Exists(gwcaPath))
                {
                    NativeLibrary.Load(gwcaPath);
                    logger.LogDebug("{methodName}: Preloaded gwca.dll from {path}", nameof(PreloadNativeDependencies), gwcaPath);
                }
                else
                {
                    logger.LogError("{methodName}: gwca.dll not found at {path}", nameof(PreloadNativeDependencies), gwcaPath);
                }

                break;
            }
        }
    }
}
