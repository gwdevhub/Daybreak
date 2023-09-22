using Ionic.Zip;

namespace Daybreak.Services.UMod.Models;
internal sealed class Packet
{
    public TexmodMessage? TexmodMessage { get; init; }
    public ZipEntry? Data { get; init; }
}
