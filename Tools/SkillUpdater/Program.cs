using System.Reflection;

namespace Daybreak.Tools.SkillUpdater;

internal static class Program
{
    private const string UserAgent = "Daybreak-SkillUpdater/1.0 (+https://github.com/gwdevhub/Daybreak)";

    public static async Task<int> Main()
    {
        using var cancellationSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cancellationSource.Cancel();
        };

        var repoRoot = LocateRepoRoot();
        var skillFile = Path.Combine(repoRoot, "Daybreak.Shared", "Models", "Guildwars", "Skill.g.cs");

        Console.WriteLine($"Repo root:   {repoRoot}");
        Console.WriteLine($"Output file: {skillFile}");
        Console.WriteLine();

        try
        {
            using var wikiClient = new WikiHttpClient(UserAgent);
            using var apiClient = new GwToolboxClient(UserAgent);

            Console.WriteLine("Fetching skill data from api.gwtoolbox.com…");
            var apiSkills = await apiClient.FetchSkillsAsync(cancellationSource.Token);
            Console.WriteLine($"Fetched {apiSkills.Count} skill records.");
            Console.WriteLine();

            var enumerator = new SkillEnumerator(wikiClient);
            Console.WriteLine("Enumerating the skill roster from the wiki…");
            var roster = await enumerator.EnumerateAsync(cancellationSource.Token);
            Console.WriteLine($"Collected {roster.Count} skill pages.");
            Console.WriteLine();

            var skills = new List<ParsedSkill>();
            var warnings = new List<string>();
            foreach (var entry in roster)
            {
                // Monster and environment skill pages routinely omit the id
                // from their infobox. The API still knows them, so fall back to
                // its name index rather than emitting an unusable id-0 entry.
                if (entry.Ids.Count == 0)
                {
                    if (apiSkills.TryGetUniqueByName(entry.Name, out var byName))
                    {
                        skills.Add(SkillMapper.Map(byName.Id, entry.Name, byName));
                    }
                    else
                    {
                        warnings.Add($"skipped '{entry.Name}': the wiki lists no id and the name is not unique in the API");
                    }

                    continue;
                }

                foreach (var id in entry.Ids)
                {
                    if (!apiSkills.TryGetById(id, out var apiSkill))
                    {
                        warnings.Add($"skipped '{entry.Name}' (id {id}): no record in the API");
                        continue;
                    }

                    skills.Add(SkillMapper.Map(id, entry.Name, apiSkill));
                }
            }

            Console.WriteLine($"Mapped {skills.Count} skills from API data.");
            Console.WriteLine();

            var iconResolver = new IconResolver(wikiClient);
            var iconUrls = await iconResolver.ResolveAsync(roster, cancellationSource.Token);
            Console.WriteLine();

            Console.WriteLine("Rendering Skill.g.cs…");
            var (content, renderWarnings) = SkillFileWriter.Render(skills, iconUrls);
            await File.WriteAllTextAsync(skillFile, content, cancellationSource.Token);
            Console.WriteLine($"Wrote {content.Length:N0} chars to {skillFile}");
            foreach (var warn in renderWarnings.Concat(warnings))
            {
                Console.Error.WriteLine($"  ! {warn}");
            }

            return 0;
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("aborted.");
            return 130;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"FATAL: {ex}");
            return 1;
        }
    }

    private static string LocateRepoRoot()
    {
        // The compiled tool sits at <repo>/Tools/SkillUpdater/bin/<config>/<tfm>/.
        // Walk upwards until we find Daybreak.slnx — works for both `dotnet run`
        // and a published binary launched from anywhere inside the repo.
        var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Daybreak.slnx")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate Daybreak.slnx walking upwards from the assembly location.");
    }
}
