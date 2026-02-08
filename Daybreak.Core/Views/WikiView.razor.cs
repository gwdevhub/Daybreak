using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class WikiViewModel : ViewModelBase<WikiViewModel, WikiView>
{
    private const string WikiPrefix = "wiki/";
    private const string MarkdownExtension = ".md";

    public string? CurrentPage { get; private set; }
    public List<string> Pages { get; } = [];

    public override ValueTask ParametersSet(WikiView view, CancellationToken cancellationToken)
    {
        this.CurrentPage = view.Page;
        this.Pages.ClearAnd().AddRange(LoadPages());
        return base.ParametersSet(view, cancellationToken);
    }

    private static IEnumerable<string> LoadPages()
    {
        var assembly = typeof(WikiViewModel).Assembly;
        return assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(WikiPrefix, StringComparison.OrdinalIgnoreCase)
                        && name.EndsWith(MarkdownExtension, StringComparison.OrdinalIgnoreCase))
            .Select(name => Path.GetFileNameWithoutExtension(name[WikiPrefix.Length..]))
            .OfType<string>();
    }
}
