using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Logging;

namespace Daybreak.Services.Logging;

public sealed class EventViewerLogsWriter : IEventViewerLogsWriter
{
    private const string AppName = "Daybreak";
    private const string LogType = "Application";

    private readonly bool initialized = true;

    public EventViewerLogsWriter()
    {
        try
        {
            if (EventLog.SourceExists(AppName))
            {
                return;
            }

            EventLog.CreateEventSource(AppName, LogType);
        }
        catch
        {
            this.initialized = false;
        }
    }

    public void WriteLog(Log log)
    {
        if (this.initialized is false)
        {
            return;
        }

        if (log.LogLevel is not LogLevel.Critical)
        {
            return;
        }

        EventLog.WriteEntry(AppName, $"[{log.LogTime}]\t[{log.LogLevel}]\t[{log.Category}]\n{log.Message}", GetFromLogLevel(log.LogLevel));
    }

    private static EventLogEntryType GetFromLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Debug => EventLogEntryType.Information,
            LogLevel.Information => EventLogEntryType.Information,
            LogLevel.Warning => EventLogEntryType.Warning,
            LogLevel.Error => EventLogEntryType.Error,
            LogLevel.Critical => EventLogEntryType.Error,
            _ => EventLogEntryType.Information
        };
    }
}
