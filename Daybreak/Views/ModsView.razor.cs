using Daybreak.Models;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class ModsViewModel(
    INotificationService notificationService,
    IViewManager viewManager,
    IModsManager modsManager)
    : ViewModelBase<ModsViewModel, ModsView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly IViewManager viewManager = viewManager;
    private readonly IModsManager modService = modsManager;

    private CancellationTokenSource? cts;

    public IEnumerable<ModListEntry> Mods { get; private set; } = [];

    public override ValueTask ParametersSet(ModsView view, CancellationToken cancellationToken)
    {
        this.Mods = [.. this.modService.GetMods().OrderBy(m => m.Name)
            .Select(m => new ModListEntry
            {
                Name = m.Name,
                Description = m.Description,
                IsVisible = m.IsVisible,
                ModService = m,
                IsEnabled = m.IsEnabled,
                CanManage = m.CanCustomManage,
                IsInstalled = m.IsInstalled,
                CanUpdate = false,
                Loading = true,
            })];
        this.viewManager.ShowViewRequested += this.ViewManager_ShowViewRequested;
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        Task.Factory.StartNew(() => this.CheckForUpdates(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        return base.ParametersSet(view, cancellationToken);
    }

    public void ToggleMod(ModListEntry mod)
    {
        if (!mod.IsInstalled)
        {
            return;
        }

        mod.IsEnabled = !mod.IsEnabled;
        mod.ModService.IsEnabled = mod.IsEnabled;
        this.RefreshView();
    }

    public void InstallMod(ModListEntry mod)
    {
        this.viewManager.ShowView<ModInstallationConfirmationView>((nameof(ModInstallationConfirmationView.Name), mod.Name));
    }

    public async Task ManageMod(ModListEntry mod)
    {
        if (!mod.CanManage)
        {
            return;
        }

        await mod.ModService.OnCustomManagement(CancellationToken.None);
    }

    public async Task UpdateMod(ModListEntry mod)
    {
        if (!mod.CanUpdate)
        {
            return;
        }

        mod.Loading = true;
        await this.RefreshViewAsync();
        if (await mod.ModService.PerformUpdate(this.cts?.Token ?? CancellationToken.None))
        {
            mod.CanUpdate = false;
            this.notificationService.NotifyInformation($"{mod.Name} updated", $"{mod.Name} has been updated successfully.");
        }
        else
        {
            mod.CanUpdate = true;
            this.notificationService.NotifyError($"{mod.Name} update failed", $"{mod.Name} has failed to update. Please check logs for more details.");
        }

        mod.Loading = false;
        await this.RefreshViewAsync();
    }

    private void ViewManager_ShowViewRequested(object? sender, TrailBlazr.Models.ViewRequest e)
    {
        if (e.ViewType == typeof(ModsView))
        {
            return;
        }

        this.viewManager.ShowViewRequested -= this.ViewManager_ShowViewRequested;
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.cts = default;
    }

    private async ValueTask CheckForUpdates(CancellationToken cancellationToken)
    {
        foreach(var mod in this.Mods)
        {
            mod.Loading = true;
        }

        await this.RefreshViewAsync();
        foreach (var mod in this.Mods)
        {
            if (mod.ModService.IsInstalled)
            {
                mod.CanUpdate = await mod.ModService.IsUpdateAvailable(cancellationToken);
            }
            else
            {
                mod.CanUpdate = false;
            }

            mod.Loading = false;
        }

        await this.RefreshViewAsync();
    }
}
