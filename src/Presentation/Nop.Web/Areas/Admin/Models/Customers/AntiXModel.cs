using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents an AntiX model
/// </summary>
public partial record AntiXModel : BaseNopEntityModel
{
    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AntiX.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AntiX.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AntiX.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}

