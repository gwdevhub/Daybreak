using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Daybreak.Shared.Utils;

public static class NativeMethods
{
    public const int STD_OUTPUT_HANDLE = -11;
    public const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    public const uint ENABLE_PROCESSED_OUTPUT = 0x0001;

    public const uint WM_KEYDOWN = 0x0100;
    public const uint SWP_SHOWWINDOW = 0x0040;
    public const nint HWND_TOPMOST = -1;
    public const nint HWND_TOP = 0;
    public const int WH_KEYBOARD_LL = 13;
    public const uint LIST_MODULES_32BIT = 0x01;

    public delegate nint HookProc(int nCode, nint wParam, nint lParam);

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
    public enum MinidumpType : uint
    {
        MiniDumpNormal = 0x00000000,
        MiniDumpWithDataSegs = 0x00000001,
        MiniDumpWithFullMemory = 0x00000002,
        MiniDumpWithHandleData = 0x00000004,
        MiniDumpFilterMemory = 0x00000008,
        MiniDumpScanMemory = 0x00000010,
        MiniDumpWithUnloadedModules = 0x00000020,
        MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
        MiniDumpFilterModulePaths = 0x00000080,
        MiniDumpWithProcessThreadData = 0x00000100,
        MiniDumpWithPrivateReadWriteMemory = 0x00000200,
        MiniDumpWithoutOptionalData = 0x00000400,
        MiniDumpWithFullMemoryInfo = 0x00000800,
        MiniDumpWithThreadInfo = 0x00001000,
        MiniDumpWithCodeSegs = 0x00002000,
        MiniDumpWithoutAuxiliaryState = 0x00004000,
        MiniDumpWithFullAuxiliaryState = 0x00008000,
        MiniDumpWithPrivateWriteCopyMemory = 0x00010000,
        MiniDumpIgnoreInaccessibleMemory = 0x00020000,
        MiniDumpWithTokenInformation = 0x00040000,
        MiniDumpWithModuleHeaders = 0x00080000,
        MiniDumpFilterTriage = 0x00100000,
        MiniDumpValidTypeFlags = 0x001fffff,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessEntry32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public nint th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    };
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
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public uint nLength;
        public nint lpSecurityDescriptor;
        public bool bInheritHandle;
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
    public enum MemoryAllocationType : uint
    {
        MEM_COMMIT = 0x00001000,
        MEM_RESERVE = 0x00002000,
        MEM_RESET = 0x00080000,
        MEM_RESET_UNDO = 0x1000000,
        MEM_LARGE_PAGES = 0x20000000,
        MEM_PHYSICAL = 0x00400000,
        MEM_TOP_DOWN = 0x00100000,
        MEM_WRITE_WATCH = 0x00200000
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct RECT(int left, int top, int right, int bottom)
    {
        public readonly int Left = left, Top = top, Right = right, Bottom = bottom;

        public int Height => this.Bottom - this.Top;

        public int Width => this.Right - this.Left;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SystemHandleInformation
    {
        public uint OwnerPID;
        public byte ObjectType;
        public byte HandleFlags;
        public ushort HandleValue;
        public nuint ObjectPointer;
        public nint AccessMask;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectBasicInformation
    {
        public uint Attributes;
        public uint GrantedAccess;
        public uint HandleCount;
        public uint PointerCount;
        public uint PagedPoolUsage;
        public uint NonPagedPoolUsage;
        public uint Reserved1;
        public uint Reserved2;
        public uint Reserved3;
        public uint NameInformationLength;
        public uint TypeInformationLength;
        public uint SecurityDescriptorLength;
        public FILETIME CreateTime;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct IoStatusBlock
    {
        public uint Status;
        public ulong Information;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowInfo
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WindowInfo(bool? _) : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            this.cbSize = (uint)Marshal.SizeOf<WindowInfo>();
        }
    }

    [Flags]
    public enum DuplicateOptions : uint
    {
        DUPLICATE_CLOSE_SOURCE = 0x00000001,
        DUPLICATE_SAME_ACCESS = 0x00000002
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
    public enum NtStatus : uint
    {
        STATUS_SUCCESS = 0x00000000,
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004
    }
    [Flags]
    public enum ObjectInformationClass : uint
    {
        ObjectBasicInformation = 0,
        ObjectNameInformation = 1,
        ObjectTypeInformation = 2,
        ObjectAllTypesInformation = 3,
        ObjectHandleInformation = 4
    }
    [Flags]
    public enum SystemInformationClass : uint
    {
        SystemHandleInformation = 16
    }
    [Flags]
    public enum FileInformationClass
    {
        FileNameInformation = 9
    }
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
    public enum ProcessInfoClass : uint
    {
        ProcessBasicInformation = 0x00,
        ProcessDebugPort = 0x07,
        ProcessExceptionPort = 0x08,
        ProcessAccessToken = 0x09,
        ProcessWow64Information = 0x1A,
        ProcessImageFileName = 0x1B,
        ProcessDebugObjectHandle = 0x1E,
        ProcessDebugFlags = 0x1F,
        ProcessExecuteFlags = 0x22,
        ProcessInstrumentationCallback = 0x28,
        MaxProcessInfoClass = 0x64
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
    public enum SaferLevel : uint
    {
        Disallowed = 0,
        Untrusted = 0x1000,
        Constrained = 0x10000,
        NormalUser = 0x20000,
        FullyTrusted = 0x40000
    }
    public enum SaferLevelScope : uint
    {
        Machine = 1,
        User = 2
    }
    public enum SaferOpen : uint
    {
        Open = 1
    }
    public enum SaferTokenBehaviour : uint
    {
        Default = 0x0,
        NullIfEqual = 0x1,
        CompareOnly = 0x2,
        MakeInert = 0x4,
        WantFlags = 0x8
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
    public struct Sid_Identifier_Authority
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Value;
    }
    [Flags]
    public enum CreationFlags : uint
    {
        CreateSuspended = 0x00000004,
        DetachedProcess = 0x00000008,
        CreateNoWindow = 0x08000000,
        ExtendedStartupInfoPresent = 0x00080000
    }

    public delegate bool Win32Callback(nint hwnd, nint lParam);

    public const int WM_SYSCOMMAND = 0x112;

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint GetConsoleWindow();
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint GetStdHandle(int nStdHandle);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

    [DllImport("Dbghelp.dll", SetLastError = true)]
    public static extern bool MiniDumpWriteDump(nint hProcess, int processId, SafeHandle hFile, MinidumpType dumpType, nint expParam, nint userStreamParam, nint callbackParam);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);
    [DllImport("kernel32.dll")]
    public static extern bool Process32First(nint hSnapshot, ref ProcessEntry32 lppe);
    [DllImport("kernel32.dll")]
    public static extern bool Process32Next(nint hSnapshot, ref ProcessEntry32 lppe);
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);
    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(nint hObject);
    [DllImport("kernel32.dll")]
    public static extern bool DuplicateHandle(nint hSourceProcessHandle, nint hSourceHandle, nint hTargetProcessHandle, out nint lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, DuplicateOptions dwOptions);
    [DllImport("kernel32.dll")]
    public static extern nint OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, uint dwProcessID);
    [DllImport("ntdll.dll", SetLastError = true)]
    public static extern NtStatus NtQueryInformationFile(nint FileHandle, ref IoStatusBlock IoStatusBlock, nint FileInformation, int FileInformationLength, FileInformationClass FileInformationClass);
    [DllImport("ntdll.dll")]
    public static extern NtStatus NtQueryObject(nint ObjectHandle, ObjectInformationClass ObjectInformationClass, nint ObjectInformation, int ObjectInformationLength, out int ReturnLength);
    [DllImport("ntdll.dll")]
    public static extern NtStatus NtQuerySystemInformation(SystemInformationClass SystemInformationClass, nint SystemInformation, int SystemInformationLength, out int ReturnLength);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool QueryFullProcessImageName(nint hProcess, uint dwFlags, StringBuilder lpExeName, ref uint lpdwSize);
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(nint hwnd, nint insertAfter, int x, int y, int cx, int cy, uint flags);
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(nint hwnd, int cmd);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowTextLength(nint hWnd);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(nint hWnd, StringBuilder lpString, int nMaxCount);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern nint LoadLibrary(string lpFileName);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool FreeLibrary(nint hModule);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint SetWindowsHookEx(int idHook, HookProc lpfn, nint hMod, int dwThreadId);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(nint hHook);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint CallNextHookEx(nint hHook, int code, nint wParam, nint lParam);
    [DllImport("user32.dll")]
    public static extern nint GetForegroundWindow();
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(nint hProcess, uint lpBaseAddress, nint lpBuffer, uint nSize, out uint lpNumberOfBytesRead);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern bool ReadProcessMemory(nint hProcess, nint lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out nint lpNumberOfBytesRead);
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowInfo(nint hwnd, ref WindowInfo pwi);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern bool CreateProcess(
        string lpApplicationName, string lpCommandLine, ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags,
        nint lpEnvironment, string lpCurrentDirectory, [In] ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern uint ResumeThread(nint hThread);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nint LocalFree(nint hMem);
    [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    public static extern bool WriteProcessMemory(nint hProcess, nint lpBaseAddress, byte[] lpBuffer, int dwSize, out nint lpNumberOfBytesWritten);
    [DllImport("ntdll.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern int NtQueryInformationProcess(nint hProcess, ProcessInfoClass pic, out ProcessBasicInformation pbi, int cb, out int pSize);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferCreateLevel(SaferLevelScope scopeId, SaferLevel levelId, SaferOpen openFlags, out nint levelHandle, nint reserved);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferComputeTokenFromLevel(nint levelHandle, nint inAccessToken, out nint outAccessToken, SaferTokenBehaviour flags, nint lpReserved);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SaferCloseLevel(nint levelHandle);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool SetTokenInformation(nint tokenHandle, TokenInformationClass tokenInformationokenInformationClass, ref TokenMandatoryLabel tokenInformation, uint tokenInformationLength);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool ConvertStringSidToSid(string stringSid, out nint ptrSid);
    [DllImport("advapi32.dll")]
    public static extern uint GetLengthSid(nint pSid);
    [DllImport("advapi32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    public static extern bool CreateProcessAsUser(
        nint hToken,
        string lpApplicationName,
        string lpCommandLine,
        ref SecurityAttributes lpProcessAttributes,
        ref SecurityAttributes lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        nint lpEnvironment,
        string lpCurrentDirectory,
        ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern nint GetModuleHandle(string lpModuleName);
    [DllImport("kernel32.dll")]
    public static extern nint CreateRemoteThread(
        nint hProcess,
        nint lpThreadAttributes,
        uint dwStackSize,
        nint lpStartAddress,
        nint lpParameter,
        uint dwCreationFlags,
        out nint lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern nint VirtualAllocEx(
        nint hProcess,
        nint lpAddress,
        nint dwSize,
        uint dwAllocationType,
        uint dwProtect);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern bool VirtualFreeEx(
        nint hProcess,
        nint lpAddress,
        uint dwSize,
        uint dwFreeType);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern nint GetProcAddress(nint hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint WaitForSingleObject(nint hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetExitCodeThread(nint hHandle, out nint dwMilliseconds);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        nint hProcess,
        nint lpBaseAddress,
        nint lpBuffer,
        int nSize,
        out nint lpNumberOfBytesWritten);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        nint hProcess,
        nint lpBaseAddress,
        nint lpBuffer,
        int nSize,
        out nint lpNumberOfBytesRead);
    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);

    [DllImport("user32.Dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(nint parentHandle, Win32Callback callback, nint lParam);
}
