using Daybreak.Services.Telemetry;
using Daybreak.Shared.Models.Views;
using Daybreak.Shared.Services.Navigation;
using Daybreak.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using System.Diagnostics;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Navigation;
internal sealed class BlazorViewManager(
    IServiceManager serviceManager,
    ILogger<BlazorViewManager> logger)
    : IBlazorViewManager
{
    private readonly IServiceManager serviceManager = serviceManager;
    private readonly ILogger<BlazorViewManager> logger = logger;

    private Activity? currentActivity;

    public void RegisterView<TView, TViewModel>(bool isSingleton = false)
        where TView : DaybreakView<TView, TViewModel>
        where TViewModel : DaybreakViewModel<TViewModel, TView>
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Registering view {ViewType} with ViewModel {ViewModelType}", typeof(TView).Name, typeof(TViewModel).Name);
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
    }

    public void ShowView<TView, TViewModel>(object? dataContext = null)
        where TView : DaybreakView<TView, TViewModel>
        where TViewModel : DaybreakViewModel<TViewModel, TView>
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        try
        {
            var appViewModel = this.serviceManager.GetRequiredService<AppViewModel>();
            this.currentActivity?.Stop();
            this.currentActivity = TelemetryHost.Source.StartActivity(typeof(TView).Name, ActivityKind.Internal);
            this.currentActivity?.Start();
            this.currentActivity?.SetTag("daybreak.view", typeof(TView).FullName);
            this.currentActivity?.SetTag("daybreak.viewModel", typeof(TViewModel).FullName);
            this.currentActivity?.SetTag("daybreak.dataContext", dataContext?.GetType().FullName ?? "null");
            this.currentActivity?.SetTag("component", nameof(BlazorViewManager));

            appViewModel.SetView<TView>("Hello world");
            scopedLogger.LogInformation("Showing view {ViewType} with ViewModel {ViewModelType}", typeof(TView).Name, typeof(TViewModel).Name);
        }
        catch(Exception ex)
        {
            scopedLogger.LogError(ex, "Failed to show view {ViewType} with ViewModel {ViewModelType}", typeof(TView).Name, typeof(TViewModel).Name);
        }
    }
}
