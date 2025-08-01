using Daybreak.Shared.Models.Views;

namespace Daybreak.Shared.Services.Navigation;
public interface IBlazorViewManager
{
    void RegisterView<TView, TViewModel>(bool isSingleton = false)
        where TView : DaybreakView<TView, TViewModel>
        where TViewModel : DaybreakViewModel<TViewModel, TView>;

    void ShowView<TView, TViewModel>(object? dataContext = null)
        where TView : DaybreakView<TView, TViewModel>
        where TViewModel : DaybreakViewModel<TViewModel, TView>;
}
