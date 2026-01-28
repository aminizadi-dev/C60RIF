using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents an agency search model
/// </summary>
public partial record AgencySearchModel : BaseSearchModel
{
    #region Properties

    public int CityId { get; set; }

    #endregion
}

