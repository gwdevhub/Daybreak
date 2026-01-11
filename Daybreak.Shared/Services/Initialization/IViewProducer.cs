using TrailBlazr.ViewModels;
using TrailBlazr.Views;

namespace Daybreak.Shared.Services.Initialization;

public interface IViewProducer
{
    void RegisterView<TView, TViewModel>(bool isSingleton = false)
        where TView : ViewBase<TView, TViewModel>
        where TViewModel : ViewModelBase<TViewModel, TView>;
}
