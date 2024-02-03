using Daybreak.Models.Guildwars;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Daybreak.Models.Builds;
public sealed class SingleBuildEntry : IBuildEntry
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string? name;
    private string? sourceUrl;
    private Profession primary = Profession.None;
    private Profession secondary = Profession.None;
    private List<AttributeEntry> attributes = [];
    private List<Skill> skills = [Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill, Skill.NoSkill];

    public string? PreviousName { get; set; }
    public string? Name
    {
        get => this.name;
        set
        {
            this.name = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Name)));
        }
    }
    public string? SourceUrl
    {
        get => this.sourceUrl;
        set
        {
            this.sourceUrl = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SourceUrl)));
        }
    }
    public Profession Primary
    {
        get => this.primary;
        set
        {
            this.primary = value;
            this.UpdateAttributes();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Primary)));
        }
    }
    public Profession Secondary
    {
        get => this.secondary;
        set
        {
            this.secondary = value;
            this.UpdateAttributes();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Secondary)));
        }
    }
    public List<AttributeEntry> Attributes
    {
        get => this.attributes;
        set
        {
            this.attributes = value;
            this.UpdateSkills();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Attributes)));
        }
    }
    public List<Skill> Skills
    {
        get => this.skills;
        set
        {
            this.skills = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Skills)));
        }
    }
    public Skill FirstSkill
    {
        get => this.Skills[0];
        set
        {
            this.Skills[0] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FirstSkill)));
        }
    }
    public Skill SecondSkill
    {
        get => this.Skills[1];
        set
        {
            this.Skills[1] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SecondSkill)));
        }
    }
    public Skill ThirdSkill
    {
        get => this.Skills[2];
        set
        {
            this.Skills[2] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ThirdSkill)));
        }
    }
    public Skill FourthSkill
    {
        get => this.Skills[3];
        set
        {
            this.Skills[3] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FourthSkill)));
        }
    }
    public Skill FifthSkill
    {
        get => this.Skills[4];
        set
        {
            this.Skills[4] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FifthSkill)));
        }
    }
    public Skill SixthSkill
    {
        get => this.Skills[5];
        set
        {
            this.Skills[5] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SixthSkill)));
        }
    }
    public Skill SeventhSkill
    {
        get => this.Skills[6];
        set
        {
            this.Skills[6] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.SeventhSkill)));
        }
    }
    public Skill EigthSkill
    {
        get => this.Skills[7];
        set
        {
            this.Skills[7] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.EigthSkill)));
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
}
