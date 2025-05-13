using Daybreak.Models;
using System.Collections.Generic;
using System.Windows.Media;

namespace Daybreak.Services.TradeChat;

public interface IWordHighlightingService
{
    IEnumerable<ColoredTextElement> ParseString(string s, SolidColorBrush foreground, SolidColorBrush buy, SolidColorBrush sell, SolidColorBrush trade);
}
