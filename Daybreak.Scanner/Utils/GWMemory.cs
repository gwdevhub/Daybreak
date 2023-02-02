namespace Daybreak.Scanner.Utils;

internal class GWMemory
{
    public static string? WindowTitle { get; private set; }
    public static string? Email { get; private set; }
    public static string? CharacterName { get; private set; }

    internal static void FindAddressesIfNeeded(GWCAMemory cli)
    {
        IntPtr tmp;
        var imagebase = cli.GetImageBase();
        cli.InitScanner(imagebase.Item1, imagebase.Item2);
        tmp = cli.ScanForPtr(new byte[] {0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x10, 0x56, 0x6A}, 0x22, true);
        if (tmp != IntPtr.Zero)
        {
            WindowTitle = cli.ReadWString(tmp, 64);
        }

        tmp = cli.ScanForPtr(new byte[] {0x83, 0xC4, 0x40, 0x5F, 0x5E, 0x5B, 0x8B, 0x4D});
        if (tmp != IntPtr.Zero)
        {
            var emailAddPtr = cli.Read<IntPtr>(new IntPtr(tmp.ToInt64() - 0x48));
            Email = cli.ReadWString(emailAddPtr, 64);
            var charNamePtr = cli.Read<IntPtr>(new IntPtr(tmp.ToInt64() - 0x2E));
            CharacterName = cli.ReadWString(charNamePtr, 64);
        }

        tmp = cli.ScanForPtr(new byte[] { 0x50, 0x6A, 0x0F, 0x6A, 0x00, 0xFF, 0x35 }, +7, true);
        if (tmp != IntPtr.Zero)
        {
            var baseAddress = tmp;
            
        }

        cli.TerminateScanner();
    }
}
