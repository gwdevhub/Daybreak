using System.Runtime.InteropServices;

namespace Daybreak.API;

internal unsafe static partial class NativeMethods
{
    public const int STD_OUTPUT_HANDLE = -11;
    public const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    public const uint ENABLE_PROCESSED_OUTPUT = 0x0001;

    public const int WM_KEYDOWN = 0x0100;
    public const int WM_KEYUP = 0x0101;
    public const int WM_CHAR = 0x0102;
    public const int VK_RIGHT = 0x27;

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AllocConsole();

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint GetConsoleWindow();

    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nint GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

    [LibraryImport("user32.dll")]
    public static partial nint SendMessageW(nint hWnd, int Msg, nint wParam, nint lParam);
}
