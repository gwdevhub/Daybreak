namespace Daybreak.Models.Guildwars;

public readonly struct Trapezoid
{
    public int Id { get; init; }

    public int PathingMapId { get; init; }

    public float XTL { get; init; }

    public float XTR { get; init; }

    public float YT { get; init; }

    public float XBL { get; init; }

    public float XBR { get; init; }

    public float YB { get; init; }
}
