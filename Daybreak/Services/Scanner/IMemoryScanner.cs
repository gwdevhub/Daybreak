using Daybreak.Models.Interop;
using System.Diagnostics;

namespace Daybreak.Services.Scanner;

public interface IMemoryScanner
{
    public int ModuleStartAddress { get; }
    public byte[]? Memory { get; }
    public int Size { get; }
    public bool Scanning { get; }
    public Process? Process { get; }

    void BeginScanner(Process process);
    void EndScanner();
    T Read<T>(int address);
    T[] ReadArray<T>(int address, int size);
    T[] ReadArray<T>(GuildwarsArray guildwarsArray);
    byte[]? ReadBytes(int address, int size);
    string ReadWString(int address, int maxsize);
    T ReadPtrChain<T>(int Base, int finalPointerOffset = 0, params int[] offsets);
    int ScanForAssertion(string? assertionFile, string? assertionMessage);
    int ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false);
}
