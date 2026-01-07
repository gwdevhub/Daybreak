namespace Daybreak.Shared.Models;

public static class InjectorResponses
{
    public enum GenericResults
    {
        Success =                       0,
        InvalidArgs =                   -1,
        InvalidMode =                   -2,
        InvalidInjector =               -3,
    }

    public enum InjectResult
    {
        Success =                       0,
        InvalidArgs =                   -1,
        InvalidInjector =               -3,
        InvalidProcess =                -101,
        InvalidDllPath =                -102,
        InvalidKernel32 =               -103,
        InvalidLoadLibraryW =           -104,
        ModulePathAllocationFailed =    -105,
        WriteMemoryFailed =             -106,
        CreateRemoteThreadFailed =      -107,
        RemoteThreadTimeout =           -108,
        DllExitCodeFailed =             -109,
        MemoryFreeFailed =              -110,
        InvalidGetProcAddress =         -111,
        DllPathWriteFailed =            -112,
        FunctionNameWriteFailed =       -113,
        InjectDataWriteFailed =         -114,
        StubAllocationFailed =          -115,
        InvalidLoadLibraryA =           -116,
    }

    public enum LaunchResult
    {
        Success =                       0,
        InvalidArgs =                   -1,
        InvalidInjector =               -3,
        InvalidPath =                   -301,
        InvalidElevated =               -302,
        LaunchTimeout =                 -303,
        PatchFailed =                   -304,
        LaunchFailed =                  -305,
    }

    public enum ResumeResult
    {
        Success =                       0,
        InvalidArgs =                   -1,
        InvalidInjector =               -3,
        InvalidThreadHandle =           -400,
    }
}
