using System.Collections.Generic;

namespace Daybreak.Shared.Models.FocusView;
public sealed class CharacterComponentContext
{
    public required uint CurrentExperience { get; init; }
    public required CharacterSelectComponentEntry CurrentCharacter { get; init; }
    public required List<CharacterSelectComponentEntry> Characters { get; init; }
}
