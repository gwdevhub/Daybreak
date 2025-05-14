namespace Daybreak.Shared.Models.Interop;
public readonly struct InstanceInfoContext
{
    public readonly GuildwarsPointer<uint> TerrainInfo;
    public readonly InstanceType InstanceType;
    public readonly GuildwarsPointer<AreaInfoContext> AreaInfo;
    public readonly uint TerrainCount;
    public readonly GuildwarsPointer<uint> TerrainInfo2;
}
