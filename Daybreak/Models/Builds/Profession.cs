using System;
using System.Collections.Generic;
using System.Linq;

namespace Daybreak.Models.Builds
{
    public sealed class Profession
    {
        public static Profession None { get; } = new() { Name = "None", Id = 0 };
        public static Profession Warrior { get; } = new() { Name = "Warrior", Id = 1 };
        public static Profession Ranger { get; } = new() { Name = "Ranger", Id = 2 };
        public static Profession Monk { get; } = new() { Name = "Monk", Id = 3 };
        public static Profession Necromancer { get; } = new() { Name = "Necromancer", Id = 4 };
        public static Profession Mesmer { get; } = new() { Name = "Mesmer", Id = 5 };
        public static Profession Elementalist { get; } = new() { Name = "Elementalist", Id = 6 };
        public static Profession Assassin { get; } = new() { Name = "Assassin", Id = 7 };
        public static Profession Ritualist { get; } = new() { Name = "Ritualist", Id = 8 };
        public static Profession Paragon { get; } = new() { Name = "Paragon", Id = 9 };
        public static Profession Dervish { get; } = new() { Name = "Dervish", Id = 10 };
        public static IEnumerable<Profession> Professions { get; } = new List<Profession>
        {
            None,
            Warrior,
            Ranger,
            Monk,
            Necromancer,
            Mesmer,
            Elementalist,
            Assassin,
            Ritualist,
            Paragon,
            Dervish
        };
        public static bool TryParse(int id, out Profession profession)
        {
            profession = Professions.Where(prof => prof.Id == id).FirstOrDefault();
            if (profession is null)
            {
                return false;
            }

            return true;
        }
        public static bool TryParse(string name, out Profession profession)
        {
            profession = Professions.Where(prof => prof.Name == name).FirstOrDefault();
            if (profession is null)
            {
                return false;
            }

            return true;
        }
        public static Profession Parse(int id)
        {
            if (TryParse(id, out var profession) is false)
            {
                throw new InvalidOperationException($"Could not find a profession with id {id}");
            }

            return profession;
        }
        public static Profession Parse(string name)
        {
            if (TryParse(name, out var profession) is false)
            {
                throw new InvalidOperationException($"Could not find a profession with name {name}");
            }

            return profession;
        }

        public string Name { get; private set; }
        public int Id { get; set; }
        private Profession()
        {
        }
    }
}
