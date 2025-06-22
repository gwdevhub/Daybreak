using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Shared.Models.Guildwars;
public sealed class Hero : IWikiEntity
{
    public static readonly Hero None = new() { Id = 0, Profession = Profession.None, Name = string.Empty, WikiUrl = string.Empty };
    public static readonly Hero Norgu = new() { Id = 1, Name = "Norgu", WikiUrl = "https://wiki.guildwars.com/wiki/Norgu", Profession = Profession.Mesmer };
    public static readonly Hero Goren = new() { Id = 2, Name = "Goren", WikiUrl = "https://wiki.guildwars.com/wiki/Goren", Profession = Profession.Warrior };
    public static readonly Hero Tahlkora = new() { Id = 3, Name = "Tahlkora", WikiUrl = "https://wiki.guildwars.com/wiki/Tahlkora", Profession = Profession.Monk };
    public static readonly Hero MasterOfWhispers = new() { Id = 4, Name = "Master of Whispers", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_Whispers", Profession = Profession.Necromancer };
    public static readonly Hero AcolyteJin = new() { Id = 5, Name = "Acolyte Jin", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Jin", Profession = Profession.Ranger };
    public static readonly Hero Koss = new() { Id = 6, Name = "Koss", WikiUrl = "https://wiki.guildwars.com/wiki/Koss", Profession = Profession.Warrior };
    public static readonly Hero Dunkoro = new() { Id = 7, Name = "Dunkoro", WikiUrl = "https://wiki.guildwars.com/wiki/Dunkoro", Profession = Profession.Monk };
    public static readonly Hero AcolyteSousuke = new() { Id = 8, Name = "Acolyte Sousuke", WikiUrl = "https://wiki.guildwars.com/wiki/Acolyte_Sousuke", Profession = Profession.Elementalist };
    public static readonly Hero Melonni = new() { Id = 9, Name = "Melonni", WikiUrl = "https://wiki.guildwars.com/wiki/Melonni", Profession = Profession.Dervish };
    public static readonly Hero ZhedShadowhoof = new() { Id = 10, Name = "Zhed Shadowhoof", WikiUrl = "https://wiki.guildwars.com/wiki/Zhed_Shadowhoof", Profession = Profession.Elementalist };
    public static readonly Hero GeneralMorgahn = new() { Id = 11, Name = "General Morgahn", WikiUrl = "https://wiki.guildwars.com/wiki/General_Morgahn", Profession = Profession.Paragon };
    public static readonly Hero MagridTheSly = new() { Id = 12, Name = "Magrid the Sly", WikiUrl = "https://wiki.guildwars.com/wiki/Magrid_the_Sly", Profession = Profession.Ranger };
    public static readonly Hero Zenmai = new() { Id = 13, Name = "Zenmai", WikiUrl = "https://wiki.guildwars.com/wiki/Zenmai", Profession = Profession.Assassin };
    public static readonly Hero Olias = new() { Id = 14, Name = "Olias", WikiUrl = "https://wiki.guildwars.com/wiki/Olias", Profession = Profession.Necromancer };
    public static readonly Hero Razah = new() { Id = 15, Name = "Razah", WikiUrl = "https://wiki.guildwars.com/wiki/Razah", Profession = Profession.None };
    public static readonly Hero MOX = new() { Id = 16, Name = "M.O.X.", WikiUrl = "https://wiki.guildwars.com/wiki/M.O.X.", Profession = Profession.Dervish };
    public static readonly Hero KeiranThackeray = new() { Id = 17, Name = "Keiran Thackeray", WikiUrl = "https://wiki.guildwars.com/wiki/Keiran_Thackeray", Profession = Profession.Paragon };
    public static readonly Hero Jora = new() { Id = 18, Name = "Jora", WikiUrl = "https://wiki.guildwars.com/wiki/Jora", Profession = Profession.Warrior };
    public static readonly Hero PyreFierceshot = new() { Id = 19, Name = "Pyre Fierceshot", WikiUrl = "https://wiki.guildwars.com/wiki/Pyre_Fierceshot", Profession = Profession.Ranger };
    public static readonly Hero Anton = new() { Id = 20, Name = "Anton", WikiUrl = "https://wiki.guildwars.com/wiki/Anton", Profession = Profession.Assassin };
    public static readonly Hero Livia = new() { Id = 21, Name = "Livia", WikiUrl = "https://wiki.guildwars.com/wiki/Livia", Profession = Profession.Necromancer };
    public static readonly Hero Hayda = new() { Id = 22, Name = "Hayda", WikiUrl = "https://wiki.guildwars.com/wiki/Hayda", Profession = Profession.Paragon };
    public static readonly Hero Kahmu = new() { Id = 23, Name = "Kahmu", WikiUrl = "https://wiki.guildwars.com/wiki/Kahmu", Profession = Profession.Dervish };
    public static readonly Hero Gwen = new() { Id = 24, Name = "Gwen", WikiUrl = "https://wiki.guildwars.com/wiki/Gwen", Profession = Profession.Mesmer };
    public static readonly Hero Xandra = new() { Id = 25, Name = "Xandra", WikiUrl = "https://wiki.guildwars.com/wiki/Xandra", Profession = Profession.Ritualist };
    public static readonly Hero Vekk = new() { Id = 26, Name = "Vekk", WikiUrl = "https://wiki.guildwars.com/wiki/Vekk", Profession = Profession.Elementalist };
    public static readonly Hero OgdenStonehealer = new() { Id = 27, Name = "Ogden Stonehealer", WikiUrl = "https://wiki.guildwars.com/wiki/Ogden_Stonehealer", Profession = Profession.Monk };
    public static readonly Hero Merc1 = new() { Id = 28, Name = "Mercenary Hero 1", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc2 = new() { Id = 29, Name = "Mercenary Hero 2", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc3 = new() { Id = 30, Name = "Mercenary Hero 3", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc4 = new() { Id = 31, Name = "Mercenary Hero 4", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc5 = new() { Id = 32, Name = "Mercenary Hero 5", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc6 = new() { Id = 33, Name = "Mercenary Hero 6", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc7 = new() { Id = 34, Name = "Mercenary Hero 7", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Merc8 = new() { Id = 35, Name = "Mercenary Hero 8", WikiUrl = string.Empty, Profession = Profession.None };
    public static readonly Hero Miku = new() { Id = 36, Name = "Miku", WikiUrl = "https://wiki.guildwars.com/wiki/Miku", Profession = Profession.Assassin };
    public static readonly Hero ZeiRi = new() { Id = 37, Name = "Zei Ri", WikiUrl = "https://wiki.guildwars.com/wiki/Zei_Ri", Profession = Profession.Ritualist };

    public static readonly List<Hero> Heroes = [
        None,
        Norgu,
        Goren,
        Tahlkora,
        MasterOfWhispers,
        AcolyteJin,
        Koss,
        Dunkoro,
        AcolyteSousuke,
        Melonni,
        ZhedShadowhoof,
        GeneralMorgahn,
        MagridTheSly,
        Zenmai,
        Olias,
        Razah,
        MOX,
        KeiranThackeray,
        Jora,
        PyreFierceshot,
        Anton,
        Livia,
        Hayda,
        Kahmu,
        Gwen,
        Xandra,
        Vekk,
        OgdenStonehealer,
        Merc1,
        Merc2,
        Merc3,
        Merc4,
        Merc5,
        Merc6,
        Merc7,
        Merc8,
        Miku,
        ZeiRi
        ];

    public static bool TryParse(int id, out Hero hero)
    {
        hero = Heroes.Where(n => n.Id == id).FirstOrDefault()!;
        if (hero is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Hero hero)
    {
        hero = Heroes.Where(n => n.Name == name).FirstOrDefault()!;
        if (hero is null)
        {
            return false;
        }

        return true;
    }
    public static Hero Parse(int id)
    {
        if (TryParse(id, out var hero) is false)
        {
            throw new InvalidOperationException($"Could not find a hero with id {id}");
        }

        return hero;
    }
    public static Hero Parse(string name)
    {
        if (TryParse(name, out var hero) is false)
        {
            throw new InvalidOperationException($"Could not find a hero with name {name}");
        }

        return hero;
    }

    private Hero()
    {
    }

    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string WikiUrl { get; init; }
    public required Profession Profession { get; init; }
}
