using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Reports;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Reports;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class ChartReportController : BaseAdminController
{
    #region Fields

    protected readonly IAgencyService _agencyService;
    protected readonly ICityService _cityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPassengerChartReportService _passengerChartReportService;

    #endregion

    #region Ctor

    public ChartReportController(
        IAgencyService agencyService,
        ICityService cityService,
        ILocalizationService localizationService,
        IPassengerChartReportService passengerChartReportService)
    {
        _agencyService = agencyService;
        _cityService = cityService;
        _localizationService = localizationService;
        _passengerChartReportService = passengerChartReportService;
    }

    #endregion

    #region Pages

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> Age()
    {
        return ChartPageAsync("ChartReportAge", "Admin.Reports.Charts.Age", nameof(AgeData),
            showCity: true, showAgency: true, showDateRange: true, dateHintKey: "Admin.Reports.Charts.TravelStartHint");
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> Education()
    {
        return ChartPageAsync("ChartReportEducation", "Admin.Reports.Charts.Education", nameof(EducationData),
            showCity: true, showAgency: true, showDateRange: true, dateHintKey: "Admin.Reports.Charts.TravelStartHint");
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> LiberationByYear()
    {
        return ChartPageAsync("ChartReportLiberationByYear", "Admin.Reports.Charts.LiberationByYear", nameof(LiberationByYearData),
            showCity: true, showAgency: true, showYear: true, yearsAction: nameof(LiberationYears));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> LiberationByCity()
    {
        return ChartPageAsync("ChartReportLiberationByCity", "Admin.Reports.Charts.LiberationByCity", nameof(LiberationByCityData),
            showDateRange: true, dateHintKey: "Admin.Reports.Charts.LiberationDateHint");
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> AntiX()
    {
        return ChartPageAsync("ChartReportAntiX", "Admin.Reports.Charts.AntiX", nameof(AntiXData));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> MaritalStatus()
    {
        return ChartPageAsync("ChartReportMaritalStatus", "Admin.Reports.Charts.MaritalStatus", nameof(MaritalStatusData));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW)]
    public virtual Task<IActionResult> Employment()
    {
        return ChartPageAsync("ChartReportEmployment", "Admin.Reports.Charts.Employment", nameof(EmploymentData));
    }

    #endregion

    #region Data

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> AgeData(int cityId = 0, int agencyId = 0, string dateFrom = null, string dateTo = null)
    {
        var points = await _passengerChartReportService.GetAgeDistributionAsync(cityId, agencyId, ParseDate(dateFrom), ParseDate(dateTo));
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> EducationData(int cityId = 0, int agencyId = 0, string dateFrom = null, string dateTo = null)
    {
        var points = await _passengerChartReportService.GetEducationDistributionAsync(cityId, agencyId, ParseDate(dateFrom), ParseDate(dateTo));
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> LiberationByYearData(int year = 0, int cityId = 0, int agencyId = 0)
    {
        var points = await _passengerChartReportService.GetLiberationByMonthAsync(year, cityId, agencyId);
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> LiberationYears(int cityId = 0, int agencyId = 0)
    {
        var years = await _passengerChartReportService.GetLiberationYearsAsync(cityId, agencyId);
        var selected = years.Contains(1403) ? 1403 : years.FirstOrDefault();
        if (selected == 0)
            selected = new PersianDateTime(DateTime.Now).Year;

        return Json(years.Select(year => new { year, selected = year == selected }));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> LiberationByCityData(string dateFrom = null, string dateTo = null)
    {
        var points = await _passengerChartReportService.GetLiberationByCityAsync(ParseDate(dateFrom), ParseDate(dateTo));
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> AntiXData()
    {
        var points = await _passengerChartReportService.GetAntiXDistributionAsync();
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> MaritalStatusData()
    {
        var points = await _passengerChartReportService.GetMaritalStatusAsync();
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> EmploymentData()
    {
        var points = await _passengerChartReportService.GetEmploymentStatusAsync();
        return Json(ToChart(points));
    }

    [CheckPermission(StandardPermission.Reports.REPORTS_VIEW, CheckPermissionAttribute.CheckPermissionResultType.Json)]
    public virtual async Task<IActionResult> Agencies(int cityId)
    {
        if (cityId <= 0)
            return Json(Array.Empty<object>());

        var agencies = await _agencyService.GetAgenciesByCityIdAsync(cityId, showHidden: true);
        var result = agencies
            .OrderBy(agency => agency.Name)
            .Select(agency => new { id = agency.Id, name = agency.Name });

        return Json(result);
    }

    #endregion

    #region Utilities

    protected virtual async Task<IActionResult> ChartPageAsync(string systemName, string titleKey, string dataAction,
        bool showCity = false, bool showAgency = false, bool showDateRange = false, bool showYear = false,
        string yearsAction = null, string dateHintKey = null)
    {
        var model = new ChartReportModel
        {
            SystemName = systemName,
            Title = await _localizationService.GetResourceAsync(titleKey),
            DataUrl = Url.Action(dataAction, "ChartReport"),
            YearsUrl = string.IsNullOrEmpty(yearsAction) ? null : Url.Action(yearsAction, "ChartReport"),
            AgenciesUrl = Url.Action(nameof(Agencies), "ChartReport"),
            ShowCity = showCity,
            ShowAgency = showAgency,
            ShowDateRange = showDateRange,
            ShowYear = showYear,
            DateHint = string.IsNullOrEmpty(dateHintKey) ? null : await _localizationService.GetResourceAsync(dateHintKey),
            AllText = await _localizationService.GetResourceAsync("Admin.Reports.Charts.All"),
            PersonText = await _localizationService.GetResourceAsync("Admin.Reports.Charts.Person"),
            TotalText = await _localizationService.GetResourceAsync("Admin.Reports.Charts.Total"),
            NoDataText = await _localizationService.GetResourceAsync("Admin.Reports.Charts.NoData")
        };

        if (showCity)
        {
            model.AvailableCities.Add(new SelectListItem { Value = "0", Text = model.AllText });
            var cities = await _cityService.GetAllCitiesAsync(pageIndex: 0, pageSize: int.MaxValue);
            foreach (var city in cities.OrderBy(city => city.Name))
                model.AvailableCities.Add(new SelectListItem { Value = city.Id.ToString(), Text = city.Name });
        }

        if (showAgency)
            model.AvailableAgencies.Add(new SelectListItem { Value = "0", Text = model.AllText });

        return View("Chart", model);
    }

    protected static DateTime? ParseDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (PersianDateTime.TryParse(value.Trim(), out var persianDate))
            return persianDate.ToDateTime();

        return null;
    }

    protected static IEnumerable<object> ToChart(IList<ChartReportPoint> points)
    {
        return points.Select(point => new { label = point.Label, count = point.Count });
    }

    #endregion
}
