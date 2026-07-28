using Daybreak.Services.Logging;
using Serilog.Events;

namespace Daybreak.Services.Telemetry;

public sealed class TelemetryLogSink : RedactingLogEventSink
{
    public readonly static TelemetryLogSink Instance = new();

    public Action<LogEvent>? LoggingHandler { get; set; }

    private TelemetryLogSink()
    {
    }

    protected override void EmitRedacted(LogEvent logEvent)
    {
        this.LoggingHandler?.Invoke(logEvent);
    }
}
