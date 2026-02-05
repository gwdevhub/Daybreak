using Daybreak.Shared.Services.ExecutableManagement;
using TrailBlazr.Services;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Copy;
public sealed class GuildWarsCopySelectionViewModel(
    IViewManager viewManager,
    IGuildWarsExecutableManager guildWarsExecutableManager)
    : ViewModelBase<GuildWarsCopySelectionViewModel, GuildWarsCopySelectionView>
{
    private readonly IViewManager viewManager = viewManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager = guildWarsExecutableManager;

    public List<string> Executables { get; private set; } = [];

    public override ValueTask Initialize(CancellationToken cancellationToken)
    {
        this.Executables = [.. this.guildWarsExecutableManager.GetExecutableList()];
        this.RefreshView();
        return base.Initialize(cancellationToken);
    }

    public void SelectExecutable(string executablePath)
    {
        this.viewManager.ShowView<GuildWarsCopyView>((nameof(GuildWarsCopyView.Source), executablePath));
    }
}
