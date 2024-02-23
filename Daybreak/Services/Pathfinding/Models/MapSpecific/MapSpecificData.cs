using Daybreak.Models.Guildwars;
using System.Collections.Generic;
using System.Numerics;

namespace Daybreak.Services.Pathfinding.Models.MapSpecific;
/// <summary>
/// Credit to tedy @https://github.com/gwdevhub/GWToolboxpp/commit/66d70a28a90aa3d3b149a679185518a2f3ee09ad#diff-5535af0e9717d38e74483adec182f1b8f66724598a2158ce271141fe8d6651eb
/// </summary>
internal sealed class MapSpecificData
{
    public static readonly MapSpecificData IsleOfJade = new()
    {
        Map = Map.IsleOfJade,
        Teleports = [ 
            new Teleport { Enter = new Vector3(6796.00f, 735.00f, 12), Exit = new Vector3(2465.00f, 803.00f, 28), Direction = TeleportDirection.TwoWay },
            new Teleport { Enter = new Vector3(-3710.00f, 674.00f, 5), Exit = new Vector3(596.00f, 709.00f, 26), Direction = TeleportDirection.TwoWay }]
    };

    public static readonly MapSpecificData IsleOfJadeMission = new()
    {
        Map = Map.IsleOfJadeMission,
        Teleports = [
            new Teleport { Enter = new Vector3(6796.00f, 735.00f, 12), Exit = new Vector3(2465.00f, 803.00f, 28), Direction = TeleportDirection.TwoWay },
            new Teleport { Enter = new Vector3(-3710.00f, 674.00f, 5), Exit = new Vector3(596.00f, 709.00f, 26), Direction = TeleportDirection.TwoWay }]
    };

    public static readonly MapSpecificData IsleOfJadeOutpost = new()
    {
        Map = Map.IsleOfJadeOutpost,
        Teleports = [
            new Teleport { Enter = new Vector3(6796.00f, 735.00f, 12), Exit = new Vector3(2465.00f, 803.00f, 28), Direction = TeleportDirection.TwoWay },
            new Teleport { Enter = new Vector3(-3710.00f, 674.00f, 5), Exit = new Vector3(596.00f, 709.00f, 26), Direction = TeleportDirection.TwoWay }]
    };

    public static readonly List<MapSpecificData> MapSpecificDatas = [
        IsleOfJade,
        IsleOfJadeMission,
        IsleOfJadeOutpost,
    ];

    public Map Map { get; private init; } = default!;

    public List<Teleport> Teleports { get; private init; } = default!;

    internal MapSpecificData()
    {
    }
}
