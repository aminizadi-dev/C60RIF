using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// Represents a passenger performance report row model
/// </summary>
public partial record PassengerPerformanceReportModel : BaseNopModel
{
    public int? CustomerId { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.User")]
    public string UserDisplay { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.Email")]
    public string Email { get; set; }

    public bool IsSystemTotal { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountToday")]
    public int CountToday { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountThisWeek")]
    public int CountThisWeek { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountThisMonth")]
    public int CountThisMonth { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountThisYear")]
    public int CountThisYear { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountAllTime")]
    public int CountAllTime { get; set; }

    [NopResourceDisplayName("Admin.Reports.PassengerPerformance.Fields.CountUnattributed")]
    public int CountUnattributed { get; set; }
}
