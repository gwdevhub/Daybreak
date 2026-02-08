using Daybreak.Shared.Attributes;
using Daybreak.Shared.Models.Themes;

namespace Daybreak.Attributes;
public sealed class DaybreakThemeOptionsAttribute : OptionValuesFactoryAttribute
{
    public override List<object> ValuesFactory()
    {
        return [.. Theme.Themes.Cast<object>()];
    }
}
