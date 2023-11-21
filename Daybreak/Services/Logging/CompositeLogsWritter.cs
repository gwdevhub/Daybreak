using System.Collections.Generic;
using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class CompositeLogsWriter : ILogsWriter
{
    private readonly IEnumerable<ILogsWriter> logsWriters;

    public CompositeLogsWriter(params ILogsWriter[] innerLogsWriters)
    {
        this.logsWriters = innerLogsWriters;
    }

    public void WriteLog(Log log)
    {
        foreach (var logWriter in this.logsWriters)
        {
            logWriter.WriteLog(log);
        }
    }
}
