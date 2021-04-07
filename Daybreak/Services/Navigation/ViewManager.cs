using Slim;
using System;
using System.Extensions;
using System.Windows.Controls;

namespace Daybreak.Services.ViewManagement
{
    public sealed class ViewManager : IViewManager
    {
        private readonly IServiceManager serviceManager;
        private Panel container;

        public ViewManager(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager.ThrowIfNull(nameof(serviceManager));
        }

        public void RegisterContainer(Panel panel)
        {
            if (this.container is not null)
            {
                throw new InvalidOperationException("Cannot register a container multiple times");
            }

            this.container = panel;
        }

        public void RegisterView<T>() where T : UserControl
        {
            this.serviceManager.RegisterTransient<T, T>();
        }

        public void ShowView<T>() where T : UserControl
        {
            var view = this.serviceManager.GetService<T>();
            this.container.Children.Clear();
            this.container.Children.Add(view);
        }
    }
}
