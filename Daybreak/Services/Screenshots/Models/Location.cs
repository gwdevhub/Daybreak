using Daybreak.Shared.Models.Guildwars;
using System.Collections.Generic;

namespace Daybreak.Services.Screenshots.Models;

internal sealed class Location
{
    public static readonly Location AscalonPreSearing = new(
        Region.PresearingAscalon,
        [
            new Entry
            {
                Map = Map.AscalonCityPresearing,
                Url = "http://bloogum.net/guildwars/pre/openingcutscene/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.AscalonCityPresearing,
                Url = "http://bloogum.net/guildwars/pre/ascaloncity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.LakesideCounty,
                Url = "http://bloogum.net/guildwars/pre/lakesidecounty/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.AshfordAbbeyOutpost,
                Url = "http://bloogum.net/guildwars/pre/ashfordabbey/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheCatacombs,
                Url = "http://bloogum.net/guildwars/pre/thecatacombs/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.GreenHillsCounty,
                Url = "http://bloogum.net/guildwars/pre/greenhillscounty/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.TheBarradinEstateOutpost,
                Url = "http://bloogum.net/guildwars/pre/thebarradinestate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.WizardsFolly,
                Url = "http://bloogum.net/guildwars/pre/wizardsfolly/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.RegentValleyPreSearing,
                Url = "http://bloogum.net/guildwars/pre/regentvalley/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.FortRanikPreSearingOutpost,
                Url = "http://bloogum.net/guildwars/pre/fortranik/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheNorthlands,
                Url = "http://bloogum.net/guildwars/pre/thenorthlands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.AscalonAcademyPvPBattleMission,
                Url = "http://bloogum.net/guildwars/pre/ascalonacademy/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.GreenHillsCounty,
                Url = "https://media.discordapp.net/attachments/279231165045407744/1053453780580044940/image.png?ex=656a6c49&is=6557f749&hm=beab3433db2cac2d519fc8158978a949e06a57df50b587f41afe3718dc4dad20&=&format=webp&width=954&height=521",
                Credit = "",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location Ascalon = new(
        Region.Ascalon,
        [
            new Entry
            {
                Map = Map.AscalonCityOutpost,
                Url = "http://bloogum.net/guildwars/ascalon/ascaloncity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.AscalonCityWintersdayOutpost,
                Url = "http://bloogum.net/guildwars/ascalon/ascaloncity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 7,
                Count = 10
            },
            new Entry
            {
                Map = Map.OldAscalon,
                Url = "http://bloogum.net/guildwars/ascalon/oldascalon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.OldAscalon,
                Url = "http://bloogum.net/guildwars/ascalon/regentvalley/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.PockmarkFlats,
                Url = "http://bloogum.net/guildwars/ascalon/pockmarkflats/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.EasternFrontier,
                Url = "http://bloogum.net/guildwars/ascalon/easternfrontier/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.EasternFrontier,
                Url = "http://bloogum.net/guildwars/ascalon/thebreach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.DiessaLowlands,
                Url = "http://bloogum.net/guildwars/ascalon/diessalowlands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.DragonsGullet,
                Url = "http://bloogum.net/guildwars/ascalon/dragonsgullet/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.AscalonFoothills,
                Url = "http://bloogum.net/guildwars/ascalon/ascalonfoothills/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheGreatNorthernWall,
                Url = "http://bloogum.net/guildwars/ascalon/thegreatnorthernwall/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.FortRanik,
                Url = "http://bloogum.net/guildwars/ascalon/fortranik/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.RuinsOfSurmia,
                Url = "http://bloogum.net/guildwars/ascalon/ruinsofsurmia/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.NolaniAcademy,
                Url = "http://bloogum.net/guildwars/ascalon/nolaniacademy/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.AscalonCityWintersdayOutpost,
                Url = "https://wiki.guildwars.com/images/0/0b/Ascalon_City_Wintersday_2009.png",
                Credit = "https://wiki.guildwars.com/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheGreatNorthernWall,
                Url = "https://media.discordapp.net/attachments/279231165045407744/853784390357876746/gw005.jpg?ex=656d152d&is=655aa02d&hm=03e8464e9fc60f8c832eebc246a4fff422ebb865d27dc189b727f22aacab6fb2&=&format=webp&width=954&height=380",
                Credit = "https://discordapp.com/users/Chrono#6655",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location NorthernShiverpeaks = new(
        Region.ShiverpeakMountains,
        [
            new Entry
            {
                Map = Map.TravelersVale,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/travelersvale/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.YaksBendOutpost,
                Url = "http://bloogum.netguildwars/nshiverpeaks/yaksbend/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.BorlisPass,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/borlispass/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.IronHorseMine,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/ironhorsemine/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.TheFrostGate,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/thefrostgate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.AnvilRock,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/anvilrock/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.IceToothCaveOutpost,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/icetoothcave/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.DeldrimorBowl,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/deldrimorbowl/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.BeaconsPerchOutpost,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/beaconsperch/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.GriffonsMouth,
                Url = "http://bloogum.net/guildwars/nshiverpeaks/griffonsmouth/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.IronHorseMine,
                Url = "https://media.discordapp.net/attachments/279231165045407744/927936695230414888/gw064.jpg?ex=656f3864&is=655cc364&hm=75456e75a3a77e0dcbc8b0f1d3dfa292e477b535e90c7a8f40566e0ffa52cb64&=&format=webp&width=903&height=564",
                Credit = "https://discordapp.com/users/Pekka#4619",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location Kryta = new(
        Region.Kryta,
        [
            new Entry
            {
                Map = Map.ScoundrelsRise,
                Url = "http://bloogum.net/guildwars/kryta/scoundrelsrise/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.GatesOfKryta,
                Url = "http://bloogum.net/guildwars/kryta/gatesofkryta/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.LionsGate,
                Url = "http://bloogum.net/guildwars/kryta/lionsgate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.LionsArchOutpost,
                Url = "http://bloogum.net/guildwars/kryta/lionsarch/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.LionsArchWintersdayOutpost,
                Url = "http://bloogum.net/guildwars/kryta/lionsarch/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 17,
                Count = 11
            },
            new Entry
            {
                Map = Map.LionsArchCanthanNewYearOutpost,
                Url = "http://bloogum.net/guildwars/kryta/lionsarch/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 29,
                Count = 11
            },
            new Entry
            {
                Map = Map.LionsArchHalloweenOutpost,
                Url = "http://bloogum.net/guildwars/kryta/lionsarch/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 41,
                Count = 4
            },
            new Entry
            {
                Map = Map.WarinKrytaLionsArchKeep,
                Url = "http://bloogum.net/guildwars/kryta/lionsarchkeep/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.NorthKrytaProvince,
                Url = "http://bloogum.net/guildwars/kryta/northkrytaprovince/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 42
            },
            new Entry
            {
                Map = Map.DAlessioSeaboard,
                Url = "http://bloogum.net/guildwars/kryta/dallesioseaboard/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 39
            },
            new Entry
            {
                Map = Map.NeboTerrace,
                Url = "http://bloogum.net/guildwars/kryta/neboterrace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.BergenHotSpringsOutpost,
                Url = "http://bloogum.net/guildwars/kryta/bergenhotsprings/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.CursedLands,
                Url = "http://bloogum.net/guildwars/kryta/cursedlands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BeetletunOutpost,
                Url = "http://bloogum.net/guildwars/kryta/beetletun/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.WatchtowerCoast,
                Url = "http://bloogum.net/guildwars/kryta/watchtowercoast/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.DivinityCoast,
                Url = "http://bloogum.net/guildwars/kryta/divinitycoast/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.TempleOfTheAges,
                Url = "http://bloogum.net/guildwars/kryta/templeofages/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.TheBlackCurtain,
                Url = "http://bloogum.net/guildwars/kryta/theblackcurtain/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 25
            },
            new Entry
            {
                Map = Map.KessexPeak,
                Url = "http://bloogum.net/guildwars/kryta/kessexpeak/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.TalmarkWilderness,
                Url = "http://bloogum.net/guildwars/kryta/talmarkwilderness/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.MajestysRest,
                Url = "http://bloogum.net/guildwars/kryta/majestysrest/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.TearsOfTheFallen,
                Url = "http://bloogum.net/guildwars/kryta/tearsofthefallen/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.TwinSerpentLakes,
                Url = "http://bloogum.net/guildwars/kryta/twinserpentlakes/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.StingrayStrand,
                Url = "http://bloogum.net/guildwars/kryta/stingraystrand/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.FishermensHavenOutpost,
                Url = "http://bloogum.net/guildwars/kryta/fishermenshaven/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.RiversideProvince,
                Url = "http://bloogum.net/guildwars/kryta/riversideprovince/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.SanctumCay,
                Url = "http://bloogum.net/guildwars/kryta/sanctumcay/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.SanctumCay,
                Url = "https://i.imgur.com/9jrmIAM.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.LionsArchWintersdayOutpost,
                Url = "https://wiki.guildwars.com/images/b/b6/Lions_Arch_Wintersday_2009.png",
                Credit = "https://wiki.guildwars.com/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BeneathLionsArch,
                Url = "https://i.imgur.com/PVtkF7W.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BeneathLionsArch,
                Url = "https://i.imgur.com/Aeg5Qld.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.WarinKrytaTheMausoleum,
                Url = "https://i.imgur.com/0PHeleU.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.WarinKrytaTheMausoleum,
                Url = "https://i.imgur.com/4l6X5L3.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.WarinKrytaTheMausoleum,
                Url = "https://i.imgur.com/zyf33kW.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.WarinKrytaTheMausoleum,
                Url = "https://i.imgur.com/jYvlbej.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.WarinKrytaTheMausoleum,
                Url = "https://i.imgur.com/ByyS5ya.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location MaguumaJungle = new(
        Region.MaguumaJungle,
        [
            new Entry
            {
                Map = Map.DruidsOverlookOutpost,
                Url = "http://bloogum.net/guildwars/maguuma/druidsoverlook/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.SageLands,
                Url = "http://bloogum.net/guildwars/maguuma/sagelands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 27
            },
            new Entry
            {
                Map = Map.TheWilds,
                Url = "http://bloogum.net/guildwars/maguuma/thewilds/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 19
            },
            new Entry
            {
                Map = Map.MamnoonLagoon,
                Url = "http://bloogum.net/guildwars/maguuma/mamnoonlagoon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 17
            },
            new Entry
            {
                Map = Map.QuarrelFallsOutpost,
                Url = "http://bloogum.net/guildwars/maguuma/quarrelfalls/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.Silverwood,
                Url = "http://bloogum.net/guildwars/maguuma/silverwood/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.BloodstoneFen,
                Url = "http://bloogum.net/guildwars/maguuma/bloodstonefen/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.EttinsBack,
                Url = "http://bloogum.net/guildwars/maguuma/ettinsback/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.VentarisRefugeOutpost,
                Url = "http://bloogum.net/guildwars/maguuma/ventarisrefuge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.ReedBog,
                Url = "http://bloogum.net/guildwars/maguuma/reedbog/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.TheFalls,
                Url = "http://bloogum.net/guildwars/maguuma/thefalls/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.DryTop,
                Url = "http://bloogum.net/guildwars/maguuma/drytop/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.TangleRoot,
                Url = "http://bloogum.net/guildwars/maguuma/tangleroot/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.MaguumaStadeOutpost,
                Url = "http://bloogum.net/guildwars/maguuma/maguumastade/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.AuroraGlade,
                Url = "http://bloogum.net/guildwars/maguuma/auroraglade/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.HengeOfDenraviOutpost,
                Url = "http://bloogum.net/guildwars/maguuma/hengeofdenravi/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.MamnoonLagoon,
                Url = "https://i.imgur.com/d78EuZt.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location CrystalDesert = new(
        Region.CrystalDesert,
        [
            new Entry
            {
                Map = Map.TheAmnoonOasisOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/amnoonoasis/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.ProphetsPath,
                Url = "http://bloogum.net/guildwars/crystaldesert/prophetspath/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.HeroesAudienceOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/heroesaudience/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.SaltFlats,
                Url = "http://bloogum.net/guildwars/crystaldesert/seaflats/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.SeekersPassageOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/seekerspassage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.DivinersAscent,
                Url = "http://bloogum.net/guildwars/crystaldesert/divinersascent/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.ElonaReach,
                Url = "http://bloogum.net/guildwars/crystaldesert/elonareach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 23
            },
            new Entry
            {
                Map = Map.SkywardReach,
                Url = "http://bloogum.net/guildwars/crystaldesert/skywardreach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.DestinysGorgeOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/destinysgorge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.TheScar,
                Url = "http://bloogum.net/guildwars/crystaldesert/thescar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 25
            },
            new Entry
            {
                Map = Map.ThirstyRiver,
                Url = "http://bloogum.net/guildwars/crystaldesert/thirstyriver/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.TheAridSea,
                Url = "http://bloogum.net/guildwars/crystaldesert/thearidsea/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.VultureDrifts,
                Url = "http://bloogum.net/guildwars/crystaldesert/vulturedrifts/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.DunesOfDespair,
                Url = "http://bloogum.net/guildwars/crystaldesert/dunesofdespair/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.AuguryRockOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/auguryrock/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 25
            },
            new Entry
            {
                Map = Map.TombOfThePrimevalKings,
                Url = "http://bloogum.net/guildwars/crystaldesert/tomboftheprimevalkings/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.TombOfThePrimevalKingsHalloweenOutpost,
                Url = "http://bloogum.net/guildwars/crystaldesert/tomboftheprimevalkings/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 14,
                Count = 4
            },
            new Entry
            {
                Map = Map.TheDragonsLair,
                Url = "http://bloogum.net/guildwars/crystaldesert/thedragonslair/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 60
            },
            new Entry
            {
                Map = Map.TombOfThePrimevalKings,
                Url = "https://cdn.discordapp.com/attachments/279231165045407744/1137832214151843930/gw079.jpg?ex=656cd953&is=655a6453&hm=af57440037882d84e959b950945b738e611d976a2f0beb24d9b1a0fcd626086d&",
                Credit = "https://discordapp.com/users/Soldrand#2252",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location SouthernShiverpeaks = new(
        Region.ShiverpeakMountains,
        [
            new Entry
            {
                Map = Map.DroknarsForgeOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/droknarsforge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.DroknarsForgeWintersdayOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/droknarsforge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 27,
                Count = 9
            },
            new Entry
            {
                Map = Map.DroknarsForgeHalloweenOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/droknarsforge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 37,
                Count = 4
            },
            new Entry
            {
                Map = Map.WitmansFolly,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/witmansfolly/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 48
            },
            new Entry
            {
                Map = Map.PortSledgeOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/portsledge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.TalusChute,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/taluschute/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.IceCavesofSorrow,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/icecavesofsorrow/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },
            new Entry
            {
                Map = Map.CampRankorOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/camprankor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },

            new Entry
            {
                Map = Map.SnakeDance,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/snakedance/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 23
            },
            new Entry
            {
                Map = Map.DreadnoughtsDrift,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/dreadnoughtsdrift/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.LornarsPass,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/lornarspass/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 33
            },
            new Entry
            {
                Map = Map.DeldrimorWarCampOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/deldrimorwarcamp/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.GrenthsFootprint,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/grenthsfootprint/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.SpearheadPeak,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/spearheadpeak/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.TheGraniteCitadelOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/thegranitecitadel/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.TascasDemise,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/tascasdemise/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.MineralSprings,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/mineralsprings/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 41
            },
            new Entry
            {
                Map = Map.Icedome,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/icedome/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.CopperhammerMinesOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/copperhammermines/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.FrozenForest,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/frozenforest/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 49
            },
            new Entry
            {
                Map = Map.IronMinesofMoladune,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/ironminesofmoladune/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.IceFloe,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/icefloe/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.MarhansGrottoOutpost,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/marhansgrotto/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.ThunderheadKeep,
                Url = "http://bloogum.net/guildwars/sshiverpeaks/thunderheadkeep/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 57
            },
            new Entry
            {
                Map = Map.MineralSprings,
                Url = "https://i.imgur.com/CFP4AmT.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TalusChute,
                Url = "https://cdn.discordapp.com/attachments/279231165045407744/1079051027753480232/gw500.jpg?ex=656b4294&is=6558cd94&hm=95afe18ecddc81d5cedc9865e5db76986dc4ee2fe34f4832ffcc35854298f371&",
                Credit = "https://discordapp.com/users/miragee#4827",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TalusChute,
                Url = "https://cdn.discordapp.com/attachments/279231165045407744/1079051028000948345/gw507.jpg?ex=656b4294&is=6558cd94&hm=9e85f4560d8184fbb3a575515a72bdfb1d5812a64b57e486acb9f2a1ce658b4a&",
                Credit = "https://discordapp.com/users/miragee#4827",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
        ]);
    public static readonly Location RingOfFireIslandChain = new(
        Region.RingOfFireIslands,
        [
            new Entry
            {
                Map = Map.EmberLightCampOutpost,
                Url = "http://bloogum.net/guildwars/fireisles/emberlightcamp/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.PerditionRock,
                Url = "http://bloogum.net/guildwars/fireisles/perditionrock/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.RingOfFire,
                Url = "http://bloogum.net/guildwars/fireisles/ringoffire/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.AbaddonsMouth,
                Url = "http://bloogum.net/guildwars/fireisles/abaddonsmouth/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.HellsPrecipice,
                Url = "http://bloogum.net/guildwars/fireisles/hellsprecipice/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 38
            },
            new Entry
            {
                Map = Map.RingOfFire,
                Url = "https://i.imgur.com/srejpIP.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.RingOfFire,
                Url = "https://i.imgur.com/70Oc160.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location FarShiverpeaks = new(
        Region.FarShiverpeaks,
        [
            new Entry
            {
                Map = Map.EmberLightCampOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/borealstation/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.IceCliffChasms,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/icecliffchasms/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 38
            },
            new Entry
            {
                Map = Map.EyeOfTheNorthOutpostWintersdayOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/eyeofthenorth/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.EyeOfTheNorthOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/eyeofthenorth/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 4,
                Count = 3
            },
            new Entry
            {
                Map = Map.HallOfMonuments,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/hallofmonuments/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.GunnarsHoldOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/gunnarshold/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.NorrhartDomains,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/norrhartdomains/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 48
            },
            new Entry
            {
                Map = Map.OlafsteadOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/olafstead/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.VarajarFells,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/varajarfells/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 51
            },
            new Entry
            {
                Map = Map.SifhallaOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/sifhalla/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.DrakkarLake,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/drakkarlake/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },
            new Entry
            {
                Map = Map.JagaMoraine,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/jagamoraine/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.BjoraMarches,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/bjoramarches/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 56
            },
            new Entry
            {
                Map = Map.LongeyesLedgeOutpost,
                Url = "http://bloogum.net/guildwars/fshiverpeaks/longeyesledge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.EyeOfTheNorthOutpostWintersdayOutpost,
                Url = "https://wiki.guildwars.com/images/1/14/Eye_of_the_North_Wintersday_2009.png",
                Credit = "https://wiki.guildwars.com/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location CharrHomelands = new(
        Region.CharrHomelands,
        [
            new Entry
            {
                Map = Map.GrothmarWardowns,
                Url = "http://bloogum.net/guildwars/charr/grothmarwardowns/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 64
            },
            new Entry
            {
                Map = Map.DoomloreShrineOutpost,
                Url = "http://bloogum.net/guildwars/charr/doomloreshrine/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.DaladaUplands,
                Url = "http://bloogum.net/guildwars/charr/daladauplands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 61
            },
            new Entry
            {
                Map = Map.SacnothValley,
                Url = "http://bloogum.net/guildwars/charr/sacnothvalley/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 60
            },
            new Entry
            {
                Map = Map.GrothmarWardowns,
                Url = "https://i.imgur.com/aECgPky.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location TarnishedCoast = new(
        Region.TarnishedCoast,
        [
            new Entry
            {
                Map = Map.VerdantCascades,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/verdantcascades/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.UmbralGrottoOutpost,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/umbralgrotto/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.GaddsEncampmentOutpost,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/gaddsencampment/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.SparkflySwamp,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/sparkflyswamp/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.VloxsFalls,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/vloxsfalls/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.ArborBay,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/arborbay/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.AlcaziaTangle,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/alcaziatangle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 41
            },
            new Entry
            {
                Map = Map.TarnishedHavenOutpost,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/tarnishedhaven/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.RivenEarth,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/rivenearth/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },
            new Entry
            {
                Map = Map.RataSumOutpost,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/ratasum/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.MagusStones,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/magusstones/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.PolymockColiseum,
                Url = "http://bloogum.net/guildwars/tarnishedcoast/polymockcoliseum/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
        ]);
    public static readonly Location DepthsOfTyria = new(
        Region.DepthsOfTyria,
        [
            new Entry
            {
                Map = Map.TheFissureofWoe,
                Url = "http://bloogum.net/guildwars/dungeons/fissureofwoe/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.CatacombsofKathandraxLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/catacombsofkathandrax/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.CatacombsofKathandraxLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/catacombsofkathandrax/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.CatacombsofKathandraxLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/catacombsofkathandrax/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.RragarsMenagerieLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/rragarsmenagerie/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },
            new Entry
            {
                Map = Map.RragarsMenagerieLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/rragarsmenagerie/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },new Entry
            {
                Map = Map.RragarsMenagerieLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/rragarsmenagerie/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 50
            },
            new Entry
            {
                Map = Map.CathedralofFlamesLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/cathedralofflame/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 56
            },
            new Entry
            {
                Map = Map.CathedralofFlamesLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/cathedralofflame/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 56
            },
            new Entry
            {
                Map = Map.CathedralofFlamesLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/cathedralofflame/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 56
            },
            new Entry
            {
                Map = Map.OozePit,
                Url = "http://bloogum.net/guildwars/dungeons/oozepit/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.OozePitMission,
                Url = "http://bloogum.net/guildwars/dungeons/oozepit/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.DarkrimeDelvesLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/darkrimedelves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 58
            },
            new Entry
            {
                Map = Map.DarkrimeDelvesLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/darkrimedelves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 58
            },
            new Entry
            {
                Map = Map.DarkrimeDelvesLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/darkrimedelves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 58
            },
            new Entry
            {
                Map = Map.FrostmawsBurrowsLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/frostmawburrows/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.FrostmawsBurrowsLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/frostmawburrows/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.FrostmawsBurrowsLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/frostmawburrows/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.FrostmawsBurrowsLevel4,
                Url = "http://bloogum.net/guildwars/dungeons/frostmawburrows/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.FrostmawsBurrowsLevel5,
                Url = "http://bloogum.net/guildwars/dungeons/frostmawburrows/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 54
            },
            new Entry
            {
                Map = Map.SepulchreOfDragrimmarLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/sepulchreofdragrimmar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 35
            },
            new Entry
            {
                Map = Map.SepulchreOfDragrimmarLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/sepulchreofdragrimmar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 35
            },
            new Entry
            {
                Map = Map.RavensPointLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/ravenspoint/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 64
            },
            new Entry
            {
                Map = Map.RavensPointLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/ravenspoint/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 64
            },
            new Entry
            {
                Map = Map.RavensPointLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/ravenspoint/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 64
            },
            new Entry
            {
                Map = Map.VloxenExcavationsLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/vloxenexcavations/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.VloxenExcavationsLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/vloxenexcavations/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.VloxenExcavationsLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/vloxenexcavations/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.BogrootGrowthsLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/bogrootgrowths/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.BogrootGrowthsLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/bogrootgrowths/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.BloodstoneCavesLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/bloodstonecaves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.BloodstoneCavesLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/bloodstonecaves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.BloodstoneCavesLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/bloodstonecaves/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.ShardsOfOrrLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/shardsoforr/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.ShardsOfOrrLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/shardsoforr/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.ShardsOfOrrLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/shardsoforr/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.OolasLabLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/oolaslab/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.OolasLabLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/oolaslab/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.OolasLabLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/oolaslab/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.ArachnisHauntLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/arachnishaunt/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.ArachnisHauntLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/arachnishaunt/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.SlaversExileLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/slaversexile/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 59
            },
            new Entry
            {
                Map = Map.SlaversExileLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/slaversexile/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 59
            },
            new Entry
            {
                Map = Map.SlaversExileLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/slaversexile/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 59
            },
            new Entry
            {
                Map = Map.SlaversExileLevel4,
                Url = "http://bloogum.net/guildwars/dungeons/slaversexile/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 59
            },
            new Entry
            {
                Map = Map.SlaversExileLevel5,
                Url = "http://bloogum.net/guildwars/dungeons/slaversexile/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 59
            },
            new Entry
            {
                Map = Map.FronisIrontoesLairMission,
                Url = "http://bloogum.net/guildwars/dungeons/fronisirontoeslair/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.SecretLairOftheSnowmen,
                Url = "http://bloogum.net/guildwars/dungeons/secretlairofthesnowmen/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.SecretLairOftheSnowmen2,
                Url = "http://bloogum.net/guildwars/dungeons/secretlairofthesnowmen/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.SecretLairOftheSnowmen3,
                Url = "http://bloogum.net/guildwars/dungeons/secretlairofthesnowmen/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.HeartOftheShiverpeaksLevel1,
                Url = "http://bloogum.net/guildwars/dungeons/heartoftheshiverpeaks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.HeartOftheShiverpeaksLevel2,
                Url = "http://bloogum.net/guildwars/dungeons/heartoftheshiverpeaks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.HeartOftheShiverpeaksLevel3,
                Url = "http://bloogum.net/guildwars/dungeons/heartoftheshiverpeaks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.RragarsMenagerieLevel1,
                Url = "https://i.imgur.com/LK2d7p4.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.RragarsMenagerieLevel1,
                Url = "https://i.imgur.com/KwjNmsq.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.RragarsMenagerieLevel3,
                Url = "https://i.imgur.com/6NWaY0C.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.HeartOftheShiverpeaksLevel1,
                Url = "https://i.imgur.com/a1XpYMc.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BattledepthsLevel1,
                Url = "https://i.imgur.com/Czaf9Tq.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BattledepthsLevel1,
                Url = "https://i.imgur.com/iuepDVG.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.BattledepthsLevel1,
                Url = "https://i.imgur.com/8SjTjCX.jpeg",
                Credit = "https://imgur.com/gallery/VkhAw",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
        ]);
    public static readonly Location ShingJeaIsland = new(
        Region.ShingJeaIsland,
        [
            new Entry
            {
                Map = Map.MonasteryOverlook1,
                Url = "http://bloogum.net/guildwars/shingjea/monasteryoverlook/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.MonasteryOverlook2,
                Url = "http://bloogum.net/guildwars/shingjea/monasteryoverlook/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.ShingJeaMonasteryOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeamonastery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 9,
                Count = 6
            },
            new Entry
            {
                Map = Map.ShingJeaMonasteryMission,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeamonastery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 9,
                Count = 6
            },
            new Entry
            {
                Map = Map.ShingJeaMonasteryRaidOnShingJeaMonastery,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeamonastery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 9,
                Count = 6
            },
            new Entry
            {
                Map = Map.ShingJeaMonasteryCanthanNewYearOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeamonastery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.ShingJeaMonasteryDragonFestivalOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeamonastery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 5,
                Count = 4
            },
            new Entry
            {
                Map = Map.SunquaVale,
                Url = "http://bloogum.net/guildwars/shingjea/sunquevale/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 40
            },
            new Entry
            {
                Map = Map.TsumeiVillageOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/tsumeivillage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.TsumeiVillageMission,
                Url = "http://bloogum.net/guildwars/shingjea/tsumeivillage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.TsumeiVillageMission2,
                Url = "http://bloogum.net/guildwars/shingjea/tsumeivillage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.TsumeiVillageWindsOfChangeATreatysATreaty,
                Url = "http://bloogum.net/guildwars/shingjea/tsumeivillage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.PanjiangPeninsula,
                Url = "http://bloogum.net/guildwars/shingjea/panjiangpeninsula/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.RanMusuGardensOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/ranmusugardens/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.KinyaProvince,
                Url = "http://bloogum.net/guildwars/shingjea/kinyaprovince/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.MinisterChosEstateExplorable,
                Url = "http://bloogum.net/guildwars/shingjea/ministerchosestate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.MinisterChosEstateMission2,
                Url = "http://bloogum.net/guildwars/shingjea/ministerchosestate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.MinisterChosEstateOutpostMission,
                Url = "http://bloogum.net/guildwars/shingjea/ministerchosestate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.LinnokCourtyard,
                Url = "http://bloogum.net/guildwars/shingjea/linnokcourtyard/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.SaoshangTrail,
                Url = "http://bloogum.net/guildwars/shingjea/saoshangtrail/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.SeitungHarborOutpost,
                Url = "http://bloogum.net/guildwars/shingjea/seitungharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.SeitungHarborAreaDeadlyCargo,
                Url = "http://bloogum.net/guildwars/shingjea/seitungharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.SeitungHarborMission,
                Url = "http://bloogum.net/guildwars/shingjea/seitungharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.SeitungHarborMission2,
                Url = "http://bloogum.net/guildwars/shingjea/seitungharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.JayaBluffs,
                Url = "http://bloogum.net/guildwars/shingjea/jayabluffs/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.JayaBluffsMission,
                Url = "http://bloogum.net/guildwars/shingjea/jayabluffs/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.ZenDaijunExplorable,
                Url = "http://bloogum.net/guildwars/shingjea/zendaijun/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.ZenDaijunOutpostMission,
                Url = "http://bloogum.net/guildwars/shingjea/zendaijun/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.HaijuLagoon,
                Url = "http://bloogum.net/guildwars/shingjea/haijulagoon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 38
            },
            new Entry
            {
                Map = Map.HaijuLagoonMission,
                Url = "http://bloogum.net/guildwars/shingjea/haijulagoon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 38
            },
            new Entry
            {
                Map = Map.ShingJeaArena,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeaarena/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.ShingJeaArenaMission,
                Url = "http://bloogum.net/guildwars/shingjea/shingjeaarena/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.SunquaVale,
                Url = "https://media.discordapp.net/attachments/279231165045407744/859817527739547678/gw010.jpg?ex=657092f9&is=655e1df9&hm=b77dd201214212cfc6dd68db2218f550136878f6a2dca37830cc0dcad4201363&=&format=webp&width=954&height=479",
                Credit = "https://discordapp.com/users/Sara#2170",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location KainengCity = new(
        Region.KainengCity,
        [
            new Entry
            {
                Map = Map.KainengCenterCanthanNewYearOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.KainengCenterCanthanNewYearOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 7,
                Count = 5
            },
            new Entry
            {
                Map = Map.KainengCenterOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 12,
                Count = 18
            },
            new Entry
            {
                Map = Map.KainengCenterSunspearsInCantha,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 12,
                Count = 18
            },
            new Entry
            {
                Map = Map.KainengCenterWindsOfChangeAChanceEncounter,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 12,
                Count = 18
            },
            new Entry
            {
                Map = Map.KainengCenterWindsOfChangeRaidonKainengCenter,
                Url = "http://bloogum.net/guildwars/kaineng/kainengcenter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 12,
                Count = 18
            },
            new Entry
            {
                Map = Map.BejunkanPier,
                Url = "http://bloogum.net/guildwars/kaineng/bejunkanpier/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.BukdekByway,
                Url = "http://bloogum.net/guildwars/kaineng/bukdekbyway/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 28
            },
            new Entry
            {
                Map = Map.BukdekBywayWindsOfChangeCanthaCourierCrisis,
                Url = "http://bloogum.net/guildwars/kaineng/bukdekbyway/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 28
            },
            new Entry
            {
                Map = Map.TheMarketplaceOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/themarketplace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.TheMarketplaceAreaTrackingtheCorruption,
                Url = "http://bloogum.net/guildwars/kaineng/themarketplace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.KainengDocks,
                Url = "http://bloogum.net/guildwars/kaineng/kainengdocks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.WajjunBazaar,
                Url = "http://bloogum.net/guildwars/kaineng/wajjunbazaar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.WajjunBazaarPOX,
                Url = "http://bloogum.net/guildwars/kaineng/wajjunbazaar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.WajjunBazaarWindsOfChangeMinistryOfOppression,
                Url = "http://bloogum.net/guildwars/kaineng/wajjunbazaar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.WajjunBazaarWindsOfChangeViolenceInTheStreets,
                Url = "http://bloogum.net/guildwars/kaineng/wajjunbazaar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.SenjisCornerOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/senjiscorner/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.XaquangSkyway,
                Url = "http://bloogum.net/guildwars/kaineng/xaquangskyway/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.DragonsThroat,
                Url = "http://bloogum.net/guildwars/kaineng/dragonsthroat/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.DragonsThroatAreaWhatWaitsInShadow,
                Url = "http://bloogum.net/guildwars/kaineng/dragonsthroat/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.NahpuiQuarterExplorable,
                Url = "http://bloogum.net/guildwars/kaineng/nahpuiquarter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.NahpuiQuarterOutpostMission,
                Url = "http://bloogum.net/guildwars/kaineng/nahpuiquarter/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.ShadowsPassage,
                Url = "http://bloogum.net/guildwars/kaineng/shadowspassage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.ShadowsPassageWindsofChangeCallingAllThugs,
                Url = "http://bloogum.net/guildwars/kaineng/shadowspassage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.ShenzunTunnels,
                Url = "http://bloogum.net/guildwars/kaineng/shenzuntunnels/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.TahnnakaiTempleExplorable,
                Url = "http://bloogum.net/guildwars/kaineng/tahnnakaitemple/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.TahnnakaiTempleOutpostMission,
                Url = "http://bloogum.net/guildwars/kaineng/tahnnakaitemple/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.TahnnakaiTempleWindsOfChangeTheRescueAttempt,
                Url = "http://bloogum.net/guildwars/kaineng/tahnnakaitemple/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.ZinKuCorridorOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/zinkucorridor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.VizunahSquareMission,
                Url = "http://bloogum.net/guildwars/kaineng/vizunahsquare/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.VizunahSquareForeignQuarterOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/vizunahsquare/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.VizunahSquareLocalQuarterOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/vizunahsquare/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.TheUndercity,
                Url = "http://bloogum.net/guildwars/kaineng/theundercity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.SunjiangDistrictExplorable,
                Url = "http://bloogum.net/guildwars/kaineng/sunjiangdistrict/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.SunjiangDistrictOutpostMission,
                Url = "http://bloogum.net/guildwars/kaineng/sunjiangdistrict/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.MaatuKeepOutpost,
                Url = "http://bloogum.net/guildwars/kaineng/maatukeep/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.PongmeiValley,
                Url = "http://bloogum.net/guildwars/kaineng/pongmeivalley/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.RaisuPavilion,
                Url = "http://bloogum.net/guildwars/kaineng/raisupavilion/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.RaisuPalace,
                Url = "http://bloogum.net/guildwars/kaineng/raisupalace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.RaisuPalaceOutpostMission,
                Url = "http://bloogum.net/guildwars/kaineng/raisupalace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.ImperialSanctumOutpostMission,
                Url = "http://bloogum.net/guildwars/kaineng/imperialsanctum/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.DivinePath,
                Url = "http://bloogum.net/guildwars/kaineng/divinepath/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
        ]);
    public static readonly Location EchovaldForest = new(
        Region.EchovaldForest,
        [
            new Entry
            {
                Map = Map.TanglewoodCopseOutpost,
                Url = "http://bloogum.net/guildwars/echovald/tanglewoodcopse/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.ArborstoneExplorable,
                Url = "http://bloogum.net/guildwars/echovald/arborstone/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.ArborstoneOutpostMission,
                Url = "http://bloogum.net/guildwars/echovald/arborstone/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.AltrummRuins,
                Url = "http://bloogum.net/guildwars/echovald/altrummruins/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.AltrummRuinsFindingJinnai,
                Url = "http://bloogum.net/guildwars/echovald/altrummruins/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.HouseZuHeltzerOutpost,
                Url = "http://bloogum.net/guildwars/echovald/housezuheltzer/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.Ferndale,
                Url = "http://bloogum.net/guildwars/echovald/ferndale/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 25
            },
            new Entry
            {
                Map = Map.AspenwoodGateKurzickOutpost,
                Url = "http://bloogum.net/guildwars/echovald/aspenwoodgate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.FortAspenwoodKurzickOutpost,
                Url = "http://bloogum.net/guildwars/echovald/fortaspenwood/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.FortAspenwoodMission,
                Url = "http://bloogum.net/guildwars/echovald/fortaspenwood/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.SaintAnjekasShrineOutpost,
                Url = "http://bloogum.net/guildwars/echovald/saintanjekasshrine/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.DrazachThicket,
                Url = "http://bloogum.net/guildwars/echovald/drazachthicket/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.LutgardisConservatoryOutpost,
                Url = "http://bloogum.net/guildwars/echovald/lutgardisconservatory/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.BrauerAcademyOutpost,
                Url = "http://bloogum.net/guildwars/echovald/braueracademy/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.MelandrusHope,
                Url = "http://bloogum.net/guildwars/echovald/melandrushope/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 28
            },
            new Entry
            {
                Map = Map.JadeFlatsKurzickOutpost,
                Url = "http://bloogum.net/guildwars/echovald/jadeflats/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheJadeQuarryKurzickOutpost,
                Url = "http://bloogum.net/guildwars/echovald/thejadequarry/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheEternalGrove,
                Url = "http://bloogum.net/guildwars/echovald/theeternalgrove/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.TheEternalGroveOutpostMission,
                Url = "http://bloogum.net/guildwars/echovald/theeternalgrove/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.VasburgArmoryOutpost,
                Url = "http://bloogum.net/guildwars/echovald/vasburgarmory/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.MorostavTrail,
                Url = "http://bloogum.net/guildwars/echovald/morostavtrail/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 17
            },
            new Entry
            {
                Map = Map.DurheimArchivesOutpost,
                Url = "http://bloogum.net/guildwars/echovald/durheimarchives/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.MourningVeilFalls,
                Url = "http://bloogum.net/guildwars/echovald/mourningveilfalls/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.AmatzBasin,
                Url = "http://bloogum.net/guildwars/echovald/amatzbasin/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.UnwakingWatersKurzickOutpost,
                Url = "http://bloogum.net/guildwars/echovald/unwakingwaters/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.UrgozsWarren,
                Url = "http://bloogum.net/guildwars/echovald/urgozswarren/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
        ]);
    public static readonly Location JadeSea = new(
        Region.TheJadeSea,
        [
            new Entry
            {
                Map = Map.BoreasSeabedExplorable,
                Url = "http://bloogum.net/guildwars/jadesea/boreasseabed/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 42
            },
            new Entry
            {
                Map = Map.BoreasSeabedOutpostMission,
                Url = "http://bloogum.net/guildwars/jadesea/boreasseabed/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 42
            },
            new Entry
            {
                Map = Map.ZosShivrosChannel,
                Url = "http://bloogum.net/guildwars/jadesea/zosshivroschannel/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.CavalonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/cavalon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.Archipelagos,
                Url = "http://bloogum.net/guildwars/jadesea/archipelagos/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.BreakerHollowOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/breakerhollow/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.MountQinkai,
                Url = "http://bloogum.net/guildwars/jadesea/mountqinkai/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.AspenwoodGateLuxonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/aspenwoodgate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.FortAspenwoodLuxonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/fortaspenwood/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.JadeFlatsLuxonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/jadeflats/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheJadeQuarryLuxonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/thejadequarry/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.TheJadeQuarryMission,
                Url = "http://bloogum.net/guildwars/jadesea/thejadequarry/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.MaishangHills,
                Url = "http://bloogum.net/guildwars/jadesea/maishanghills/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.BaiPaasuReachOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/baipaasureach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.EredonTerraceOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/eredonterrace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.GyalaHatchery,
                Url = "http://bloogum.net/guildwars/jadesea/gyalahatchery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.GyalaHatcheryOutpostMission,
                Url = "http://bloogum.net/guildwars/jadesea/gyalahatchery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.LeviathanPitsOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/leviathanpits/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.SilentSurf,
                Url = "http://bloogum.net/guildwars/jadesea/silentsurf/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 27
            },
            new Entry
            {
                Map = Map.SeafarersRestOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/seafarersrest/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.RheasCrater,
                Url = "http://bloogum.net/guildwars/jadesea/rheascrater/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 28
            },
            new Entry
            {
                Map = Map.TheAuriosMines,
                Url = "http://bloogum.net/guildwars/jadesea/theauriosmines/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.UnwakingWaters,
                Url = "http://bloogum.net/guildwars/jadesea/unwakingwaters/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.UnwakingWatersLuxonOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/gyalahatchery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.UnwakingWatersMission,
                Url = "http://bloogum.net/guildwars/jadesea/gyalahatchery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.HarvestTempleOutpost,
                Url = "http://bloogum.net/guildwars/jadesea/harvesttemple/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheDeep,
                Url = "http://bloogum.net/guildwars/jadesea/thedeep/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
        ]);
    public static readonly Location Istan = new(
        Region.Istan,
        [
            new Entry
            {
                Map = Map.IslandOfShehkah,
                Url = "http://bloogum.net/guildwars/istan/islandofshehkah/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.ChahbekVillage,
                Url = "http://bloogum.net/guildwars/istan/chahbekvillage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.ChurrhirFields,
                Url = "http://bloogum.net/guildwars/istan/churrhirfields/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.KamadanJewelOfIstanWintersdayOutpost,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.KamadanJewelOfIstanHalloweenOutpost,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 6,
                Count = 5
            },
            new Entry
            {
                Map = Map.KamadanJewelOfIstanOutpost,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 11,
                Count = 6
            },
            new Entry
            {
                Map = Map.KamadanJewelOfIstanExplorable,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 11,
                Count = 6
            },
            new Entry
            {
                Map = Map.KamadanJewelOfIstanCanthanNewYearOutpost,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 11,
                Count = 6
            },
            new Entry
            {
                Map = Map.KamadanMission,
                Url = "http://bloogum.net/guildwars/istan/kamadan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 11,
                Count = 6
            },
            new Entry
            {
                Map = Map.SunDocks,
                Url = "http://bloogum.net/guildwars/istan/sundocks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.SunspearArena,
                Url = "http://bloogum.net/guildwars/istan/sunspeararena/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.SunspearArenaMission,
                Url = "http://bloogum.net/guildwars/istan/sunspeararena/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.PlainsOfJarin,
                Url = "http://bloogum.net/guildwars/istan/plainsofjarin/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 27
            },
            new Entry
            {
                Map = Map.SunspearGreatHallOutpost,
                Url = "http://bloogum.net/guildwars/istan/sunspeargreathall/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheAstralariumOutpost,
                Url = "http://bloogum.net/guildwars/istan/theastralarium/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.ChampionsDawnOutpost,
                Url = "http://bloogum.net/guildwars/istan/championsdawn/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.CliffsOfDohjok,
                Url = "http://bloogum.net/guildwars/istan/cliffsofdohjol/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 26
            },
            new Entry
            {
                Map = Map.ZehlonReach,
                Url = "http://bloogum.net/guildwars/istan/zehlonreach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.JokanurDiggings,
                Url = "http://bloogum.net/guildwars/istan/jokanurdiggings/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.FahranurTheFirstCity,
                Url = "http://bloogum.net/guildwars/istan/fahranurthefirstcity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.FahranurMission,
                Url = "http://bloogum.net/guildwars/istan/fahranurthefirstcity/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.BlacktideDen,
                Url = "http://bloogum.net/guildwars/istan/blacktideden/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.LahtendaBog,
                Url = "http://bloogum.net/guildwars/istan/lahtendabog/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.BeknurHarbor,
                Url = "http://bloogum.net/guildwars/istan/beknurharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.BeknurHarborOutpost,
                Url = "http://bloogum.net/guildwars/istan/beknurharbor/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.IssnurIsles,
                Url = "http://bloogum.net/guildwars/istan/issnurisles/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 35
            },
            new Entry
            {
                Map = Map.KodlonuHamletOutpost,
                Url = "http://bloogum.net/guildwars/istan/kodlonuhamlet/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.MehtaniKeys,
                Url = "http://bloogum.net/guildwars/istan/mehtanikeys/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 32
            },
            new Entry
            {
                Map = Map.Consulate,
                Url = "http://bloogum.net/guildwars/istan/consulate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.ConsulateDocks,
                Url = "http://bloogum.net/guildwars/istan/consulatedocks/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
        ]);
    public static readonly Location Kourna = new(
        Region.Kourna,
        [
            new Entry
            {
                Map = Map.YohlonHavenOutpost,
                Url = "http://bloogum.net/guildwars/kourna/yohlonhaven/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.ArkjokWard,
                Url = "http://bloogum.net/guildwars/kourna/arkjokward/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 49
            },
            new Entry
            {
                Map = Map.SunspearSanctuaryOutpost,
                Url = "http://bloogum.net/guildwars/kourna/sunspearsanctuary/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.CommandPost,
                Url = "http://bloogum.net/guildwars/kourna/commandpost/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.SunwardMarches,
                Url = "http://bloogum.net/guildwars/kourna/sunwardmarches/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 47
            },
            new Entry
            {
                Map = Map.VentaCemetery,
                Url = "http://bloogum.net/guildwars/kourna/ventacemetery/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.MargaCoast,
                Url = "http://bloogum.net/guildwars/kourna/margacoast/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 57
            },
            new Entry
            {
                Map = Map.DajkahInlet,
                Url = "http://bloogum.net/guildwars/kourna/dajkahinlet/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.JahaiBluffs,
                Url = "http://bloogum.net/guildwars/kourna/jahaibluffs/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.KodonurCrossroads,
                Url = "http://bloogum.net/guildwars/kourna/kodonurcrossroads/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.DejarinEstate,
                Url = "http://bloogum.net/guildwars/kourna/dejarinestate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 45
            },
            new Entry
            {
                Map = Map.PogahnPassage,
                Url = "http://bloogum.net/guildwars/kourna/pogahnpassage/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.GandaraTheMoonFortress,
                Url = "http://bloogum.net/guildwars/kourna/gandarathemoonfortress/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.CampHojanuOutpost,
                Url = "http://bloogum.net/guildwars/kourna/camphojanu/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.BarbarousShore,
                Url = "http://bloogum.net/guildwars/kourna/barbarousshore/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 38
            },
            new Entry
            {
                Map = Map.RilohnRefuge,
                Url = "http://bloogum.net/guildwars/kourna/rilohnrefuge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            },
            new Entry
            {
                Map = Map.TheFloodplainOfMahnkelon,
                Url = "http://bloogum.net/guildwars/kourna/thefloodplainofmahnkelon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.ModdokCrevice,
                Url = "http://bloogum.net/guildwars/kourna/moddokcrevice/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.BahdokCaverns,
                Url = "http://bloogum.net/guildwars/kourna/bahdokcaverns/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.WehhanTerracesOutpost,
                Url = "http://bloogum.net/guildwars/kourna/wehhanterraces/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.NunduBay,
                Url = "http://bloogum.net/guildwars/kourna/nundubay/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 37
            },
            new Entry
            {
                Map = Map.TuraisProcession,
                Url = "http://bloogum.net/guildwars/kourna/turaisprocession/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 35
            },
            new Entry
            {
                Map = Map.SunwardMarches,
                Url = "https://i.imgur.com/8pSyjun.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location Vabbi = new(
        Region.Vabbi,
        [
            new Entry
            {
                Map = Map.YatendiCanyons,
                Url = "http://bloogum.net/guildwars/vabbi/yatendicanyons/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.ChantryOfSecretsOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/chantryofsecrets/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.VehtendiValley,
                Url = "http://bloogum.net/guildwars/vabbi/vehtendivalley/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 43
            },
            new Entry
            {
                Map = Map.YahnurMarketOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/yahnurmarket/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.ForumHighlands,
                Url = "http://bloogum.net/guildwars/vabbi/forumhighlands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 52
            },
            new Entry
            {
                Map = Map.TiharkOrchard,
                Url = "http://bloogum.net/guildwars/vabbi/tiharkorchard/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.ResplendentMakuun,
                Url = "http://bloogum.net/guildwars/vabbi/resplendentmakuun/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.ResplendentMakuun2,
                Url = "http://bloogum.net/guildwars/vabbi/resplendentmakuun/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.BokkaAmphitheatre,
                Url = "http://bloogum.net/guildwars/vabbi/bokkaamphitheatre/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.BokkaAmphitheatreNOX,
                Url = "http://bloogum.net/guildwars/vabbi/bokkaamphitheatre/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.HonurHillOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/honurhill/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.TheKodashBazaarOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/thekodashbazaar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.TheMirrorOfLyss,
                Url = "http://bloogum.net/guildwars/vabbi/themirroroflyss/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 33
            },
            new Entry
            {
                Map = Map.TheMirrorOfLyss2,
                Url = "http://bloogum.net/guildwars/vabbi/themirroroflyss/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 33
            },
            new Entry
            {
                Map = Map.DzagonurBastion,
                Url = "http://bloogum.net/guildwars/vabbi/dzagonurbastion/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.WildernessOfBahdza,
                Url = "http://bloogum.net/guildwars/vabbi/wildernessofbahdza/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 32
            },
            new Entry
            {
                Map = Map.DashaVestibule,
                Url = "http://bloogum.net/guildwars/vabbi/dashavestibule/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheHiddenCityOfAhdashim,
                Url = "http://bloogum.net/guildwars/vabbi/thehiddencityofadashim/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 36
            },
            new Entry
            {
                Map = Map.MihanuTownshipOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/mihanutownship/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.HoldingsOfChokhin,
                Url = "http://bloogum.net/guildwars/vabbi/holdingsofchokhin/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 27
            },
            new Entry
            {
                Map = Map.GardenOfSeborhin,
                Url = "http://bloogum.net/guildwars/vabbi/gardenofseborhin/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.GrandCourtOfSebelkeh,
                Url = "http://bloogum.net/guildwars/vabbi/grandcourtofsebelkeh/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.JennursHorde,
                Url = "http://bloogum.net/guildwars/vabbi/jennurshorde/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.VehjinMines,
                Url = "http://bloogum.net/guildwars/vabbi/vehjinmines/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 29
            },
            new Entry
            {
                Map = Map.BasaltGrottoOutpost,
                Url = "http://bloogum.net/guildwars/vabbi/basaltgrotto/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
        ]);
    public static readonly Location TheDesolation = new(
        Region.TheDesolation,
        [
            new Entry
            {
                Map = Map.GateOfDesolation,
                Url = "http://bloogum.net/guildwars/desolation/gateofdesolation/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.TheSulfurousWastes,
                Url = "http://bloogum.net/guildwars/desolation/thesulfurouswastes/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.RemainsOfSahlahja,
                Url = "http://bloogum.net/guildwars/desolation/remainsofsahlahja/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.RemainsOfSahlahja,
                Url = "http://bloogum.net/guildwars/desolation/dynastictombs/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.JokosDomain,
                Url = "http://bloogum.net/guildwars/desolation/jokosdomain/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.TheShatteredRavines,
                Url = "http://bloogum.net/guildwars/desolation/theshatteredravines/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.LairOfTheForgottenOutpost,
                Url = "http://bloogum.net/guildwars/desolation/lairoftheforgotten/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.PoisonedOutcrops,
                Url = "http://bloogum.net/guildwars/desolation/poisonedoutcrops/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 33
            },
            new Entry
            {
                Map = Map.BonePalaceOutpost,
                Url = "http://bloogum.net/guildwars/desolation/bonepalace/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.TheAlkaliPan,
                Url = "http://bloogum.net/guildwars/desolation/thealkalipan/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 24
            },
            new Entry
            {
                Map = Map.CrystalOverlook,
                Url = "http://bloogum.net/guildwars/desolation/crystaloverlook/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 32
            },
            new Entry
            {
                Map = Map.RuinsOfMorah,
                Url = "http://bloogum.net/guildwars/desolation/ruinsofmorah/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.TheRupturedHeart,
                Url = "http://bloogum.net/guildwars/desolation/therupturedheart/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 18
            },
            new Entry
            {
                Map = Map.TheMouthOfTormentOutpost,
                Url = "http://bloogum.net/guildwars/desolation/themouthoftorment/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.TheSulfurousWastes,
                Url = "https://media.discordapp.net/attachments/279231165045407744/1037396920001372240/unknown.png?ex=65709bab&is=655e26ab&hm=8329f7f07e635c9493c12f53054d090908b8643d0932313d49f2b2ff79601a36&=&format=webp",
                Credit = "https://discordapp.com/users/Planewalker#5903",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            }
        ]);
    public static readonly Location GateOfTorment = new(
        Region.RealmOfTorment,
        [
            new Entry
            {
                Map = Map.GateOfTormentOutpost,
                Url = "http://bloogum.net/guildwars/torment/gateoftorment/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.NightfallenJahai,
                Url = "http://bloogum.net/guildwars/torment/nightfallenjahai/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 39
            },
            new Entry
            {
                Map = Map.GateOftheNightfallenLandsOutpost,
                Url = "http://bloogum.net/guildwars/torment/gateofthenightfallenlands/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.NightfallenGarden,
                Url = "http://bloogum.net/guildwars/torment/nightfallengarden/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 31
            },
            new Entry
            {
                Map = Map.GateOfPain,
                Url = "http://bloogum.net/guildwars/torment/gateofpain/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.DomainOfPain,
                Url = "http://bloogum.net/guildwars/torment/domainofpain/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 30
            },
            new Entry
            {
                Map = Map.GateOfFearOutpost,
                Url = "http://bloogum.net/guildwars/torment/gateoffear/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.DomainOfFear,
                Url = "http://bloogum.net/guildwars/torment/domainoffear/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 28
            },
            new Entry
            {
                Map = Map.GateOfSecretsOutpost,
                Url = "http://bloogum.net/guildwars/torment/gateofsecrets/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.DomainOfSecrets,
                Url = "http://bloogum.net/guildwars/torment/domainofsecrets/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 32
            },
            new Entry
            {
                Map = Map.GateOfMadness,
                Url = "http://bloogum.net/guildwars/torment/gateofmadness/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 2
            },
            new Entry
            {
                Map = Map.DepthsOfMadness,
                Url = "http://bloogum.net/guildwars/torment/depthsofmadness/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 46
            },
            new Entry
            {
                Map = Map.HeartOfAbaddon,
                Url = "http://bloogum.net/guildwars/torment/heartofabaddon/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.AbaddonsGate,
                Url = "http://bloogum.net/guildwars/torment/abaddonsgate/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.ThroneOfSecrets,
                Url = "http://bloogum.net/guildwars/torment/throneofsecrets/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 9
            },
            new Entry
            {
                Map = Map.DomainOfAnguish,
                Url = "http://bloogum.net/guildwars/torment/gateofanguish/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 4
            },
            new Entry
            {
                Map = Map.NightfallenJahai,
                Url = "https://i.imgur.com/2tKgyv4.jpeg",
                Credit = "https://imgur.com/a/PzYch4c",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 1
            }
        ]);
    public static readonly Location BattleIsles = new(
        Region.TheBattleIsles,
        [
            new Entry
            {
                Map = Map.GreatTempleOfBalthazarOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/greattempleofbalthazar/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 6
            },
            new Entry
            {
                Map = Map.IsleOfTheNameless,
                Url = "http://bloogum.net/guildwars/battleisles/isleofthenameless/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.IsleOfTheNamelessPvP,
                Url = "http://bloogum.net/guildwars/battleisles/isleofthenameless/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 22
            },
            new Entry
            {
                Map = Map.ZaishenMenagerieGrounds,
                Url = "http://bloogum.net/guildwars/battleisles/zaishenmenageriegrounds/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 34
            },
            new Entry
            {
                Map = Map.ZaishenMenagerieOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/zaishenmenagerie/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 3
            },
            new Entry
            {
                Map = Map.HeroesAscentOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/heroesascent/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.CodexArenaOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/codexarena/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.RandomArenasOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/randomarenas/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.ZaishenChallengeOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/zaishenchallenge/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.ZaishenEliteOutpost,
                Url = "http://bloogum.net/guildwars/battleisles/zaishenelite/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 5
            },
            new Entry
            {
                Map = Map.EmbarkBeach,
                Url = "http://bloogum.net/guildwars/battleisles/embarkbeach/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
        ]);
    public static readonly Location GuildHalls = new(
        Region.TheBattleIsles,
        [
            new Entry
            {
                Map = Map.WarriorsIsle,
                Url = "http://bloogum.net/guildwars/gh/warriorsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.WarriorsIsleMission,
                Url = "http://bloogum.net/guildwars/gh/warriorsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.WarriorsIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/warriorsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 20
            },
            new Entry
            {
                Map = Map.WizardsIsle,
                Url = "http://bloogum.net/guildwars/gh/wizardsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.WizardsIsleMission,
                Url = "http://bloogum.net/guildwars/gh/wizardsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.WizardsIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/wizardsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 21
            },
            new Entry
            {
                Map = Map.IsleOfTheDeadGuildHall,
                Url = "http://bloogum.net/guildwars/gh/isleofthedead/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.IsleOfTheDeadGuildHallMission,
                Url = "http://bloogum.net/guildwars/gh/isleofthedead/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.IsleOfTheDeadGuildHallOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofthedead/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 10
            },
            new Entry
            {
                Map = Map.FrozenIsle,
                Url = "http://bloogum.net/guildwars/gh/frozenisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.FrozenIsleMission,
                Url = "http://bloogum.net/guildwars/gh/frozenisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.FrozenIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/frozenisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.HuntersIsle,
                Url = "http://bloogum.net/guildwars/gh/huntersisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.HuntersIsleMission,
                Url = "http://bloogum.net/guildwars/gh/huntersisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.HuntersIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/huntersisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.DruidsIsle,
                Url = "http://bloogum.net/guildwars/gh/druidsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.DruidsIsleMission,
                Url = "http://bloogum.net/guildwars/gh/druidsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.DruidsIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/druidsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 7
            },
            new Entry
            {
                Map = Map.NomadsIsle,
                Url = "http://bloogum.net/guildwars/gh/nomadsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.NomadsIsleMission,
                Url = "http://bloogum.net/guildwars/gh/nomadsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.NomadsIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/nomadsisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.BurningIsle,
                Url = "http://bloogum.net/guildwars/gh/burningisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.BurningIsleMission,
                Url = "http://bloogum.net/guildwars/gh/burningisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.BurningIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/burningisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 15
            },
            new Entry
            {
                Map = Map.IsleOfMeditation,
                Url = "http://bloogum.net/guildwars/gh/isleofmeditation/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.IsleOfMeditationMission,
                Url = "http://bloogum.net/guildwars/gh/isleofmeditation/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.IsleOfMeditationOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofmeditation/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 16
            },
            new Entry
            {
                Map = Map.IsleOfJade,
                Url = "http://bloogum.net/guildwars/gh/isleofjade/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.IsleOfJadeMission,
                Url = "http://bloogum.net/guildwars/gh/isleofjade/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.IsleOfJadeOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofjade/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.IsleOfWeepingStone,
                Url = "http://bloogum.net/guildwars/gh/isleofweepingstone/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 17
            },
            new Entry
            {
                Map = Map.IsleOfWeepingStoneMission,
                Url = "http://bloogum.net/guildwars/gh/isleofweepingstone/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 17
            },
            new Entry
            {
                Map = Map.IsleOfWeepingStoneOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofweepingstone/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 17
            },
            new Entry
            {
                Map = Map.ImperialIsle,
                Url = "http://bloogum.net/guildwars/gh/imperialisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.ImperialIsleMission,
                Url = "http://bloogum.net/guildwars/gh/imperialisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.ImperialIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/imperialisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 13
            },
            new Entry
            {
                Map = Map.UnchartedIsle,
                Url = "http://bloogum.net/guildwars/gh/unchartedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.UnchartedIsleMission,
                Url = "http://bloogum.net/guildwars/gh/unchartedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.UnchartedIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/unchartedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 14
            },
            new Entry
            {
                Map = Map.CorruptedIsle,
                Url = "http://bloogum.net/guildwars/gh/corruptedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.CorruptedIsleMission,
                Url = "http://bloogum.net/guildwars/gh/corruptedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.CorruptedIsleOutpost,
                Url = "http://bloogum.net/guildwars/gh/corruptedisle/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 8
            },
            new Entry
            {
                Map = Map.IsleOfSolitude,
                Url = "http://bloogum.net/guildwars/gh/isleofsolitude/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.IsleOfSolitudeMission,
                Url = "http://bloogum.net/guildwars/gh/isleofsolitude/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.IsleOfSolitudeOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofsolitude/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 11
            },
            new Entry
            {
                Map = Map.IsleOfWurms,
                Url = "http://bloogum.net/guildwars/gh/isleofwurms/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.IsleOfWurmsMission,
                Url = "http://bloogum.net/guildwars/gh/isleofwurms/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            },
            new Entry
            {
                Map = Map.IsleOfWurmsOutpost,
                Url = "http://bloogum.net/guildwars/gh/isleofwurms/{ID}.jpg",
                Credit = "http://bloogum.net/guildwars/",
                IdFormat = "D2",
                StartIndex = 1,
                Count = 12
            }
        ]);

    public static List<Location> Locations { get; } =
    [
        AscalonPreSearing,
        Ascalon,
        NorthernShiverpeaks,
        Kryta,
        MaguumaJungle,
        CrystalDesert,
        SouthernShiverpeaks,
        RingOfFireIslandChain,
        FarShiverpeaks,
        CharrHomelands,
        TarnishedCoast,
        DepthsOfTyria,
        ShingJeaIsland,
        KainengCity,
        EchovaldForest,
        JadeSea,
        Istan,
        Kourna,
        Vabbi,
        TheDesolation,
        GateOfTorment,
        BattleIsles,
        GuildHalls
    ];

    public Region Region { get; }
    public List<Entry> Entries { get; } = [];
    internal Location(Region region, List<Entry> entries)
    {
        this.Region = region;
        this.Entries = entries;
    }
}
