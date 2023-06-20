using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;
public sealed class Rune : ItemBase, IWikiEntity, IIconUrlEntity, IItemModHash
{
    public static readonly Rune RuneOfSuperiorVigor = new() { Id = 5551, Modifiers = new List<ItemModifier> { 632815616, 604504321, 623903235, 669647554, 3221225472 }, ModHash = "25B80000240801012530020327EA02C2", Name = "Rune of Superior Vigor", WikiUrl = "https://wiki.guildwars.com/wiki/Rune_of_Vigor", IconUrl = "https://wiki.guildwars.com/images/9/9e/Rune_All_Sup.png" };
    public static readonly Rune RuneOfMajorVigor = new() { Id = 5550, Name = "Rune of Major Vigor", Modifiers = new List<ItemModifier> { 632815616, 604504320, 623903233, 669582018, 3221225472 }, WikiUrl = "https://wiki.guildwars.com/wiki/Rune_of_Vigor", IconUrl = "https://wiki.guildwars.com/images/f/f6/Rune_All_Major.png" };
    public static readonly Rune RuneOfMinorVigor = new() { Id = 898, Name = "Rune of Minor Vigor", Modifiers = new List<ItemModifier> { 632815616, 604504258, 623903109, 669516482, 3221225472 }, WikiUrl = "https://wiki.guildwars.com/wiki/Rune_of_Vigor", IconUrl = "https://wiki.guildwars.com/images/f/fb/Rune_All_Minor.png" };
    public static readonly Rune RuneOfVitae = new() { Id = 898, Name = "Rune of Vitae", Modifiers = new List<ItemModifier> { 632815616, 604504594, 623903781, 591923712, 3221225472 }, WikiUrl = "https://wiki.guildwars.com/wiki/Rune_of_Vitae", IconUrl = "https://wiki.guildwars.com/images/f/fb/Rune_All_Minor.png" };

    public static IReadOnlyList<Rune> Runes { get; } = new List<Rune>
    {
        RuneOfSuperiorVigor,
        RuneOfMajorVigor,
        RuneOfMinorVigor,
        RuneOfVitae
    };

    public string? WikiUrl { get; init; }

    public string? IconUrl { get; init; }

    public string? ModHash { get; init; }

    private Rune()
    {
    }
}
