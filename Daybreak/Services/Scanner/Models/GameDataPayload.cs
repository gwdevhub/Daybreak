using System.Collections.Generic;

namespace Daybreak.Services.Scanner.Models;
internal sealed class GameDataPayload
{
    public List<LivingEntityPayload>? LivingEntities { get; set; }
    public MainPlayerPayload? MainPlayer { get; set; }
    public List<MapIconPayload>? MapIcons { get; set; }
    public List<PartyPlayerPayload>? Party { get; set; }
    public List<WorldPlayerPayload>? WorldPlayers { get; set; }
    public uint TargetId { get; set; }
}
