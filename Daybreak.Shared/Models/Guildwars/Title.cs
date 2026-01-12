using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Guildwars;

public sealed class Title : IWikiEntity
{
    public static readonly Title None = new() { Id = 0xFF, };
    public static readonly Title Hero = new() { Id = 0, Name = "Hero", WikiUrl = "https://wiki.guildwars.com/wiki/Hero_(title)", Tiers = ["Hero", "Fierce Hero", "Mighty Hero", "Deadly Hero", "Terrifying Hero", "Conquering Hero", "Subjugating Hero", "Vanquishing Hero", "Renowed Hero", "Illustrious Hero", "Eminent Hero", "King's Hero", "Emperor's Hero", "Balthazar's Hero", "Legendary Hero"] };
    public static readonly Title TyrianCartographer = new() { Id = 1, Name = "Tyrian Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = ["Tyrian Explorer", "Tyrian Pathfinder", "Tyrian Trailblazer", "Tyrian Cartographer", "Tyrian Master Cartographer", "Tyrian Grandmaster Cartographer"] };
    public static readonly Title CanthanCartographer = new() { Id = 2, Name = "Canthan Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = ["Canthan Explorer", "Canthan Pathfinder", "Canthan Trailblazer", "Canthan Cartographer", "Canthan Master Cartographer", "Canthan Grandmaster Cartographer"] };
    public static readonly Title Gladiator = new() { Id = 3, Name = "Gladiator", WikiUrl = "https://wiki.guildwars.com/wiki/Gladiator", Tiers = ["Gladiator", "Fierce Gladiator", "Mighty Gladiator", "Deadly Gladiator", "Terrifying Gladiator", "Conquering Gladiator", "Subjugating Gladiator", "Vanquishing Gladiator", "King's Gladiator", "Emperor's Gladiator", "Balthazar's Gladiator", "Legendary Gladiator"] };
    public static readonly Title Champion = new() { Id = 4, Name = "Champion", WikiUrl = "https://wiki.guildwars.com/wiki/Champion", Tiers = ["Champion", "Fierce Champion", "Mighty Champion", "Deadly Champion", "Terrifying Champion", "Conquering Champion", "Subjugating Champion", "Vanquishing Champion", "King's Champion", "Emperor's Champion", "Balthazar's Champion", "Legendary Champion"] };
    public static readonly Title Kurzick = new() { Id = 5, Name = "Faction Allegiance", WikiUrl = "https://wiki.guildwars.com/wiki/Allegiance_rank", Tiers = ["Kurzick Supporter", "Friend of the Kurzicks", "Companion of the Kurzicks", "Ally of the Kurzicks", "Sentinel of the Kurzicks", "Steward of the Kurzicks", "Defender of the Kurzicks", "Warden of the Kurzicks", "Bastion of the Kurzicks", "Champion of the Kurzicks", "Hero of the Kurzicks", "Savior of the Kurzicks"] };
    public static readonly Title Luxon = new() { Id = 6, Name = "Faction Allegiance", WikiUrl = "https://wiki.guildwars.com/wiki/Allegiance_rank", Tiers = ["Luxon Supporter", "Friend of the Kurzicks", "Companion of the Luxons", "Ally of the Luxons", "Sentinel of the Luxons", "Steward of the Luxons", "Defender of the Luxons", "Warden of the Luxons", "Bastion of the Luxons", "Champion of the Luxons", "Hero of the Luxons", "Savior of the Luxons"] };
    public static readonly Title Drunkard = new() { Id = 7, Name = "Drunkard", WikiUrl = "https://wiki.guildwars.com/wiki/Drunkard", Tiers = ["Drunkard", "Incorrigible Ale-Hound"] };
    public static readonly Title Survivor = new() { Id = 9, Name = "Survivor", WikiUrl = "https://wiki.guildwars.com/wiki/Survivor", Tiers = ["Survivor", "Indomitable Survivor", "Legendary Survivor"] };
    public static readonly Title KindOfABigDeal = new() { Id = 10, Name = "Kind of a Big Deal", WikiUrl = "https://wiki.guildwars.com/wiki/Kind_of_a_Big_Deal", Tiers = ["Kind Of A Big Deal", "People Know Me", "I'm Very Important", "I Have Many Leather-Bound Books", "My Guild Hall Smells of Rich Mahogany", "God Walking Amongst Mere Mortals"] };
    public static readonly Title ProtectorTyria = new() { Id = 13, Name = "Protector of Tyria", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = ["Protector of Tyria"] };
    public static readonly Title ProtectorCantha = new() { Id = 14, Name = "Protector of Cantha", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = ["Protector of Cantha"] };
    public static readonly Title Lucky = new() { Id = 15, Name = "Lucky", WikiUrl = "https://wiki.guildwars.com/wiki/Lucky_and_Unlucky", Tiers = ["Charmed", "Lucky", "Favored", "Prosperous", "Golden", "Blessed by Fate"] };
    public static readonly Title Unlucky = new() { Id = 16, Name = "Unlucky", WikiUrl = "https://wiki.guildwars.com/wiki/Lucky_and_Unlucky", Tiers = ["Hapless", "Unlucky", "Unfavored", "Tragic", "Wretched", "Jinxed", "Cursed by Fate"] };
    public static readonly Title Sunspear = new() { Id = 17, Name = "Sunspear", WikiUrl = "https://wiki.guildwars.com/wiki/Sunspear_rank", Tiers = ["Sunspear Sergeant", "Sunspear Master Sergeant", "Second Spear", "First Spear", "Sunspear Captain", "Sunspear Commander", "Sunspear General", "Sunspear Castellan", "Spearmarshal", "Legendary Spearmarshal"] };
    public static readonly Title ElonianCartographer = new() { Id = 18, Name = "Elonian Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = ["Elonian Explorer", "Elonian Pathfinder", "Elonian Trailblazer", "Elonian Cartographer", "Elonian Master Cartographer", "Elonian Grandmaster Cartographer"] };
    public static readonly Title ProtectorElona = new() { Id = 19, Name = "Protector of Elona", WikiUrl = "https://wiki.guildwars.com/wiki/Protector", Tiers = ["Protector of Elona"] };
    public static readonly Title Lightbringer = new() { Id = 20, Name = "Lightbringer", WikiUrl = "https://wiki.guildwars.com/wiki/Lightbringer_rank", Tiers = ["Lightbringer", "Adept Lightbringer", "Brave Lightbringer", "Mighty Lightbringer", "Conquering Lightbringer", "Vanquishing Lightbringer", "Revered Lightbringer", "Holy Lightbringer"] };
    public static readonly Title LegendaryDefenderOfAscalon = new() { Id = 21, Name = "Defender of Ascalon", WikiUrl = "https://wiki.guildwars.com/wiki/Defender_of_Ascalon", Tiers = ["Legendary Defender of Ascalon"] };
    public static readonly Title Commander = new() { Id = 22, Name = "Commander", WikiUrl = "https://wiki.guildwars.com/wiki/Commander", Tiers = ["Commander", "Victorious Commander", "Triumphant Commander", "Keen Commander", "Battle Commander", "Field Commander", "Lieutenant Commander", "Wing Commander", "Cobra Commander", "Supreme Commander", "Master And Commander", "Legendary Commander"] };
    public static readonly Title Gamer = new() { Id = 23, Name = "Gamer", WikiUrl = "https://wiki.guildwars.com/wiki/Gamer", Tiers = ["Skillz", "Pro Skillz", "Numchuck Skillz", "Mad Skillz", "Über Micro Skillz", "Gosu Skillz", "1337 Skillz", "iddqd Skillz", "T3h Haxz0rz Skillz", "Pure Pwnage Skillz", "These skillz go to", "Real Ultimate Power Skillz"] };
    public static readonly Title SkillHunterTyria = new() { Id = 24, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = ["Tyrian Elite Skill Hunter"] };
    public static readonly Title VanquisherTyria = new() { Id = 25, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = ["Tyrian Vanquisher"] };
    public static readonly Title SkillHunterCantha = new() { Id = 26, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = ["Canthan Elite Skill Hunter"] };
    public static readonly Title VanquisherCantha = new() { Id = 27, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = ["Canthan Vanquisher"] };
    public static readonly Title SkillHunterElona = new() { Id = 28, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = ["Elonian Elite Skill Hunter"] };
    public static readonly Title VanquisherElona = new() { Id = 29, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = ["Elonian Vanquisher"] };
    public static readonly Title LegendaryCartographer = new() { Id = 30, Name = "Cartographer", WikiUrl = "https://wiki.guildwars.com/wiki/Cartographer", Tiers = ["Legendary Cartographer"] };
    public static readonly Title LegendaryGuardian = new() { Id = 31, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = ["Legendary Guardian"] };
    public static readonly Title LegendarySkillHunter = new() { Id = 32, Name = "Skill Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Skill_Hunter", Tiers = ["Legendary Skill Hunter"] };
    public static readonly Title LegendaryVanquisher = new() { Id = 33, Name = "Vanquisher", WikiUrl = "https://wiki.guildwars.com/wiki/Vanquisher", Tiers = ["Legendary Vanquisher"] };
    public static readonly Title SweetTooth = new() { Id = 34, Name = "Sweet Tooth", WikiUrl = "https://wiki.guildwars.com/wiki/Sweet_Tooth", Tiers = ["Sweet Tooth", "Connoisseur of Confectionaries"] };
    public static readonly Title GuardianTyria = new() { Id = 35, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = ["Guardian of Tyria"] };
    public static readonly Title GuardianCantha = new() { Id = 36, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = ["Guardian of Cantha"] };
    public static readonly Title GuardianElona = new() { Id = 37, Name = "Guardian", WikiUrl = "https://wiki.guildwars.com/wiki/Guardian_(title)", Tiers = ["Guardian of Elona"] };
    public static readonly Title Asura = new() { Id = 38, Name = "Asura", WikiUrl = "https://wiki.guildwars.com/wiki/Asura_rank", Tiers = ["Not Too Smelly", "Not Too Dopey", "Not Too Clumsy", "Not Too Boring", "Not Too Annoying", "Not Too Grumpy", "Not Too Silly", "Not Too Lazy", "Not Too Foolish", "Not Too Shabby"] };
    public static readonly Title Deldrimor = new() { Id = 39, Name = "Deldrimor", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_rank", Tiers = ["Delver", "Stout Delver", "Gutsy Delver", "Risky Delver", "Bold Delver", "Daring Delver", "Adventurous Delver", "Courageous Delver", "Epic Delver", "Legendary Delver"] };
    public static readonly Title EbonVanguard = new() { Id = 40, Name = "Ebon Vanguard", WikiUrl = "https://wiki.guildwars.com/wiki/Ebon_Vanguard_rank", Tiers = ["Agent", "Covert Agent", "Stealth Agent", "Mysterious Agent", "Shadow Agent", "Underground Agent", "Special Agent", "Valued Agent", "Superior Agent", "Secret Agent"] };
    public static readonly Title Norn = new() { Id = 41, Name = "Norn", WikiUrl = "https://wiki.guildwars.com/wiki/Norn_rank", Tiers = ["Slayer of Imps", "Slayer of Beasts", "Slayer of Nightmares", "Slayer of Giants", "Slayer of Wurms", "Slayer of Demons", "Slayer of Heroes", "Slayer of Champions", "Slayer of Hordes", "Slayer of All"] };
    public static readonly Title MasterOfTheNorth = new() { Id = 42, Name = "Master of the North", WikiUrl = "https://wiki.guildwars.com/wiki/Master_of_the_North", Tiers = ["Adventurer of the North", "Pioneer of the North", "Veteran of the North", "Conqueror of the North", "Master of the North", "Legendary Master of the North"] };
    public static readonly Title PartyAnimal = new() { Id = 43, Name = "Party Animal", WikiUrl = "https://wiki.guildwars.com/wiki/Party_Animal", Tiers = ["Party Animal", "Life of the Party"] };
    public static readonly Title Zaishen = new() { Id = 44, Name = "Zaishen", WikiUrl = "https://wiki.guildwars.com/wiki/Zaishen_rank", Tiers = ["Zaishen Supporter", "Friend of the Zaishen", "Companion of the Zaishen", "Ally of the Zaishen", "Sentinel of the Zaishen", "Steward of the Zaishen", "Defender of the Zaishen", "Warden of the Zaishen", "Bastion  of the Zaishen", "Champion of the Zaishen", "Hero of the Zaishen", "Legendary Hero of the Zaishen"] };
    public static readonly Title TreasureHunter = new() { Id = 45, Name = "Treasure Hunter", WikiUrl = "https://wiki.guildwars.com/wiki/Treasure_Hunter", Tiers = ["Treasure Hunter", "Adept Treasure Hunter", "Advanced Treasure Hunter", "Expert Treasure Hunter", "Elite Treasure Hunter", "Master Treasure Hunter", "Grandmaster Treasure Hunter"] };
    public static readonly Title Wisdom = new() { Id = 46, Name = "Wisdom", WikiUrl = "https://wiki.guildwars.com/wiki/Wisdom", Tiers = ["Seeker of Wisdom", "Collector of Wisdom", "Devotee of Wisdom", "Devourer of Wisdom", "Font of Wisdom", "Oracle of Wisdom", "Source of Wisdom"] };
    public static readonly Title Codex = new() { Id = 47, Name = "Codex", WikiUrl = "https://wiki.guildwars.com/wiki/Codex_Title", Tiers = ["Codex Initiate", "Codex Acolyte", "Codex Disciple", "Codex Zealot", "Codex Stalwart", "Codex Adept", "Codex Exemplar", "Codex Prodigy", "Codex Champion", "Codex Paragon", "Codex Master", "Codex Grandmaster"] };

    public static readonly IEnumerable<Title> Titles =
    [
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
    ];

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

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("wikiUrl")]
    public string? WikiUrl { get; init; }

    [JsonPropertyName("tiers")]
    public List<string>? Tiers { get; init; }

    private Title()
    {
    }
}
