using Daybreak.Shared.Models;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void SaveWindowPositionAndSize();
    void MoveWindowToSavedPosition();
    void MoveGuildwarsToScreen(Screen screen);
}
