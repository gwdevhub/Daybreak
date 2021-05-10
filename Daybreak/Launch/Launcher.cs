using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Services.ApplicationLifetime;
using Daybreak.Services.Logging;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Slim;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Extensions;

namespace Daybreak.Launch
{
    public sealed class Launcher : ExtendedApplication<MainWindow>
    {
        public static IServiceManager ApplicationServiceManager { get; private set; }
        private readonly static Launcher launcher = new();

        [STAThread]
        public static int Main()
        {
            return LaunchMainWindow();
        }

        protected override void RegisterServices(IServiceProducer serviceProducer)
        {
            ProjectConfiguration.RegisterFactories(this.ServiceManager);
            ProjectConfiguration.RegisterServices(this.ServiceManager);
            ProjectConfiguration.RegisterLifetimeServices(this.ServiceManager.GetService<IApplicationLifetimeManager>());
            ProjectConfiguration.RegisterViews(this.ServiceManager.GetService<IViewManager>());
        }
        protected override bool HandleException(Exception e)
        {
            if (e is null)
            {
                return false;
            }

            this.ServiceManager.GetService<ILogger>().LogCritical(e);
            if (e is FatalException fatalException)
            {
                MessageBox.Show(fatalException.ToString());
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

            MessageBox.Show(e.ToString());
            return true;
        }
        protected override void ApplicationStarting()
        {
            ApplicationServiceManager = this.ServiceManager;
            this.ServiceManager.GetService<IApplicationLifetimeManager>().OnStartup();
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
