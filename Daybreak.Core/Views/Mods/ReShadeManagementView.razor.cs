using Daybreak.Shared.Models.ReShade;
using Daybreak.Shared.Services.ReShade;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Mods;

public sealed class ReShadeManagementViewModel(IReShadeService reShadeService)
    : ViewModelBase<ReShadeManagementViewModel, ReShadeManagementView>
{
    private readonly IReShadeService reShadeService = reShadeService;

    public bool IsLoading { get; private set; }
    public List<ShaderPackage> ShaderPackages { get; } = [];

    public override ValueTask ParametersSet(ReShadeManagementView view, CancellationToken cancellationToken)
    {
        this.LoadShaderPackages();
        return base.ParametersSet(view, cancellationToken);
    }

    public void OpenReShadeFolder()
    {
        this.reShadeService.OpenReShadeFolder();
    }

    public async void InstallShaderPackage(ShaderPackage package)
    {
        this.IsLoading = true;
        await this.RefreshViewAsync();
        await this.reShadeService.InstallPackage(package, CancellationToken.None);
        this.IsLoading = false;
        await this.RefreshViewAsync();
    }

    private async void LoadShaderPackages()
    {
        this.IsLoading = true;
        await this.RefreshViewAsync();
        var packages = await this.reShadeService.GetStockPackages(CancellationToken.None);
        this.ShaderPackages.ClearAnd().AddRange(packages);
        this.IsLoading = false;
        await this.RefreshViewAsync();
    }
}
