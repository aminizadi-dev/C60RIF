using Nop.Web.Areas.Admin.Models.Reports;

namespace Nop.Web.Areas.Admin.Factories;

/// <summary>
/// Represents the passenger performance report model factory interface
/// </summary>
public partial interface IPassengerPerformanceReportModelFactory
{
    Task<PassengerPerformanceReportIndexModel> PrepareIndexModelAsync();

    Task<PassengerPerformanceReportListModel> PrepareListModelAsync(PassengerPerformanceReportSearchModel searchModel);
}
