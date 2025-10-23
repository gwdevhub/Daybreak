using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;
using System.Extensions;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class ModInstallationViewModel(
    IModsManager modsManager,
    IViewManager viewManager)
    : ViewModelBase<ModInstallationViewModel, ModInstallationView>
{
    private readonly IModsManager modsManager = modsManager;
    private readonly IViewManager viewManager = viewManager;

    public IModService? Mod { get; private set; }
    public double Progress { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public bool ContinueEnabled { get; private set; } = false;

    private readonly SemaphoreSlim installationSemaphore = new(1, 1);

    public override ValueTask ParametersSet(ModInstallationView view, CancellationToken cancellationToken)
    {
        if (this.modsManager.GetMods().FirstOrDefault(m => m.Name == view.Name) is not IModService selectedMod)
        {
            throw new InvalidOperationException($"Mod not found by name {view.Name}");
        }

        this.Mod = selectedMod;
        this.Progress = 0;
        this.Description = string.Empty;
        this.ContinueEnabled = false;
        _ = Task.Run(this.PerformInstallation);
        return base.ParametersSet(view, cancellationToken);
    }

    public void Continue()
    {
        this.viewManager.ShowView<ModsView>();
    }

    private async ValueTask PerformInstallation()
    {
        if (this.Mod is null)
        {
            return;
        }

        using var ctx = await this.installationSemaphore.Acquire();

        if (this.Mod.IsInstalled)
        {
            this.viewManager.ShowView<ModsView>();
            return;
        }

        var operation = this.Mod.PerformInstallation(CancellationToken.None);
        operation.ProgressChanged += this.OnInstallationProgressChanged;
        try
        {
            var result = await operation;
            this.ContinueEnabled = true;
            this.Description = result
                ? "Mod installation completed successfully."
                : "Mod installation failed";
            this.RefreshView();
            return;
        }
        finally
        {
            this.RefreshView();
            operation.ProgressChanged -= this.OnInstallationProgressChanged;
        }
    }

    private void OnInstallationProgressChanged(object? sender, ProgressUpdate args)
    {
        this.Progress = args.Percentage;
        this.Description = args.StatusMessage ?? string.Empty;
        this.RefreshView();
    }
}
