using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a clinic search model
/// </summary>
public partial record ClinicSearchModel : BaseSearchModel
{
    public int CityId { get; set; }
}

