using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Reports;

/// <summary>
/// A printable bar-chart report page
/// </summary>
public partial record ChartReportModel : BaseNopModel
{
    public string SystemName { get; set; }

    public string Title { get; set; }

    public string DataUrl { get; set; }

    public string YearsUrl { get; set; }

    public string AgenciesUrl { get; set; }

    public bool ShowCity { get; set; }

    public bool ShowAgency { get; set; }

    public bool ShowDateRange { get; set; }

    public bool ShowYear { get; set; }

    public string DateHint { get; set; }

    public string AllText { get; set; }

    public string PersonText { get; set; }

    public string TotalText { get; set; }

    public string NoDataText { get; set; }

    [NopResourceDisplayName("Admin.Reports.Charts.City")]
    public int CityId { get; set; }

    [NopResourceDisplayName("Admin.Reports.Charts.Agency")]
    public int AgencyId { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Reports.Charts.DateFrom")]
    public DateTime? DateFrom { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Reports.Charts.DateTo")]
    public DateTime? DateTo { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; } = new List<SelectListItem>();

    public IList<SelectListItem> AvailableAgencies { get; set; } = new List<SelectListItem>();
}
