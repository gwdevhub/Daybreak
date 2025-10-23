using Daybreak.Shared.Services.Mods;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class ModsViewModel(IModsManager modsManager)
    : ViewModelBase<ModsViewModel, ModsView>
{
    private readonly IModsManager modService = modsManager;

    public IEnumerable<IModService> Mods { get; private set; } = [];

    public override ValueTask ParametersSet(ModsView view, CancellationToken cancellationToken)
    {
        return base.ParametersSet(view, cancellationToken);
    }
}
