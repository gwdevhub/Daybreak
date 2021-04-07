using System.Collections.Generic;

namespace Daybreak.Services.Bloogum.Models
{
    public sealed class Location
    {
        public static readonly Location AscalonPreSearing = new(
            "pre",
            new List<Category>
            {
                    new Category("openingcutscene", 5),
                    new Category("ascaloncity", 3),
                    new Category("lakesidecounty", 15),
                    new Category("ashfordabbey", 1),
                    new Category("thecatacombs", 13),
                    new Category("greenhillscounty", 14),
                    new Category("thebarradinestate", 3),
                    new Category("wizardsfolly", 20),
                    new Category("regentvalley", 31),
                    new Category("fortranik", 1),
                    new Category("thenorthlands", 18),
                    new Category("ascalonacademy", 10)
            });
        public static readonly Location Ascalon = new(
            "ascalon",
            new List<Category>
            {
                    new Category("ascaloncity", 16),
                    new Category("oldascalon", 13),
                    new Category("regentvalley", 9),
                    new Category("pockmarkflats", 12),
                    new Category("easternfrontier", 6),
                    new Category("thebreach", 6),
                    new Category("diessalowlands", 5),
                    new Category("dragonsgullet", 4),
                    new Category("ascalonfoothills", 2),
                    new Category("thegreatnorthernwall", 6),
                    new Category("fortranik", 11),
                    new Category("ruinsofsurmia", 8),
                    new Category("nolaniacademy", 18)
            });
        public static readonly Location NorthernShiverpeaks = new(
            "nshiverpeaks",
            new List<Category>
            {
                    new Category("travelersvale", 20),
                    new Category("yaksbend", 7),
                    new Category("borlispass", 37),
                    new Category("ironhorsemine", 18),
                    new Category("thefrostgate", 18),
                    new Category("anvilrock", 21),
                    new Category("icetoothcave", 2),
                    new Category("deldrimorbowl", 26),
                    new Category("beaconsperch", 8),
                    new Category("griffonsmouth", 7)
            });
        public static readonly Location Kryta = new(
            "kryta",
            new List<Category>
            {
                    new Category("scoundrelsrise", 5),
                    new Category("gatesofkryta", 29),
                    new Category("lionsgate", 2),
                    new Category("lionsarch", 44),
                    new Category("lionsarchkeep", 9),
                    new Category("northkrytaprovince", 42),
                    new Category("dallesioseaboard", 39),
                    new Category("neboterrace", 15),
                    new Category("bergenhotsprings", 8),
                    new Category("cursedlands", 1),
                    new Category("beetletun", 6),
                    new Category("watchtowercoast", 14),
                    new Category("divinitycoast", 34),
                    new Category("templeofages", 5),
                    new Category("theblackcurtain", 25),
                    new Category("kessexpeak", 11),
                    new Category("talmarkwilderness", 30),
                    new Category("majestysrest", 14),
                    new Category("tearsofthefallen", 14),
                    new Category("twinserpentlakes", 16),
                    new Category("stingraystrand", 15),
                    new Category("fishermenshaven", 4),
                    new Category("riversideprovince", 31),
                    new Category("sanctumcay", 21)
            });
        public static readonly Location MaguumaJungle = new(
            "maguuma",
            new List<Category>
            {
                    new Category("majestysrest", 14),
                    new Category("druidsoverlook", 1),
                    new Category("sagelands", 27),
                    new Category("thewilds", 19),
                    new Category("mamnoonlagoon", 17),
                    new Category("quarrelfalls", 2),
                    new Category("silverwood", 29),
                    new Category("bloodstonefen", 24),
                    new Category("ettinsback", 21),
                    new Category("ventarisrefuge", 1),
                    new Category("reedbog", 12),
                    new Category("thefalls", 22),
                    new Category("drytop", 5),
                    new Category("tangleroot", 20),
                    new Category("maguumastade", 1),
                    new Category("auroraglade", 12),
                    new Category("hengeofdenravi", 12)
            });
        public static readonly Location CrystalDesert = new(
            "crystaldesert",
            new List<Category>
            {
                    new Category("amnoonoasis", 6),
                    new Category("prophetspath", 24),
                    new Category("heroesaudience", 3),
                    new Category("seaflats", 22),
                    new Category("seekerspassage", 2),
                    new Category("divinersascent", 21),
                    new Category("elonareach", 23),
                    new Category("skywardreach", 34),
                    new Category("destinysgorge", 4),
                    new Category("thescar", 25),
                    new Category("thirstyriver", 24),
                    new Category("thearidsea", 40),
                    new Category("vulturedrifts", 40),
                    new Category("dunesofdespair", 36),
                    new Category("auguryrock", 25),
                    new Category("tomboftheprimevalkings", 17),
                    new Category("thedragonslair", 60)
            });
        public static readonly Location SouthernShiverpeaks = new(
            "sshiverpeaks",
            new List<Category>
            {
                    new Category("droknarsforge", 40),
                    new Category("witmansfolly", 48),
                    new Category("portsledge", 7),
                    new Category("taluschute", 54),
                    new Category("icecavesofsorrow", 50),
                    new Category("camprankor", 2),
                    new Category("snakedance", 23),
                    new Category("dreadnoughtsdrift", 6),
                    new Category("lornarspass", 33),
                    new Category("deldrimorwarcamp", 3),
                    new Category("grenthsfootprint", 26),
                    new Category("spearheadpeak", 37),
                    new Category("thegranitecitadel", 11),
                    new Category("tascasdemise", 11),
                    new Category("mineralsprings", 41),
                    new Category("icedome", 10),
                    new Category("copperhammermines", 2),
                    new Category("frozenforest", 49),
                    new Category("ironminesofmoladune", 46),
                    new Category("icefloe", 54),
                    new Category("marhansgrotto", 3),
                    new Category("thunderheadkeep", 57)
            });
        public static readonly Location RingOfFireIslandChain = new(
            "fireisles",
            new List<Category>
            {
                    new Category("emberlightcamp", 3),
                    new Category("perditionrock", 37),
                    new Category("ringoffire", 36),
                    new Category("abaddonsmouth", 47),
                    new Category("hellsprecipice", 38)
            });
        public static readonly Location FarShiverpeaks = new(
            "fshiverpeaks",
            new List<Category>
            {
                    new Category("borealstation", 2),
                    new Category("icecliffchasms", 38),
                    new Category("eyeofthenorth", 6),
                    new Category("hallofmonuments", 2),
                    new Category("gunnarshold", 9),
                    new Category("norrhartdomains", 48),
                    new Category("olafstead", 3),
                    new Category("varajarfells", 51),
                    new Category("sifhalla", 8),
                    new Category("drakkarlake", 50),
                    new Category("jagamoraine", 47),
                    new Category("bjoramarches", 56),
                    new Category("longeyesledge", 4)
            });
        public static readonly Location CharrHomelands = new(
            "charr",
            new List<Category>
            {
                    new Category("grothmarwardowns", 64),
                    new Category("doomloreshrine", 5),
                    new Category("daladauplands", 61),
                    new Category("sacnothvalley", 60)
            });
        public static readonly Location TarnishedCoast = new(
            "tarnishedcoast",
            new List<Category>
            {
                    new Category("verdantcascades", 52),
                    new Category("umbralgrotto", 1),
                    new Category("gaddsencampment", 6),
                    new Category("sparkflyswamp", 37),
                    new Category("vloxsfalls", 4),
                    new Category("arborbay", 52),
                    new Category("alcaziatangle", 41),
                    new Category("tarnishedhaven", 3),
                    new Category("rivenearth", 50),
                    new Category("ratasum", 6),
                    new Category("magusstones", 37),
                    new Category("polymockcoliseum", 4)
            });
        public static readonly Location DepthsOfTyria = new(
            "dungeons",
            new List<Category>
            {
                    new Category("fissureofwoe", 14),
                    new Category("catacombsofkathandrax", 47),
                    new Category("rragarsmenagerie", 50),
                    new Category("cathedralofflame", 56),
                    new Category("oozepit", 52),
                    new Category("darkrimedelves", 58),
                    new Category("frostmawsburrows", 54),
                    new Category("sepulchreofdragrimmar", 35),
                    new Category("ravenspoint", 64),
                    new Category("vloxenexcavations", 52),
                    new Category("bogrootgrowths", 47),
                    new Category("bloodstonecaves", 20),
                    new Category("shardsoforr", 40),
                    new Category("oolaslab", 46),
                    new Category("arachnishaunt", 34),
                    new Category("slaversexile", 59),
                    new Category("fronisirontoeslair", 11),
                    new Category("secretlairofthesnowmen", 16),
                    new Category("heartoftheshiverpeaks", 40)
            });
        public static readonly Location ShingJeaIsland = new(
            "shingjea",
            new List<Category>
            {
                    new Category("monasteryoverlook", 12),
                    new Category("shingjeamonastery", 15),
                    new Category("sunquavale", 40),
                    new Category("tsumeivillage", 3),
                    new Category("panjiangpeninsula", 47),
                    new Category("ranmusugardens", 4),
                    new Category("kinyaprovince", 29),
                    new Category("ministerchosestate", 26),
                    new Category("linnokcourtyard", 1),
                    new Category("saoshangtrail", 3),
                    new Category("seitungharbor", 8),
                    new Category("jayabluffs", 24),
                    new Category("zendaijun", 24),
                    new Category("haijulagoon", 38),
                    new Category("shingjeaarena", 2)
            });
        public static readonly Location KainengCity = new(
            "kaineng",
            new List<Category>
            {
                    new Category("kainengcenter", 30),
                    new Category("bejunkanpier", 3),
                    new Category("bukdekbyway", 28),
                    new Category("themarketplace", 5),
                    new Category("kainengdocks", 5),
                    new Category("wajjunbazaar", 30),
                    new Category("senjiscorner", 1),
                    new Category("xaquangskyway", 10),
                    new Category("dragonsthroat", 3),
                    new Category("nahpuiquarter", 22),
                    new Category("shadowspassage", 2),
                    new Category("shenzuntunnels", 13),
                    new Category("tahnnakaitemple", 13),
                    new Category("zinkucorridor", 5),
                    new Category("vizunahsquare", 12),
                    new Category("theundercity", 2),
                    new Category("sunjiangdistrict", 8),
                    new Category("maatukeep", 1),
                    new Category("pongmeivalley", 13),
                    new Category("raisupavilion", 5),
                    new Category("raisupalace", 37),
                    new Category("imperialsanctum", 9),
                    new Category("divinepath", 5)
            });
        public static readonly Location EchovaldForest = new(
            "echovald",
            new List<Category>
            {
                    new Category("tanglewoodcopse", 2),
                    new Category("arborstone", 37),
                    new Category("altrummruins", 9),
                    new Category("housezuheltzer", 6),
                    new Category("ferndale", 25),
                    new Category("aspenwoodgate", 1),
                    new Category("fortaspenwood", 11),
                    new Category("saintanjekasshrine", 1),
                    new Category("drazachthicket", 24),
                    new Category("lutgardisconservatory", 2),
                    new Category("braueracademy", 3),
                    new Category("melandrushope", 28),
                    new Category("jadeflats", 2),
                    new Category("thejadequarry", 2),
                    new Category("theeternalgrove", 31),
                    new Category("vasburgarmory", 4),
                    new Category("morostavtrail", 17),
                    new Category("durheimarchives", 3),
                    new Category("mourningveilfalls", 31),
                    new Category("amatzbasin", 13),
                    new Category("unwakingwaters", 4),
                    new Category("urgozswarren", 13)
            });
        public static readonly Location JadeSea = new(
            "jadesea",
            new List<Category>
            {
                    new Category("boreasseabed", 42),
                    new Category("zosshivroschannel", 9),
                    new Category("cavalon", 7),
                    new Category("archipelagos", 29),
                    new Category("breakerhollow", 1),
                    new Category("mountqinkai", 24),
                    new Category("aspenwoodgate", 2),
                    new Category("fortaspenwood", 3),
                    new Category("jadeflats", 1),
                    new Category("thejadequarry", 8),
                    new Category("maishanghills", 26),
                    new Category("baipaasureach", 1),
                    new Category("eredonterrace", 4),
                    new Category("gyalahatchery", 36),
                    new Category("leviathanpits", 3),
                    new Category("silentsurf", 27),
                    new Category("seafarersrest", 2),
                    new Category("rheascrater", 28),
                    new Category("theauriosmines", 10),
                    new Category("unwakingwaters", 14),
                    new Category("harvesttemple", 2),
                    new Category("thedeep", 18)
            });
        public static readonly Location Istan = new(
            "istan",
            new List<Category>
            {
                    new Category("islandofshehkah", 12),
                    new Category("chahbekvillage", 1),
                    new Category("churrhirfields", 6),
                    new Category("kamadan", 16),
                    new Category("sundocks", 2),
                    new Category("sunspeararena", 1),
                    new Category("plainsofjarin", 27),
                    new Category("sunspeargreathall", 1),
                    new Category("theastralarium", 7),
                    new Category("championsdawn", 2),
                    new Category("cliffsofdohjok", 26),
                    new Category("zehlonreach", 46),
                    new Category("jokanurdiggings", 3),
                    new Category("fahranurthefirstcity", 29),
                    new Category("blacktideden", 3),
                    new Category("lahtendabog", 24),
                    new Category("beknurharbor", 3),
                    new Category("issnurisles", 35),
                    new Category("kodlonuhamlet", 9),
                    new Category("mehtanikeys", 32),
                    new Category("consulate", 1),
                    new Category("consulatedocks", 2)
            });
        public static readonly Location Kourna = new(
            "kourna",
            new List<Category>
            {
                    new Category("yohlonhaven", 4),
                    new Category("arkjokward", 49),
                    new Category("sunspearsanctuary", 8),
                    new Category("commandpost", 4),
                    new Category("sunwardmarches", 47),
                    new Category("ventacemetery", 2),
                    new Category("margacoast", 57),
                    new Category("dajkahinlet", 5),
                    new Category("jahaibluffs", 37),
                    new Category("kodonurcrossroads", 3),
                    new Category("dejarinestate", 45),
                    new Category("pogahnpassage", 2),
                    new Category("gandarathemoonfortress", 36),
                    new Category("camphojanu", 3),
                    new Category("barbarousshore", 38),
                    new Category("rilohnrefuge", 1),
                    new Category("thefloodplainofmahnkelon", 20),
                    new Category("moddokcrevice", 3),
                    new Category("bahdokcaverns", 3),
                    new Category("wehhanterraces", 6),
                    new Category("nundubay", 37),
                    new Category("turaisprocession", 35)
            });
        public static readonly Location Vabbi = new(
            "vabbi",
            new List<Category>
            {
                    new Category("yatendicanyons", 24),
                    new Category("chantryofsecrets", 4),
                    new Category("vehtendivalley", 43),
                    new Category("yahnurmarket", 6),
                    new Category("forumhighlands", 52),
                    new Category("tiharkorchard", 6),
                    new Category("resplendentmakuun", 46),
                    new Category("bokkaamphitheatre", 5),
                    new Category("honurhill", 3),
                    new Category("thekodashbazaar", 18),
                    new Category("themirroroflyss", 33),
                    new Category("dzagonurbastion", 4),
                    new Category("wildernessofbahdza", 32),
                    new Category("dashavestibule", 2),
                    new Category("thehiddencityofadashim", 36),
                    new Category("mihanutownship", 3),
                    new Category("holdingsofchokhin", 27),
                    new Category("gardenofseborhin", 34),
                    new Category("grandcourtofsebelkeh", 9),
                    new Category("jennurshorde", 4),
                    new Category("vehjinmines", 29),
                    new Category("basaltgrotto", 2)
            });
        public static readonly Location TheDesolation = new(
            "desolation",
            new List<Category>
            {
                    new Category("gateofdesolation", 6),
                    new Category("thesulfurouswastes", 30),
                    new Category("remainsofsahlahja", 4),
                    new Category("dynastictombs", 4),
                    new Category("jokosdomain", 34),
                    new Category("theshatteredravines", 31),
                    new Category("lairoftheforgotten", 6),
                    new Category("poisonedoutcrops", 33),
                    new Category("bonepalace", 4),
                    new Category("thealkalipan", 24),
                    new Category("crystaloverlook", 32),
                    new Category("ruinsofmorah", 2),
                    new Category("therupturedheart", 18),
                    new Category("themouthoftorment", 5)
            });
        public static readonly Location GateOfTorment = new(
            "torment",
            new List<Category>
            {
                    new Category("gateoftorment", 15),
                    new Category("nightfallenjahai", 39),
                    new Category("gateofthenightfallenlands", 4),
                    new Category("nightfallengarden", 31),
                    new Category("gateofpain", 4),
                    new Category("domainofpain", 30),
                    new Category("gateoffear", 3),
                    new Category("domainoffear", 28),
                    new Category("gateofsecrets", 6),
                    new Category("domainofsecrets", 32),
                    new Category("gateofmadness", 2),
                    new Category("depthsofmadness", 46),
                    new Category("heartofabaddon", 11),
                    new Category("abaddonsgate", 10),
                    new Category("throneofsecrets", 9),
                    new Category("gateofanguish", 4)
            });
        public static readonly Location BattleIsles = new(
            "battleisles",
            new List<Category>
            {
                    new Category("greattempleofbalthazar", 6),
                    new Category("isleofthenameless", 22),
                    new Category("zaishenmenagerie", 3),
                    new Category("zaishenmenageriegrounds", 34),
                    new Category("heroesascent", 7),
                    new Category("codexarena", 7),
                    new Category("randomarenas", 8),
                    new Category("zaishenchallenge", 10),
                    new Category("zaishenelite", 5),
                    new Category("embarkbeach", 12)
            });
        public static readonly Location GuildHalls = new(
            "gh",
            new List<Category>
            {
                    new Category("warriorsisle", 20),
                    new Category("wizardsisle", 21),
                    new Category("isleofthedead", 10),
                    new Category("frozenisle", 14),
                    new Category("huntersisle", 14),
                    new Category("druidsisle", 7),
                    new Category("nomadsisle", 12),
                    new Category("burningisle", 15),
                    new Category("isleofmeditation", 16),
                    new Category("isleofjade", 8),
                    new Category("isleofweepingstone", 17),
                    new Category("imperialisle", 13),
                    new Category("uncharteredisle", 14),
                    new Category("corruptedisle", 8),
                    new Category("isleofsolitude", 11),
                    new Category("isleofwurms", 12)
            });

        public static List<Location> Locations { get; } = new List<Location>
        {
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
        };

        public string LocationName { get; }
        public List<Category> Categories { get; } = new();
        private Location(string locationName, List<Category> categories)
        {
            this.LocationName = locationName;
            this.Categories = categories;
        }
    }
}
