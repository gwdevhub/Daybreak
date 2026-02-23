namespace Daybreak.API.Interop.GuildWars;

[GWCAEquivalent("EnumPreference")]
public enum EnumPreference : uint
{
    CharSortOrder = 0,
    AntiAliasing = 1,
    Reflections = 2,
    ShaderQuality = 3,
    ShadowQuality = 4,
    TerrainQuality = 5,
    InterfaceSize = 6,
    FrameLimiter = 7,
    Count = 8
}

[GWCAEquivalent("CharSortOrder")]
public enum CharSortOrder : uint
{
    None = 0,
    Alphabetize = 1,
    PvPRP = 2
}

[GWCAEquivalent("NumberPreference")]
public enum NumberPreference : uint
{
    AutoTournPartySort = 0,
    ChatState = 1,
    Count = 0x2b
}

[GWCAEquivalent("FlagPreference")]
public enum FlagPreference : uint
{
}

[GWCAEquivalent("StringPreference")]
public enum StringPreference : uint
{
    Unk1 = 0,
    Unk2 = 1,
    LastCharacterName = 2,
    Count = 3
}
