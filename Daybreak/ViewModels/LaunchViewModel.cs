using Daybreak.Shared.Models.Views;
using Daybreak.Views;

namespace Daybreak.ViewModels;
public sealed class LaunchViewModel : DaybreakViewModel<LaunchViewModel, LaunchView>
{
    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        return base.Initialize(cancellationToken);
    }
}
