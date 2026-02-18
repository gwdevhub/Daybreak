using System;
using System.Collections.Immutable;
using System.Text;

namespace Daybreak.Generators;

/// <summary>
/// Minimal PE export‐name reader.  Zero external dependencies — reads the
/// PE export directory directly from the on-disk binary so we can use it
/// inside a Roslyn source generator (netstandard2.0).
/// </summary>
internal static class PeExportReader
{
    public static ImmutableArray<string> ReadExportNames(byte[] data)
    {
        // Minimum size: DOS header (64 bytes)
        if (data.Length < 64)
            return [];

        // ── DOS Header ─────────────────────────────────────────────
        // e_magic == "MZ"
        if (data[0] != 0x4D || data[1] != 0x5A)
            return [];

        // e_lfanew at offset 0x3C → offset of PE signature
        int peOffset = BitConverter.ToInt32(data, 0x3C);
        if (peOffset < 0 || peOffset + 4 > data.Length)
            return [];

        // ── PE Signature ───────────────────────────────────────────
        if (data[peOffset] != 'P' || data[peOffset + 1] != 'E' ||
            data[peOffset + 2] != 0 || data[peOffset + 3] != 0)
            return [];

        // ── COFF Header (20 bytes) ─────────────────────────────────
        int coffOffset = peOffset + 4;
        if (coffOffset + 20 > data.Length)
            return [];

        ushort numberOfSections = BitConverter.ToUInt16(data, coffOffset + 2);
        ushort sizeOfOptionalHeader = BitConverter.ToUInt16(data, coffOffset + 16);
        int optionalOffset = coffOffset + 20;

        if (optionalOffset + sizeOfOptionalHeader > data.Length)
            return [];

        // ── Optional Header ────────────────────────────────────────
        ushort magic = BitConverter.ToUInt16(data, optionalOffset);
        bool isPE32Plus = magic == 0x20B;
        // PE32 = 0x10B, PE32+ = 0x20B

        // NumberOfRvaAndSizes
        int numRvaOffset = optionalOffset + (isPE32Plus ? 108 : 92);
        if (numRvaOffset + 4 > data.Length)
            return [];

        uint numberOfRvaAndSizes = BitConverter.ToUInt32(data, numRvaOffset);
        if (numberOfRvaAndSizes == 0)
            return [];

        // Data directory [0] = Export Table (RVA, Size)
        int dataDirOffset = optionalOffset + (isPE32Plus ? 112 : 96);
        if (dataDirOffset + 8 > data.Length)
            return [];

        uint exportRva = BitConverter.ToUInt32(data, dataDirOffset);
        uint exportSize = BitConverter.ToUInt32(data, dataDirOffset + 4);
        if (exportRva == 0 || exportSize == 0)
            return [];

        // ── Section Headers ────────────────────────────────────────
        int sectionsOffset = optionalOffset + sizeOfOptionalHeader;
        var sections = new SectionInfo[numberOfSections];
        for (int i = 0; i < numberOfSections; i++)
        {
            int off = sectionsOffset + (i * 40);
            if (off + 40 > data.Length) break;
            sections[i] = new SectionInfo(
                BitConverter.ToUInt32(data, off + 12), // VirtualAddress
                BitConverter.ToUInt32(data, off + 8),  // VirtualSize
                BitConverter.ToUInt32(data, off + 20), // PointerToRawData
                BitConverter.ToUInt32(data, off + 16)  // SizeOfRawData
            );
        }

        // ── Export Directory Table ─────────────────────────────────
        int exportDirFileOff = RvaToFileOffset(exportRva, sections);
        if (exportDirFileOff < 0 || exportDirFileOff + 40 > data.Length)
            return [];

        uint numberOfNames = BitConverter.ToUInt32(data, exportDirFileOff + 24);
        uint addressOfNamesRva = BitConverter.ToUInt32(data, exportDirFileOff + 32);

        int namesTableOff = RvaToFileOffset(addressOfNamesRva, sections);
        if (namesTableOff < 0)
            return [];

        // ── Read Name Strings ──────────────────────────────────────
        var builder = ImmutableArray.CreateBuilder<string>((int)numberOfNames);
        for (int i = 0; i < (int)numberOfNames; i++)
        {
            int entryOff = namesTableOff + (i * 4);
            if (entryOff + 4 > data.Length) break;

            uint nameRva = BitConverter.ToUInt32(data, entryOff);
            int nameOff = RvaToFileOffset(nameRva, sections);
            if (nameOff < 0) continue;

            // Read null-terminated ASCII string
            int end = nameOff;
            while (end < data.Length && data[end] != 0) end++;
            builder.Add(Encoding.ASCII.GetString(data, nameOff, end - nameOff));
        }

        return builder.ToImmutable();
    }

    // ────────────────────────────────────────────────────────────────

    private static int RvaToFileOffset(uint rva, SectionInfo[] sections)
    {
        for (int i = 0; i < sections.Length; i++)
        {
            uint va = sections[i].VirtualAddress;
            uint end = va + Math.Max(sections[i].VirtualSize, sections[i].SizeOfRawData);
            if (rva >= va && rva < end)
                return (int)(rva - va + sections[i].PointerToRawData);
        }

        return -1;
    }

    private readonly struct SectionInfo(uint va, uint vs, uint ptr, uint srd)
    {
        public readonly uint VirtualAddress = va;
        public readonly uint VirtualSize = vs;
        public readonly uint PointerToRawData = ptr;
        public readonly uint SizeOfRawData = srd;
    }
}
