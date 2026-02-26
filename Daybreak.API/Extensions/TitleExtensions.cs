using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Extensions;

public static class TitleExtensions
{
    public static bool IsPercentageBased(this Title title)
    {
        return (title.Props & 1) != 0;
    }

    public static bool HasTiers(this Title title)
    {
        return (title.Props & 3) == 2;
    }
}
