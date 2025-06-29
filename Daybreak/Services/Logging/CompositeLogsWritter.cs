using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class CompositeLogsWriter(params ILogsWriter[] innerLogsWriters) : ILogsWriter
{
    private readonly IEnumerable<ILogsWriter> logsWriters = innerLogsWriters;

    public void WriteLog(Log log)
    {
        foreach (var logWriter in this.logsWriters)
        {
            logWriter.WriteLog(log);
        }
    }
}
