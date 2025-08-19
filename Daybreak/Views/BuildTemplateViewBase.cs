using Microsoft.AspNetCore.Components;
using TrailBlazr.Views;

namespace Daybreak.Views;
public abstract class BuildTemplateViewBase<TView, TViewModel> : ViewBase<TView, TViewModel>
    where TView : BuildTemplateViewBase<TView, TViewModel>
    where TViewModel : BuildTemplateViewModelBase<TViewModel, TView>
{
    [Parameter]
    public string BuildName { get; set; } = string.Empty;
}
