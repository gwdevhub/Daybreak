using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;

internal class PartyPlayerPayload : LivingEntityPayload
{
    public BuildPayload? Build { get; set; }
    public List<uint>? UnlockedProfession { get; set; }
}
