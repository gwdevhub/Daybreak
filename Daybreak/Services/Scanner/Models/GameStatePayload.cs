using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class GameStatePayload
{
    public CameraPayload? Camera { get; set; }
    public List<StatePayload>? States { get; set; }
}
