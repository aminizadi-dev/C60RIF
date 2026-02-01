using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a clinic model
/// </summary>
public partial record ClinicModel : BaseNopEntityModel
{
    #region Ctor

    public ClinicModel()
    {
        AvailableCities = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Clinics.Fields.City")]
    public int CityId { get; set; }

    public string CityName { get; set; }

    public IList<SelectListItem> AvailableCities { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Clinics.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Clinics.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Clinics.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}

