using Daybreak.Shared.Models;
using System.Drawing;

namespace Daybreak.Shared.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void SaveWindowPositionAndSize();
    void MoveWindowToSavedPosition();
    bool MoveGuildwarsToScreen(Screen screen);
    void ResetSavedPosition();
    Rectangle GetSavedPosition();
}
