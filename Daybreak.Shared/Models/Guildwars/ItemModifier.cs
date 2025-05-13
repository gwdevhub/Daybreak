namespace Daybreak.Models.Guildwars;
public readonly struct ItemModifier
{
    public uint Modifier { get; init; }
    public uint Identifier => this.Modifier >> 16;
    public uint Argument1 => (this.Modifier & 0x0000FF00) >> 8;
    public uint Argument2 => this.Modifier & 0x000000FF;

    public static implicit operator uint(ItemModifier modifier) => modifier.Modifier;
    public static implicit operator ItemModifier(uint modifier) => new(){ Modifier = modifier };
}
