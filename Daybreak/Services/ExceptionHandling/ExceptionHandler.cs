using Daybreak.Services.Notifications.Handlers;
using Daybreak.Shared;
using Daybreak.Shared.Exceptions;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System.Core.Extensions;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using WpfExtended.Blazor.Exceptions;

namespace Daybreak.Services.ExceptionHandling;

internal sealed class ExceptionHandler(
    INotificationService notificationService,
    ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly ILogger<ExceptionHandler> logger = logger.ThrowIfNull();

    public bool HandleException(Exception e)
    {
        if (e is null)
        {
            WriteCrashDump();
            return false;
        }

        if (this.logger is null)
        {
            WriteCrashDump();
            return false;
        }

        if (e is FatalException fatalException)
        {
            this.logger.LogCritical(e, $"{nameof(FatalException)} encountered. Closing application");
            File.WriteAllText("crash.log", e.ToString());
            WriteCrashDump();
            return false;
        }
        else if (e is CoreWebView2Exception coreWebView2Exception &&
            coreWebView2Exception.Args.ProcessFailedKind is CoreWebView2ProcessFailedKind.RenderProcessExited)
        {
            if (Global.CoreWebView2 is null)
            {
                this.logger.LogCritical(e, "CoreWebView2 is null. Cannot handle exception");
                return false;
            }

            this.logger.LogError(e, "CoreWebView2 render process exited unexpectedly. Reloading browser");
            Global.CoreWebView2.Reload();
        }
        else if (e is TaskCanceledException)
        {
            this.logger.LogDebug(e, $"Encountered {nameof(TaskCanceledException)}. Ignoring");
            return true;
        }
        else if (e is TargetInvocationException targetInvocationException && e.InnerException is FatalException innerFatalException)
        {
            this.logger.LogCritical(e, $"{nameof(FatalException)} encountered. Closing application");
            File.WriteAllText("crash.log", e.ToString());
            WriteCrashDump();
            return false;
        }
        else if (e is AggregateException aggregateException)
        {
            if (aggregateException.InnerExceptions.FirstOrDefault() is COMException comException &&
                comException.Message.Contains("Invalid window handle"))
            {
                /* 
                 * Ignore exception caused by browser failing to initialize due to missing window.
                 * Likely caused by switching views before browser was initialized.
                 */
                this.logger.LogError(e, "Failed to initialize browser");
                return true;
            }
            else if (aggregateException.InnerExceptions.FirstOrDefault() is OperationCanceledException)
            {
                this.logger.LogError(e, "Encountered operation canceled exception");
                return true;
            }
            else if (aggregateException.InnerExceptions.FirstOrDefault() is ArgumentException argumentException &&
                aggregateException.Message.Contains("A Task's exception(s) were not observed") &&
                argumentException.Message?.Contains("Process with an Id of") is true)
            {
                this.logger.LogError(e, "Encountered argument exception. Guild Wars process terminated unexpectedly");
                return true;
            }
        }
        else if (e.Message.Contains("Invalid window handle.") && e.StackTrace?.Contains("CoreWebView2Environment.CreateCoreWebView2ControllerAsync") is true)
        {
            /*
             * Ignore exception caused by browser failing to initialize due to missing window.
             * Likely caused by switching views before the browser was initialized.
             */
            this.logger.LogError(e, "Failed to initialize browser");
            return true;
        }
        else if (e is ArgumentException argumentException &&
            argumentException.Message.Contains("Process with an Id of"))
        {
            this.logger.LogError(e, "Encountered argument exception. Guild Wars process terminated unexpectedly");
            return true;
        }

        this.logger.LogError(e, $"Unhandled exception caught {e.GetType()}");
        this.notificationService.NotifyError<MessageBoxHandler>(e.GetType().Name, e.ToString());
        return true;
    }

    private static void WriteCrashDump()
    {
        string dumpFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"crash-{DateTime.Now.ToOADate()}.dmp");
        using var fs = new FileStream(dumpFilePath, FileMode.Create, FileAccess.Write);
        var process = Process.GetCurrentProcess();
        NativeMethods.MiniDumpWriteDump(process.Handle, process.Id, fs.SafeFileHandle, NativeMethods.MinidumpType.MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
    }
}
