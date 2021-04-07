using System;
using System.Collections.Generic;
using System.Extensions;
using IServiceProvider = Slim.IServiceProvider;

namespace Daybreak.Services.ApplicationLifetime
{
    public sealed class ApplicationLifetimeManager : IApplicationLifetimeManager
    {
        private readonly IServiceProvider serviceProvider;
        private List<Type> RegisteredTypes { get; } = new List<Type>();

        public ApplicationLifetimeManager(IServiceProvider serviceProvider)
        {
            serviceProvider.ThrowIfNull(nameof(serviceProvider));

            this.serviceProvider = serviceProvider;
        }

        public void RegisterService<T>() where T : IApplicationLifetimeService
        {
            this.RegisteredTypes.Add(typeof(T));
        }

        public void OnStartup()
        {
            foreach (var serviceType in this.RegisteredTypes)
            {
                var service = this.serviceProvider.GetService(serviceType);
                service.As<IApplicationLifetimeService>().OnStartup();
            }
        }

        public void OnClosing()
        {
            foreach (var serviceType in this.RegisteredTypes)
            {
                var service = this.serviceProvider.GetService(serviceType);
                service.As<IApplicationLifetimeService>().OnClosing();
            }
        }
    }
}
