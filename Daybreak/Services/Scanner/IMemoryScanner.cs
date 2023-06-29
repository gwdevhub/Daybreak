using Daybreak.Models.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Daybreak.Services.Scanner;

public interface IMemoryScanner
{
    public uint ModuleStartAddress { get; }
    public byte[]? Memory { get; }
    public uint Size { get; }
    public bool Scanning { get; }
    public Process? Process { get; }

    void BeginScanner(Process process);
    void EndScanner();
    T Read<T>(GuildwarsPointer<T> pointer, uint offset = 0);
    T Read<T>(uint address);
    T[] ReadArray<T>(uint address, uint size);
    T[] ReadArray<T>(GuildwarsArray<T> guildwarsArray);
    T[] ReadArray<T>(GuildwarsPointerArray<T> guildwarsPointerArray);
    T ReadItemAtIndex<T>(uint address, int index);
    T ReadItemAtIndex<T>(GuildwarsArray<T> guildwarsArray, int index);
    byte[]? ReadBytes(uint address, uint size);
    string ReadWString(uint address, uint maxsize);
    T ReadPtrChain<T>(uint Base, uint finalPointerOffset = 0, params uint[] offsets);
    uint ScanForAssertion(string? assertionFile, string? assertionMessage);
    uint ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false);
}
