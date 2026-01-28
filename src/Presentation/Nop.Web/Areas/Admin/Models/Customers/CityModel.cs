using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a city model
/// </summary>
public partial record CityModel : BaseNopEntityModel
{
    #region Ctor

    public CityModel()
    {
        AgencySearchModel = new AgencySearchModel();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Cities.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Cities.Fields.NumberOfAgencies")]
    public int NumberOfAgencies { get; set; }

    public AgencySearchModel AgencySearchModel { get; set; }

    #endregion
}

