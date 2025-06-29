using PeNet;
using PeNet.Header.Pe;

namespace Daybreak.Services.Guildwars.Utils;
/// <summary>
/// https://github.com/gwdevhub/gwlauncher/blob/master/GW%20Launcher/Guildwars/FileIdFinder.cs
/// </summary>
internal sealed class GuildWarsExecutableParser
{
    private readonly static byte[] VersionPattern = [0x8B, 0xC8, 0x33, 0xDB, 0x39, 0x8D, 0xC0, 0xFD, 0xFF, 0xFF, 0x0F, 0x95, 0xC3];

    private readonly PeFile peFile;
    private readonly ImageSectionHeader textSection;

    private GuildWarsExecutableParser(string path)
    {
        this.peFile = new PeFile(path);
        this.textSection = this.peFile.ImageSectionHeaders?.FirstOrDefault(s => s.Name == ".text") ?? throw new InvalidOperationException("Unable to find .text section");
    }

    public Task<int> GetVersion(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var offset = this.Find(VersionPattern);
            var functionRva = this.FollowCall(offset - 5);
            var fileId = (int)this.Read(functionRva + 1);
            return fileId;
        }, cancellationToken);
    }

    private uint Find(byte[] pattern, int offset = 0)
    {
        var buffer = new byte[this.textSection.SizeOfRawData];
        Buffer.BlockCopy(this.peFile.RawFile.ToArray(), (int)this.textSection.PointerToRawData, buffer, 0, (int)this.textSection.SizeOfRawData);
        var pos = IndexOf(buffer, pattern);
        if (pos == -1)
        {
            throw new Exception("Couldn't find the pattern");
        }

        return this.textSection.VirtualAddress + (uint)pos + (uint)offset;
    }

    private uint FollowCall(uint callRva)
    {
        var posInFile = this.RvaToOffset(callRva);
        var op = this.peFile.RawFile.ToArray()[posInFile];
        var callParam = BitConverter.ToInt32(this.peFile.RawFile.ToArray(), posInFile + 1);

        if (op != 0xE8 && op != 0xE9)
        {
            throw new Exception($"Unsupported opcode '0x{op:X2} ({op})'");
        }

        return callRva + (uint)callParam + 5;
    }

    private uint Read(uint rva)
    {
        var posInFile = this.RvaToOffset(rva);
        return BitConverter.ToUInt32(this.peFile.RawFile.ToArray(), posInFile);
    }

    private int RvaToOffset(uint rva)
    {
        var section = this.peFile.ImageSectionHeaders!.FirstOrDefault(s => rva >= s.VirtualAddress && rva < s.VirtualAddress + s.VirtualSize);
        return section is null
            ? throw new Exception("Could not find section for RVA")
            : (int)(rva - section.VirtualAddress + section.PointerToRawData);
    }

    private static int IndexOf(byte[] haystack, byte[] needle)
    {
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            if (needle.SequenceEqual(haystack.Skip(i).Take(needle.Length)))
            {
                return i;
            }
        }

        return -1;
    }

    public static GuildWarsExecutableParser? TryParse(string filePath)
    {
        try
        {
            var parser = new GuildWarsExecutableParser(filePath);
            return parser;
        }
        catch
        {
            return default;
        }
    }
}
