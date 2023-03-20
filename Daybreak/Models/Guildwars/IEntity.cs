namespace Daybreak.Models.Guildwars;

public interface IEntity
{
    int Id { get; }
    uint Timer { get; }
    Position? Position { get; }
}
