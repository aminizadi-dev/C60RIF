namespace Nop.Services.Reports;

/// <summary>
/// Represents a passenger performance report line per user or system total
/// </summary>
public partial class PassengerPerformanceReportLine
{
    public int? CustomerId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public bool IsSystemTotal { get; set; }

    public int CountToday { get; set; }

    public int CountThisWeek { get; set; }

    public int CountThisMonth { get; set; }

    public int CountThisYear { get; set; }

    public int CountAllTime { get; set; }

    public int CountUnattributed { get; set; }
}
