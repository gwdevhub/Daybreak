using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Services.Logging
{
    public sealed class CVLoggerFactory : ILoggerFactory
    {
        private readonly LoggerFactory loggerFactory = new();

        public CVLoggerFactory(ILogsManager logsManager)
        {
            this.loggerFactory.AddProvider(new JsonLoggerProvider(logsManager));
        }

        public void AddProvider(ILoggerProvider provider)
        {
            ((ILoggerFactory)this.loggerFactory).AddProvider(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return ((ILoggerFactory)this.loggerFactory).CreateLogger(categoryName);
        }

        public void Dispose()
        {
            ((IDisposable)this.loggerFactory).Dispose();
        }
    }
}
