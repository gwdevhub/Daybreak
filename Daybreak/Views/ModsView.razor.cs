using Daybreak.Shared.Services.Mods;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class ModsViewModel(
    IViewManager viewManager,
    IModsManager modsManager)
    : ViewModelBase<ModsViewModel, ModsView>
{
    private readonly IViewManager viewManager = viewManager;
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
            this.viewManager.ShowView<ModInstallationConfirmationView>((nameof(ModInstallationConfirmationView.Name), mod.Name));
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
