namespace Daybreak.Shared.Services.Formatting;

/// <summary>
/// Renders a numeric skill resource cost (energy, recharge, activation, etc.)
/// using the wiki's fraction glyphs (¼/½/¾) where applicable, mirroring how
/// the source wiki presents these numbers so the UI matches what players
/// already recognise.
/// </summary>
public interface ISkillNumberFormatter
{
    /// <summary>
    /// Renders an integer/decimal/quarter-fraction cost. Returns
    /// <see cref="string.Empty"/> for <c>null</c>.
    /// </summary>
    string Format(double? value);

    /// <summary>
    /// Renders a value stored as a unit fraction (e.g. <c>0.15</c>) back as
    /// a rounded percentage (<c>"15%"</c>). Used by the few skill fields
    /// that the wiki originally expressed as percentages — currently just
    /// the blood-magic Sacrifice cost.
    /// </summary>
    string FormatPercent(double? value);
}
