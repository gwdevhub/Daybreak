namespace Daybreak.Shared.Converters;

public static class TimespanToETAConverter
{
    private const char PluralAppend = 's';

    public static string GetETAString(TimeSpan timeSpan)
    {
        if ((int)timeSpan.TotalDays > 0)
        {
            return $"{(int)timeSpan.TotalDays} day{((int)timeSpan.TotalDays > 1 ? PluralAppend : string.Empty)} remaining";
        }

        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2} remaining";
    }
}
