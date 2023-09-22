namespace Daybreak.Services.UMod.Models;
internal readonly struct TexmodMessage
{
    public readonly ControlMessage Message;
    public readonly uint Value;
    public readonly uint Hash;

    public TexmodMessage(ControlMessage message, uint value, uint hash)
    {
        this.Message = message;
        this.Value = value;
        this.Hash = hash;
    }
}
