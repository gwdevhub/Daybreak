using Daybreak.Models;
using Microsoft.Extensions.Logging;
using System.Extensions;

namespace Daybreak.Services.Logging
{
    public sealed class JsonLoggerProvider : ILoggerProvider
    {
        private readonly ILogsManager logsManager;

        public JsonLoggerProvider(ILogsManager logsManager)
        {
            this.logsManager = logsManager.ThrowIfNull(nameof(logsManager));
        }

        public void LogEntry(Log log)
        {
            this.logsManager.WriteLog(log);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new JsonLogger(categoryName, this);
        }

        public void Dispose()
        {
        }
    }
}
