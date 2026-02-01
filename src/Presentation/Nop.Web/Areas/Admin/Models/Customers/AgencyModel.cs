using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents an agency model
/// </summary>
public partial record AgencyModel : BaseNopEntityModel
{
    #region Ctor

    public AgencyModel()
    {
        AvailableCities = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Cities.Agencies.Fields.City")]
    public int CityId { get; set; }

    public string CityName { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Agencies.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Agencies.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Agencies.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}

