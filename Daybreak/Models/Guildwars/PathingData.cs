﻿using System.Collections.Generic;

namespace Daybreak.Models.Guildwars;

public readonly struct PathingData
{
    public List<Trapezoid> Trapezoids { get; init; }
}
