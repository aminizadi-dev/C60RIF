using System;
using System.Globalization;

namespace Nop.Core;

/// <summary>
/// Provides UTC date ranges for Persian calendar periods (day, week, month, year).
/// Week starts on Saturday, consistent with <see cref="PersianDateTime"/>.
/// </summary>
public static class PersianDateRangeHelper
{
    /// <summary>
    /// Gets the UTC range [start, end) for the Persian calendar day containing the reference local time.
    /// </summary>
    public static (DateTime startUtc, DateTime endUtc) GetPersianDayRangeUtc(DateTime? referenceLocal = null)
    {
        var local = referenceLocal ?? DateTime.Now;
        var persian = new PersianDateTime(local);
        var pc = new PersianCalendar();
        var startLocal = pc.ToDateTime(persian.Year, persian.Month, persian.Day, 0, 0, 0, 0);
        return ToUtcRange(startLocal, startLocal.AddDays(1));
    }

    /// <summary>
    /// Gets the UTC range [start, end) for the Persian calendar week containing the reference local time (Saturday start).
    /// </summary>
    public static (DateTime startUtc, DateTime endUtc) GetPersianWeekRangeUtc(DateTime? referenceLocal = null)
    {
        var local = referenceLocal ?? DateTime.Now;
        var pc = new PersianCalendar();
        var date = new DateTime(local.Year, local.Month, local.Day, local.Hour, local.Minute, local.Second, local.Millisecond, DateTimeKind.Unspecified);

        while (pc.GetDayOfWeek(date) != DayOfWeek.Saturday)
            date = date.AddDays(-1);

        var startLocal = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Unspecified);
        return ToUtcRange(startLocal, startLocal.AddDays(7));
    }

    /// <summary>
    /// Gets the UTC range [start, end) for the Persian calendar month containing the reference local time.
    /// </summary>
    public static (DateTime startUtc, DateTime endUtc) GetPersianMonthRangeUtc(DateTime? referenceLocal = null)
    {
        var local = referenceLocal ?? DateTime.Now;
        var persian = new PersianDateTime(local);
        var pc = new PersianCalendar();
        var startLocal = pc.ToDateTime(persian.Year, persian.Month, 1, 0, 0, 0, 0);
        var endLocal = persian.Month < 12
            ? pc.ToDateTime(persian.Year, persian.Month + 1, 1, 0, 0, 0, 0)
            : pc.ToDateTime(persian.Year + 1, 1, 1, 0, 0, 0, 0);

        return ToUtcRange(startLocal, endLocal);
    }

    /// <summary>
    /// Gets the UTC range [start, end) for the Persian calendar year containing the reference local time.
    /// </summary>
    public static (DateTime startUtc, DateTime endUtc) GetPersianYearRangeUtc(DateTime? referenceLocal = null)
    {
        var local = referenceLocal ?? DateTime.Now;
        var persian = new PersianDateTime(local);
        var pc = new PersianCalendar();
        var startLocal = pc.ToDateTime(persian.Year, 1, 1, 0, 0, 0, 0);
        var endLocal = pc.ToDateTime(persian.Year + 1, 1, 1, 0, 0, 0, 0);

        return ToUtcRange(startLocal, endLocal);
    }

    static (DateTime startUtc, DateTime endUtc) ToUtcRange(DateTime startLocal, DateTime endLocal)
    {
        var startUtc = DateTime.SpecifyKind(startLocal, DateTimeKind.Local).ToUniversalTime();
        var endUtc = DateTime.SpecifyKind(endLocal, DateTimeKind.Local).ToUniversalTime();
        return (startUtc, endUtc);
    }
}
