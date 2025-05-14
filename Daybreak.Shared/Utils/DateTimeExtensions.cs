using System;

namespace Daybreak.Shared.Utils;

public static class DateTimeExtensions
{
    /// <summary>
    /// Converts <see cref="DateTime"/> to <see cref="DateTimeOffset"/> safely and clamps the values between <see cref="DateTimeOffset.MinValue"/> and <see cref="DateTimeOffset.MaxValue"/>
    /// </summary>
    public static DateTimeOffset ToSafeDateTimeOffset(this DateTime dateTime)
    {
        var utcDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return utcDateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                   ? DateTimeOffset.MinValue
                   : utcDateTime.ToUniversalTime() >= DateTimeOffset.MaxValue.UtcDateTime
                        ? DateTimeOffset.MaxValue
                        : new DateTimeOffset(utcDateTime);
    }

    /// <summary>
    /// Converts <see cref="DateTime"/> to <see cref="DateTimeOffset"/> safely and clamps the values between <see cref="DateTimeOffset.MinValue"/> and <see cref="DateTimeOffset.MaxValue"/>
    /// </summary>
    public static DateTimeOffset? ToSafeDateTimeOffset(this DateTime? dateTime)
    {
        if (!dateTime.HasValue)
        {
            return default;
        }

        return dateTime.Value.ToSafeDateTimeOffset();
    }
}
