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
            using var client = new WikiHttpClient(UserAgent);
            var enumerator = new SkillEnumerator(client);

            Console.WriteLine("Enumerating skills from wiki…");
            var skills = await enumerator.EnumerateAsync(cancellationSource.Token);
            Console.WriteLine($"Collected {skills.Count} skills.");
            Console.WriteLine();

            var iconResolver = new IconResolver(client);
            var iconUrls = await iconResolver.ResolveAsync(skills, cancellationSource.Token);
            Console.WriteLine();

            Console.WriteLine("Rendering Skill.g.cs…");
            var (content, warnings) = SkillFileWriter.Render(skills, iconUrls);
            await File.WriteAllTextAsync(skillFile, content, cancellationSource.Token);
            Console.WriteLine($"Wrote {content.Length:N0} chars to {skillFile}");
            foreach (var warn in warnings)
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
