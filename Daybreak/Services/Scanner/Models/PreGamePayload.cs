using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class PreGamePayload
{
    public int ChosenCharacterIndex { get; set; }
    public List<string>? Characters { get; set; }
}
