using Serilog.Core;
using Serilog.Events;

namespace Daybreak.Services.Telemetry;

public sealed class TelemetryLogSink : ILogEventSink
{
    public readonly static TelemetryLogSink Instance = new();

    public Action<LogEvent>? LoggingHandler { get; set; }

    private TelemetryLogSink()
    {
    }

    public void Emit(LogEvent logEvent)
    {
        this.LoggingHandler?.Invoke(logEvent);
    }
}
