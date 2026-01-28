using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
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
        AvailableAntiXItems = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Passengers.List.SearchRecoveryNo")]
    public int SearchRecoveryNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchCity")]
    public int SearchCityId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchAgency")]
    public int SearchAgencyId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchAntiX")]
    public int SearchAntiXId { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchPersonName")]
    public string SearchPersonName { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; }
    public IList<SelectListItem> AvailableAgencies { get; set; }
    public IList<SelectListItem> AvailableAntiXItems { get; set; }

    #endregion
}

