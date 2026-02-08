using System.Runtime.InteropServices;

namespace Daybreak.Injector;

/// <summary>
/// Win32 P/Invoke declarations used by the Daybreak Injector.
/// Uses source-generated marshalling (LibraryImport) where possible.
/// </summary>
internal static partial class NativeMethods
{
    // ──────────────────────────────────────────────
    //  kernel32.dll
    // ──────────────────────────────────────────────

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CloseHandle(nint hObject);

    [LibraryImport("kernel32.dll")]
    public static partial uint ResumeThread(nint hThread);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint OpenThread(ThreadAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwThreadId);

    [LibraryImport("kernel32.dll")]
    public static partial nint OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessID);

    [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", StringMarshalling = StringMarshalling.Utf16)]
    public static partial nint GetModuleHandle(string lpModuleName);

    [LibraryImport("kernel32.dll", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    public static partial nint GetProcAddress(nint hModule, string procName);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint VirtualAllocEx(nint hProcess, nint lpAddress, nint dwSize, uint dwAllocationType, uint dwProtect);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool VirtualFreeEx(nint hProcess, nint lpAddress, uint dwSize, uint dwFreeType);

    [LibraryImport("kernel32.dll")]
    public static partial nint CreateRemoteThread(nint hProcess, nint lpThreadAttributes, uint dwStackSize, nint lpStartAddress, nint lpParameter, uint dwCreationFlags, out nint lpThreadId);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial uint GetExitCodeThread(nint hHandle, out nint dwExitCode);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out nint lpNumberOfBytesRead);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, nint lpBuffer, int nSize, out nint lpNumberOfBytesRead);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, byte[] lpBuffer, int dwSize, out nint lpNumberOfBytesWritten);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, nint lpBuffer, int nSize, out nint lpNumberOfBytesWritten);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint LocalFree(nint hMem);

    // CreateProcess needs manual DllImport due to complex string + struct marshalling with AOT
    [DllImport("kernel32.dll", EntryPoint = "CreateProcessW", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CreateProcess(
        string? lpApplicationName,
        string lpCommandLine,
        ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
        uint dwCreationFlags,
        nint lpEnvironment,
        string? lpCurrentDirectory,
        [In] ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);

    // ──────────────────────────────────────────────
    //  advapi32.dll
    // ──────────────────────────────────────────────

    [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUserW", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CreateProcessAsUser(
        nint hToken,
        string? lpApplicationName,
        string lpCommandLine,
        ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles,
        uint dwCreationFlags,
        nint lpEnvironment,
        string? lpCurrentDirectory,
        ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SaferCreateLevel(SaferLevelScope scopeId, SaferLevel levelId, SaferOpen openFlags, out nint levelHandle, nint reserved);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SaferComputeTokenFromLevel(nint levelHandle, nint inAccessToken, out nint outAccessToken, uint flags, nint lpReserved);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SaferCloseLevel(nint levelHandle);

    [LibraryImport("advapi32.dll", EntryPoint = "ConvertStringSidToSidW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ConvertStringSidToSid(string stringSid, out nint ptrSid);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetTokenInformation(nint tokenHandle, TokenInformationClass tokenInformationClass, ref TokenMandatoryLabel tokenInformation, uint tokenInformationLength);

    [LibraryImport("advapi32.dll")]
    public static partial uint GetLengthSid(nint pSid);

    // ──────────────────────────────────────────────
    //  ntdll.dll
    // ──────────────────────────────────────────────

    [LibraryImport("ntdll.dll", SetLastError = true)]
    public static partial int NtQueryInformationProcess(nint hProcess, ProcessInfoClass pic, out ProcessBasicInformation pbi, int cb, out int pSize);

    // ──────────────────────────────────────────────
    //  Enums
    // ──────────────────────────────────────────────

    [Flags]
    public enum CreationFlags : uint
    {
        CreateSuspended = 0x00000004,
        DetachedProcess = 0x00000008,
        CreateNoWindow = 0x08000000,
        ExtendedStartupInfoPresent = 0x00080000
    }

    public enum SaferLevelScope : uint
    {
        Machine = 1,
        User = 2
    }

    public enum SaferLevel : uint
    {
        Disallowed = 0,
        Untrusted = 0x1000,
        Constrained = 0x10000,
        NormalUser = 0x20000,
        FullyTrusted = 0x40000
    }

    public enum SaferOpen : uint
    {
        Open = 1
    }

    public enum TokenInformationClass : uint
    {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId,
        TokenGroupsAndPrivileges,
        TokenSessionReference,
        TokenSandBoxInert,
        TokenAuditPolicy,
        TokenOrigin,
        TokenElevationType,
        TokenLinkedToken,
        TokenElevation,
        TokenHasRestrictions,
        TokenAccessInformation,
        TokenVirtualizationAllowed,
        TokenVirtualizationEnabled,
        TokenIntegrityLevel,
        TokenUiAccess,
        TokenMandatoryPolicy,
        TokenLogonSid,
        MaxTokenInfoClass
    }

    public enum ProcessInfoClass : uint
    {
        ProcessBasicInformation = 0x00,
    }

    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

    [Flags]
    public enum MemoryProtection : uint
    {
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READ_WRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
    }

    [Flags]
    public enum AllocationType : uint
    {
        Commit = 0x00001000,
        Reserve = 0x00002000,
        Reset = 0x00080000,
        ResetUndo = 0x1000000,
        LargePages = 0x20000000,
        Physical = 0x00400000,
        TopDown = 0x00100000,
        WriteWatch = 0x00200000
    }

    [Flags]
    public enum ThreadAccess : uint
    {
        Terminate = 0x0001,
        SuspendResume = 0x0002,
        GetContext = 0x0008,
        SetContext = 0x0010,
        SetInformation = 0x0020,
        QueryInformation = 0x0040,
        SetThreadToken = 0x0080,
        Impersonate = 0x0100,
        DirectImpersonation = 0x0200
    }

    // ──────────────────────────────────────────────
    //  Structs
    // ──────────────────────────────────────────────

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct StartupInfo
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public nint lpReserved2;
        public nint hStdInput;
        public nint hStdOutput;
        public nint hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessInformation
    {
        public nint hProcess;
        public nint hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public uint nLength;
        public nint lpSecurityDescriptor;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TokenMandatoryLabel
    {
        public SidAndAttributes Label;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SidAndAttributes
    {
        public nint Sid;
        public int Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessBasicInformation
    {
        private readonly nint Reserved1;
        public nint PebBaseAddress;
        private readonly nint Reserved2;
        private readonly nint Reserved3;
        private readonly nuint UniqueProcessId;
        private readonly nint Reserved4;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PEB
    {
        private readonly byte InheritedAddressSpace;
        private readonly byte ReadImageFileExecOptions;
        private readonly byte BeingDebugged;
        private readonly byte BitField;
        private readonly nint Mutant;
        public nint ImageBaseAddress;
    }
}
