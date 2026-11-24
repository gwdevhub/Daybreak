using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Services.Interop;

public sealed class PreferencesService()
{
    public uint? GetEnumPreference(EnumPreference pref) => GWCA.GW.UI.GetEnumPreference((GWCA.GW.UI.EnumPreference)pref);

    public bool SetEnumPreference(EnumPreference pref, uint value) => GWCA.GW.UI.SetEnumPreference((GWCA.GW.UI.EnumPreference)pref, value);
}
