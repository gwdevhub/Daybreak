using Daybreak.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Extensions;

namespace Daybreak.Services.Logging
{
    public sealed class JsonLogger : ILogger
    {
        private readonly string category;
        private readonly JsonLoggerProvider jsonLoggerProvider;

        public JsonLogger(string category, JsonLoggerProvider jsonLoggerProvider)
        {
            this.category = category;
            this.jsonLoggerProvider = jsonLoggerProvider.ThrowIfNull(nameof(jsonLoggerProvider));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            var log = new Log
            {
                LogLevel = logLevel,
                EventId = eventId.Name,
                Message = message,
                Category = category,
                LogTime = DateTime.Now
            };

            this.jsonLoggerProvider.LogEntry(log);
        }
    }
}
