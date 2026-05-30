using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// Represents system-wide summary cards for passenger performance report
/// </summary>
public partial record PassengerPerformanceReportSummaryModel : BaseNopModel
{
    public int CountToday { get; set; }

    public int CountThisWeek { get; set; }

    public int CountThisMonth { get; set; }

    public int CountThisYear { get; set; }

    public int CountAllTime { get; set; }

    public int CountUnattributed { get; set; }

    public string CurrentPersianYear { get; set; }
}
