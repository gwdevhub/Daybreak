using Daybreak.Shared.Services.Menu;
using Daybreak.Views;
using System.Core.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.ViewModels;
public sealed class LaunchViewModel(
    IViewManager viewManager,
    IMenuService menuService)
    : ViewModelBase<LaunchViewModel, LaunchView>
{
    private readonly IMenuService menuService = menuService.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public override ValueTask ParametersSet(LaunchView view, CancellationToken cancellationToken)
    {
        this.menuService.CloseMenu();
        return base.ParametersSet(view, cancellationToken);
    }
}
