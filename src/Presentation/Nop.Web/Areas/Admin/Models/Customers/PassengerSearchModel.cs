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
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Passengers.List.SearchRecoveryNo")]
    public int SearchRecoveryNo { get; set; }

    [NopResourceDisplayName("Admin.Passengers.List.SearchPersonName")]
    public string SearchPersonName { get; set; }

    #endregion
}

