using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.LaunchConfigurations;
using Daybreak.Shared.Services.Screens;
using TrailBlazr.ViewModels;

namespace Daybreak.Views.Mods;
public sealed class ScreenSelectorViewModel(
    IGuildwarsScreenPlacer guildwarsScreenPlacer,
    ILaunchConfigurationService launchConfigurationService)
    : ViewModelBase<ScreenSelectorViewModel, ScreenSelectorView>
{
    private readonly IGuildwarsScreenPlacer guildwarsScreenPlacer = guildwarsScreenPlacer;
    private LaunchConfigurationWithCredentials? configuration;

    public IEnumerable<Screen> Screens { get; private set; } = [];
    public Screen? SelectedScreen { get; private set; }

    public override ValueTask ParametersSet(ScreenSelectorView view, CancellationToken cancellationToken)
    {
        this.configuration = string.IsNullOrEmpty(view.LaunchConfigurationIdentifier) ? null :
            launchConfigurationService.GetLaunchConfigurations().FirstOrDefault(c => c.Identifier == view.LaunchConfigurationIdentifier)
                ?? throw new InvalidOperationException("Launch configuration no longer exists");
        this.Screens = this.guildwarsScreenPlacer.GetScreens();
        this.SelectedScreen = this.guildwarsScreenPlacer.GetDesiredScreen(this.configuration);
        return base.ParametersSet(view, cancellationToken);
    }

    public void SelectScreen(Screen screen)
    {
        this.guildwarsScreenPlacer.SetDesiredScreen(screen, this.configuration);
        this.SelectedScreen = screen;
        this.RefreshView();
    }
}
