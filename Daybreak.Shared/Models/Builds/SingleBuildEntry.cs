using Daybreak.Shared.Models.Guildwars;
using System.ComponentModel;
using Attribute = Daybreak.Shared.Models.Guildwars.Attribute;

namespace Daybreak.Shared.Models.Builds;
public sealed class SingleBuildEntry : BuildEntryBase, IBuildEntry, INotifyPropertyChanged, IEquatable<SingleBuildEntry>
{
    public Profession Primary
    {
        get;
        set
        {
            if (value is null)
            {
                return;
            }

            field = value;
            this.UpdateAttributes();
            this.OnPropertyChanged(nameof(this.Primary));
        }
    } = Profession.None;
    public Profession Secondary
    {
        get;
        set
        {
            if (value is null)
            {
                return;
            }

            field = value;
            this.UpdateAttributes();
            this.OnPropertyChanged(nameof(this.Secondary));
        }
    } = Profession.None;
    public List<AttributeEntry> Attributes
    {
        get;
        set
        {
            if (value is null)
            {
                return;
            }

            field = value;
            this.UpdateSkills();
            this.OnPropertyChanged(nameof(this.Attributes));
        }
    } = [];
    public List<Skill> Skills
    {
        get;
        set
        {
            if (value is null)
            {
                return;
            }

            field = value;
            this.OnPropertyChanged(nameof(this.Skills));
        }
    } = [Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill];
    public Skill FirstSkill
    {
        get => this.Skills[0];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[0] = value;
            this.OnPropertyChanged(nameof(this.FirstSkill));
        }
    }
    public Skill SecondSkill
    {
        get => this.Skills[1];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[1] = value;
            this.OnPropertyChanged(nameof(this.SecondSkill));
        }
    }
    public Skill ThirdSkill
    {
        get => this.Skills[2];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[2] = value;
            this.OnPropertyChanged(nameof(this.ThirdSkill));
        }
    }
    public Skill FourthSkill
    {
        get => this.Skills[3];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[3] = value;
            this.OnPropertyChanged(nameof(this.FourthSkill));
        }
    }
    public Skill FifthSkill
    {
        get => this.Skills[4];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[4] = value;
            this.OnPropertyChanged(nameof(this.FifthSkill));
        }
    }
    public Skill SixthSkill
    {
        get => this.Skills[5];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[5] = value;
            this.OnPropertyChanged(nameof(this.SixthSkill));
        }
    }
    public Skill SeventhSkill
    {
        get => this.Skills[6];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[6] = value;
            this.OnPropertyChanged(nameof(this.SeventhSkill));
        }
    }
    public Skill EigthSkill
    {
        get => this.Skills[7];
        set
        {
            if (value is null)
            {
                return;
            }

            this.Skills[7] = value;
            this.OnPropertyChanged(nameof(this.EigthSkill));
        }
    }

    private void UpdateAttributes()
    {
        var attributesToAdd = new List<Attribute>();
        if (this.Primary.PrimaryAttribute is not null)
        {
            attributesToAdd.Add(this.Primary.PrimaryAttribute);
        }

        attributesToAdd.AddRange(this.Primary.Attributes);
        attributesToAdd.AddRange(this.Secondary.Attributes);
        this.Attributes = attributesToAdd.Distinct().Select(attribute =>
        {
            if (this.Attributes.FirstOrDefault(attributeEntry => attributeEntry.Attribute == attribute) is AttributeEntry attributeEntry)
            {
                return attributeEntry;
            }

            return new AttributeEntry { Attribute = attribute };
        }).ToList();
    }

    private void UpdateSkills()
    {
        if (this.FirstSkill.Profession != Profession.None &&
            this.FirstSkill.Profession != this.Primary &&
            this.FirstSkill.Profession != this.Secondary)
        {
            this.FirstSkill = Skill.NoSkill;
        }

        if (this.SecondSkill.Profession != Profession.None &&
            this.SecondSkill.Profession != this.Primary &&
            this.SecondSkill.Profession != this.Secondary)
        {
            this.SecondSkill = Skill.NoSkill;
        }

        if (this.ThirdSkill.Profession != Profession.None &&
            this.ThirdSkill.Profession != this.Primary &&
            this.ThirdSkill.Profession != this.Secondary)
        {
            this.ThirdSkill = Skill.NoSkill;
        }

        if (this.FourthSkill.Profession != Profession.None &&
            this.FourthSkill.Profession != this.Primary &&
            this.FourthSkill.Profession != this.Secondary)
        {
            this.FourthSkill = Skill.NoSkill;
        }

        if (this.FifthSkill.Profession != Profession.None &&
            this.FifthSkill.Profession != this.Primary &&
            this.FifthSkill.Profession != this.Secondary)
        {
            this.FifthSkill = Skill.NoSkill;
        }

        if (this.SixthSkill.Profession != Profession.None &&
            this.SixthSkill.Profession != this.Primary &&
            this.SixthSkill.Profession != this.Secondary)
        {
            this.SixthSkill = Skill.NoSkill;
        }

        if (this.SeventhSkill.Profession != Profession.None &&
            this.SeventhSkill.Profession != this.Primary &&
            this.SeventhSkill.Profession != this.Secondary)
        {
            this.SeventhSkill = Skill.NoSkill;
        }

        if (this.EigthSkill.Profession != Profession.None &&
            this.EigthSkill.Profession != this.Primary &&
            this.EigthSkill.Profession != this.Secondary)
        {
            this.EigthSkill = Skill.NoSkill;
        }
    }

    public bool Equals(SingleBuildEntry? other)
    {
        return this.Name == other?.Name &&
            this.PreviousName == other?.PreviousName &&
            this.SourceUrl == other?.SourceUrl &&
            this.Primary == other?.Primary &&
            this.Secondary == other?.Secondary &&
            this.Skills.SequenceEqual(other.Skills);
    }

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as SingleBuildEntry);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.Name, this.PreviousName, this.SourceUrl, this.Primary.Name, this.Secondary.Name, string.Join(';', this.Skills.Select(s => s.Id)));
    }
}
