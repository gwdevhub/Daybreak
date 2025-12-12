using Daybreak.Utils;
using System.Extensions;
using System.IO;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class WikiViewModel : ViewModelBase<WikiViewModel, WikiView>
{
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
        var webRoot = WebRootUtil.GetWebRootPath();
        var wikiPath = Path.Combine(webRoot, "wiki");
        return Directory.EnumerateFiles(wikiPath, "*.md")
            .Select(Path.GetFileNameWithoutExtension)
            .OfType<string>();
    }
}
