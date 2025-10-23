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
        this.Mods = [.. this.modService.GetMods().OrderBy(m => m.Name)];
        return base.ParametersSet(view, cancellationToken);
    }

    public void ToggleMod(IModService mod)
    {
        if (!mod.IsInstalled)
        {
            //TODO: Handle mod installation flow
            return;
        }

        mod.IsEnabled = !mod.IsEnabled;
        this.RefreshView();
    }

    public async Task ManageMod(IModService mod)
    {
        if (!mod.CanCustomManage)
        {
            return;
        }

        await mod.OnCustomManagement(CancellationToken.None);
    }
}
