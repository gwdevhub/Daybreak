using Daybreak.Configuration;
using Daybreak.Exceptions;
using Daybreak.Services.ApplicationLifetime;
using Daybreak.Services.ViewManagement;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;
using Slim;
using System;
using System.Extensions;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Extensions.Http;

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

        protected override ILoggerFactory SetupLoggerFactory()
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new FileLoggerProvider("Daybreak.db"));
            return factory;
        }
        protected override void SetupServiceManager(IServiceManager serviceManager)
        {
            serviceManager.RegisterResolver(
                new HttpClientResolver()
                .WithHttpMessageHandlerFactory((serviceProvider, categoryType) =>
                {
                    var loggerType = typeof(ILogger<>).MakeGenericType(categoryType);
                    var logger = serviceProvider.GetService(loggerType).As<ILogger>();
                    return new LoggingHttpHandler(logger, new HttpClientHandler());
                }));
        }
        protected override void RegisterServices(IServiceProducer serviceProducer)
        {
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

            this.ServiceManager.GetService<ILogger>().LogCritical(e, $"Unhandled exception");
            if (e is FatalException fatalException)
            {
                this.ServiceManager.GetService<ILogger>().LogCritical(e, $"{nameof(FatalException)} encountered. Closing application.");
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
