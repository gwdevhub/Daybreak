using TrailBlazr.Views;

namespace Daybreak.Views;
public abstract class ElevationViewBase<TView, TViewModel>
    : ViewBase<TView, TViewModel>
    where TViewModel : ElevationViewModelBase<TViewModel, TView>
    where TView : ElevationViewBase<TView, TViewModel>
{
}
