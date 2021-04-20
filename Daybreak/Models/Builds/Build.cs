using System.Collections.Generic;

namespace Daybreak.Models.Builds
{
    public sealed class Build
    {
        public BuildMetadata BuildMetadata { get; set; }
        public Profession Primary { get; set; }
        public Profession Secondary { get; set; }
        public List<AttributeEntry> Attributes { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
    }
}
