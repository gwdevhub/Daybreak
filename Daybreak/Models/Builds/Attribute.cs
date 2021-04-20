using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Builds
{
    public sealed class Attribute
    {
        public static Attribute FastCasting { get; } = new() { Name = "Fast Casting", Id = 0 };
        public static Attribute IllusionMagic { get; } = new() { Name = "Illusion Magic", Id = 1 };
        public static Attribute DominationMagic { get; } = new() { Name = "Domination Magic", Id = 2 };
        public static Attribute InspirationMagic { get; } = new() { Name = "Inspiration Magic", Id = 3 };
        public static Attribute BloodMagic { get; } = new() { Name = "Blood Magic", Id = 4 };
        public static Attribute DeathMagic { get; } = new() { Name = "Death Magic", Id = 5 };
        public static Attribute SoulReaping { get; } = new() { Name = "Soul Reaping", Id = 6 };
        public static Attribute Curses { get; } = new() { Name = "Curses", Id = 7 };
        public static Attribute AirMagic { get; } = new() { Name = "Air Magic", Id = 8 };
        public static Attribute EarthMagic { get; } = new() { Name = "Earth Magic", Id = 9 };
        public static Attribute FireMagic { get; } = new() { Name = "Fire Magic", Id = 10 };
        public static Attribute WaterMagic { get; } = new() { Name = "Water Magic", Id = 11 };
        public static Attribute EnergyStorage { get; } = new() { Name = "Energy Storage", Id = 12 };
        public static Attribute HealingPrayers { get; } = new() { Name = "Healing Prayers", Id = 13 };
        public static Attribute SmitingPrayers { get; } = new() { Name = "Smiting Prayers", Id = 14 };
        public static Attribute ProtectionPrayers { get; } = new() { Name = "Protection Prayers", Id = 15 };
        public static Attribute DivineFavor { get; } = new() { Name = "Divine Favor", Id = 16 };
        public static Attribute Strength { get; } = new() { Name = "Strength", Id = 17 };
        public static Attribute AxeMastery { get; } = new() { Name = "Axe Mastery", Id = 18 };
        public static Attribute HammerMastery { get; } = new() { Name = "Hammer Mastery", Id = 19 };
        public static Attribute Swordsmanship { get; } = new() { Name = "Swordsmanship", Id = 20};
        public static Attribute Tactics { get; } = new() { Name = "Tactics", Id = 21 };
        public static Attribute BeastMastery { get; } = new() { Name = "Beast Mastery", Id = 22 };
        public static Attribute Expertise { get; } = new() { Name = "Expertise", Id = 23 };
        public static Attribute WildernessSurvival { get; } = new() { Name = "Wilderness Survival", Id = 24 };
        public static Attribute Marksmanship { get; } = new() { Name = "Marksmanship", Id = 25 };
        public static Attribute DaggerMastery { get; } = new() { Name = "Dagger Mastery", Id = 29 };
        public static Attribute DeadlyArts { get; } = new() { Name = "Deadly Arts", Id = 30 };
        public static Attribute ShadowArts { get; } = new() { Name = "Shadow Arts", Id = 31 };
        public static Attribute Communing { get; } = new() { Name = "Communing", Id = 32 };
        public static Attribute RestorationMagic { get; } = new() { Name = "Restoration Magic", Id = 33 };
        public static Attribute ChannelingMagic { get; } = new() { Name = "Channeling Magic", Id = 34 };
        public static Attribute CriticalStrikes { get; } = new() { Name = "Critical Strikes", Id = 35 };
        public static Attribute SpawningPower { get; } = new() { Name = "Spawning Power", Id = 36 };
        public static Attribute SpearMastery { get; } = new() { Name = "Spear Mastery", Id = 37 };
        public static Attribute Command { get; } = new() { Name = "Command", Id = 38 };
        public static Attribute Motivation { get; } = new() { Name = "Motivation", Id = 39 };
        public static Attribute Leadership { get; } = new() { Name = "Leadership", Id = 40 };
        public static Attribute ScytheMastery { get; } = new() { Name = "Scythe Mastery", Id = 41 };
        public static Attribute WindPrayers { get; } = new() { Name = "Wind Prayers", Id = 42 };
        public static Attribute EarthPrayers { get; } = new() { Name = "Earth Prayers", Id = 43 };
        public static Attribute Mysticism { get; } = new() { Name = "Mysticism", Id = 44 };
        public static IEnumerable<Attribute> Attributes { get; } = new List<Attribute>
        {
            FastCasting,
            IllusionMagic,
            DominationMagic,
            InspirationMagic,
            BloodMagic,
            DeathMagic,
            SoulReaping,
            Curses,
            AirMagic,
            EarthMagic,
            FireMagic,
            WaterMagic,
            EnergyStorage,
            HealingPrayers,
            SmitingPrayers,
            ProtectionPrayers,
            DivineFavor,
            Strength,
            AxeMastery,
            HammerMastery,
            Swordsmanship,
            Tactics,
            BeastMastery,
            Expertise,
            WildernessSurvival,
            Marksmanship,
            DaggerMastery,
            DeadlyArts,
            ShadowArts,
            Communing,
            RestorationMagic,
            ChannelingMagic,
            CriticalStrikes,
            SpawningPower,
            SpearMastery,
            Command,
            Motivation,
            Leadership,
            ScytheMastery,
            WindPrayers,
            EarthMagic,
            Mysticism
        };

        public static bool TryParse(int id, out Attribute attribute)
        {
            attribute = Attributes.Where(attr => attr.Id == id).FirstOrDefault();
            if (attribute is null)
            {
                return false;
            }

            return true;
        }
        public static bool TryParse(string name, out Attribute attribute)
        {
            attribute = Attributes.Where(attr => attr.Name == name).FirstOrDefault();
            if (attribute is null)
            {
                return false;
            }

            return true;
        }
        public static Attribute Parse(int id)
        {
            if (TryParse(id, out var attribute) is false)
            {
                throw new InvalidOperationException($"Could not find an attribute with id {id}");
            }

            return attribute;
        }
        public static Attribute Parse(string name)
        {
            if (TryParse(name, out var attribute) is false)
            {
                throw new InvalidOperationException($"Could not find an attribute with name {name}");
            }

            return attribute;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        private Attribute()
        {
        }
    }
}
