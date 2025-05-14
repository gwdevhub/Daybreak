using Daybreak.Shared.Models;
using Daybreak.Shared.Services.TradeChat;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Daybreak.Services.TradeChat;

internal sealed partial class WordHighlightingService : IWordHighlightingService
{
    private static readonly Regex WordsSplitRegex = SplitByWordsRegex();
    private static readonly Regex BuyWordsRegex = BuyWordsGroupRegex();
    private static readonly Regex SellWordsRegex = SellWordsGroupRegex();
    private static readonly Regex TradeWordsRegex = TradeWordsGroupRegex();

    public IEnumerable<ColoredTextElement> ParseString(string s, SolidColorBrush foreground, SolidColorBrush buy, SolidColorBrush sell, SolidColorBrush trade)
    {
        var words = WordsSplitRegex.Split(s);
        foreach (var word in words)
        {
            if (BuyWordsRegex.IsMatch(word))
            {
                yield return new ColoredTextElement { Color = buy, Text = word };
                continue;
            }

            if (SellWordsRegex.IsMatch(word))
            {
                yield return new ColoredTextElement { Color = sell, Text = word };
                continue;
            }

            if (TradeWordsRegex.IsMatch(word))
            {
                yield return new ColoredTextElement { Color = trade, Text = word };
                continue;
            }

            yield return new ColoredTextElement { Color = foreground, Text = word };
        }
    }

    [GeneratedRegex("[\\s+\\|+]", RegexOptions.Compiled)]
    private static partial Regex SplitByWordsRegex();

    [GeneratedRegex("((wtb)|(buy)|(buying)|(buyin))", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex BuyWordsGroupRegex();

    [GeneratedRegex("((wts)|(sell)|(selling)|(sellin))", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex SellWordsGroupRegex();

    [GeneratedRegex("((wtt)|(trade)|(trading)|(lf)|(tradin))", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex TradeWordsGroupRegex();
}
