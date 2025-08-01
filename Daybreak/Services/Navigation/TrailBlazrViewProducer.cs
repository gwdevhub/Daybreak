using Daybreak.Shared.Services.Navigation;
using Slim;
using System.Core.Extensions;
using TrailBlazr.Models;
using TrailBlazr.ViewModels;
using TrailBlazr.Views;

namespace Daybreak.Services.Navigation;
public sealed class TrailBlazrViewProducer(IServiceManager serviceManager)
    : IViewProducer
{
    private readonly IServiceManager serviceManager = serviceManager;

    public void RegisterView<TView, TViewModel>(bool isSingleton = false)
        where TView : ViewBase<TView, TViewModel>
        where TViewModel : ViewModelBase<TViewModel, TView>
    {
        this.serviceManager.ThrowIfNull();
        if (isSingleton)
        {
            this.serviceManager.RegisterSingleton<TView>();
            this.serviceManager.RegisterSingleton<TViewModel>();
        }
        else
        {
            this.serviceManager.RegisterScoped<TView>();
            this.serviceManager.RegisterScoped<TViewModel>();
        }

        this.serviceManager.RegisterSingleton(sp => new ViewRegistration<TView, TViewModel>());
    }
}
