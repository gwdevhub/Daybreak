using Daybreak.Shared.Models.UMod;
using Daybreak.Shared.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Mods;

public sealed class UModManagementViewModel(
    IUModService uModService,
    ILogger<UModManagementViewModel> logger)
    : ViewModelBase<UModManagementViewModel, UModManagementView>
{
    private readonly IUModService uModService = uModService;
    private readonly ILogger<UModManagementViewModel> logger = logger;

    public List<UModEntry> Mods { get; } = [];
    public bool IsLoadingMods { get; private set; }

    public override ValueTask ParametersSet(UModManagementView view, CancellationToken cancellationToken)
    {
        this.Mods.ClearAnd()
            .AddRange(this.uModService.GetMods());
        this.RefreshView();
        return base.ParametersSet(view, cancellationToken);
    }

    public async void LoadModsFromDisk()
    {
        this.IsLoadingMods = true;
        await this.RefreshViewAsync();
        var modsLoaded = await this.uModService.LoadModsFromDisk(CancellationToken.None);
        if (modsLoaded.Count > 0)
        {
            this.Mods.ClearAnd()
                .AddRange(this.uModService.GetMods());
        }

        this.IsLoadingMods = false;
        await this.RefreshViewAsync();
    }

    public void RemoveMod(UModEntry uModEntry)
    {
        if (uModEntry.PathToFile is null)
        {
            return;
        }

        this.uModService.RemoveMod(uModEntry.PathToFile);
        this.Mods.ClearAnd()
            .AddRange(this.uModService.GetMods());
        this.RefreshView();
    }

    public void ToggleMod(UModEntry uModEntry)
    {
        uModEntry.Enabled = !uModEntry.Enabled;
        this.uModService.SaveMods(this.Mods);
        this.RefreshView();
    }

    public void MoveModUp(UModEntry uModEntry)
    {
        var index = this.Mods.IndexOf(uModEntry);
        if (index <= 0)
        {
            return;
        }

        this.Mods.RemoveAt(index);
        this.Mods.Insert(index - 1, uModEntry);
        this.uModService.SaveMods(this.Mods);
        this.RefreshView();
    }

    public void MoveModDown(UModEntry uModEntry)
    {
        var index = this.Mods.IndexOf(uModEntry);
        if (index < 0 || index >= this.Mods.Count - 1)
        {
            return;
        }

        this.Mods.RemoveAt(index);
        this.Mods.Insert(index + 1, uModEntry);
        this.uModService.SaveMods(this.Mods);
        this.RefreshView();
    }
}
