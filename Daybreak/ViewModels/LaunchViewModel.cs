using Daybreak.Views;
using TrailBlazr.ViewModels;

namespace Daybreak.ViewModels;
public sealed class LaunchViewModel : ViewModelBase<LaunchViewModel, LaunchView>
{
    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        return base.Initialize(cancellationToken);
    }
}
