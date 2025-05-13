namespace Daybreak.Models.Interop;

public readonly struct ItemModifier
{
    public readonly uint Modifier;
    public readonly uint Identifier => this.Modifier >> 16;
    public readonly uint Argument1 => (this.Modifier & 0x0000FF00) >> 8;
    public readonly uint Argument2 => this.Modifier & 0x000000FF;
}
