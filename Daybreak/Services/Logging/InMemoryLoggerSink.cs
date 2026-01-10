using Daybreak.Launch;
using Daybreak.Models;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using System.Collections.Immutable;

namespace Daybreak.Services.Logging;

public sealed class InMemorySink : ILogEventSink, IDisposable
{
    private const int MaxLogEvents = 50000;

    private readonly StructuredLogFormatter structuredFormatter = new(Launcher.OutputTemplate);
    private readonly MessageTemplateTextFormatter stringFormatter = new(Launcher.OutputTemplate);
    private readonly List<StructuredLogEntry> logEvents = [];
    private readonly Lock snapShotLock = new();

    public readonly static InMemorySink Instance = new();
    public event EventHandler<StructuredLogEntry>? LogEventEmitted;

    private InMemorySink()
    {
    }

    public IEnumerable<StructuredLogEntry> LogEvents => this.logEvents;

    public void Dispose()
    {
        this.logEvents.Clear();
    }

    public void Emit(LogEvent logEvent)
    {
        lock (this.snapShotLock)
        {
            if (this.logEvents.Count > MaxLogEvents)
            {
                this.logEvents.RemoveRange(0, MaxLogEvents / 10);
            }

            var log = this.FormatStructuredLogEvent(logEvent);
            this.logEvents.Add(log);
            this.LogEventEmitted?.Invoke(this, log);
        }
    }

    public ImmutableArray<StructuredLogEntry> GetSnapshot()
    {
        lock (this.snapShotLock)
        {
            return [.. this.logEvents];
        }
    }

    private string FormatStringLogEvent(LogEvent logEvent)
    {
        using var writer = new StringWriter();
        this.stringFormatter.Format(logEvent, writer);
        return writer.ToString();
    }

    private StructuredLogEntry FormatStructuredLogEvent(LogEvent logEvent)
    {
        var tokens = this.structuredFormatter.Format(logEvent);
        var text = this.FormatStringLogEvent(logEvent);
        return new StructuredLogEntry(logEvent, [.. tokens], text);
    }
}
