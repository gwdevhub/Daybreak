﻿using Slim;
using System;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Daybreak.Services.Navigation
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

        public void RegisterView<T>()
            where T : UserControl
        {
            this.serviceManager.RegisterTransient<T, T>();
        }

        public void RegisterPermanentView<T>()
            where T : UserControl
        {
            this.serviceManager.RegisterSingleton<T, T>();
        }

        public void ShowView<T>() where T : UserControl
        {
            this.ShowViewInner(typeof(T), null);
        }

        public void ShowView<T>(object dataContext) where T : UserControl
        {
            this.ShowViewInner(typeof(T), dataContext);
        }

        public void ShowView(Type type)
        {
            this.ShowViewInner(type, null);
        }

        public void ShowView(Type type, object dataContext)
        {
            this.ShowViewInner(type, dataContext);
        }

        private void ShowViewInner(Type viewType, object dataContext)
        {
            var scopedManager = this.serviceManager.CreateScope();
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = scopedManager.GetService(viewType).As<UserControl>();
                this.container.Children.Clear();
                this.container.Children.Add(view);
                view.DataContext = dataContext;
            });
        }
    }
}
