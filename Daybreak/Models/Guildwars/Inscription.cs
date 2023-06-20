using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public sealed class Inscription : ItemBase, IWikiEntity, IIconUrlEntity
{
    public static readonly Inscription WeaponInscription = new() { Id = 15542, Name = "Weapon Inscription", WikiUrl = "https://wiki.guildwars.com/wiki/Inscription", IconUrl = "https://wiki.guildwars.com/images/2/22/Inscription_weapons.png" };
    
    public static readonly Inscription FocusInscription = new() { Id = 19123, Name = "Focus Inscription", WikiUrl = "https://wiki.guildwars.com/wiki/Inscription", IconUrl = "https://wiki.guildwars.com/images/5/59/Inscription_focus_items.png" };

    public static readonly Inscription FocusOrShieldInscription = new() { Id = 15541, Name = "Focus Items or Shields Inscription", WikiUrl = "https://wiki.guildwars.com/wiki/Inscription", IconUrl = "https://wiki.guildwars.com/images/6/61/Inscription_focus_items_or_shields.png" };

    public static IReadOnlyList<Inscription> Inscriptions { get; } = new List<Inscription>
    {
        WeaponInscription,
        FocusInscription,
        FocusOrShieldInscription
    };

    public string? WikiUrl { get; init; }

    public string? IconUrl { get; init; }

    private Inscription()
    {
    }
}
