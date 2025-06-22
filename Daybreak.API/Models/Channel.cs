namespace Daybreak.API.Models;

public enum Channel : uint
{
    Alliance = 0,
    Allies = 1, // coop with two groups for instance.
    GWCA1 = 2,
    All = 3,
    GWCA2 = 4,
    Moderator = 5,
    Emote = 6,
    Warning = 7, // shows in the middle of the screen and does not parse <c> tags
    GWCA3 = 8,
    Guild = 9,
    Global = 10,
    Group = 11,
    Trade = 12,
    Advisory = 13,
    Whisper = 14,
    Count,

    // non-standard channel, but useful.
    Command
};
