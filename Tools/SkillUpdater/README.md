# SkillUpdater

One-shot console tool that regenerates `Daybreak.Shared/Models/Guildwars/Skill.g.cs`.
Skill *values* come from the GWToolbox API
(<https://api.gwtoolbox.com/v1/en/skills.json>), which is generated straight from
the Guild Wars client; <https://wiki.guildwars.com> supplies only the roster and
the icon URLs. This tool does not read the existing `Skill.g.cs`.

## Run

```bash
dotnet run --project Tools/SkillUpdater
```

## Why two sources

The API is the source of truth for every value because it is the client's own
data: descriptions arrive already rendered ("20...44...50% faster") instead of
as wiki markup the generator has to interpret (`{{gr|20|50||%}}`), and the
numbers behind them are exact.

The wiki is kept for two things the API cannot provide:

- **The roster.** The client's skill names are not unique — four different
  skills are called "Charm Animal", and ~1,600 unreleased/internal skills sit
  alongside the live ones. The wiki disambiguates them ("Charm Animal (White
  Mantle)"), and those unique names are what the generated C# identifiers and
  the icon lookup are keyed on.
- **The icons.** The API serves DDS textures; Daybreak's builder and UI load the
  wiki's JPEGs dynamically.

## What it does

1. Downloads the API's skill catalogue and indexes it by skill id.
2. Enumerates every skill page via the MediaWiki API
   (`generator=categorymembers` over the five campaign categories
   `Core_skills`, `Prophecies_skills`, `Factions_skills`, `Nightfall_skills`,
   `Eye_of_the_North_skills`) with `prop=revisions&rvslots=main&rvprop=content`,
   filters to pages containing a `{{Skill infobox}}` template, and reads the
   page title plus the infobox's `id` field.
3. Maps each id's API record onto Daybreak's model (`SkillMapper`).
4. Resolves the canonical CDN URL for each skill icon by batched
   `prop=imageinfo` queries — preferring the high-resolution
   `<Name> (large).jpg`, falling back to `<Name>.jpg`, leaving the URL
   empty when neither file exists.
5. Writes a sorted, grouped `Skill.g.cs`.

## Mapping notes

`SkillMapper` translates the client's encodings, which are not Daybreak's:

| Daybreak | API | Translation |
| --- | --- | --- |
| `Energy` | `energy_cost` | An encoded cost: `11` means 15, `12` means 25. |
| `Adrenaline` | `adrenaline` | Stored in 25ths of a strike; rounded up. |
| `Sacrifice` | `health_cost` | A whole percentage; Daybreak stores a fraction. |
| `Upkeep` | `duration0` | Not a field — `131072` is the "maintained" sentinel, and those are exactly the `Upkeep = -1` skills. |
| `Type` | `type` (+ `weapon_req`, `combo`, `profession`, `touch_range`, `activation`) | `GW::Constants::SkillType` is one value where Daybreak's `SkillType` is a flags enum, so sub-type flags (`Touch`, `Flash`, weapon, `Lead`/`OffHand`/`Dual`, `Binding`/`Nature`/`EbonVanguard`) are recovered from the accompanying fields. |

`campaign`, `profession` and `attribute` ids already match Daybreak's own, so
they are looked up directly. Note that the API omits any key equal to its
default, so a missing `energy_cost` means 0 — and a missing `attribute` means
the skill has no governing attribute.
