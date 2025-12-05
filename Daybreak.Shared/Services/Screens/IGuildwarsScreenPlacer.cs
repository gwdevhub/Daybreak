using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.Screens;

public interface IGuildwarsScreenPlacer : IModService
{
    IEnumerable<Screen> GetScreens();

    void SetDesiredScreen(Screen screen);

    Screen? GetDesiredScreen();
}
