namespace Daybreak.Shared.Models.Guildwars;

public sealed class Material : ItemBase, IWikiEntity
{
    public static readonly Material Bone = new() { Id = 921, Name = "Bone", Multiple = "Bones", WikiUrl = "https://wiki.guildwars.com/wiki/Bone" };
    public static readonly Material BoltOfCloth = new() { Id = 925, Name = "Bolt of Cloth", Multiple = "Bolts of Cloth", WikiUrl = "https://wiki.guildwars.com/wiki/Bolt_of_Cloth" };
    public static readonly Material PileOfGlitteringDust = new() { Id = 929, Name = "Pile of Glittering Dust", Multiple = "Piles of Glittering Dust", WikiUrl = "https://wiki.guildwars.com/wiki/Pile_of_Glittering_Dust" };
    public static readonly Material Feather = new() { Id = 933, Name = "Feather", Multiple = "Feathers", WikiUrl = "https://wiki.guildwars.com/wiki/Feather" };
    public static readonly Material PlantFiber = new() { Id = 934, Name = "Plant Fiber", Multiple = "Plant Fibers", WikiUrl = "https://wiki.guildwars.com/wiki/Plant_Fiber" };
    public static readonly Material TannedHideSquare = new() { Id = 940, Name = "Tanned Hide Square", Multiple = "Tanned Hide Squares", WikiUrl = "https://wiki.guildwars.com/wiki/Tanned_Hide_Square" };
    public static readonly Material WoodPlank = new() { Id = 946, Name = "Wood Plank", Multiple = "Wood Planks", WikiUrl = "https://wiki.guildwars.com/wiki/Wood_Plank" };
    public static readonly Material IronIngot = new() { Id = 948, Name = "Iron Ingot", Multiple = "Iron Ingots", WikiUrl = "https://wiki.guildwars.com/wiki/Iron_Ingot" };
    public static readonly Material Scale = new() { Id = 953, Name = "Scale", Multiple = "Scales", WikiUrl = "https://wiki.guildwars.com/wiki/Scale" };
    public static readonly Material ChitinFragment = new() { Id = 954, Name = "Chitin Fragment", Multiple = "Chitin Fragments", WikiUrl = "https://wiki.guildwars.com/wiki/Chitin_Fragment" };
    public static readonly Material GraniteSlab = new() { Id = 955, Name = "Granite Slab", Multiple = "Granite Slabs", WikiUrl = "https://wiki.guildwars.com/wiki/Granite_Slab" };

    public static readonly Material AmberChunk = new() { Id = 6532, Name = "Amber Chunk", Multiple = "Amber Chunks", WikiUrl = "https://wiki.guildwars.com/wiki/Amber_Chunk" };
    public static readonly Material BoltOfDamask = new() { Id = 927, Name = "Bolt of Damask", Multiple = "Bolts of Damask", WikiUrl = "https://wiki.guildwars.com/wiki/Bolt_of_Damask" };
    public static readonly Material BoltOfLinen = new() { Id = 926, Name = "Bolt of Linen", Multiple = "Bolts of Linen", WikiUrl = "https://wiki.guildwars.com/wiki/Bolt_of_Linen" };
    public static readonly Material BoltOfSilk = new() { Id = 928, Name = "Bolt of Silk", Multiple = "Bolts of Silk", WikiUrl = "https://wiki.guildwars.com/wiki/Bolt_of_Silk" };
    public static readonly Material DeldrimorSteelIngot = new() { Id = 950, Name = "Deldrimor Steel Ingot", Multiple = "Deldrimor Steel Ingots", WikiUrl = "https://wiki.guildwars.com/wiki/Deldrimor_Steel_Ingot" };
    public static readonly Material Diamond = new() { Id = 935, Name = "Diamond", Multiple = "Diamonds", WikiUrl = "https://wiki.guildwars.com/wiki/Diamond" };
    public static readonly Material ElonianLeatherSquare = new() { Id = 943, Name = "Elonian Leather Square", Multiple = "Elonian Leather Squares", WikiUrl = "https://wiki.guildwars.com/wiki/Elonian_Leather_Square" };
    public static readonly Material FurSquare = new() { Id = 941, Name = "Fur Square", Multiple = "Fur Squares", WikiUrl = "https://wiki.guildwars.com/wiki/Fur_Square" };
    public static readonly Material GlobOfEctoplasm = new() { Id = 930, Name = "Glob of Ectoplasm", Multiple = "Globs of Ectoplasm", WikiUrl = "https://wiki.guildwars.com/wiki/Glob_of_Ectoplasm" };
    public static readonly Material JadeiteShard = new() { Id = 6533, Name = "Jadeite Shard", Multiple = "Jadeite Shards", WikiUrl = "https://wiki.guildwars.com/wiki/Jadeite_Shard" };
    public static readonly Material LeatherSquare = new() { Id = 942, Name = "Leather Square", Multiple = "Leather Squares", WikiUrl = "https://wiki.guildwars.com/wiki/Leather_Square" };
    public static readonly Material LumpOfCharcoal = new() { Id = 922, Name = "Lump of Charcoal", Multiple = "Lumps of Charcoal", WikiUrl = "https://wiki.guildwars.com/wiki/Lump_of_Charcoal" };
    public static readonly Material MonstrousClaw = new() { Id = 923, Name = "Monstrous Claw", Multiple = "Monstrous Claws", WikiUrl = "https://wiki.guildwars.com/wiki/Monstrous_Claw" };
    public static readonly Material MonstrousEye = new() { Id = 931, Name = "Monstrous Eye", Multiple = "Monstrous Eyes", WikiUrl = "https://wiki.guildwars.com/wiki/Monstrous_Eye" };
    public static readonly Material MonstrousFang = new() { Id = 932, Name = "Monstrous Fang", Multiple = "Monstrous Fangs", WikiUrl = "https://wiki.guildwars.com/wiki/Monstrous_Fang" };
    public static readonly Material ObsidianShard = new() { Id = 945, Name = "Obsidian Shard", Multiple = "Obsidian Shards", WikiUrl = "https://wiki.guildwars.com/wiki/Obsidian_Shard" };
    public static readonly Material OnyxGemstone = new() { Id = 936, Name = "Onyx Gemstone", Multiple = "Onyx Gemstones", WikiUrl = "https://wiki.guildwars.com/wiki/Onyx_Gemstone" };
    public static readonly Material RollOfParchment = new() { Id = 951, Name = "Roll of Parchment", Multiple = "Rolls of Parchment", WikiUrl = "https://wiki.guildwars.com/wiki/Roll_of_Parchment" };
    public static readonly Material RollOfVellum = new() { Id = 952, Name = "Roll of Vellum", Multiple = "Rolls of Velum", WikiUrl = "https://wiki.guildwars.com/wiki/Roll_of_Vellum" };
    public static readonly Material Ruby = new() { Id = 937, Name = "Ruby", Multiple = "Rubies", WikiUrl = "https://wiki.guildwars.com/wiki/Ruby" };
    public static readonly Material Sapphire = new() { Id = 938, Name = "Sapphire", Multiple = "Sapphires", WikiUrl = "https://wiki.guildwars.com/wiki/Sapphire" };
    public static readonly Material SpiritwoodPlank = new() { Id = 956, Name = "Spiritwood Plank", Multiple = "Spiritwood Planks", WikiUrl = "https://wiki.guildwars.com/wiki/Spiritwood_Plank" };
    public static readonly Material SteelIngot = new() { Id = 949, Name = "Steel Ingot", Multiple = "Steel Ingots", WikiUrl = "https://wiki.guildwars.com/wiki/Steel_Ingot" };
    public static readonly Material TemperedGlassVial = new() { Id = 939, Name = "Tempered Glass Vial", Multiple = "Tempered Glass Vials", WikiUrl = "https://wiki.guildwars.com/wiki/Tempered_Glass_Vial" };
    public static readonly Material VialOfInk = new() { Id = 944, Name = "Vial of Ink", Multiple = "Vials of Ink", WikiUrl = "https://wiki.guildwars.com/wiki/Vial_of_Ink" };

    public static IReadOnlyList<Material> Common { get; } =
    [
        Bone,
        BoltOfCloth,
        PileOfGlitteringDust,
        Feather,
        PlantFiber,
        TannedHideSquare,
        WoodPlank,
        IronIngot,
        Scale,
        ChitinFragment,
        GraniteSlab
    ];
    public static IReadOnlyList<Material> Rare { get; } =
    [
        AmberChunk,
        BoltOfDamask,
        BoltOfLinen,
        BoltOfSilk,
        DeldrimorSteelIngot,
        Diamond,
        ElonianLeatherSquare,
        FurSquare,
        GlobOfEctoplasm,
        JadeiteShard,
        LeatherSquare,
        LumpOfCharcoal,
        MonstrousClaw,
        MonstrousEye,
        MonstrousFang,
        ObsidianShard,
        OnyxGemstone,
        RollOfParchment,
        RollOfVellum,
        Ruby,
        Sapphire,
        SpiritwoodPlank,
        SteelIngot,
        TemperedGlassVial,
        VialOfInk
    ];
    public static IReadOnlyList<Material> All { get; } =
    [
        Bone,
        BoltOfCloth,
        PileOfGlitteringDust,
        Feather,
        PlantFiber,
        TannedHideSquare,
        WoodPlank,
        IronIngot,
        Scale,
        ChitinFragment,
        GraniteSlab,
        AmberChunk,
        BoltOfDamask,
        BoltOfLinen,
        BoltOfSilk,
        DeldrimorSteelIngot,
        Diamond,
        ElonianLeatherSquare,
        FurSquare,
        GlobOfEctoplasm,
        JadeiteShard,
        LeatherSquare,
        LumpOfCharcoal,
        MonstrousClaw,
        MonstrousEye,
        MonstrousFang,
        ObsidianShard,
        OnyxGemstone,
        RollOfParchment,
        RollOfVellum,
        Ruby,
        Sapphire,
        SpiritwoodPlank,
        SteelIngot,
        TemperedGlassVial,
        VialOfInk
    ];

    public string? Multiple { get; init; }
    public string? WikiUrl { get; init; }

    private Material()
    {
    }
}
