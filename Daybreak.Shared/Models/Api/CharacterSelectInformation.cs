namespace Daybreak.Shared.Models.Api;

public sealed record CharacterSelectInformation(CharacterSelectEntry? CurrentCharacter, List<CharacterSelectEntry> CharacterNames)
{
}
