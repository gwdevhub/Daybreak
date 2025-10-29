using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Screens;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Mods;
public sealed class ScreenSelectorViewModel(
    IGuildwarsScreenPlacer guildwarsScreenPlacer)
    : ViewModelBase<ScreenSelectorViewModel, ScreenSelectorView>
{
    private readonly IGuildwarsScreenPlacer guildwarsScreenPlacer = guildwarsScreenPlacer;

    public IEnumerable<Screen> Screens { get; private set; } = [];
    public Screen? SelectedScreen { get; private set; }

    public override ValueTask ParametersSet(ScreenSelectorView view, CancellationToken cancellationToken)
    {
        this.Screens = this.guildwarsScreenPlacer.GetScreens();
        this.SelectedScreen = this.guildwarsScreenPlacer.GetDesiredScreen();
        return base.ParametersSet(view, cancellationToken);
    }

    public void SelectScreen(Screen screen)
    {
        this.guildwarsScreenPlacer.SetDesiredScreen(screen);
        this.SelectedScreen = screen;
        this.RefreshView();
    }
}
