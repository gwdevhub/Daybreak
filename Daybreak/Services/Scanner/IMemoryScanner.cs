using Daybreak.Models.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Daybreak.Services.Scanner;

public interface IMemoryScanner
{
    public IntPtr ModuleStartAddress { get; }
    public byte[]? Memory { get; }
    public int Size { get; }
    public bool Scanning { get; }
    public Process? Process { get; }

    void BeginScanner(Process process);
    void EndScanner();
    T Read<T>(IntPtr address);
    T[] ReadArray<T>(IntPtr address, int size);
    T[] ReadArray<T>(GuildwarsArray guildwarsArray);
    byte[]? ReadBytes(IntPtr address, int size);
    string ReadWString(IntPtr address, int maxsize);
    T ReadPtrChain<T>(IntPtr Base, int finalPointerOffset = 0, params int[] offsets);
    IntPtr ScanForAssertion(string? assertionFile, string? assertionMessage);
    IntPtr ScanForPtr(byte[] pattern, string? mask = default, bool readptr = false);
}
