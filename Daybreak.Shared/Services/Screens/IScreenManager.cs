using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void SaveWindowPositionAndSize();
    void MoveWindowToSavedPosition();
    bool MoveGuildwarsToScreen(Screen screen);
}
