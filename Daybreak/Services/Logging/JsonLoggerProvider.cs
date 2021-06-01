using Daybreak.Models;
using Microsoft.CorrelationVector;
using Microsoft.Extensions.Logging;
using System.Extensions;

namespace Daybreak.Services.Logging
{
    public sealed class JsonLoggerProvider : ILoggerProvider
    {
        private readonly ILogsManager logsManager;
        private CorrelationVector correlationVector;

        public JsonLoggerProvider(ILogsManager logsManager)
        {
            this.logsManager = logsManager.ThrowIfNull(nameof(logsManager));
            this.correlationVector = new CorrelationVector();
        }

        public void LogEntry(Log log)
        {
            if (this.correlationVector is not null)
            {
                log.Message = $"[{this.correlationVector.Value}] {log.Message}";
                this.correlationVector.Increment();
            }

            this.logsManager.WriteLog(log);
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (this.correlationVector is not null)
            {
                this.correlationVector = CorrelationVector.Extend(this.correlationVector.ToString());
            }

            return new JsonLogger(categoryName, this);
        }

        public void Dispose()
        {
        }
    }
}
