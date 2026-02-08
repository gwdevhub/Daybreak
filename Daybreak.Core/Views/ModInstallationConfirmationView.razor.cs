using Daybreak.Shared.Services.Mods;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class ModInstallationConfirmationViewModel(
    IModsManager modsManager,
    IViewManager viewManager)
    : ViewModelBase<ModInstallationConfirmationViewModel, ModInstallationConfirmationView>
{
    private readonly IModsManager modsManager = modsManager;
    private readonly IViewManager viewManager = viewManager;

    public IModService? Mod { get; private set; }

    public override ValueTask ParametersSet(ModInstallationConfirmationView view, CancellationToken cancellationToken)
    {
        if (this.modsManager.GetMods().FirstOrDefault(m => m.Name == view.Name) is not IModService selectedMod)
        {
            throw new InvalidOperationException($"Mod not found by name {view.Name}");
        }

        this.Mod = selectedMod;
        return base.ParametersSet(view, cancellationToken);
    }

    public void Confirm()
    {
        if (this.Mod is null)
        {
            return;
        }

        this.viewManager.ShowView<ModInstallationView>((nameof(ModInstallationView.Name), this.Mod.Name));
    }

    public void Cancel()
    {
        this.viewManager.ShowView<ModsView>();
    }
}
