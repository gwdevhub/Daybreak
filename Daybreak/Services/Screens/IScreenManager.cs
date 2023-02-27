using Daybreak.Models;
using System.Collections.Generic;

namespace Daybreak.Services.Screens;

public interface IScreenManager
{
    IEnumerable<Screen> Screens { get; }
    void MoveGuildwarsToScreen(Screen screen);
}
