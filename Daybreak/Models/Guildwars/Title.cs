using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Guildwars;

public sealed class Title
{
    public static Title None { get; } = new Title { Id = 0xFF, };
    public static Title Hero { get; } = new Title { Id = 0, Name = "Hero", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_(title)", Tiers = new List<string> { "Hero", "Fierce Hero", "Mighty Hero", "Deadly Hero", "Terrifying Hero", "Conquering Hero", "Subjugating Hero", "Vanquishing Hero", "Renowed Hero", "Illustrious Hero", "Eminent Hero", "King's Hero", "Emperor's Hero", "Balthazar's Hero", "Legendary Hero" } };
    public static Title TyrianCartographer { get; } = new Title { Id = 1, Name = "Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = new List<string> { "Tyrian Explorer", "Tyrian Pathfinder", "Tyrian Trailblazer", "Tyrian Cartographer", "Tyrian Master Cartographer", "Tyrian Grandmaster Cartographer" } };
    public static Title CanthanCartographer { get; } = new Title { Id = 2, Name = "Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = new List<string> { "Canthan Explorer", "Canthan Pathfinder", "Canthan Trailblazer", "Canthan Cartographer", "Canthan Master Cartographer", "Canthan Grandmaster Cartographer" } };
    public static Title Gladiator { get; } = new Title { Id = 3, Name = "Gladiator", WikiUrl = "https://wiki.guildwars.com/wiki/Gladiator", Tiers = new List<string> { "Gladiator", "Fierce Gladiator", "Mighty Gladiator", "Deadly Gladiator", "Terrifying Gladiator", "Conquering Gladiator", "Subjugating Gladiator", "Vanquishing Gladiator", "King's Gladiator", "Emperor's Gladiator", "Balthazar's Gladiator", "Legendary Gladiator" } };
    public static Title Champion { get; } = new Title { Id = 4, Name = "Champion", WikiUrl = "https://wiki.guildwars.com/wiki/Champion", Tiers = new List<string> { "Champion", "Fierce Champion", "Mighty Champion", "Deadly Champion", "Terrifying Champion", "Conquering Champion", "Subjugating Champion", "Vanquishing Champion", "King's Champion", "Emperor's Champion", "Balthazar's Champion", "Legendary Champion" } };
    public static Title Kurzick { get; } = new Title { Id = 5, Name = "Faction Allegiance", WikiUrl = "https://wiki.guildwars.com/wiki/Allegiance_rank", Tiers = new List<string> { "Kurzick Supporter", "Friend of the Kurzicks", "Companion of the Kurzicks", "Ally of the Kurzicks", "Sentinel of the Kurzicks", "Steward of the Kurzicks", "Defender of the Kurzicks", "Warden of the Kurzicks", "Bastion of the Kurzicks", "Champion of the Kurzicks", "Hero of the Kurzicks", "Savior of the Kurzicks" } };
    public static Title Luxon { get; } = new Title { Id = 6, Name = "Faction Allegiance", WikiUrl = "https://wiki.guildwars.com/wiki/Allegiance_rank", Tiers = new List<string> { "Luxon Supporter", "Friend of the Kurzicks", "Companion of the Luxons", "Ally of the Luxons", "Sentinel of the Luxons", "Steward of the Luxons", "Defender of the Luxons", "Warden of the Luxons", "Bastion of the Luxons", "Champion of the Luxons", "Hero of the Luxons", "Savior of the Luxons" } };
    public static Title Drunkard { get; } = new Title { Id = 7, Name = "Drunkard", WikiUrl = "https://wiki.guildwars.com/wiki/Drunkard", Tiers = new List<string> { "Drunkard", "Incorrigible Ale-Hound" } };
    public static Title Survivor { get; } = new Title { Id = 9, Name = "Survivor", WikiUrl = "https://wiki.guildwars.com/wiki/Survivor", Tiers = new List<string> { "Survivor", "Indomitable Survivor", "Legendary Survivor" } };
    public static Title KindOfABigDeal { get; } = new Title { Id = 10, Name = "Kind of a Big Deal", WikiUrl = "https://wiki.guildwars.com/wiki/Kind_of_a_Big_Deal", Tiers = new List<string> { "Kind Of A Big Deal", "People Know Me", "I'm Very Important", "I Have Many Leather-Bound Books", "My Guild Hall Smells of Rich Mahogany", "God Walking Amongst Mere Mortals" } };
    public static Title ProtectorTyria { get; } = new Title { Id = 13, Name = "Protector of Tyria", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = new List<string> { "Protector of Tyria" } };
    public static Title ProtectorCantha { get; } = new Title { Id = 14, Name = "Protector of Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = new List<string> { "Protector of Cantha" } };
    public static Title Lucky { get; } = new Title { Id = 15, Name = "Lucky", WikiUrl = "https://wiki.guildwars.com/wiki/Lucky_and_Unlucky", Tiers = new List<string> { "Charmed", "Lucky", "Favored", "Prosperous", "Golden", "Blessed by Fate" } };
    public static Title Unlucky { get; } = new Title { Id = 16, Name = "Unlucky", WikiUrl = "https://wiki.guildwars.com/wiki/Lucky_and_Unlucky", Tiers = new List<string> { "Hapless", "Unlucky", "Unfavored", "Tragic", "Wretched", "Jinxed", "Cursed by Fate" } };
    public static Title Sunspear { get; } = new Title { Id = 17, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_rank", Tiers = new List<string> { "Sunspear Sergeant", "Sunspear Master Sergeant", "Second Spear", "First Spear", "Sunspear Captain", "Sunspear Commander", "Sunspear General", "Sunspear Castellan", "Spearmarshal", "Legendary Spearmarshal" } };
    public static Title ElonianCartographer { get; } = new Title { Id = 18, Name = "Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = new List<string> { "Elonian Explorer", "Elonian Pathfinder", "Elonian Trailblazer", "Elonian Cartographer", "Elonian Master Cartographer", "Elonian Grandmaster Cartographer" } };
    public static Title ProtectorElona { get; } = new Title { Id = 19, Name = "Protector of Elona", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = new List<string> { "Protector of Elona" } };
    public static Title Lightbringer { get; } = new Title { Id = 20, Name = "Lightbringer", WikiUrl = "https://wiki.guildwars.com/wiki/Lightbringer_rank", Tiers = new List<string> { "Lightbringer", "Adept Lightbringer", "Brave Lightbringer", "Mighty Lightbringer", "Conquering Lightbringer", "Vanquishing Lightbringer", "Revered Lightbringer", "Holy Lightbringer" } };
    public static Title LegendaryDefenderOfAscalon { get; } = new Title { Id = 21, Name = "Defender of Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Defender_of_Ascalon", Tiers = new List<string> { "Legendary Defender of Ascalon" } };
    public static Title Commander { get; } = new Title { Id = 22, Name = "Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Commander", Tiers = new List<string> { "Commander", "Victorious Commander", "Triumphant Commander", "Keen Commander", "Battle Commander", "Field Commander", "Lieutenant Commander", "Wing Commander", "Cobra Commander", "Supreme Commander", "Master And Commander", "Legendary Commander" } };
    public static Title Gamer { get; } = new Title { Id = 23, Name = "Gamer", WikiUrl = "https://wiki.guildwars.com/wiki/Gamer", Tiers = new List<string> { "Skillz", "Pro Skillz", "Numchuck Skillz", "Mad Skillz", "Über Micro Skillz", "Gosu Skillz", "1337 Skillz", "iddqd Skillz", "T3h Haxz0rz Skillz", "Pure Pwnage Skillz", "These skillz go to", "Real Ultimate Power Skillz" } };
    public static Title SkillHunterTyria { get; } = new Title { Id = 24, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = new List<string> { "Tyrian Elite Skill Hunter" } };
    public static Title VanquisherTyria { get; } = new Title { Id = 25, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = new List<string> { "Tyrian Vanquisher" } };
    public static Title SkillHunterCantha { get; } = new Title { Id = 26, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = new List<string> { "Canthan Elite Skill Hunter" } };
    public static Title VanquisherCantha { get; } = new Title { Id = 27, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = new List<string> { "Canthan Vanquisher" } };
    public static Title SkillHunterElona { get; } = new Title { Id = 28, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = new List<string> { "Elonian Elite Skill Hunter" } };
    public static Title VanquisherElona { get; } = new Title { Id = 29, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = new List<string> { "Elonian Vanquisher" } };
    public static Title LegendaryCartographer { get; } = new Title { Id = 30, Name = "Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = new List<string> { "Legendary Cartographer" } };
    public static Title LegendaryGuardian { get; } = new Title { Id = 31, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = new List<string> { "Legendary Guardian" } };
    public static Title LegendarySkillHunter { get; } = new Title { Id = 32, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = new List<string> { "Legendary Skill Hunter" } };
    public static Title LegendaryVanquisher { get; } = new Title { Id = 33, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = new List<string> { "Legendary Vanquisher" } };
    public static Title SweetTooth { get; } = new Title { Id = 34, Name = "Sweet Tooth", WikiUrl = "https://wiki.guildwars.com/wiki/Sweet_Tooth", Tiers = new List<string> { "Sweet Tooth", "Connoisseur of Confectionaries" } };
    public static Title GuardianTyria { get; } = new Title { Id = 35, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = new List<string> { "Guardian of Tyria" } };
    public static Title GuardianCantha { get; } = new Title { Id = 36, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = new List<string> { "Guardian of Cantha" } };
    public static Title GuardianElona { get; } = new Title { Id = 37, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = new List<string> { "Guardian of Elona" } };
    public static Title Asura { get; } = new Title { Id = 38, Name = "Asura", WikiUrl = "https://wiki.guildwars.com/wiki/Asura_rank", Tiers = new List<string> { "Not Too Smelly", "Not Too Dopey", "Not Too Clumsy", "Not Too Boring", "Not Too Annoying", "Not Too Grumpy", "Not Too Silly", "Not Too Lazy", "Not Too Foolish", "Not Too Shabby" } };
    public static Title Deldrimor { get; } = new Title { Id = 39, Name = "Deldrimor", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_rank", Tiers = new List<string> { "Delver", "Stout Delver", "Gutsy Delver", "Risky Delver", "Bold Delver", "Daring Delver", "Adventurous Delver", "Courageous Delver", "Epic Delver", "Legendary Delver" } };
    public static Title EbonVanguard { get; } = new Title { Id = 40, Name = "Ebon Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/Ebon_Vanguard_rank", Tiers = new List<string> { "Agent", "Covert Agent", "Stealth Agent", "Mysterious Agent", "Shadow Agent", "Underground Agent", "Special Agent", "Valued Agent", "Superior Agent", "Secret Agent" } };
    public static Title Norn { get; } = new Title { Id = 41, Name = "Norn", WikiUrl = "https://wiki.guildwars.com/wiki/Norn_rank", Tiers = new List<string> { "Slayer of Imps", "Slayer of Beasts", "Slayer of Nightmares", "Slayer of Giants", "Slayer of Wurms", "Slayer of Demons", "Slayer of Heroes", "Slayer of Champions", "Slayer of Hordes", "Slayer of All" } };
    public static Title MasterOfTheNorth { get; } = new Title { Id = 42, Name = "Master of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_the_North", Tiers = new List<string> { "Adventurer of the North", "Pioneer of the North", "Veteran of the North", "Conqueror of the North", "Master of the North", "Legendary Master of the North" } };
    public static Title PartyAnimal { get; } = new Title { Id = 43, Name = "Party Animal", WikiUrl = "https://wiki.guildwars.com/wiki/Party_Animal", Tiers = new List<string> { "Party Animal", "Life of the Party" } };
    public static Title Zaishen { get; } = new Title { Id = 44, Name = "Zaishen", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_rank", Tiers = new List<string> { "Zaishen Supporter", "Friend of the Zaishen", "Companion of the Zaishen", "Ally of the Zaishen", "Sentinel of the Zaishen", "Steward of the Zaishen", "Defender of the Zaishen", "Warden of the Zaishen", "Bastion  of the Zaishen", "Champion of the Zaishen", "Hero of the Zaishen", "Legendary Hero of the Zaishen" } };
    public static Title TreasureHunter { get; } = new Title { Id = 45, Name = "Treasure Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Treasure_Hunter", Tiers = new List<string> { "Treasure Hunter", "Adept Treasure Hunter", "Advanced Treasure Hunter", "Expert Treasure Hunter", "Elite Treasure Hunter", "Master Treasure Hunter", "Grandmaster Treasure Hunter" } };
    public static Title Wisdom { get; } = new Title { Id = 46, Name = "Wisdom", WikiUrl = "https://wiki.guildwars.com/wiki/Wisdom", Tiers = new List<string> { "Seeker of Wisdom", "Collector of Wisdom", "Devotee of Wisdom", "Devourer of Wisdom", "Font of Wisdom", "Oracle of Wisdom", "Source of Wisdom" } };
    public static Title Codex { get; } = new Title { Id = 47, Name = "Codex", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Title", Tiers = new List<string> { "Codex Initiate", "Codex Acolyte", "Codex Disciple", "Codex Zealot", "Codex Stalwart", "Codex Adept", "Codex Exemplar", "Codex Prodigy", "Codex Champion", "Codex Paragon", "Codex Master", "Codex Grandmaster" } };

    public static IEnumerable<Title> Titles { get; } = new List<Title>
    {
        None,
        Hero,
        TyrianCartographer,
        CanthanCartographer,
        Gladiator,
        Champion,
        Kurzick,
        Luxon,
        Drunkard,
        Survivor,
        KindOfABigDeal,
        ProtectorTyria,
        ProtectorCantha,
        Lucky,
        Unlucky,
        Sunspear,
        ElonianCartographer,
        ProtectorElona,
        Lightbringer,
        LegendaryDefenderOfAscalon,
        Commander,
        Gamer,
        SkillHunterTyria,
        VanquisherTyria,
        SkillHunterCantha,
        VanquisherCantha,
        SkillHunterElona,
        VanquisherElona,
        LegendaryCartographer,
        LegendaryGuardian,
        LegendarySkillHunter,
        LegendaryVanquisher,
        SweetTooth,
        GuardianTyria,
        GuardianCantha,
        GuardianElona,
        Asura,
        Deldrimor,
        EbonVanguard,
        Norn,
        MasterOfTheNorth,
        PartyAnimal,
        Zaishen,
        TreasureHunter,
        Wisdom,
        Codex
    };

    public static bool TryParse(int id, out Title title)
    {
        title = Titles.Where(title => title.Id == id).FirstOrDefault()!;
        if (title is null)
        {
            return false;
        }

        return true;
    }
    public static bool TryParse(string name, out Title title)
    {
        title = Titles.Where(title => title.Name == name).FirstOrDefault()!;
        if (title is null)
        {
            return false;
        }

        return true;
    }
    public static Title Parse(int id)
    {
        if (TryParse(id, out var title) is false)
        {
            throw new InvalidOperationException($"Could not find a title with id {id}");
        }

        return title;
    }
    public static Title Parse(string name)
    {
        if (TryParse(name, out var title) is false)
        {
            throw new InvalidOperationException($"Could not find a title with name {name}");
        }

        return title;
    }

    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? WikiUrl { get; private set; }
    public List<string>? Tiers { get; private set; }

    private Title()
    {
    }
}
