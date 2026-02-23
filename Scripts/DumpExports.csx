using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

var bytes = File.ReadAllBytes("Dependencies/GWCA/gwca.dll");
var peOff = BitConverter.ToInt32(bytes, 60);
var numSections = BitConverter.ToUInt16(bytes, peOff + 6);
var optHeaderSize = BitConverter.ToUInt16(bytes, peOff + 20);
var optOff = peOff + 24;
var exportRva = BitConverter.ToUInt32(bytes, optOff + 96);
var sectionsOff = optOff + optHeaderSize;

uint RvaToOffset(uint rva)
{
    for (int i = 0; i < numSections; i++)
    {
        int so = sectionsOff + i * 40;
        uint va = BitConverter.ToUInt32(bytes, so + 12);
        uint size = BitConverter.ToUInt32(bytes, so + 8);
        uint raw = BitConverter.ToUInt32(bytes, so + 20);
        if (rva >= va && rva < va + size) return raw + (rva - va);
    }
    return 0;
}

var expOff = (int)RvaToOffset(exportRva);
var numNames = BitConverter.ToInt32(bytes, expOff + 24);
var namesRva = BitConverter.ToUInt32(bytes, expOff + 32);
var namesOff = (int)RvaToOffset(namesRva);
var names = new List<string>();
for (int i = 0; i < numNames; i++)
{
    var nameRva = BitConverter.ToUInt32(bytes, namesOff + i * 4);
    var nameOff = (int)RvaToOffset(nameRva);
    int end = nameOff;
    while (bytes[end] != 0) end++;
    names.Add(Encoding.ASCII.GetString(bytes, nameOff, end - nameOff));
}

var mangled = names.Where(n => n.StartsWith("?")).OrderBy(x => x).ToList();
var cExports = names.Where(n => !n.StartsWith("?")).OrderBy(x => x).ToList();

Console.WriteLine($"=== Mangled C++ exports ({mangled.Count}) ===");
foreach (var n in mangled) Console.WriteLine(n);
Console.WriteLine();
Console.WriteLine($"=== C exports ({cExports.Count}) ===");
foreach (var n in cExports) Console.WriteLine(n);
