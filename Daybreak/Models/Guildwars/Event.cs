using System;
using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

/// <summary>
/// Seasonal holidays and in-game events, as retrieved from https://www.guildwars.com/en/events.
/// All events begin and end at 12pm UTC-7
/// </summary>
public sealed class Event
{
    public static readonly Event CanthanNewYear = new()
    {
        Title = "Canthan New Year",
        Description = "Join the people of Cantha as they prepare a heavenly feast to honor the Celestial Animal of the coming year! This annual week-long celebration features Canthan cuisine, festive fireworks, special prizes, and the famed Shing Jea Boardwalk!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Canthan_New_Year",
        From = new DateOnly(1, 1, 31),
        To = new DateOnly(1, 2, 7)
    };

    public static readonly Event LuckyTreatsWeek = new()
    {
        Title = "Lucky Treats Week",
        Description = "Shamrock Ale and Four-Leaf Clovers will be dropping throughout Tyria! Go fast, adventurers, and gather all the green you can before this special week ends!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Lucky_Treats_Week",
        From = new DateOnly(1, 3, 14),
        To = new DateOnly(1, 3, 21)
    };

    public static readonly Event SweetTreatsWeek = new()
    {
        Title = "Sweet Treats Week",
        Description = "Sweet Treats Week is upon us, which means it’s time to load up on springtime goodies. Chocolate Bunnies and Golden Eggs will be randomly mixed in with regular drops all across Tyria. Get hopping!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Sweet_Treats_Week",
        From = new DateOnly(1, 4, 22),
        To = new DateOnly(1, 5, 6)
    };

    public static readonly Event AnniversaryCelebration = new()
    {
        Title = "Guild Wars Anniversary Celebration",
        Description = "Guild Wars is another year older and the party just won't stop! Strap on your party hats, unpack your fireworks, and get ready to celebrate!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Anniversary_Celebration",
        From = new DateOnly(1, 4, 10),
        To = new DateOnly(1, 4, 17)
    };

    public static readonly Event DragonFestival = new()
    {
        Title = "Dragon Festival",
        Description = "The Dragon Festival is almost upon us and Cantha is ready to roll out the red carpet. Time to stock up on bottle rockets, gorge on red bean cakes, and collect enough victory tokens for your pick of festival hats.",
        WikiUrl = "https://wiki.guildwars.com/wiki/Dragon_Festival",
        From = new DateOnly(1, 6, 27),
        To = new DateOnly(1, 7, 4)
    };

    public static readonly Event WintersdayInJuly = new()
    {
        Title = "Wintersday In July",
        Description = "Just can't wait ‘til Wintersday? Jump into a mini version of Tyria’s famed winter celebration! Stock up on seasonal treats or target your friends in the Snowball Arena during this week-long celebration.",
        WikiUrl = "https://wiki.guildwars.com/wiki/Wintersday_in_July",
        From = new DateOnly(1, 7, 24),
        To = new DateOnly(1, 7, 31)
    };

    public static readonly Event WayfarersReverie = new()
    {
        Title = "Wayfarer's Reverie",
        Description = "The spirit of adventure beckons to you! It’s time to seek out the festival coordinator on each continent and start a quest to prove your adventuring prowess. Collect enough evidence of your travels and receive a special gift!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Wayfarer%27s_Reverie",
        From = new DateOnly(1, 8, 25),
        To = new DateOnly(1, 9, 1)
    };

    public static readonly Event PirateWeek = new()
    {
        Title = "Pirate Week",
        Description = "Avast! This seafaring celebration will have NPCs in Lion’s Arch and Kamadan talking like salty sea dogs. Join in the fun or walk the plank. Yarr!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Pirate_Week",
        From = new DateOnly(1, 9, 13),
        To = new DateOnly(1, 9, 20)
    };

    public static readonly Event BreastCancerAwarenessMonth = new()
    {
        Title = "Breast Cancer Awareness Month",
        Description = "Sadie Salvitas arrives in Lion’s Arch on October 1 and will be selling vials of pink dye throughout the month in support of Breast Cancer Awareness.",
        WikiUrl = "https://wiki.guildwars.com/wiki/Login_screen_announcements/Archive#Breast_Cancer_Awareness_Month",
        From = new DateOnly(1, 10, 1),
        To = new DateOnly(1, 11, 2)
    };

    public static readonly Event Halloween = new()
    {
        Title = "Halloween",
        Description = "Get ready for two whole weeks of mischief and mayhem in Tyria, courtesy of Mad King Thorn and his Lunatic Court. The madness escalates to the final event on October 31, when the Mad King himself appears every three hours—join if you dare!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Halloween",
        From = new DateOnly(1, 10, 18),
        To = new DateOnly(1, 11, 2)
    };

    public static readonly Event SpecialTreatsWeek = new()
    {
        Title = "Special Treats Week",
        Description = "It's time for special, seasonal delights such as Slices of Pumpkin Pie and Hard Apple Cider to begin to drop all over Tyria. Gather what you can, but don't stuff yourself; Wintersday is just around the corner!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Special_Treats_Week",
        From = new DateOnly(1, 11, 21),
        To = new DateOnly(1, 11, 28)
    };

    public static readonly Event Wintersday = new()
    {
        Title = "Wintersday",
        Description = "See towns glimmering with festive décor, deliver presents to needy little urchins, defeat the scheming Grentches, and spread Wintersday cheer to your fellow Tyrians! The fun culminates with a grand finale that runs every three hours on January 1!",
        WikiUrl = "https://wiki.guildwars.com/wiki/Wintersday",
        From = new DateOnly(1, 12, 19),
        To = new DateOnly(1, 1, 2)
    };

    public static readonly IEnumerable<Event> Events = new List<Event>
    {
        CanthanNewYear,
        LuckyTreatsWeek,
        SweetTreatsWeek,
        AnniversaryCelebration,
        DragonFestival,
        WintersdayInJuly,
        WayfarersReverie,
        PirateWeek,
        BreastCancerAwarenessMonth,
        Halloween,
        SpecialTreatsWeek,
        Wintersday
    };

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? WikiUrl { get; set; }

    public DateOnly From { get; set; }

    public DateOnly To { get; set; }

    private Event()
    {
    }
}
