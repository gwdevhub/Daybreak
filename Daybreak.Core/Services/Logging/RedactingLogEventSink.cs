using Serilog.Core;
using Serilog.Events;

namespace Daybreak.Services.Logging;

/// <summary>
/// Base class for sinks that must never receive sensitive user information. Every emitted
/// <see cref="LogEvent"/> is passed through <see cref="LogEventRedactor"/> before being handed to
/// the concrete sink via <see cref="EmitRedacted"/>.
/// <para>
/// This lets us keep the Console sink (only visible to the local user) unredacted while masking
/// sensitive user information for sinks that persist or forward logs off the machine, such as the
/// in-memory sink used for exporting logs and the telemetry sink used to forward logs to the server.
/// </para>
/// </summary>
public abstract class RedactingLogEventSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        this.EmitRedacted(LogEventRedactor.Redact(logEvent));
    }

    /// <summary>
    /// Handles the log event after sensitive information has been masked.
    /// </summary>
    protected abstract void EmitRedacted(LogEvent logEvent);
}
