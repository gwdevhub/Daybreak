using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Extensions;

public static class CharacterInformationExtensions
{
    public unsafe static Profession GetPrimaryProfession(this CharacterInformation characterInformation)
    {
        return (Profession)((characterInformation.Props[2] >> 20) & 0xF);
    }

    public unsafe static Profession GetSecondaryProfession(this CharacterInformation characterInformation)
    {
        return (Profession)((characterInformation.Props[7] >> 10) & 0xF);
    }

    public unsafe static uint GetLevel(this CharacterInformation characterInformation)
    {
        return (characterInformation.Props[7] >> 4) & 0x3F;
    }

    public unsafe static Campaign GetCampaign(this CharacterInformation characterInformation)
    {
        return (Campaign)(characterInformation.Props[7] & 0xF);
    }

    public unsafe static MapID GetMapId(this CharacterInformation characterInformation)
    {
        return (MapID)((characterInformation.Props[0] >> 16) & 0xFFFF);
    }

    public unsafe static bool IsPvP(this CharacterInformation characterInformation)
    {
        return ((characterInformation.Props[7] >> 9) & 0x1) == 0x1;
    }
}
