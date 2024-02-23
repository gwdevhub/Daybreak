using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models.MapSpecific;
/// <summary>
/// Credit to tedy @https://github.com/gwdevhub/GWToolboxpp/commit/66d70a28a90aa3d3b149a679185518a2f3ee09ad#diff-5535af0e9717d38e74483adec182f1b8f66724598a2158ce271141fe8d6651eb
/// </summary>
internal sealed class Teleport
{
    public Vector3 Enter { get; set; }
    public Vector3 Exit { get; set; }
    public TeleportDirection Direction { get; set; }
}
