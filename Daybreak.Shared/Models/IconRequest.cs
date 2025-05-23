﻿using Daybreak.Shared.Models.Guildwars;

namespace Daybreak.Shared.Models;

public sealed class IconRequest
{
    public Skill? Skill { get; set; }
    public string? IconBase64 { get; set; }
    public bool Finished { get; set; }
}
