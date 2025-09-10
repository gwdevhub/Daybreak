using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Core.Extensions;

namespace Daybreak.Services.Logging;

public sealed class JSConsoleInterop(ILogger<JSConsoleInterop> logger) : IDisposable
{
    private const int MaxRetries = 10;

    private readonly ILogger<JSConsoleInterop> logger = logger.ThrowIfNull();

    private DotNetObjectReference<JSConsoleInterop>? dotNetRef;

    public void Dispose()
    {
        this.dotNetRef?.Dispose();
        this.dotNetRef = default;
    }

    public async ValueTask InitializeConsoleRedirection(IJSRuntime jsRuntime)
    {
        try
        {
            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    // Check if the blazorConsoleInterop object exists
                    var scriptLoaded = await jsRuntime.InvokeAsync<bool>("eval",
                        "typeof window.blazorConsoleInterop !== 'undefined' && typeof window.blazorConsoleInterop.initialize === 'function'");

                    if (scriptLoaded)
                    {
                        this.dotNetRef = DotNetObjectReference.Create(this);
                        await jsRuntime.InvokeVoidAsync("window.blazorConsoleInterop.initialize", this.dotNetRef);
                        this.logger.LogInformation("Console redirection initialized successfully");
                        return;
                    }
                }
                catch (JSException)
                {
                    // Script not ready yet, continue retrying
                }

                await Task.Delay(1000);
            }

            this.logger.LogWarning("Console redirection script failed to load after {maxRetries} attempts", MaxRetries);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to initialize console redirection");
        }
    }

    [JSInvokable]
    public void LogFromJavaScript(string level, string message, string? stack = null)
    {
        var logLevel = level.ToLowerInvariant() switch
        {
            "log" or "info" => LogLevel.Information,
            "warn" => LogLevel.Warning,
            "error" => LogLevel.Error,
            "debug" => LogLevel.Debug,
            "trace" => LogLevel.Trace,
            _ => LogLevel.Information
        };

        var fullMessage = stack is not null ? $"{message}\n{stack}" : message;
        this.logger.Log(logLevel, "[JavaScript] {message}", fullMessage);
    }
}
