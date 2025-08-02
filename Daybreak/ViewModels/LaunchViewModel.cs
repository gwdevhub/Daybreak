using Daybreak.Views;
using System.Core.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.ViewModels;
public sealed class LaunchViewModel(IViewManager viewManager)
    : ViewModelBase<LaunchViewModel, LaunchView>
{
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        
        return base.Initialize(cancellationToken);
    }
}
