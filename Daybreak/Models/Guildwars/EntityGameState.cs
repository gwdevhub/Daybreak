namespace Daybreak.Models.Guildwars;
public sealed class EntityGameState
{
    public int Id { get; set; }
    public Position Position { get; set; }
    public LivingEntityState State { get; set; }
}
