using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AttributeContext
{
    public readonly uint Id;
    public readonly uint LevelBase;
    public readonly uint Level;
    public readonly uint DecrementPoints;
    public readonly uint IncrementPoints;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AttributeInfo
{
    public readonly uint ProfessionId;
    public readonly uint AttributeId;
    public readonly uint NameId;
    public readonly uint DescriptionId;
    public readonly uint IsPve;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct PartyAttribute
{
    public readonly uint AgentId;
    public readonly AttributeContext Attribute0;
    public readonly AttributeContext Attribute1;
    public readonly AttributeContext Attribute2;
    public readonly AttributeContext Attribute3;
    public readonly AttributeContext Attribute4;
    public readonly AttributeContext Attribute5;
    public readonly AttributeContext Attribute6;
    public readonly AttributeContext Attribute7;
    public readonly AttributeContext Attribute8;
    public readonly AttributeContext Attribute9;
    public readonly AttributeContext Attribute10;
    public readonly AttributeContext Attribute11;
    public readonly AttributeContext Attribute12;
    public readonly AttributeContext Attribute13;
    public readonly AttributeContext Attribute14;
    public readonly AttributeContext Attribute15;
    public readonly AttributeContext Attribute16;
    public readonly AttributeContext Attribute17;
    public readonly AttributeContext Attribute18;
    public readonly AttributeContext Attribute19;
    public readonly AttributeContext Attribute20;
    public readonly AttributeContext Attribute21;
    public readonly AttributeContext Attribute22;
    public readonly AttributeContext Attribute23;
    public readonly AttributeContext Attribute24;
    public readonly AttributeContext Attribute25;
    public readonly AttributeContext Attribute26;
    public readonly AttributeContext Attribute27;
    public readonly AttributeContext Attribute28;
    public readonly AttributeContext Attribute29;
    public readonly AttributeContext Attribute30;
    public readonly AttributeContext Attribute31;
    public readonly AttributeContext Attribute32;
    public readonly AttributeContext Attribute33;
    public readonly AttributeContext Attribute34;
    public readonly AttributeContext Attribute35;
    public readonly AttributeContext Attribute36;
    public readonly AttributeContext Attribute37;
    public readonly AttributeContext Attribute38;
    public readonly AttributeContext Attribute39;
    public readonly AttributeContext Attribute40;
    public readonly AttributeContext Attribute41;
    public readonly AttributeContext Attribute42;
    public readonly AttributeContext Attribute43;
    public readonly AttributeContext Attribute44;
    public readonly AttributeContext Attribute45;
    public readonly AttributeContext Attribute46;
    public readonly AttributeContext Attribute47;
    public readonly AttributeContext Attribute48;
    public readonly AttributeContext Attribute49;
    public readonly AttributeContext Attribute50;
    public readonly AttributeContext Attribute51;
    public readonly AttributeContext Attribute52;
    public readonly AttributeContext Attribute53;

    public readonly AttributeContext[] Attributes =>
    [
        this.Attribute0, this.Attribute1, this.Attribute2, this.Attribute3, this.Attribute4, this.Attribute5, this.Attribute6, this.Attribute7, this.Attribute8, this.Attribute9,
        this.Attribute10, this.Attribute11, this.Attribute12, this.Attribute13, this.Attribute14, this.Attribute15, this.Attribute16, this.Attribute17, this.Attribute18, this.Attribute19,
        this.Attribute20, this.Attribute21, this.Attribute22, this.Attribute23, this.Attribute24, this.Attribute25, this.Attribute26, this.Attribute27, this.Attribute28, this.Attribute29,
        this.Attribute30, this.Attribute31, this.Attribute32, this.Attribute33, this.Attribute34, this.Attribute35, this.Attribute36, this.Attribute37, this.Attribute38, this.Attribute39,
        this.Attribute40, this.Attribute41, this.Attribute42, this.Attribute43, this.Attribute44, this.Attribute45, this.Attribute46, this.Attribute47, this.Attribute48, this.Attribute49,
        this.Attribute50, this.Attribute51, this.Attribute52, this.Attribute53
    ];
}
