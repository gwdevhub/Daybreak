namespace Daybreak.API.Interop.GuildWars;

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

public enum CharSortOrder : uint
{
    None = 0,
    Alphabetize = 1,
    PvPRP = 2
}

public enum NumberPreference : uint
{
    AutoTournPartySort = 0,
    ChatState = 1,
    Count = 0x2b
}

public enum FlagPreference : uint
{
}

public enum StringPreference : uint
{
    Unk1 = 0,
    Unk2 = 1,
    LastCharacterName = 2,
    Count = 3
}
