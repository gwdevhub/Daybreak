using Daybreak.Models;
using System.Collections.Generic;

namespace Daybreak.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void SaveWindowPositionAndSize();
    void MoveWindowToSavedPosition();
    void MoveGuildwarsToScreen(Screen screen);
}
