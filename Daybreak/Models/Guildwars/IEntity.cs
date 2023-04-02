namespace Daybreak.Models.Guildwars;

public interface IEntity : IPositionalEntity
{
    int Id { get; }
    uint Timer { get; }
}
