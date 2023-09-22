using Ionic.Zip;

namespace Daybreak.Services.UMod.Models;
public sealed class TpfEntry
{
    public string? Name { get; init; }
    public ZipEntry? Entry { get; init; }
    public uint CrcHash { get; init; }
}
