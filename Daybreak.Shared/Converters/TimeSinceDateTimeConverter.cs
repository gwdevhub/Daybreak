using System;
using System.Globalization;
using System.Windows.Data;

namespace Daybreak.Converters;

public sealed class TimeSinceDateTimeConverter : IValueConverter
{
    private const char PluralAppend = 's';

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return this.GetTimeString(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private object GetTimeString(object value)
    {
        if (value is not DateTime dateTime)
        {
            return default!;
        }

        var difference = DateTime.Now - dateTime;

        // If less than a minute, return seconds
        if (difference.TotalSeconds < 60)
        {
            var totalSeconds = (int)difference.TotalSeconds;
            return $"{totalSeconds} second{(difference.TotalSeconds > 1 ? PluralAppend : string.Empty)} ago";
        }

        // If less than an hour, return minutes
        if (difference.TotalMinutes < 60)
        {
            var totalMinutes = (int)difference.TotalMinutes;
            return $"{totalMinutes} minute{(difference.TotalMinutes > 1 ? PluralAppend : string.Empty)} ago";
        }

        // If less than a day, return hours
        if (difference.TotalHours < 24)
        {
            var totalHours = (int)difference.TotalHours;
            return $"{totalHours} hour{(difference.TotalHours > 1 ? PluralAppend : string.Empty)} ago";
        }

        // If less than a week, return days
        if (difference.TotalDays < 7)
        {
            var totalDays = (int)difference.TotalDays;
            return $"{totalDays} day{(difference.TotalDays > 1 ? PluralAppend : string.Empty)} ago";
        }

        // If less than a month, return weeks
        var numberOfDaysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        if (difference.TotalDays < numberOfDaysInCurrentMonth)
        {
            var numberOfWeeks = (int)difference.TotalDays / 7;
            return $"{numberOfWeeks} week{(numberOfWeeks > 1 ? PluralAppend : string.Empty)} ago";
        }

        // If less than a year, return months
        var daysInCurrentYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
        if (difference.TotalDays < daysInCurrentYear)
        {
            var days = difference.TotalDays;
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var monthCount = 0;
            while(days > 0)
            {
                monthCount++;
                var numberOfDaysInMonth = DateTime.DaysInMonth(year, month);
                days -= numberOfDaysInMonth;
                if (month == 1)
                {
                    month = 12;
                    year--;
                }
                else
                {
                    month--;
                }
            }

            return $"{monthCount} month{(monthCount > 1 ? PluralAppend : string.Empty)} ago";
        }

        // Return the number of years
        var remainingDays = difference.TotalDays;
        var yearCount = 0;
        var currentYear = DateTime.Now.Year;
        while(remainingDays > 0)
        {
            yearCount++;
            remainingDays -= DateTime.IsLeapYear(currentYear) ? 366 : 365;
            currentYear--;
        }

        return $"{yearCount} year{(yearCount > 1 ? PluralAppend : string.Empty)} ago";
    }
}
