using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed class PreferencesService()
{
    public uint? GetEnumPreference(EnumPreference pref)
    {
        return GWCA.GW.UI.GetPreference(pref);
    }

    public bool SetEnumPreference(EnumPreference pref, uint value)
    {
        return GWCA.GW.UI.SetPreference(pref, value);
    }
}
