using Daybreak.Models;
using Daybreak.Services.Logging;
using System;

namespace Daybreak.Utils
{
    public static class LoggingExtensions
    {
        public static void LogInformation(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Information, message);
        }

        public static void LogError(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Error, message);
        }

        public static void LogCritical(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Critical, message);
        }

        public static void LogWarning(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Warning, message);
        }

        public static void LogInformation(this ILogger logger, Exception e)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Information, e);
        }

        public static void LogWarning(this ILogger logger, Exception e)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Warning, e);
        }

        public static void LogError(this ILogger logger, Exception e)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Error, e);
        }

        public static void LogCritical(this ILogger logger, Exception e)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Log(LogLevel.Critical, e);
        }
    }
}
