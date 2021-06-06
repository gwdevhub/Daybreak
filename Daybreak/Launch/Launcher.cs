using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Services.ApplicationLifetime;
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
            ProjectConfiguration.RegisterLifetimeServices(this.ServiceManager.GetService<IApplicationLifetimeManager>());
            ProjectConfiguration.RegisterViews(this.ServiceManager.GetService<IViewManager>());
        }
        protected override bool HandleException(Exception e)
        {
            if (e is null)
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
                     * Likely caused by switching windows before browser was initialized.
                     */
                    return true;
                }
            }

            this.logger.LogError(e, $"Unhandled exception caught {e.GetType()}");
            MessageBox.Show(e.ToString());
            return true;
        }
        protected override void ApplicationStarting()
        {
            this.ServiceManager.GetService<IApplicationLifetimeManager>().OnStartup();
            this.logger = this.ServiceManager.GetService<ILogger<Launcher>>();
            this.RegisterViewContainer();
        }
        protected override void ApplicationClosing()
        {
            this.ServiceManager.GetService<IApplicationLifetimeManager>().OnClosing();
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
