using Daybreak.Shared.Models;
using Daybreak.Shared.Models.LaunchConfigurations;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.Screens;

public interface IGuildwarsScreenPlacer : IModService
{
    IEnumerable<Screen> GetScreens();

    void SetDesiredScreen(Screen screen);

    void SetDesiredScreen(Screen screen, LaunchConfigurationWithCredentials? configuration) => this.SetDesiredScreen(screen);

    Screen? GetDesiredScreen();

    Screen? GetDesiredScreen(LaunchConfigurationWithCredentials? configuration) => this.GetDesiredScreen();
}
