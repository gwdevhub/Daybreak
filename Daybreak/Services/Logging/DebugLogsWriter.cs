using System.Diagnostics;
using System.Logging;

namespace Daybreak.Services.Logging
{
    public sealed class DebugLogsWriter : IDebugLogsWriter
    {
        public void WriteLog(Log log)
        {
            Debug.WriteLine($"[{log.LogTime}]\t[{log.LogLevel}]\t[{log.Category}]\n{log.Message}");
        }
    }
}
