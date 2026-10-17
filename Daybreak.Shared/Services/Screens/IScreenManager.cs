using Daybreak.Shared.Models;
using System.Drawing;

namespace Daybreak.Shared.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void SaveWindowPositionAndSize();
    void MoveWindowToSavedPosition();
    bool MoveGuildwarsToScreen(Screen screen);
    bool MoveGuildwarsToScreen(Screen screen, uint processId) => this.MoveGuildwarsToScreen(screen);
    void ResetSavedPosition();
    Rectangle GetSavedPosition();
}
