namespace Daybreak.Services.Pathfinding.Models.MapSpecific;
internal readonly struct TeleportNode
{
    public Teleport Teleport1 { get; init; }
    public Teleport Teleport2 { get; init; }
    public float Distance { get; init; }
}
