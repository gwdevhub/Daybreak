using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

public enum InstanceType
{
    Outpost,
    Explorable,
    Loading
};

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly unsafe struct InstanceInfoContext
{
    [FieldOffset(0x0004)]
    public readonly InstanceType InstanceType;
    [FieldOffset(0x0008)]
    public readonly AreaInfo* CurrentMapInfo;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AreaInfo
{
    public readonly uint Campaign;
    public readonly uint Continent;
    public readonly uint Region;
    public readonly uint RegionType;
    public readonly uint Flags;
}
