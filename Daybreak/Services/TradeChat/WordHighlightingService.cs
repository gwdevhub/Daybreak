using Daybreak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Daybreak.Services.TradeChat;

public sealed class WordHighlightingService : IWordHighlightingService
{
    private static readonly Regex WordsSplitRegex = new("[\\s+\\|+]", RegexOptions.Compiled);
    private static readonly string[] BuyWords = new[]
    {
        "wtb",
        "buy",
        "buying",
        "buyin",
    };
    private static readonly string[] SellWords = new[]
    {
        "wts",
        "sell",
        "selling",
        "sellin"
    };
    private static readonly string[] TradeWords = new[]
    {
        "wtt",
        "trade",
        "trading",
        "lf",
        "tradin"
    };

    public IEnumerable<ColoredTextElement> ParseString(string s, SolidColorBrush foreground, SolidColorBrush buy, SolidColorBrush sell, SolidColorBrush trade)
    {
        var words = WordsSplitRegex.Split(s);
        foreach (var word in words)
        {
            if (BuyWords.Any(w => word.Contains(w, StringComparison.InvariantCultureIgnoreCase)))
            {
                yield return new ColoredTextElement { Color = buy, Text = word };
                continue;
            }

            if (SellWords.Any(w => word.Contains(w, StringComparison.InvariantCultureIgnoreCase)))
            {
                yield return new ColoredTextElement { Color = sell, Text = word };
                continue;
            }

            if (TradeWords.Any(w => word.Contains(w, StringComparison.InvariantCultureIgnoreCase)))
            {
                yield return new ColoredTextElement { Color = trade, Text = word };
                continue;
            }

            yield return new ColoredTextElement { Color = foreground, Text = word };
        }
    }
}
