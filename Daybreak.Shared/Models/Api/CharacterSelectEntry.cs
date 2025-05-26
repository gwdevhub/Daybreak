namespace Daybreak.Shared.Models.Api;
public sealed record CharacterSelectEntry(string Uuid, string Name, uint Primary, uint Secondary, uint Campaign, uint Map, uint Level, bool Pvp)
{
}
