using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Services.ExceptionHandling;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;
using System.Logging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Daybreak.Services.ExceptionHandling;

internal sealed class ExceptionHandler(
    ICrashDumpService crashDumpService,
    INotificationService notificationService,
    ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    private enum HandleResult
    {
        Handled,
        Unhandled,
        Fatal
    }

    private static readonly IReadOnlyCollection<Func<Exception?, ScopedLogger<ExceptionHandler>, HandleResult>> ExceptionHandlers = [
        ExitOnNullException,
        ExitOnFatalException,
        IgnoreCancelledExceptions,
        HandleRecoverableBrowserExceptions,
        IgnoreUnexpectedGuildWarsCrashes
        ];

    private readonly ICrashDumpService crashDumpService = crashDumpService.ThrowIfNull();
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<ExceptionHandler> logger = logger.ThrowIfNull();

    public bool HandleException(Exception e)
    {
        var exceptionDate = DateTime.UtcNow;
        if (this.logger is null)
        {
            WriteCrashFiles(e, exceptionDate);
            return false;
        }

        var scopedLogger = this.logger.CreateScopedLogger();
        foreach (var handler in ExceptionHandlers)
        {
            switch (handler(e, scopedLogger))
            {
                case HandleResult.Handled:
                    return true;
                case HandleResult.Fatal:
                    WriteCrashFiles(e, exceptionDate);
                    return false;
                case HandleResult.Unhandled:
                default:
                    continue;
            }
        }

        scopedLogger.LogError(e, "Unhandled exception caught {exceptionType}", e.GetType());
        this.notificationService.NotifyError<MessageBoxHandler>(e.GetType().Name, e.ToString());
        return true;
    }

    private static HandleResult ExitOnNullException(Exception? e, ScopedLogger<ExceptionHandler> _) => e is null ? HandleResult.Fatal : HandleResult.Unhandled;

    private static HandleResult ExitOnFatalException(Exception? e, ScopedLogger<ExceptionHandler> _) => 
        (e is FatalException || (e is TargetInvocationException && e.InnerException is FatalException))
        ? HandleResult.Fatal
        : HandleResult.Unhandled;

    private static HandleResult IgnoreCancelledExceptions(Exception? e, ScopedLogger<ExceptionHandler> logger) =>
        (e is TaskCanceledException || e is OperationCanceledException ||
        (e is AggregateException aggregateException && 
            (aggregateException.InnerExceptions?.FirstOrDefault() is OperationCanceledException ||
             (aggregateException.InnerExceptions?.FirstOrDefault() is ArgumentException argumentException &&
                aggregateException.Message.Contains("A Task's exception(s) were not observed") &&
                argumentException.Message?.Contains("Process with an Id of") is true))))
        ? HandleResult.Handled
        : HandleResult.Unhandled;

    private static HandleResult HandleRecoverableBrowserExceptions(Exception? e, ScopedLogger<ExceptionHandler> logger) => 
        e is AggregateException aggregateException &&
        aggregateException.InnerExceptions.FirstOrDefault() is COMException comException &&
        comException.Message.Contains("Invalid window handle")
        ? HandleResult.Handled
        : HandleResult.Unhandled;

    private static HandleResult IgnoreUnexpectedGuildWarsCrashes(Exception? e, ScopedLogger<ExceptionHandler> logger)
    {
        if (e is AggregateException aggregateException &&
            aggregateException.InnerExceptions.FirstOrDefault() is ArgumentException argumentException &&
            argumentException.Message.Contains("Process with an Id of"))
        {
            logger.LogWarning(e, "Ignoring unexpected Guild Wars crash");
            return HandleResult.Handled;
        }

        if (e is ArgumentException argumentException2 &&
            argumentException2.Message.Contains("Process with an Id of"))
        {
            logger.LogWarning(e, "Ignoring unexpected Guild Wars crash");
            return HandleResult.Handled;
        }

        return HandleResult.Unhandled;
    }
        
    private void WriteCrashFiles(Exception e, DateTime crashTime)
    {
        WriteCrashLog(e, crashTime);
        this.crashDumpService.WriteCrashDump(GetCrashFileName("dmp", crashTime));
    }

    private static void WriteCrashLog(Exception? e, DateTime crashTime)
    {
        File.WriteAllText(GetCrashFileName("log", crashTime), e?.ToString() ?? "NULL EXCEPTION");
    }

    private static string GetCrashFileName(string extension, DateTime crashTime)
    {
        var directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Crashes");
        var path = Path.Combine(directoryPath, $"crash-{crashTime.ToOADate()}.{extension}");
        Directory.CreateDirectory(directoryPath);
        return path;
    }
}
