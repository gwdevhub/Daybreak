using Daybreak.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace Daybreak.Services.ExceptionHandling;

public sealed class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> logger;

    public ExceptionHandler(
        ILogger<ExceptionHandler> logger)
    {
        this.logger = logger.ThrowIfNull();
    }

    public bool HandleException(Exception e)
    {
        if (e is null)
        {
            return false;
        }

        if (this.logger is null)
        {
            return false;
        }

        if (e is FatalException fatalException)
        {
            this.logger.LogCritical(e, $"{nameof(FatalException)} encountered. Closing application");
            MessageBox.Show(fatalException.ToString());
            File.WriteAllText("crash.log", e.ToString());
            return false;
        }
        else if (e is TaskCanceledException)
        {
            this.logger.LogInformation(e, $"Encountered {nameof(TaskCanceledException)}. Ignoring");
            return true;
        }
        else if (e is TargetInvocationException targetInvocationException && e.InnerException is FatalException innerFatalException)
        {
            this.logger.LogCritical(e, $"{nameof(FatalException)} encountered. Closing application");
            MessageBox.Show(innerFatalException.ToString());
            File.WriteAllText("crash.log", e.ToString());
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

        this.logger.LogError(e, $"Unhandled exception caught {e.GetType()}");
        MessageBox.Show(e.ToString());
        return true;
    }
}
