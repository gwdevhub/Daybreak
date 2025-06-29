namespace Daybreak.Shared.Models.FocusView;
public sealed class CharacterSelectComponentEntry : IEquatable<CharacterSelectComponentEntry>
{
    public required string DisplayName { get; init; }
    public required string CharacterName { get; init; }

    public bool Equals(CharacterSelectComponentEntry? other)
    {
        return other is not null &&
               this.DisplayName == other.DisplayName &&
               this.CharacterName == other.CharacterName;
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as CharacterSelectComponentEntry);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.DisplayName, this.CharacterName);
    }
}
