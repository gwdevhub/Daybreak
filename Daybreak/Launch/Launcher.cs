using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Services.Updater.PostUpdate;
using Daybreak.Services.ViewManagement;
using Microsoft.Extensions.Logging;
using Slim;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Extensions;

namespace Daybreak.Launch
{
    public sealed class Launcher : ExtendedApplication<MainWindow>
    {
        private ILogger logger;
        private readonly static Launcher launcher = new();

        [STAThread]
        public static int Main()
        {
            return LaunchMainWindow();
        }

        protected override void SetupServiceManager(IServiceManager serviceManager)
        {
            ProjectConfiguration.RegisterResolvers(serviceManager);
        }
        protected override void RegisterServices(IServiceProducer serviceProducer)
        {
            ProjectConfiguration.RegisterServices(this.ServiceManager);
            ServiceManager.BuildSingletons();
            ProjectConfiguration.RegisterViews(this.ServiceManager.GetService<IViewManager>());
            ProjectConfiguration.RegisterPostUpdateActions(this.ServiceManager.GetService<IPostUpdateActionProducer>());
        }
        protected override bool HandleException(Exception e)
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
            else if (e.Message.Contains("Invalid window handle.") && e.StackTrace.Contains("CoreWebView2Environment.CreateCoreWebView2ControllerAsync"))
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
        protected override void ApplicationStarting()
        {
            this.logger = this.ServiceManager.GetService<ILogger<Launcher>>();
            this.RegisterViewContainer();
        }
        protected override void ApplicationClosing()
        {
        }

        private void RegisterViewContainer()
        {
            var viewManager = this.ServiceManager.GetService<IViewManager>();
            var mainWindow = this.ServiceManager.GetService<MainWindow>();
            viewManager.RegisterContainer(mainWindow.Container);
        }
        private static int LaunchMainWindow()
        {
            return launcher.Run();
        }
    }
}
