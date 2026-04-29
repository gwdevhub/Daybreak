using PeNet;
using PeNet.Header.Pe;

namespace Daybreak.Services.Guildwars.Utils;

internal sealed class GuildWarsExecutableParser
{
    // Used by older Guild Wars versions
    private readonly static byte[] VersionPattern = [0x8B, 0xC8, 0x33, 0xDB, 0x39, 0x8D, 0xC0, 0xFD, 0xFF, 0xFF, 0x0F, 0x95, 0xC3];

    // Used by newer Guild Wars versions. Matches ProcessVersionUpdate prologue:
    // push ebp; mov ebp,esp; sub esp,N; call <addr>; cmp dword ptr [global], 0
    // Byte 5 (stack size) is wildcarded since it changes across builds.
    private readonly static byte?[] FileIdFunctionPattern = [0x55, 0x8B, 0xEC, 0x83, 0xEC, null, 0xE8, null, null, null, null, 0x83, 0x3D, null, null, null, null, 0x00];

    private readonly PeFile peFile;
    private readonly ImageSectionHeader textSection;

    private GuildWarsExecutableParser(string path)
    {
        this.peFile = new PeFile(path);
        this.textSection = this.peFile.ImageSectionHeaders?.FirstOrDefault(s => s.Name == ".text") ?? throw new InvalidOperationException("Unable to find .text section");
    }

    public Task<int> GetVersionLegacy(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var offset = this.Find(VersionPattern);
            var functionRva = this.FollowCall(offset - 5);
            var fileId = (int)this.Read(functionRva + 1);
            return fileId;
        }, cancellationToken);
    }

    public Task<int> GetFileId(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            var patternRva = this.FindWithWildcards(FileIdFunctionPattern);
            var fileIdFunctionRva = this.FindGetFileIdCall(patternRva);
            var fileId = (int)this.Read(fileIdFunctionRva + 1);
            return fileId;
        }, cancellationToken);
    }

    // GetFileId is a tiny function: mov eax, <imm32>; ret (bytes: B8 xx xx xx xx C3).
    // Scan E8 (call) instructions within the matched function and follow each one
    // to find the call whose target matches that shape.
    private uint FindGetFileIdCall(uint functionRva, int scanLength = 0x80)
    {
        for (int offset = 0; offset < scanLength; offset++)
        {
            var rva = functionRva + (uint)offset;
            var posInFile = this.RvaToOffset(rva);
            if (this.peFile.RawFile.ReadByte(posInFile) != 0xE8)
            {
                continue;
            }

            var targetRva = rva + (uint)BitConverter.ToInt32(this.peFile.RawFile.ToArray(), posInFile + 1) + 5;
            var targetOffset = this.RvaToOffset(targetRva);
            // Check for: B8 xx xx xx xx C3 (mov eax, imm32; ret)
            if (this.peFile.RawFile.ReadByte(targetOffset) == 0xB8 &&
                this.peFile.RawFile.ReadByte(targetOffset + 5) == 0xC3)
            {
                return targetRva;
            }
        }

        throw new Exception("Couldn't find GetFileId call within the function");
    }

    private uint FindWithWildcards(byte?[] pattern, int offset = 0)
    {
        var buffer = this.peFile.RawFile.AsSpan((int)this.textSection.PointerToRawData, (int)this.textSection.SizeOfRawData);
        for (int i = 0; i <= buffer.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (pattern[j].HasValue &&
                    buffer[i + j] != pattern[j])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return this.textSection.VirtualAddress + (uint)i + (uint)offset;
            }
        }

        throw new Exception("Couldn't find the pattern");
    }

    private uint Find(byte[] pattern, int offset = 0)
    {
        var buffer = this.peFile.RawFile.AsSpan(this.textSection.PointerToRawData, this.textSection.SizeOfRawData);
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
        var op = this.peFile.RawFile.ReadByte(posInFile);
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

    private static int IndexOf(Span<byte> haystack, Span<byte> needle)
    {
        for (int i = 0; i <= haystack.Length - needle.Length; i++)
        {
            if (needle.SequenceEqual(haystack.Slice(i, needle.Length)))
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
