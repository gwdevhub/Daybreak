using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class GameStatePayload
{
    public List<StatePayload>? States { get; set; }
}
