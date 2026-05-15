# SkillUpdater

One-shot console tool that regenerates `Daybreak.Shared/Models/Guildwars/Skill.g.cs`
from <https://wiki.guildwars.com>. The wiki is the single source of truth for skill
data — this tool does not read the existing `Skill.g.cs`.

## Run

```bash
dotnet run --project Tools/SkillUpdater
```

## What it does

1. Enumerates every skill page via the MediaWiki API
   (`generator=categorymembers` over the five campaign categories
   `Core_skills`, `Prophecies_skills`, `Factions_skills`, `Nightfall_skills`,
   `Eye_of_the_North_skills`) with `prop=revisions&rvslots=main&rvprop=content`.
2. Filters to pages containing a `{{Skill infobox}}` template.
3. Parses each infobox via the tool-local `WikiSkillParser`.
4. Derives a deterministic `IconUrl` from the skill name using
   `Special:FilePath`.
5. Writes a sorted, grouped `Skill.g.cs`.
