using Serilog.Core;
using Serilog.Events;

namespace Daybreak.Services.Logging;

public sealed class InMemorySink : ILogEventSink, IDisposable
{
    private const int MaxLogEvents = 50000;

    private readonly List<LogEvent> logEvents = [];
    private readonly Lock snapShotLock = new();

    public readonly static InMemorySink Instance = new();

    private InMemorySink()
    {
    }

    public IEnumerable<LogEvent> LogEvents => this.logEvents.AsReadOnly();

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

            this.logEvents.Add(logEvent);
        }
    }
}
