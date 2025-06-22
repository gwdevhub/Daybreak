using System.Collections.Generic;

namespace Daybreak.Shared.Models.FocusView;
public sealed class CharacterSelectComponentContext
{
    public required CharacterSelectComponentEntry CurrentCharacter { get; init; }
    public required List<CharacterSelectComponentEntry> Characters { get; init; }
}
