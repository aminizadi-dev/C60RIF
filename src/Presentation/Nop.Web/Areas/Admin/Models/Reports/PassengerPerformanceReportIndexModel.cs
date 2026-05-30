using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// Represents the passenger performance report index page model
/// </summary>
public partial record PassengerPerformanceReportIndexModel : BaseNopModel
{
    public PassengerPerformanceReportSummaryModel Summary { get; set; } = new();

    public PassengerPerformanceReportSearchModel SearchModel { get; set; } = new();
}
