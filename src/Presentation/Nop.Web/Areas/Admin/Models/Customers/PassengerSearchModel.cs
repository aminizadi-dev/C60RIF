using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a passenger search model
/// </summary>
public partial record PassengerSearchModel : BaseSearchModel
{
    #region Ctor

    public PassengerSearchModel()
    {
        AvailableCities = new List<SelectListItem>();
        AvailableAgencies = new List<SelectListItem>();
        AvailableClinics = new List<SelectListItem>();
        AvailableAntiXItems = new List<SelectListItem>();
        AvailableRecoveryMonths = new List<SelectListItem>();
        AvailableRecoveryYears = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Passengers.List.SearchRecoveryNo")]
    public int SearchRecoveryNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchCardNo")]
    public string SearchCardNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchCity")]
    public int SearchCityId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchAgency")]
    public int SearchAgencyId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchClinic")]
    public int SearchClinicId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchAntiX")]
    public int SearchAntiXId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchPersonName")]
    public string SearchPersonName { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchGuideNameAndLegionNo")]
    public string SearchGuideNameAndLegionNo { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Passengers.List.SearchTravelStartDate")]
    public DateTime? SearchTravelStartDateUtc { get; set; }

    [UIHint("PersianDateNullable")]
    [PersianDate]
    [NopResourceDisplayName("Admin.Passengers.List.SearchTravelEndDate")]
    public DateTime? SearchTravelEndDateUtc { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchRecoveryYear")]
    public int? SearchRecoveryYear { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchRecoveryMonth")]
    public int? SearchRecoveryMonth { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; }
    public IList<SelectListItem> AvailableAgencies { get; set; }
    public IList<SelectListItem> AvailableClinics { get; set; }
    public IList<SelectListItem> AvailableAntiXItems { get; set; }
    public IList<SelectListItem> AvailableRecoveryMonths { get; set; }
    public IList<SelectListItem> AvailableRecoveryYears { get; set; }

    #endregion
}

