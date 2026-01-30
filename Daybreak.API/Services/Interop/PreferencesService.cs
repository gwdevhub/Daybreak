using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;

namespace Daybreak.API.Services.Interop;

public sealed class PreferencesService
    : IAddressHealthService
{
    private readonly MemoryScanningService memoryScanningService;
    private readonly ILogger<PreferencesService> logger;

    private readonly GWAddressCache preferencesInitializedAddress;
    private readonly GWAddressCache getEnumPreferenceAddress;
    private readonly GWAddressCache setEnumPreferenceAddress;
    private readonly GWAddressCache getNumberPreferenceAddress;
    private readonly GWAddressCache setNumberPreferenceAddress;
    private readonly GWAddressCache getFlagPreferenceAddress;
    private readonly GWAddressCache setFlagPreferenceAddress;
    private readonly GWAddressCache getStringPreferenceAddress;
    private readonly GWAddressCache setStringPreferenceAddress;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GetEnumPreferenceDelegate(uint prefId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetEnumPreferenceDelegate(uint prefId, uint value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GetNumberPreferenceDelegate(uint prefId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetNumberPreferenceDelegate(uint prefId, uint value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool GetFlagPreferenceDelegate(uint prefId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetFlagPreferenceDelegate(uint prefId, [MarshalAs(UnmanagedType.I1)] bool value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint GetStringPreferenceDelegate(uint prefId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void SetStringPreferenceDelegate(uint prefId, nint value);

    public PreferencesService(
        MemoryScanningService memoryScanningService,
        ILogger<PreferencesService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();

        this.preferencesInitializedAddress = new GWAddressCache(this.GetPreferencesInitializedAddress);
        this.getEnumPreferenceAddress = new GWAddressCache(this.GetGetEnumPreferenceAddress);
        this.setEnumPreferenceAddress = new GWAddressCache(this.GetSetEnumPreferenceAddress);
        this.getNumberPreferenceAddress = new GWAddressCache(this.GetGetNumberPreferenceAddress);
        this.setNumberPreferenceAddress = new GWAddressCache(this.GetSetNumberPreferenceAddress);
        this.getFlagPreferenceAddress = new GWAddressCache(this.GetGetFlagPreferenceAddress);
        this.setFlagPreferenceAddress = new GWAddressCache(this.GetSetFlagPreferenceAddress);
        this.getStringPreferenceAddress = new GWAddressCache(this.GetGetStringPreferenceAddress);
        this.setStringPreferenceAddress = new GWAddressCache(this.GetSetStringPreferenceAddress);
    }

    public List<AddressState> GetAddressStates()
    {
        return
        [
            new AddressState { Address = this.preferencesInitializedAddress.GetAddress() ?? 0, Name = nameof(this.preferencesInitializedAddress) },
            new AddressState { Address = this.getEnumPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.getEnumPreferenceAddress) },
            new AddressState { Address = this.setEnumPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.setEnumPreferenceAddress) },
            new AddressState { Address = this.getNumberPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.getNumberPreferenceAddress) },
            new AddressState { Address = this.setNumberPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.setNumberPreferenceAddress) },
            new AddressState { Address = this.getFlagPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.getFlagPreferenceAddress) },
            new AddressState { Address = this.setFlagPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.setFlagPreferenceAddress) },
            new AddressState { Address = this.getStringPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.getStringPreferenceAddress) },
            new AddressState { Address = this.setStringPreferenceAddress.GetAddress() ?? 0, Name = nameof(this.setStringPreferenceAddress) },
        ];
    }

    public unsafe bool PreferencesInitialized()
    {
        var addr = this.preferencesInitializedAddress.GetAddress();
        return addr is not null && addr != 0 && *(byte*)addr != 0;
    }

    public uint? GetEnumPreference(EnumPreference pref)
    {
        if (!this.PreferencesInitialized())
        {
            return null;
        }

        var funcAddr = this.getEnumPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return null;
        }

        var func = Marshal.GetDelegateForFunctionPointer<GetEnumPreferenceDelegate>((nint)funcAddr);
        return func((uint)pref);
    }

    public bool SetEnumPreference(EnumPreference pref, uint value)
    {
        if (!this.PreferencesInitialized())
        {
            return false;
        }

        var funcAddr = this.setEnumPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return false;
        }

        var func = Marshal.GetDelegateForFunctionPointer<SetEnumPreferenceDelegate>((nint)funcAddr);
        func((uint)pref, value);
        return true;
    }

    public uint? GetNumberPreference(NumberPreference pref)
    {
        if (!this.PreferencesInitialized())
        {
            return null;
        }

        var funcAddr = this.getNumberPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return null;
        }

        var func = Marshal.GetDelegateForFunctionPointer<GetNumberPreferenceDelegate>((nint)funcAddr);
        return func((uint)pref);
    }

    public bool SetNumberPreference(NumberPreference pref, uint value)
    {
        if (!this.PreferencesInitialized())
        {
            return false;
        }

        var funcAddr = this.setNumberPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return false;
        }

        var func = Marshal.GetDelegateForFunctionPointer<SetNumberPreferenceDelegate>((nint)funcAddr);
        func((uint)pref, value);
        return true;
    }

    public bool? GetFlagPreference(FlagPreference pref)
    {
        if (!this.PreferencesInitialized())
        {
            return null;
        }

        var funcAddr = this.getFlagPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return null;
        }

        var func = Marshal.GetDelegateForFunctionPointer<GetFlagPreferenceDelegate>((nint)funcAddr);
        return func((uint)pref);
    }

    public bool SetFlagPreference(FlagPreference pref, bool value)
    {
        if (!this.PreferencesInitialized())
        {
            return false;
        }

        var funcAddr = this.setFlagPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return false;
        }

        var func = Marshal.GetDelegateForFunctionPointer<SetFlagPreferenceDelegate>((nint)funcAddr);
        func((uint)pref, value);
        return true;
    }

    public unsafe string? GetStringPreference(StringPreference pref)
    {
        if (!this.PreferencesInitialized())
        {
            return null;
        }

        var funcAddr = this.getStringPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return null;
        }

        var func = Marshal.GetDelegateForFunctionPointer<GetStringPreferenceDelegate>((nint)funcAddr);
        var ptr = func((uint)pref);
        if (ptr == 0)
        {
            return null;
        }

        return Marshal.PtrToStringUni(ptr);
    }

    public bool SetStringPreference(StringPreference pref, string value)
    {
        if (!this.PreferencesInitialized())
        {
            return false;
        }

        var funcAddr = this.setStringPreferenceAddress.GetAddress();
        if (funcAddr is null or 0)
        {
            return false;
        }

        var strPtr = Marshal.StringToHGlobalUni(value);
        try
        {
            var func = Marshal.GetDelegateForFunctionPointer<SetStringPreferenceDelegate>((nint)funcAddr);
            func((uint)pref, strPtr);
            return true;
        }
        finally
        {
            Marshal.FreeHGlobal(strPtr);
        }
    }

    private nuint GetPreferencesInitializedAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        // GWCA: FindAssertion("\\Code\\Gw\\Pref\\PrApi.cpp", "location < arrsize(s_flushDelay)", 0, -0x12)
        var address = this.memoryScanningService.FindAssertion(
            @"\Code\Gw\Pref\PrApi.cpp",
            "location < arrsize(s_flushDelay)",
            0,
            -0x12);

        if (address is 0)
        {
            scopedLogger.LogError("Failed to find preferences initialized assertion");
            return 0;
        }

        // Dereference to get actual address
        var ptr = (nuint)Marshal.ReadIntPtr((nint)address);
        if (ptr is 0)
        {
            scopedLogger.LogError("Failed to dereference preferences initialized address");
            return 0;
        }

        scopedLogger.LogInformation("Preferences initialized address: 0x{address:X8}", ptr);
        return ptr;
    }

    private nuint GetGetEnumPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindUseOfString("pref < PREF_ENUMS");
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find 'pref < PREF_ENUMS' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve GetEnumPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("GetEnumPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetSetEnumPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindUseOfString("tableCount");
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find 'tableCount' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve SetEnumPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("SetEnumPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetGetNumberPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindUseOfString("pref < PREF_VALUES");
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find 'pref < PREF_VALUES' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve GetNumberPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("GetNumberPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetSetNumberPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindNthUseOfString("pref < PREF_VALUES", 1);
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find second 'pref < PREF_VALUES' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve SetNumberPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("SetNumberPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetGetFlagPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindUseOfString("pref < PREF_FLAGS");
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find 'pref < PREF_FLAGS' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve GetFlagPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("GetFlagPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetSetFlagPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindNthUseOfString("pref < PREF_FLAGS", 1);
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find second 'pref < PREF_FLAGS' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve SetFlagPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("SetFlagPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetGetStringPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindUseOfString("pref < PREF_STRINGS");
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find 'pref < PREF_STRINGS' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve GetStringPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("GetStringPreference address: 0x{address:X8}", address);
        return address;
    }

    private nuint GetSetStringPreferenceAddress()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var stringUsage = this.memoryScanningService.FindNthUseOfString("pref < PREF_STRINGS", 1);
        if (stringUsage is 0)
        {
            scopedLogger.LogError("Failed to find second 'pref < PREF_STRINGS' string usage");
            return 0;
        }

        var address = this.memoryScanningService.ToFunctionStart(stringUsage);
        if (address is 0)
        {
            scopedLogger.LogError("Failed to resolve SetStringPreference function start");
            return 0;
        }

        scopedLogger.LogInformation("SetStringPreference address: 0x{address:X8}", address);
        return address;
    }
}
