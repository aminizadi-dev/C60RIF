namespace Nop.Services.Reports;

/// <summary>
/// Represents system-wide passenger form entry totals for summary cards
/// </summary>
public partial class PassengerPerformanceReportSummary
{
    public int CountToday { get; set; }

    public int CountThisWeek { get; set; }

    public int CountThisMonth { get; set; }

    public int CountThisYear { get; set; }

    public int CountAllTime { get; set; }

    public int CountUnattributed { get; set; }

    public string CurrentPersianYear { get; set; }
}
