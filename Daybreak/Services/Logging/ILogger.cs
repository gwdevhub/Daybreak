using Daybreak.Models;
using System;

namespace Daybreak.Services.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, Exception exception);
        void Log(LogLevel logLevel, string message);
    }
}
