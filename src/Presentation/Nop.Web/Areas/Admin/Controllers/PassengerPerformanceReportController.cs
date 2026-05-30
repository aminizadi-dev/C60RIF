using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Reports;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class PassengerPerformanceReportController : BaseAdminController
{
    protected readonly IPassengerPerformanceReportModelFactory _passengerPerformanceReportModelFactory;

    public PassengerPerformanceReportController(
        IPassengerPerformanceReportModelFactory passengerPerformanceReportModelFactory)
    {
        _passengerPerformanceReportModelFactory = passengerPerformanceReportModelFactory;
    }

    [CheckPermission(new[]
    {
        StandardPermission.Reports.PASSENGER_PERFORMANCE_VIEW,
        StandardPermission.Passengers.PASSENGERS_VIEW
    })]
    public virtual async Task<IActionResult> Index()
    {
        var model = await _passengerPerformanceReportModelFactory.PrepareIndexModelAsync();
        return View(model);
    }

    [HttpPost]
    [CheckPermission(new[]
    {
        StandardPermission.Reports.PASSENGER_PERFORMANCE_VIEW,
        StandardPermission.Passengers.PASSENGERS_VIEW
    })]
    public virtual async Task<IActionResult> ReportList(PassengerPerformanceReportSearchModel searchModel)
    {
        var model = await _passengerPerformanceReportModelFactory.PrepareListModelAsync(searchModel);
        return Json(model);
    }
}
