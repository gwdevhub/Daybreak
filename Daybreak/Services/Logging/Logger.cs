using Daybreak.Models;
using System;

namespace Daybreak.Services.Logging
{
    public class Logger : ILogger
    {
        private readonly ILoggingDatabase loggingDatabase;
        public Logger(ILoggingDatabase loggingDatabase)
        {
            this.loggingDatabase = loggingDatabase;
        }

        public async void Log(LogLevel logLevel, Exception exception)
        {
            if (exception is null) throw new ArgumentNullException(nameof(exception));

            await this.loggingDatabase.InsertLog(
                new Log
                {
                    LogLevel = logLevel,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    Timestamp = DateTime.Now
                }).ConfigureAwait(false);
        }

        public async void Log(LogLevel logLevel, string message)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            await this.loggingDatabase.InsertLog(
                new Log
                {
                    LogLevel = logLevel,
                    Message = message,
                    StackTrace = Environment.StackTrace,
                    Timestamp = DateTime.Now
                }).ConfigureAwait(false);
        }
    }
}
