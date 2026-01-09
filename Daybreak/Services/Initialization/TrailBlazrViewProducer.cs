using Daybreak.Shared.Services.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions.Core;
using TrailBlazr.Models;
using TrailBlazr.ViewModels;
using TrailBlazr.Views;

namespace Daybreak.Services.Initialization;
public sealed class TrailBlazrViewProducer(IServiceCollection services, ILogger<TrailBlazrViewProducer> logger)
    : IViewProducer
{
    private readonly ILogger<TrailBlazrViewProducer> logger = logger;
    private readonly IServiceCollection services = services;

    public void RegisterView<TView, TViewModel>(bool isSingleton = false)
        where TView : ViewBase<TView, TViewModel>
        where TViewModel : ViewModelBase<TViewModel, TView>
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (isSingleton)
        {
            this.services.AddSingleton<TView>();
            this.services.AddSingleton<TViewModel>();
        }
        else
        {
            this.services.AddScoped<TView>();
            this.services.AddScoped<TViewModel>();
        }

        var viewRegistration = new ViewRegistration<TView, TViewModel>();
        this.services.AddSingleton(sp => viewRegistration);
        scopedLogger.LogDebug("Registered {View.Name}:{ViewModel.Name} as {lifetime}", typeof(TView).Name, typeof(TViewModel).Name, isSingleton ? "Singleton" : "Scoped");
    }
}
